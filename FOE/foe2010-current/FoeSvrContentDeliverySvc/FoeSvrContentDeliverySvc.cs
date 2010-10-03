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

namespace FoeSvrContentDeliverySvc
{
    public partial class FoeSvrContentDeliverySvc : ServiceBase
    {
        private string _processName = "FoeSvrContentDeliverySvc";
        private bool _isShutdownRequested = false;
        private int _runInterval = 1; // Number of seconds to wait before attempting to check for new emails again.

        public FoeSvrContentDeliverySvc()
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
                    // process request
                    DoCatalogDelivery();
                    DoContentDelivery();
                    DoFeedAdd();
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

        private void DoFeedAdd()
        {
            //FoeDebug.Print("Loading requests...");
            bool hasError = false;
            string message = "";
            // Load content requests
            FoeRequester req = null;
            FoeServerRequest requestManager = new FoeServerRequest(RequestType.Feed, FoeServerRegistry.Get("ProcessorEmail"));

            while ((req = requestManager.GetNextRequest()) != null)
            {
                //FoeDebug.Print("Processing request from " + req.UserEmail + " with request ID " + req.RequestId);

                // Check what contents are requested             


                // Get user info
                FoeUser user = FoeServerUser.GetUser(req.UserEmail);
                if (user == null)
                {
                    //FoeDebug.Print("User not registered. Skip this request.");
                    //FoeDebug.Print("---------------------------------------");

                    // User is not registered, mark this request as "E" (Error) and skip to the next one
                    requestManager.UpdateRequestStatus(req, "E");

                    FoeServerLog.Add(_processName, FoeServerLog.LogType.Warning, "User " + user.Email + " not registered. Discard content request.");

                    continue;
                }

                //FoeDebug.Print("User verified.");

                // Process request                
                List<CatalogItem> catalogs = FoeServerCatalog.GetCatalog(); // get all the catalogs on server

                if (catalogs != null)
                {

                    //FoeDebug.Print("Generated Foe Message.");

                    foreach (CatalogItem catalog in catalogs)
                    {
                        message += catalog.Code + ",";
                    }

                    // Send reply to user
                    try
                    {
                        FoeServerMessage.SendMessage(
                            FoeServerMessage.GetDefaultSmtpServer(),
                            FoeServerRegistry.Get("ProcessorEmail"),
                            req.UserEmail,
                            SubjectGenerator.ReplySubject(RequestType.Catalog, req.RequestId, FoeServerUser.GetUser(req.UserEmail).UserId),
                            message);

                        //FoeDebug.Print("Sent reply to user.");                       
                    }
                    catch (Exception except)
                    {
                        //FoeDebug.Print("Error sending email.");
                        //FoeDebug.Print(except.ToString());
                        hasError = true;

                        FoeServerLog.Add(_processName, FoeServerLog.LogType.Error, "Error delivering content to " + user.Email + "\r\n" + except.ToString());
                    }
                }

                // If there is no error, then we'll mark the request as 'C' (Completed).
                // Otherwise, we'll leave it as 'P' (Pending).
                if (!hasError)
                {
                    // mark request as "C" (Completed)
                    requestManager.UpdateRequestStatus(req, "C");

                    FoeServerLog.Add(_processName, FoeServerLog.LogType.Message, "Sent " + message + "to " + user.Email + " and added user to AutoSubscription.");

                    //FoeDebug.Print("Marked request as 'C' (Completed).");
                    //FoeDebug.Print("----------------------------------");
                }
                else
                {
                    //FoeDebug.Print("Leave request as 'P' (Pending).");
                    //FoeDebug.Print("-------------------------------");

                    FoeServerLog.Add(_processName, FoeServerLog.LogType.Error,
                        "Error delivering content but error is likely caused by temporary email downtime. " +
                        "Leave status as 'P' (Pending) so process can try again later.");
                }
            }

            // Close all requestManager connections
            requestManager.Close();
        }

        private void DoCatalogDelivery()
        {
            //FoeDebug.Print("Loading requests...");
            bool hasError = false;
            string message = "";
            // Load content requests
            FoeRequester req = null;
            FoeServerRequest requestManager = new FoeServerRequest(RequestType.Catalog, FoeServerRegistry.Get("ProcessorEmail"));

            while ((req = requestManager.GetNextRequest()) != null)
            {
                //FoeDebug.Print("Processing request from " + req.UserEmail + " with request ID " + req.RequestId);

                // Check what contents are requested             


                // Get user info
                FoeUser user = FoeServerUser.GetUser(req.UserEmail);
                if (user == null)
                {
                    //FoeDebug.Print("User not registered. Skip this request.");
                    //FoeDebug.Print("---------------------------------------");

                    // User is not registered, mark this request as "E" (Error) and skip to the next one
                    requestManager.UpdateRequestStatus(req, "E");

                    FoeServerLog.Add(_processName, FoeServerLog.LogType.Warning, "User " + user.Email + " not registered. Discard content request.");

                    continue;
                }

                //FoeDebug.Print("User verified.");

                // Process request                
                List<CatalogItem> catalogs= FoeServerCatalog.GetCatalog(); // get all the catalogs on server

                if (catalogs != null)
                {

                    //FoeDebug.Print("Generated Foe Message.");
                
                    foreach (CatalogItem catalog in catalogs)
                    {
                        message += catalog.Code + ",";
                    }

                    // Send reply to user
                    try
                    {
                        FoeServerMessage.SendMessage(
                            FoeServerMessage.GetDefaultSmtpServer(),
                            FoeServerRegistry.Get("ProcessorEmail"),
                            req.UserEmail,
                            SubjectGenerator.ReplySubject(RequestType.Catalog, req.RequestId, FoeServerUser.GetUser(req.UserEmail).UserId),
                            message);

                        //FoeDebug.Print("Sent reply to user.");                       
                    }
                    catch (Exception except)
                    {
                        //FoeDebug.Print("Error sending email.");
                        //FoeDebug.Print(except.ToString());
                        hasError = true;

                        FoeServerLog.Add(_processName, FoeServerLog.LogType.Error, "Error delivering content to " + user.Email + "\r\n" + except.ToString());
                    }
                }

                // If there is no error, then we'll mark the request as 'C' (Completed).
                // Otherwise, we'll leave it as 'P' (Pending).
                if (!hasError)
                {
                    // mark request as "C" (Completed)
                    requestManager.UpdateRequestStatus(req, "C");

                    FoeServerLog.Add(_processName, FoeServerLog.LogType.Message, "Sent " + message + "to " + user.Email + " and added user to AutoSubscription.");

                    //FoeDebug.Print("Marked request as 'C' (Completed).");
                    //FoeDebug.Print("----------------------------------");
                }
                else
                {
                    //FoeDebug.Print("Leave request as 'P' (Pending).");
                    //FoeDebug.Print("-------------------------------");

                    FoeServerLog.Add(_processName, FoeServerLog.LogType.Error,
                        "Error delivering content but error is likely caused by temporary email downtime. " +
                        "Leave status as 'P' (Pending) so process can try again later.");
                }
            }

            // Close all requestManager connections
            requestManager.Close();
        }

        private void DoContentDelivery()
        {
            //FoeDebug.Print("Loading requests...");
            bool hasError = false;            

            // Load content requests
            FoeRequester req = null;
            FoeServerRequest requestManager = new FoeServerRequest(RequestType.Content, FoeServerRegistry.Get("ProcessorEmail"));

            while ((req = requestManager.GetNextRequest()) != null)
            {
                //FoeDebug.Print("Processing request from " + req.UserEmail + " with request ID " + req.RequestId);

                // Check what contents are requested             
                

                // Get user info
                FoeUser user = FoeServerUser.GetUser(req.UserEmail);
                if (user == null)
                {
                    //FoeDebug.Print("User not registered. Skip this request.");
                    //FoeDebug.Print("---------------------------------------");

                    // User is not registered, mark this request as "E" (Error) and skip to the next one
                    requestManager.UpdateRequestStatus(req, "E");

                    FoeServerLog.Add(_processName, FoeServerLog.LogType.Warning, "User " + user.Email + " not registered. Discard content request.");

                    continue;
                }

                //FoeDebug.Print("User verified.");

                // Process request
                string catalog = req.RequestMessage;
                string rss = FoeServerCatalog.GetRssCache(catalog); // original RSS

                if (rss != null)
                {
                    // Send reply to user
                    try
                    {
                        FoeServerMessage.SendMessage(
                            FoeServerMessage.GetDefaultSmtpServer(),
                            FoeServerRegistry.Get("ProcessorEmail"),
                            req.UserEmail,
                            SubjectGenerator.ReplySubject(RequestType.Content, catalog, req.RequestId, FoeServerUser.GetUser(req.UserEmail).UserId),
                            rss);

                        //FoeDebug.Print("Sent reply to user.");

                        // Add user to auto-subscription list
                        FoeServerAutoSubscribe.Add(req.UserEmail, catalog, req.RequestId);                       

                        //FoeDebug.Print("Added user to Auto Subscription.");
                    }
                    catch (Exception except)
                    {
                        //FoeDebug.Print("Error sending email.");
                        //FoeDebug.Print(except.ToString());
                        hasError = true;

                        FoeServerLog.Add(_processName, FoeServerLog.LogType.Error, "Error delivering content to " + user.Email + "\r\n" + except.ToString());
                    }
                }               

                // If there is no error, then we'll mark the request as 'C' (Completed).
                // Otherwise, we'll leave it as 'P' (Pending).
                if (!hasError)
                {
                    // mark request as "C" (Completed)
                    requestManager.UpdateRequestStatus(req, "C");

                    FoeServerLog.Add(_processName, FoeServerLog.LogType.Message, "Sent " + catalog + "to " + user.Email + " and added user to AutoSubscription.");
                            
                    //FoeDebug.Print("Marked request as 'C' (Completed).");
                    //FoeDebug.Print("----------------------------------");
                }
                else
                {
                    //FoeDebug.Print("Leave request as 'P' (Pending).");
                    //FoeDebug.Print("-------------------------------");

                    FoeServerLog.Add(_processName, FoeServerLog.LogType.Error, 
                        "Error delivering content but error is likely caused by temporary email downtime. " +
                        "Leave status as 'P' (Pending) so process can try again later.");
                }
            }

            // Close all requestManager connections
            requestManager.Close();
        }
    }
}
