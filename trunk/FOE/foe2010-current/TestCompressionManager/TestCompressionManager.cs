using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foe.Common;
using Foe.Server;

namespace TestCompressionManager
{
    class TestCompressionManager
    {
        static void Main(string[] args)
        {
            //string msg = "This is a sentence to be compressed, converted to Base64 encoding,\r\nthen converted back to compressed form, then decompressed.";
            string msg = FoeServerCatalog.GetRssCache("CKXX");
            //PrintTitle("Testing CompressionManager");

            // Show original message
            //Console.WriteLine("Original message:");
            //Console.WriteLine(msg);
            //Console.WriteLine();

            // Show Base64 without compression
            string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(msg));
            //Console.WriteLine("Base64 without compression:");
            //Console.WriteLine(base64);
            //Console.WriteLine();

            // Show compressed base64
            byte[] compressed = CompressionManager.Compress(Encoding.UTF8.GetBytes(msg));
            base64 = Convert.ToBase64String(compressed);
            //Console.WriteLine("Compressed + Base64:");
            //Console.WriteLine(base64);
            //Console.WriteLine();

            // Show decompressed
            //compressed = Convert.FromBase64String(base64);
            compressed = Convert.FromBase64String("XQAAgAAQAAAAAAAAAAArlMVWcCtFDcKRGJnu/tFfaO6aVg==");
            //compressed = Convert.FromBase64String(base64);
            byte[] decompressed = CompressionManager.Decompress(compressed);
            string originalMsg = Encoding.UTF8.GetString(decompressed);
            //Console.WriteLine("Decompressed:");
            //Console.WriteLine(originalMsg);
            //Console.WriteLine();

            //PrintFooter();
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
