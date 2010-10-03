using System;
using System.Collections.Generic;
using System.Text;
using Foe.Common;
using OpenPOP.POP3;
using System.Xml;
using System.IO;
using System.Diagnostics;

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

        public static void SendMessage(SmtpServer server, string from, string to, string subject, string content)
        {
            // simply call the SendMessage function in the Common.MessageManager namespace
            Foe.Common.MessageManager.SendMessage(server, from, to, subject, content);
        }

        /// <summary>
        /// Download and process email messages from the POP3 server.
        /// </summary>
        /// <param name="server">POP3 server information</param>
        public static void DownloadMessages()
         {
            Trace.WriteLine("Entered DownloadMessages().");

            // Get POP3 server info
            PopServer server = GetPopServer();
            Trace.WriteLine("  Retrieved POP server info.");

            // connect to POP3 server and download messages
            //FoeDebug.Print("Connecting to POP3 server...");
            POPClient popClient = new POPClient();
            popClient.IsUsingSsl = server.SslEnabled;

            popClient.Disconnect();
            popClient.Connect(server.ServerName, server.Port);
            popClient.Authenticate(server.UserName, server.Password);

            Trace.WriteLine("  Connected to POP3.");

            // get mail count
            int count = popClient.GetMessageCount();
            Trace.WriteLine("  There are " + count.ToString() + " messages in inbox.");

            // go through each message, from newest to oldest
            for (int i = count; i >= 1; i -= 1)
            {
                Trace.WriteLine("  Opening message #" + i.ToString());

                OpenPOP.MIMEParser.Message msg = popClient.GetMessage(i, true);
                if (msg != null)
                {
                    string subject = msg.Subject;
                    string fromEmail = msg.FromEmail;

                    Trace.WriteLine("  Message came from " + msg.FromEmail + " with subject \"" + msg.Subject + "\"");

                    // Check if fromEmail is the same as processor's email on file
                    if (fromEmail.ToLower() == FoeClientRegistry.GetEntry("processoremail").Value.ToLower())
                    {
                        Trace.WriteLine("  Message came from the processor.");

                        // parse subject line
                        string[] tokens = subject.Trim().Split(new char[] { ' ' });

                        // There should be 5 or 6 tokens
                        if (tokens.Length == 5)
                        {
                            Trace.WriteLine("  There are 5 tokens.");

                            // Get the request ID for this reply
                            string requestId = tokens[2];

                            // Check if request ID matches any request the client sent
                            FoeClientRequestItem req = FoeClientRequest.Get(requestId);

                            if (req != null)
                            {
                                Trace.WriteLine("  Message Request ID matched.");

                                // Found the matching request
                                // Download the full reply
                                OpenPOP.MIMEParser.Message wholeMsg = popClient.GetMessage(i, false);
                                string msgBody = (string)wholeMsg.MessageBody[0];

                                Trace.WriteLine("  Downloaded full message body.");

                                try
                                {
                                    // decompress it
                                    byte[] compressedMsg = Convert.FromBase64String(msgBody);
                                    Trace.WriteLine("  Decoded Base64 message.");

                                    byte[] decompressedMsg = CompressionManager.Decompress(compressedMsg);
                                    Trace.WriteLine("  Decompressed message.");

                                    string foe = Encoding.UTF8.GetString(decompressedMsg);
                                    Trace.WriteLine("  Retrieved original FOE message.");

                                    // Check what is the original request type
                                    if (req.Type.ToLower() == "registe")
                                    {
                                        Trace.WriteLine("  Registration reply. Processing message.");
                                        ProcessRegistrationReply(foe);
                                        Trace.WriteLine("  Registration reply processed.");
                                    }
                                    else if (req.Type.ToLower() == "catalog")
                                    {
                                        Trace.WriteLine("  Catalog reply. Processing message.");
                                        ProcessCatalogReply(foe);
                                        Trace.WriteLine("  Catalog reply processed.");
                                    }
                                    else if (req.Type.ToLower() == "feed")
                                    {
                                        Trace.WriteLine("  feed reply. Processing message.");
                                        ProcessCatalogReply(foe);
                                        Trace.WriteLine("  feed reply processed.");
                                    }
                                }
                                catch (Exception except)
                                {
                                    // the message is likely malformed
                                    // so just ignore it
                                    Trace.WriteLine("  Exception detected: \r\n" + except.ToString());
                                }
                            }
                            else
                            {
                                Trace.WriteLine("  Message ID mismatched.");
                            }
                        }
                        //content request's reply
                        else if (tokens.Length == 6)
                        {
                            Trace.WriteLine("  There are 6 tokens.");

                            // Get the request ID for this reply
                            string catalog = tokens[1];
                            string requestId = tokens[3];

                            // Check if request ID matches any request the client sent
                            FoeClientRequestItem req = FoeClientRequest.Get(requestId);

                            if (req != null)
                            {
                                Trace.WriteLine("  Message Request ID matched.");

                                // Found the matching request
                                // Download the full reply
                                OpenPOP.MIMEParser.Message wholeMsg = popClient.GetMessage(i, false);
                                string msgBody = (string)wholeMsg.MessageBody[0];

                                Trace.WriteLine("  Downloaded full message body.");

                                try
                                {
                                    byte[] compressed = Convert.FromBase64String(msgBody);
                                    byte[] decompressed = CompressionManager.Decompress(compressed);
                                    string foe = Encoding.UTF8.GetString(decompressed);

                                    // Check what is the original request type
                                    if (req.Type.ToLower() == "content")
                                    {
                                        Trace.WriteLine("  Content reply. Processing message.");
                                        ProcessContentReply(catalog, foe);
                                        Trace.WriteLine("  Content reply processed.");
                                    }
                                }
                                catch (Exception except)
                                {
                                    // the message is likely malformed
                                    // so just ignore it
                                    Trace.WriteLine("  Exception detected: \r\n" + except.ToString());
                                }
                            }
                            else
                            {
                                Trace.WriteLine("  Message ID mismatched.");
                            }
                        }
                        else
                        {
                            Trace.WriteLine("  Message does not have 5 tokens.");
                        }
                    }
                    else
                    {
                        Trace.WriteLine("  Message did not come from processor.");
                    }
                }
                // Delete the current message
                popClient.DeleteMessage(i);
                Trace.WriteLine("  Deleted current message in inbox.");
            }
            popClient.Disconnect();
            Trace.WriteLine("  Disconnected from POP server.");

            Trace.WriteLine("  Exiting DownloadMessages().");
        }

        private static void ProcessRegistrationReply(string foe)
        {
            // Save user ID
            FoeClientRegistry.SetEntry("userid", foe);
        }

        private static void ProcessContentReply(string catalog, string foe)
        {
            // Save RSS to file
            TextWriter writer = new StreamWriter(@"rss\\" + catalog + ".rss", false);
            writer.Write(foe);
            writer.Close();

            // Update catalog last updated time
            FoeClientCatalogItem item = FoeClientCatalog.Get(catalog);
            item.DtLastUpdated = DateTime.Now;
            FoeClientCatalog.Add(item);
        }

        private static void ProcessCatalogReply(string foe)
        {
            string[] catalogs = foe.Trim().Split(new char[] { ',' });

            // Check if list is empty
            if (catalogs.Length > 0)
            {
                // Delete current catalog
                FoeClientCatalog.DeleteAll();                
                for (int i = 0; i < catalogs.Length - 1; i++)
                {                    
                    FoeClientCatalogItem item = new FoeClientCatalogItem();
                    item.Code = catalogs[i];
                    item.IsSubscribed = false;
                    item.DtLastUpdated = DateTime.Now;
                    FoeClientCatalog.Add(item);
                }
            }
        }
    }
}
