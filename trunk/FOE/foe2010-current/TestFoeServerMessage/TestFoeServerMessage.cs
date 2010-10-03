using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foe.Common;

namespace TestFoeServerMessage
{
    class TestFoeServerMessage
    {
        static void Main(string[] args)
        {
            TestFoeMessageToXml();
            TestFoeMessageImportXml();

            PrintFooter();
        }

        static void PrintTitle(string title)
        {
            string SeparatorLine = "-------------------------------------------------";
            Console.WriteLine();
            Console.WriteLine(title);
            Console.WriteLine(SeparatorLine);
        }

        static void PrintFooter()
        {
            string SeparatorLine = "-------------------------------------------------";
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(SeparatorLine);
            Console.WriteLine(SeparatorLine);
            Console.WriteLine("Test completed.");
        }

        static void TestFoeMessageToXml()
        {
            PrintTitle("Testing FoeMessage.ToXml()");

            FoeMessage message = new FoeMessage();

            string[,] data = new string[3, 2] { { "RequestId", "12345" }, { "UserId", "ABCDE" }, { "Request", "RFACHINESE" } };
            for (int i = 0; i < 3; i++)
            {
                FoeMessageItem item = new FoeMessageItem(data[i, 0], data[i, 1]);
                message.Add(item);
            }

            Console.WriteLine(message.ToXml());

            // test send email
            Console.WriteLine("Sending message to foeclient@coderation.com...");
            SmtpServer server = new SmtpServer();
            server.ServerName = "smtp.gmail.com";
            server.Port = 587;
            server.AuthRequired = true;
            server.SslEnabled = true;
            server.UserName = "foeserver@coderation.com";
            server.Password = "P@$$w0rd";
            MessageManager.SendMessage(server, "foeserver@coderation.com", "foeclient@coderation.com", "Test Message", message);
            Console.WriteLine("Message sent");
        }

        static void TestFoeMessageImportXml()
        {
            PrintTitle("Testing FoeMessage.ImportXml()");

            FoeMessage message = new FoeMessage();

            string[,] data = new string[3, 2] { { "RequestId", "12345" }, { "UserId", "ABCDE" }, { "Request", "RFACHINESE" } };
            for (int i = 0; i < 3; i++)
            {
                FoeMessageItem item = new FoeMessageItem(data[i, 0], data[i, 1]);
                message.Add(item);
            }

            message.ImportXml(message.ToXml());
            Console.WriteLine(message.ToXml());
        }
    }
}
