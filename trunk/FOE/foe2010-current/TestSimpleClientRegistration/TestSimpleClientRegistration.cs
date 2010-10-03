using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foe.Common;

namespace TestSimpleClientRegistration
{
    class TestSimpleClientRegistration
    {
        static string _userEmail = "foeclient@doraemon.coderation.com";
        static string _processorEmail = "foeserver@doraemon.coderation.com";

        static void Main(string[] args)
        {
            FoeDebug.Print("Generating a random request ID.");
            string requestId = RandomString.Generate(10);

            FoeDebug.Print("Setting up SMTP configuration.");
            SmtpServer server = new SmtpServer();
            server.ServerName = "doraemon";
            server.Port = 25;
            server.AuthRequired = true;
            server.SslEnabled = false;
            server.UserName = "foeclient";
            server.Password = "P@$$w0rd";

            FoeDebug.Print("Sending registration message.");
            MessageManager.SendMessage(
                server,
                _userEmail,
                _processorEmail,
                SubjectGenerator.RequestSubject(RequestType.Registration, requestId, "Newbie"),
                "");
        }
    }
}
