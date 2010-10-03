using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foe.Common;
using Foe.Server;

namespace TestFoeServerCatalog
{
    class TestFoeServerCatalog
    {
        static string SeparatorLine = "---------------------------------------";

        static void Main(string[] args)
        {
            TestAddCatalogItem();
            TestGetCatalogItem();
            TestGetCatalog();
            TestUpdateCatalogItem();
            TestGetCatalog();
            TestDeleteCatalogItem();
            TestGetCatalog();
        }

        static void PrintTitle(string title)
        {
            Console.WriteLine();
            Console.WriteLine(title);
            Console.WriteLine(SeparatorLine);
        }

        static void PrintFooter()
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(SeparatorLine);
            Console.WriteLine(SeparatorLine);
            Console.WriteLine("Test completed.");
        }

        static void TestAddCatalogItem()
        {
            PrintTitle("Testing AddCatalogItem");
            string[,] data = new string[3, 4]
                {
                    {"VOACHINESE", "RSS", "News feed from VOA Chinese", "http://www.voanews.com/chinese/rss"},
                    {"RFACHINESE", "RSS", "News feed from RFA Chinese", "http://www.rfa.com/chinese/rss"},
                    {"VOAENGLISH", "RSS", "News feed from VOA English", "http://www.voanews.com/rss"}
                };

            Console.WriteLine("Adding 3 catalog items...");
            for (int i = 0; i < 3; i++)
            {
                CatalogItem item = new CatalogItem();
                item.Code = data[i, 0];
                item.ContentType = data[i, 1];
                item.Description = data[i, 2];
                item.Location = data[i, 3];

                FoeServerCatalog.AddCatalogItem(item);
            }
        }

        static void TestGetCatalog()
        {
            PrintTitle("Testing GetCatalog()");
            List<CatalogItem> items = FoeServerCatalog.GetCatalog();
            if (items.Count == 0)
            {
                Console.WriteLine("No item found in catalog.");
            }
            else
            {
                foreach (CatalogItem item in items)
                {
                    Console.WriteLine();
                    Console.WriteLine("Code:        " + item.Code);
                    Console.WriteLine("ContentType: " + item.ContentType);
                    Console.WriteLine("Description: " + item.Description);
                    Console.WriteLine("Location:    " + item.Location);
                }
            }
        }

        static void TestGetCatalogItem()
        {
            PrintTitle("Testing GetCatalogItem()");

            // get RFACHINESE
            Console.WriteLine("Getting RFACHINESE...");
            CatalogItem item = FoeServerCatalog.GetCatalogItem("RFACHINESE");

            if (item == null)
            {
                Console.WriteLine("Catalog item not found.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Code:        " + item.Code);
                Console.WriteLine("ContentType: " + item.ContentType);
                Console.WriteLine("Description: " + item.Description);
                Console.WriteLine("Location:    " + item.Location);
            }

            // get a non-existent item
            Console.WriteLine("Getting a non-existing item 'SOMETHING'...");
            item = FoeServerCatalog.GetCatalogItem("SOMETHING");

            if (item == null)
            {
                Console.WriteLine("Catalog item not found.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Code:        " + item.Code);
                Console.WriteLine("ContentType: " + item.ContentType);
                Console.WriteLine("Description: " + item.Description);
                Console.WriteLine("Location:    " + item.Location);
            }
        }

        static void TestUpdateCatalogItem()
        {
            PrintTitle("Testing UpdateCatalogItem()");
            Console.WriteLine("Change RFA's Location to http://www.rfa.com/mandarin/rss");
            CatalogItem item = FoeServerCatalog.GetCatalogItem("RFACHINESE");
            item.Location = "http://www.rfa.com/mandarin/rss";
            FoeServerCatalog.UpdateCatalogItem(item);
            Console.WriteLine("Update completed.");
        }

        static void TestDeleteCatalogItem()
        {
            PrintTitle("Testing DeleteCatalogItem()");
            Console.WriteLine("Deleting all items in catalog...");
            List<CatalogItem> items = FoeServerCatalog.GetCatalog();
            foreach (CatalogItem item in items)
            {
                FoeServerCatalog.DeleteCatalogItem(item.Code);
            }
            Console.WriteLine("Deletion completed.");
        }
    }
}
