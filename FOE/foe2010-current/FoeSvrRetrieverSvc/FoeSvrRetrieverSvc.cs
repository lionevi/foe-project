using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Foe.Common;
using Foe.Server;
using System.Threading;

namespace Foe.Server.FoeSvrRetrieverSvc
{
    public partial class FoeSvrRetrieverSvc : ServiceBase
    {
        private string _processName = "FoeSvrRetrieverSvc";
        private bool _isShutdownRequested = false;
        private int _runInterval = 5; // Number of seconds to wait before attempting to check for new emails again.

        public FoeSvrRetrieverSvc()
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
                    // Download messages from mail server.
                    FoeServerMessage.DownloadMessages(FoeServerMessage.GetDefaultPopServer(), FoeServerRegistry.Get("ProcessorEmail"));
                }
                catch (Exception except)
                {
                    try
                    {
                        // log error message
                        FoeServerLog.Add(_processName, FoeServerLog.LogType.Error, "Error downloading message.\r\n" + except.ToString());
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
