using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Foe.Server;
using Foe.Common;
using System.Threading;
using System.Data.SqlClient;

namespace FoeSvrAutoSubscriptionSvc
{
    public partial class FoeSvrAutoSubscriptionSvc : ServiceBase
    {
        private string _processName = "FoeSvrAutoSubscriptionSvc";
        private bool _isShutdownRequested = false;
        private int _runInterval = 60; // Number of seconds to wait before attempting to check for new emails again.

        public FoeSvrAutoSubscriptionSvc()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Thread main = new Thread(new ThreadStart(MainProg));
            main.Start();
        }

        protected override void OnStop()
        {
            _isShutdownRequested = true;
            FoeServerLog.Add(_processName, FoeServerLog.LogType.Message, "OnStop() called. Shutdown requested.");
        }

        protected override void OnShutdown()
        {
            base.OnShutdown();

            _isShutdownRequested = true;
            FoeServerLog.Add(_processName, FoeServerLog.LogType.Message, "OnShutdown() called. Shutdown requested.");
        }

        /// <summary>
        /// Returns true if shutdown is requested.
        /// </summary>
        /// <returns>Returns true if shutdown is requested. Otherwise returns false.</returns>
        private bool WakeupCall()
        {
            return _isShutdownRequested;
        }

        private void MainProg()
        {
            FoeServerLog.Add(_processName, FoeServerLog.LogType.Message, "Processor started.");

            // The process will keep running until shutdown is requested
            while (!_isShutdownRequested)
            {
                try
                {
                    // Do work
                    DoAutoSubscription();
                }
                catch (Exception except)
                {
                    try
                    {
                        // log error message
                        FoeServerLog.Add(_processName, FoeServerLog.LogType.Error, "Error:\r\n" + except.ToString());
                    }
                    catch (Exception epicExcept)
                    {
                        // Wow, this is epic! Write to System Event Logs
                        EventLog.WriteEntry(_processName + " epic failure.\r\n" + except + "\r\n" + epicExcept, EventLogEntryType.Error);

                        // We should just terminate the process
                        _isShutdownRequested = true;
                    }
                }

                // Sleep for "_runInterval" seconds before checking email again.
                FoeServerScheduler.Sleep(WakeupCall, _runInterval);
            }

            FoeServerLog.Add(_processName, FoeServerLog.LogType.Message, "Processor stopped.");
        }

        private void DoAutoSubscription()
        {
            // Connect to DB and prepare command
            SqlConnection conn = FoeServerDb.OpenDb();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from AutoSubscriptions where DtSubscribed>=@oldestTime";
            cmd.Parameters.Add("@oldestTime", System.Data.SqlDbType.DateTime);
            cmd.Prepare();

            // Calculate the oldest time that we will support
            DateTime oldest = DateTime.Now.Subtract(new TimeSpan(2, 0, 0));

            // Run query
            cmd.Parameters["@oldestTime"].Value = oldest;
            SqlDataReader reader = cmd.ExecuteReader();

            // Get subscribers
            FoeServerAutoSubscriber sub = null;
            while ((sub = FoeServerAutoSubscribe.GetNextAutoSubscriberFromSqlReader(reader)) != null)
            {
                // Compare the dates to see if last updated is newer than the RSS cache
                FoeServerRssFeed feed = FoeServerCatalog.GetRssFeedCache(sub.CatalogCode);
                if ((feed != null) && (feed.DtLastUpdated > sub.DtLastUpdated))
                {
                    try
                    {
                        // we need to send the latest news to user
                        FoeServerCatalog.SendRssCache(sub.CatalogCode, sub.UserEmail, sub.RequestId, true);

                        FoeServerLog.Add(_processName, FoeServerLog.LogType.Message, 
                            "Sent updated RSS feed to " + sub.UserEmail);
                    }
                    catch (Exception except)
                    {
                        FoeServerLog.Add(_processName, FoeServerLog.LogType.Error, 
                            "Error sending updated RSS feed to " + sub.UserEmail + ":\r\n" + except.ToString());
                    }
                }
                else
                {
                    // User already has the latest feed.
                }
            }

            reader.Close();
            conn.Close();
        }
    }
}
