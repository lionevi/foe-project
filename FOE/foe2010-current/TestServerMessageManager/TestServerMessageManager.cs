using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foe.Server;
using Foe.Common;

namespace TestFoeServerMessage
{
    class TestFoeServerMessage
    {
        static void Main(string[] args)
        {
            PrintTitle("Testing FoeServerMessage");

            // set up POP3 server
            PopServer server = FoeServerMessage.GetDefaultPopServer();

            Console.WriteLine("Downloading messages...");
            FoeServerMessage.DownloadMessages(server, "foeserver@coderation.com");

            Console.WriteLine("Messages downloaded. Please check database for new records.");

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
    }
}
