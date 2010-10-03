using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foe.Common;

namespace TestSimpleClientContentRequest
{
    class TestSimpleClientContentRequest
    {
        static string _userEmail = "foeclient@coderation.com";
        static string _processorEmail = "foe@ibb.gov";
        static string _userId = "nSCHS1hCu";

        static void Main(string[] args)
        {
            FoeDebug.Print("Creating Foe Message content.");
            string catalog = "VOAENGLISH";

            FoeDebug.Print("Generating a random request ID.");
            string requestId = RandomString.Generate(10);

            FoeDebug.Print("Setting up SMTP configuration.");
            SmtpServer server = new SmtpServer();
            server.ServerName = "smtp.gmail.com";
            server.Port = 587;
            server.AuthRequired = true;
            server.SslEnabled = true;
            server.UserName = "foeclient@coderation.com";
            server.Password = "P@$$w0rd";

            FoeDebug.Print("Sending content request message.");
            string subject = SubjectGenerator.RequestSubject(RequestType.Content, requestId, _userId);
            MessageManager.SendMessage(
                server,
                _userEmail,
                _processorEmail,
                subject,
                catalog);
        }
    }
}
