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
using System.Net;
using System.Xml;
using System.Data.SqlClient;

namespace FoeSvrRssUpdaterSvc
{
    public partial class FoeSvrRssUpdaterSvc : ServiceBase
    {
        private string _processName = "FoeSvrRssUpdaterSvc";
        private bool _isShutdownRequested = false;
        private int _runInterval = 120; // Number of seconds to wait before attempting to check for new emails again.

        public FoeSvrRssUpdaterSvc()
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
                    // Do registration work
                    DoRssUpdate();
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

        private void DoRssUpdate()
        {
            // Get all RSS items
            List<CatalogItem> items = FoeServerCatalog.GetCatalog();

            foreach (CatalogItem item in items)
            {
                // check if it's RSS content type
                if (item.ContentType.ToUpper() != "RSS")
                {
                    // not RSS, skip it
                    continue;
                }

                // fetch the RSS feed from remote RSS server
                WebClient client = new WebClient();
                byte[] rssBytes = client.DownloadData(item.Location);

                //change rss's encoding to utf8                
                string rss = Encoding.UTF8.GetString(rssBytes).TrimStart();
                //string feed = Encoding.UTF8.GetString(feedBytes);

                // Compare md5 value to see if RSS needs to be updated
                //if (CheckIfNewer(item.Code, feed))  
                //0 old, 1 new, 2 newer
                int result = CheckIfNewer(item.Code, rss);
                if (result == 1)
                {
                    FoeServerCatalog.AddRssCache(item.Code, rss);
                    FoeServerLog.Add(_processName, FoeServerLog.LogType.Message, "Add RSS feed " + item.Code);
                }
                else if (result == 2)
                {
                    // MD5 sums are different, we need to update our RSS cache
                    // FoeServerCatalog.UpdateRssCache(item.Code, feed64);
                    FoeServerCatalog.UpdateRssCache(item.Code, rss);
                    //FoeDebug.Print(item.Code + " updated.");
                    FoeServerLog.Add(_processName, FoeServerLog.LogType.Message, "Updated RSS feed " + item.Code);
                }
                else
                {
                    // no update
                    //FoeDebug.Print("No update found for " + item.Code + ".");
                }
            }
        }

        /// <summary>
        /// Check if there is anything new in the feed provided.
        /// This function will check against the database and see if there are any new
        /// RSS items. If so, it will return true.
        /// </summary>
        /// <param name="feed">Feed to check.</param>
        /// <returns>If the feed is newer than what is in the DB, then returns true. Otherwise, return false.</returns>
        static int CheckIfNewer(string catalogCode, string rss)
        {
            //0 old, 1 new, 2 newer
            int isNewer = 0;
            // Prepare SQL connection
            SqlConnection conn = FoeServerDb.OpenDb();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from RssFeeds where Code=@catalogCode";
            cmd.Parameters.Add("@catalogCode", System.Data.SqlDbType.NVarChar, 10);
            cmd.Prepare();
            cmd.Parameters["@catalogCode"].Value = catalogCode;

            //check if update the catalog the first time
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                //Rss in the cache
                XmlDocument oldRss = new XmlDocument();
                oldRss.LoadXml((string)reader["Rss"]);
                XmlNodeList oldItems = oldRss.GetElementsByTagName("item");
                XmlNode oldItem = oldItems[0];
                string oldTitle = oldItem["title"].InnerText;

                //Rss now
                XmlDocument newRss = new XmlDocument();
                newRss.LoadXml(rss);
                XmlNodeList newItems = newRss.GetElementsByTagName("item");
                XmlNode newItem = newItems[0];
                string newTitle = newItem["title"].InnerText;

                if (oldTitle != newTitle)
                {
                    isNewer = 2;
                }
            }
            else
            {
                isNewer = 1;
            }
            reader.Close();
            conn.Close();
            return isNewer;
        }
    }
}
