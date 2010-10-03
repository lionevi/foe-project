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

            // Configure the feed display
            //wbFeedDisplay.ObjectForScripting = this;

            // Load catalog
            LoadCatalog();

            // Display whatever feed currently available
            DisplayFeeds();

            // Check for updates
            //CheckUpdate();

            // Delete all previous content requests
            //FoeClientRequest.DeleteOldRequest(new TimeSpan(0, 0, 0));

            // Send update request
            //SendSubscriptionRequest();

            // Starts Check-Update timer
            timerCheckUpdate.Interval = 5000;
            timerCheckUpdate.Start();

            // Resize window
            InitializeWindowDisplay();
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
                    ToolStripMenuItem newMenu = new ToolStripMenuItem(item.Name);
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
                FoeMessage message = new FoeMessage();
                List<FoeClientCatalogItem> catalog = FoeClientCatalog.GetAll();
                foreach (FoeClientCatalogItem item in catalog)
                {
                    if (item.IsSubscribed)
                    {
                        message.Add(new FoeMessageItem("CatalogCode", item.Code));
                    }
                }
                string requestId = FoeClientRequest.GenerateId();

                FoeClientMessage.SendMessage(
                    FoeClientMessage.GetSmtpServer(),
                    FoeClientRegistry.GetEntry("useremail").Value,
                    FoeClientRegistry.GetEntry("processoremail").Value,
                    SubjectGenerator.RequestSubject(RequestType.Content, requestId, FoeClientRegistry.GetEntry("userid").Value),
                    message);

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
                            "<tr><td bgcolor=\"#CCCCFF\"><b>" + item.Name + "</b></td></tr>\n";

                        // Check if feed file is available
                        try
                        {
                            TextReader reader = new StreamReader("rss\\" + item.Code + ".rss");
                            string feed = reader.ReadToEnd();

                            // Parse RSS
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(feed);
                            XmlNodeList news = doc.GetElementsByTagName("item");
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
            timerCheckUpdate.Stop();

            if (_isFirstRun)
            {
                timerCheckUpdate.Interval = _firstRunCheckInterval;
            }
            
            // Check email messages
            CheckUpdate();

            if ((DateTime.Now > (_lastRequestSent.Add(_requestInterval))) || _isFirstRun)
            {
                SendSubscriptionRequest();
            }

            _isFirstRun = false;

            timerCheckUpdate.Start();
        }

        private void CheckUpdate()
        {
            try
            {
                bool hasUpdate = false;
                FoeClientMessage.DownloadMessages();

                // Check if there is any update that's newer than what are currently displayed
                List<FoeClientCatalogItem> catalog = FoeClientCatalog.GetAll();
                foreach (FoeClientCatalogItem item in catalog)
                {
                    if (item.DtLastUpdated > _lastUpdated)
                    {
                        // Set timer to normal interval
                        timerCheckUpdate.Interval = _checkInterval;

                        // Redisplay feed
                        DisplayFeeds();

                        // record keeping
                        hasUpdate = true;
                        tssStatus.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " updated feeds.";
                        _lastUpdated = DateTime.Now;

                        break;
                    }
                }

                if (!hasUpdate)
                {
                    tssStatus.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " no new feeds.";
                }
            }
            catch (Exception)
            {
                tssStatus.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " error .";
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

    }
}
