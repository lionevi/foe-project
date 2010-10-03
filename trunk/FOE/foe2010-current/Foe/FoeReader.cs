using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Foe.Client;
using Foe.Common;
using System.IO;
using System.Xml;
using System.Threading;
using System.Diagnostics;
using System.Net;
using System.Collections.ObjectModel;

namespace Foe
{

    //[System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class FoeReader : Form
    {
        DateTime _lastUpdated = DateTime.Now;
        DateTime _lastRequestSent = DateTime.Now;
        TimeSpan _requestInterval = new TimeSpan(2, 0, 0); // resend request every 2 hours
        int _checkInterval = 120000; // check email every 2 minutes
        int _firstRunCheckInterval = 15000; // when the program first start, we'll check for update every 15 seconds until the first set of news feeds arrive
        bool _isFirstRun = true;       

        public FoeReader()
        {
            InitializeComponent();

            Trace.WriteLine("Program started.");

            // Configure the feed display
            //wbFeedDisplay.ObjectForScripting = this;

            _lastUpdated = DateTime.Now;
            Trace.WriteLine("Set _lastUpdated to " + _lastUpdated.ToString("yyyy-MM-dd HH:mm:ss"));

            // Load catalog
            LoadCatalog();
            Trace.WriteLine("Loaded catalog.");

            // Display whatever feed currently available
            DisplayFeeds();
            Trace.WriteLine("Display feeds.");

            // Check for updates
            //CheckUpdate();

            // Delete all previous content requests
            //FoeClientRequest.DeleteOldRequest(new TimeSpan(0, 0, 0));

            // Send update request
            //SendSubscriptionRequest();

            // Starts Check-Update timer
            timerCheckUpdate.Interval = 5000;
            timerCheckUpdate.Start();
            Trace.WriteLine("Started timer and set interval to 5000ms");
          
            // Resize window
            InitializeWindowDisplay();
            Trace.WriteLine("Initialize window position.");
        }

        private void InitializeWindowDisplay()
        {
            this.Opacity = 1;
            int width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            int height = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;
            this.Location = new Point(width - this.Size.Width, 0);
            this.Height = height;
            this.StartPosition = FormStartPosition.Manual;
        }

        private void miEmailAccount_Click(object sender, EventArgs e)
        {
            EmailInfo emailForm = new EmailInfo();
            emailForm.ShowDialog();
        }

        private void LoadCatalog()
        {
            try
            {
                // Clear existing catalog in menu
                miSubscribe.DropDownItems.Clear();

                // Load catalog from DB
                List<FoeClientCatalogItem> catalog = FoeClientCatalog.GetAll();
                foreach (FoeClientCatalogItem item in catalog)
                {
                    ToolStripMenuItem newMenu = new ToolStripMenuItem(item.Code);
                    newMenu.Tag = item;
                    newMenu.Click += miSubscribe_Click;
                    newMenu.Checked = item.IsSubscribed;
                    miSubscribe.DropDownItems.Add(newMenu);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error loading catalog. Data file probably corrupted, please reinstall Foe. Program will now close.");
                Close();
            }

        }

        private void miSubscribe_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            FoeClientCatalogItem catItem = (FoeClientCatalogItem)item.Tag;

            // Reverse the check (i.e. change to checked if currently not checked, and vice versa.)
            item.Checked = !item.Checked;

            try
            {
                // Record change to DB
                FoeClientCatalog.SetSubscription(catItem.Code, item.Checked);

                // Update subscription
                SendSubscriptionRequest();

                // Redisplay feed
                DisplayFeeds();
            }
            catch (Exception)
            {
                MessageBox.Show("Error loading catalog. Data file probably corrupted, please reinstall Foe. Program will now close.");
                Close();
            }
        }

        private void SendSubscriptionRequest()
        {
            try
            {
                // Delete all previous content requests
                FoeClientRequest.DeleteOldRequest(_requestInterval);

                // Create Foe Message
                string catalogs = "";
                List<FoeClientCatalogItem> catalog = FoeClientCatalog.GetAll();
                if (catalog.Count == 0)
                {
                    return;
                }
                foreach (FoeClientCatalogItem item in catalog)
                {
                    if (item.IsSubscribed)
                    {
                        catalogs += item.Code + ",";
                    }
                }
                string requestId = FoeClientRequest.GenerateId();

                FoeClientMessage.SendMessage(
                    FoeClientMessage.GetSmtpServer(),
                    FoeClientRegistry.GetEntry("useremail").Value,
                    FoeClientRegistry.GetEntry("processoremail").Value,
                    SubjectGenerator.RequestSubject(RequestType.Content, requestId, FoeClientRegistry.GetEntry("userid").Value),
                    catalogs);

                // save requestid to DB
                FoeClientRequestItem reqItem = new FoeClientRequestItem();
                reqItem.Id = requestId;
                reqItem.Type = "content";
                reqItem.DtRequested = DateTime.Now;
                FoeClientRequest.Add(reqItem);

                // remember when the request was sent
                _lastRequestSent = DateTime.Now;
                // Set status
                tssStatus.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " requested update.";
            }
            catch (Exception)
            {
                tssStatus.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " error sending request.";
            }
        }

        private void DisplayFeeds()
        {

            //string rssURL = "http://www.ckxx.info/rss/rss.xml";

            //WebClient client = new WebClient();
            ////XmlTextReader client = new XmlTextReader(rssURL);
            //byte[] feedBytes = client.DownloadData(rssURL);
            //// Change feed to base64
            //string feed64 = Convert.ToBase64String(feedBytes);
            //string feed = Encoding.UTF8.GetString(feedBytes);
            //XmlDocument feedRss = new XmlDocument();
            //feedRss.Load(rssURL); 
            try
            {
                wbFeedDisplay.DocumentText = "";

                string html =
                    "<html><head><meta http-equiv=\"content-type\" content=\"text/html; charset=UTF-8\"></head>" +
                    "<body bgcolor=\"#ffffff\" style=\"font-family:Verdana,Arial;font-size:10px;margin:0px;\">\n";

                List<FoeClientCatalogItem> catalog = FoeClientCatalog.GetAll();
                foreach (FoeClientCatalogItem item in catalog)
                {
                    if (item.IsSubscribed)
                    {
                        // Add a header
                        html += "<table border=\"0\" cellpadding=\"4\" cellspacing=\"0\" width=\"100%\" style=\"font-family:Verdana,Arial;font-size:13px;\">\n" +
                            "<tr><td bgcolor=\"#CCCCFF\"><b>" + item.Code + "</b></td></tr>\n";
                        
                        // Check if feed file is available
                        try
                        {
                            // Parse RSS                            

                            XmlTextReader rssReader = new XmlTextReader(@"rss\\" + item.Code + ".rss");
                            XmlDocument rssDoc = new XmlDocument();
                            // Load the XML content into a XmlDocument
                            rssDoc.Load(rssReader);                            

                            XmlNodeList news = rssDoc.GetElementsByTagName("item");
                            bool isGray = false;
                            foreach (XmlNode newsItem in news)
                            {
                                html += "<tr><td bgcolor=\"" + (isGray ? "#F7F7F7" : "#FFFFFF") + "\">";
                                html += "<a target=\"foe\" href=\"" + newsItem["link"].InnerText + "\" style=\"color:#0000ff;text-decoration:none;\">" + newsItem["title"].InnerText + "</a><br>\n";
                                //html += "<a href=\"#\" onclick=\"window.external.LoadPage('" + newsItem["link"].InnerText + "')\" style=\"color:#0000ff;text-decoration:none;\">" + newsItem["title"].InnerText + "</a><br>\n";
                                html += "</td></tr>\n";
                                isGray = !isGray;
                            }                            
                        }
                        catch (Exception)
                        {
                            // RSS file doesn't exist.
                            // Can't do much, probably feed hasn't arrived yet.
                        }

                        html += "</table><br />&nbsp;<br />\n";
                    }
                }

                html += "</body></html>";

                wbFeedDisplay.Document.OpenNew(true);
                wbFeedDisplay.DocumentText = html;

            }
            catch (Exception)
            {
                tssStatus.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " error displaying feeds.";
            }
        }

        private void timerCheckUpdate_Tick(object sender, EventArgs e)
        {
            Trace.WriteLine("Entered timerCheckUpdate_Tick().");

            timerCheckUpdate.Stop();

            Trace.WriteLine("  Stopped timer.");

            if (_isFirstRun)
            {
                timerCheckUpdate.Interval = _firstRunCheckInterval;
                Trace.WriteLine("  First time run, set interval to " + _firstRunCheckInterval.ToString() + "ms");
            }

            // Check email messages
            CheckUpdate();
            Trace.WriteLine("  Checked update.");

            if ((DateTime.Now > (_lastRequestSent.Add(_requestInterval))) || _isFirstRun)
            {
                SendSubscriptionRequest();
                Trace.WriteLine("  Sent subscription request.");
            }

            _isFirstRun = false;

            Trace.WriteLine("  Restarting timer.");
            timerCheckUpdate.Start();
            Trace.WriteLine("Exiting timerCheckUpdate_Tick()");
        }

        private void CheckUpdate()
        {
            Trace.WriteLine("Entered CheckUpdate().");
            LoadCatalog();
            try
            {
                bool hasUpdate = false;
                Trace.WriteLine("  Downloading messages.");
                FoeClientMessage.DownloadMessages();
                Trace.WriteLine("  Downloaded messages.");

                // Check if there is any update that's newer than what are currently displayed
                List<FoeClientCatalogItem> catalog = FoeClientCatalog.GetAll();
                foreach (FoeClientCatalogItem item in catalog)
                {
                    Trace.WriteLine("  Checking updated news.");
                    Trace.WriteLine("  " + item.Code + " updated at " + item.DtLastUpdated.ToString("yyyy-MM-dd HH:mm:ss"));
                    Trace.WriteLine("  _lastUpdated = " + _lastUpdated.ToString("yyyy-MM-dd HH:mm:ss"));

                    if (item.DtLastUpdated > _lastUpdated)
                    {
                        Trace.WriteLine("  Updated news is available.");

                        // Set timer to normal interval
                        timerCheckUpdate.Interval = _checkInterval;

                        // Redisplay feed
                        DisplayFeeds();
                        Trace.WriteLine("  Displayed feed.");

                        // record keeping
                        hasUpdate = true;
                        tssStatus.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " updated feeds.";
                        _lastUpdated = DateTime.Now;
                        Trace.WriteLine("  Updated _lastUpdated.");

                        break;
                    }
                }

                if (!hasUpdate)
                {
                    tssStatus.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " no new feeds.";
                    Trace.WriteLine("  No update available.");
                }
            }
            catch (Exception except)
            {
                tssStatus.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " error .";
                Trace.WriteLine("  Error occured:\r\n" + except.ToString());
            }

            Trace.WriteLine("Exiting CheckUpdate().");
        }

        private void SendCatalogRequest()
        {
            try
            {
                // Delete all previous content requests
                FoeClientRequest.DeleteOldRequest(_requestInterval);

                // Create Foe Message
                string requestId = FoeClientRequest.GenerateId();

                FoeClientMessage.SendMessage(
                    FoeClientMessage.GetSmtpServer(),
                    FoeClientRegistry.GetEntry("useremail").Value,
                    FoeClientRegistry.GetEntry("processoremail").Value,
                    SubjectGenerator.RequestSubject(RequestType.Catalog, requestId, FoeClientRegistry.GetEntry("userid").Value),
                    "");

                // save requestid to DB
                FoeClientRequestItem reqItem = new FoeClientRequestItem();
                reqItem.Id = requestId;
                reqItem.Type = "catalog";
                reqItem.DtRequested = DateTime.Now;
                FoeClientRequest.Add(reqItem);

                // remember when the request was sent
                _lastRequestSent = DateTime.Now;
                // Set status
                tssStatus.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " requested catalog.";
            }
            catch (Exception)
            {
                tssStatus.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " error sending request.";
            }
        }

        private void miAboutFoe_Click(object sender, EventArgs e)
        {
            AboutFoe about = new AboutFoe();
            about.ShowDialog();
        }

        private void checkNewsUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckUpdate();
        }

        private void sendNewsRequestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SendSubscriptionRequest();
        }

        private void FoeReader_Shown(object sender, EventArgs e)
        {
        }

        private void updateCatalogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SendCatalogRequest();
        }

        private void addFeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddFeed addFeed = new AddFeed();
            addFeed.ShowDialog();
        }     

    }
}
