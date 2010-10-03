using System;
using System.Collections.Generic;
using System.Text;
using Foe.Common;
using OpenPOP.POP3;
using System.Xml;
using System.IO;

namespace Foe.Client
{
    public class FoeClientMessage
    {
        /// <summary>
        /// Attempt to get SMTP server information. If no server is available or if information is invalid, then null will be returned.
        /// </summary>
        /// <returns>SmtpServer object if SMTP is configured. Returns null otherwise.</returns>
        public static SmtpServer GetSmtpServer()
        {
            SmtpServer server = new SmtpServer();

            try
            {
                server.ServerName = FoeClientRegistry.GetEntry("smtpserver").Value;
                server.Port = Convert.ToInt32(FoeClientRegistry.GetEntry("smtpport").Value);
                server.AuthRequired = ((FoeClientRegistry.GetEntry("smtpauthrequired").Value == "T") ? true : false);
                if (server.AuthRequired)
                {
                    server.SslEnabled = ((FoeClientRegistry.GetEntry("smtpsslenabled").Value == "T") ? true : false);
                    server.UserName = FoeClientRegistry.GetEntry("smtpusername").Value;
                    server.Password = FoeClientRegistry.GetEntry("smtppassword").Value;
                }
            }
            catch (Exception)
            {
                // Invalid SMTP server configuration
                //throw new Exception("Invalid SMTP server configurations.");
                server = null;
            }

            return server;
        }

        public static void SetSmtpServer(SmtpServer server)
        {
            FoeClientRegistry.SetEntry("smtpserver", server.ServerName);
            FoeClientRegistry.SetEntry("smtpport", server.Port.ToString());
            FoeClientRegistry.SetEntry("smtpauthrequired", (server.AuthRequired ? "T" : "F"));
            if (server.AuthRequired)
            {
                FoeClientRegistry.SetEntry("smtpsslenabled", (server.SslEnabled ? "T" : "F"));
                FoeClientRegistry.SetEntry("smtpusername", server.UserName);
                FoeClientRegistry.SetEntry("smtppassword", server.UserName);
            }
        }

        /// <summary>
        /// Attempt to get POP3 server configurations. If no POP3 is configured or if information is invalid, null will be returned.
        /// </summary>
        /// <returns>PopServer object if POP3 is configured. Returns null otherwise.</returns>
        public static PopServer GetPopServer()
        {
            PopServer server = new PopServer();

            try
            {
                server.ServerName = FoeClientRegistry.GetEntry("popserver").Value;
                server.Port = Convert.ToInt32(FoeClientRegistry.GetEntry("popport").Value);
                server.SslEnabled = ((FoeClientRegistry.GetEntry("popsslenabled").Value == "T") ? true : false);
                server.UserName = FoeClientRegistry.GetEntry("popusername").Value;
                server.Password = FoeClientRegistry.GetEntry("poppassword").Value;
            }
            catch (Exception)
            {
                // Invalid POP3 server configuration
                //throw new Exception("Invalid POP3 server configurations.");
                server = null;
            }

            return server;
        }

        public static void SetPopServer(PopServer server)
        {
            FoeClientRegistry.SetEntry("popserver", server.ServerName);
            FoeClientRegistry.SetEntry("popport", server.Port.ToString());
            FoeClientRegistry.SetEntry("popsslenabled", (server.SslEnabled ? "T" : "F"));
            FoeClientRegistry.SetEntry("popusername", server.UserName);
            FoeClientRegistry.SetEntry("poppassword", server.Password);
        }

        public static void SendMessage(SmtpServer server, string from, string to, string subject, FoeMessage message)
        {
            // simply call the SendMessage function in the Common.MessageManager namespace
            Foe.Common.MessageManager.SendMessage(server, from, to, subject, message);
        }

        /// <summary>
        /// Download and process email messages from the POP3 server.
        /// </summary>
        /// <param name="server">POP3 server information</param>
        public static void DownloadMessages()
        {
            // Get POP3 server info
            PopServer server = GetPopServer();

            // connect to POP3 server and download messages
            //FoeDebug.Print("Connecting to POP3 server...");
            POPClient popClient = new POPClient();
            popClient.IsUsingSsl = server.SslEnabled;

            popClient.Disconnect();
            popClient.Connect(server.ServerName, server.Port);
            popClient.Authenticate(server.UserName, server.Password);

            FoeDebug.Print("Connected to POP3.");

            // get mail count
            int count = popClient.GetMessageCount();

            // go through each message, from newest to oldest
            for (int i = count; i >= 1; i -= 1)
            {
                OpenPOP.MIMEParser.Message msg = popClient.GetMessage(i, true);
                if (msg != null)
                {
                    string subject = msg.Subject;
                    string fromEmail = msg.FromEmail;

                    // Check if fromEmail is the same as processor's email on file
                    if (fromEmail.ToLower() == FoeClientRegistry.GetEntry("processoremail").Value.ToLower())
                    {
                        // parse subject line
                        string[] tokens = subject.Trim().Split(new char[] { ' ' });

                        // There should be 5 tokens
                        if (tokens.Length == 5)
                        {
                            // Get the request ID for this reply
                            string requestId = tokens[2];

                            // Check if request ID matches any request the client sent
                            FoeClientRequestItem req = FoeClientRequest.Get(requestId);
                            if (req != null)
                            {
                                // Found the matching request
                                // Download the full reply
                                OpenPOP.MIMEParser.Message wholeMsg = popClient.GetMessage(i, false);
                                string msgBody = (string)wholeMsg.MessageBody[0];

                                try
                                {
                                    // decompress it
                                    byte[] compressedMsg = Convert.FromBase64String(msgBody);
                                    byte[] decompressedMsg = CompressionManager.Decompress(compressedMsg);
                                    string foeXml = Encoding.UTF8.GetString(decompressedMsg);

                                    // Check what is the original request type
                                    if (req.Type.ToLower() == "reg")
                                    {
                                        ProcessRegistrationReply(foeXml);
                                    }
                                    else if (req.Type.ToLower() == "content")
                                    {
                                        ProcessContentReply(foeXml);
                                    }
                                    else if (req.Type.ToLower() == "catalog")
                                    {
                                        ProcessCatalogReply(foeXml);
                                    }
                                }
                                catch (Exception)
                                {
                                    // the message is likely malformed
                                    // so just ignore it
                                }
                            }
                        }
                    }
                }
                // Delete the current message
                popClient.DeleteMessage(i);
            }
            popClient.Disconnect();
        }

        private static void ProcessRegistrationReply(string xml)
        {
            // Load the xml document
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            // Get user ID
            XmlNode node = doc.GetElementsByTagName("UserId")[0];
            string userId = node.InnerText;

            // Save user ID
            FoeClientRegistry.SetEntry("userid", userId);
        }

        private static void ProcessContentReply(string xml)
        {
            // Load the xml document
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            // Get RSS Feed
            string rssBase64 = doc.GetElementsByTagName("Rss")[0].InnerText;

            // Get Catalog Code
            string catalogCode = doc.GetElementsByTagName("CatalogCode")[0].InnerText;

            // The RSS is base64 encoded, so we need to decode it
            string rss = Encoding.UTF8.GetString(Convert.FromBase64String(rssBase64));

            // Save RSS feed to file
            TextWriter writer = new StreamWriter("rss\\" + catalogCode + ".rss", false);
            writer.Write(rss);
            writer.Close();

            // Update catalog last updated time
            FoeClientCatalogItem item = FoeClientCatalog.Get(catalogCode);
            item.DtLastUpdated = DateTime.Now;
            FoeClientCatalog.Add(item);
        }

        private static void ProcessCatalogReply(string xml)
        {
            List<FoeClientCatalogItem> catalog = new List<FoeClientCatalogItem>();

            // Load the xml document
            XmlDocument rawDoc = new XmlDocument();
            rawDoc.LoadXml(xml);

            // Get RSS Feed
            string rssBase64 = rawDoc.GetElementsByTagName("Rss")[0].InnerText;

            // The RSS is base64 encoded, so we need to decode it
            string rss = Encoding.UTF8.GetString(Convert.FromBase64String(rssBase64));

            // Within the RSS is the catalog in XML format
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(rss);

            // Get catalog list
            XmlNodeList nodes = doc.GetElementsByTagName("CatalogItem");
            foreach(XmlNode item in nodes)
            {
                FoeClientCatalogItem newItem = new FoeClientCatalogItem();

                newItem.Code = item["Code"].InnerText;
                newItem.ContentType = item["ContentType"].InnerText;
                newItem.Name = item["Name"].InnerText;
                newItem.Description = item["Description"].InnerText;
                newItem.IsSubscribed = false;
                newItem.DtLastUpdated = DateTime.Now;

                // Check if the catalog item already exists
                FoeClientCatalogItem curr = FoeClientCatalog.Get(newItem.Code);
                if (curr != null)
                {
                    // It already existed, we'll update it but keep the subscription status
                    newItem.IsSubscribed = curr.IsSubscribed;
                    newItem.DtLastUpdated = curr.DtLastUpdated;
                }
                catalog.Add(newItem);
            }

            // Check if list is empty
            if (catalog.Count > 0)
            {
                // Delete current catalog
                FoeClientCatalog.DeleteAll();
                foreach (FoeClientCatalogItem item in catalog)
                {
                    FoeClientCatalog.Add(item);
                }
            }
        }
    }
}
