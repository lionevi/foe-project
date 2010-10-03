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

namespace FoeSvrRegistrationSvc
{
    public partial class FoeSvrRegistrationSvc : ServiceBase
    {
        private string _processName = "FoeSvrRegistrationSvc";
        private bool _isShutdownRequested = false;
        private int _runInterval = 1; // Number of seconds to wait before attempting to check for new emails again.

        public FoeSvrRegistrationSvc()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Thread main = new Thread(new ThreadStart(MainProg));
            main.Start();
        }

        private void MainProg()
        {
            FoeServerLog.Add(_processName, FoeServerLog.LogType.Message, "Processor started.");

            // The process will keep running until shutdown is requested
            while (!_isShutdownRequested)
            {
                try
                {
                    // Do registration work
                    DoRegistration();
                }
                catch (Exception except)
                {
                    try
                    {
                        // log error message
                        FoeServerLog.Add(_processName, FoeServerLog.LogType.Error, "Error registering user.\r\n" + except.ToString());
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

        private void DoRegistration()
        {
            SqlConnection conn = FoeServerDb.OpenDb();
            SqlConnection completeConn = FoeServerDb.OpenDb();

            // Query that gets all pending requests.
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from Requests where Status='P' and ProcessorEmail=@processorEmail and RequestType=@requestType";
            cmd.Parameters.Add("@processorEmail", System.Data.SqlDbType.NVarChar, 256);
            cmd.Parameters.Add("@requestType", System.Data.SqlDbType.NVarChar, 10);
            cmd.Prepare();

            // Query that marks request as "C" (Completed)
            SqlCommand completeCmd = completeConn.CreateCommand();
            completeCmd.CommandText = "update Requests set DtProcessed=@dtProcessed, Status='C' where id=@id";
            completeCmd.Parameters.Add("@dtProcessed", System.Data.SqlDbType.DateTime);
            completeCmd.Parameters.Add("@id", System.Data.SqlDbType.Int);
            completeCmd.Prepare();

            //FoeDebug.Print("Getting pending requests.");

            // Get all pending requests
            cmd.Parameters["@processorEmail"].Value = FoeServerRegistry.Get("ProcessorEmail");
            cmd.Parameters["@requestType"].Value = FoeServerRequest.RequestTypeToString(RequestType.Registration);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int id = (int)FoeServerDb.GetInt32(reader, "Id");
                string userEmail = FoeServerDb.GetString(reader, "UserEmail");
                string requestId = FoeServerDb.GetString(reader, "RequestId");

                //FoeDebug.Print("Processing " + userEmail);

                // Create new user account
                FoeUser user = FoeServerUser.RegisterUser(userEmail);

                // Mark request as "C" (Completed)
                completeCmd.Parameters["@dtProcessed"].Value = DateTime.Now;
                completeCmd.Parameters["@id"].Value = id;
                completeCmd.ExecuteNonQuery();

                // Send response back to user
                SmtpServer server = FoeServerMessage.GetDefaultSmtpServer();
                string subject = "Re: Register " + requestId + " by Newbie";
                
                FoeServerMessage.SendMessage(server, FoeServerRegistry.Get("ProcessorEmail"), user.Email, subject, user.UserId);

                FoeServerLog.Add(_processName, FoeServerLog.LogType.Message, "Completed registration for " + userEmail);

                //FoeDebug.Print("Sent reply to " + userEmail);
                //FoeDebug.Print("");
            }
            reader.Close();

            // Close connections
            conn.Close();
            completeConn.Close();
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
    }
}
