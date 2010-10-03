using System;
using System.Collections.Generic;
using System.Text;
using Foe.Common;
using System.Xml;
using System.IO;
using System.Net.Mail;
using System.Net;

namespace Foe.Common
{
    public static class MessageManager
    {
        public static void SendMessage(SmtpServer server, string from, string to, string subject, FoeMessage message)
        {
            // compress Foe message and convert the compressed data to Base64 string
            byte[] compressedData = Foe.Common.CompressionManager.Compress(Encoding.UTF8.GetBytes(message.ToXml()));
            string based64 = Convert.ToBase64String(compressedData);

            // send email
            try
            {
                // create mail
                MailMessage mail = new MailMessage(from, to, subject, based64);

                // connect and send
                SmtpClient smtp = new SmtpClient(server.ServerName, server.Port);
                if (server.AuthRequired)
                {
                    smtp.EnableSsl = server.SslEnabled;
                    smtp.UseDefaultCredentials = false;
                    NetworkCredential cred = new NetworkCredential(server.UserName, server.Password);
                    smtp.Credentials = cred;
                }
                smtp.Send(mail);
            }
            catch (Exception except)
            {
                throw except;
            }
        }
    }
}
