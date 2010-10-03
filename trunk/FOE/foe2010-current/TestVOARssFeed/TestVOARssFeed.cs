using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml;
using System.Data.SqlClient;
using Foe.Server;
using Foe.Common;
using System.Data;

namespace TestVOARssFeed
{
    class TestVOARssFeed
    {
        static void Main(string[] args)
        {
            //// Download RSS feed
            WebClient client = new WebClient();
            //byte[] feed = client.DownloadData("http://www.ftchinese.com/rss/column/007000004");
            byte[] feed = client.DownloadData("http://www.ckxx.info/rss/rss.xml");
            string rss = Encoding.UTF8.GetString(feed).TrimStart();
           
            ////XmlTextReader reader = new XmlTextReader("http://www.ckxx.info/rss/rss.xml");          
            ////string rss = Convert.ToString(feed);
            //// Load RSS feed into an XML Document object
            //XmlDocument doc = new XmlDocument();
            //doc.LoadXml(rss);

            //string test="Re: Register requestId by Newbie";
            //string test = "abc,cde,efg,";
            //string[] tokens = test.Trim().Split(new char[] { ',' });            

            // Parse the items
            //XmlNodeList items = doc.GetElementsByTagName("item");
            //foreach (XmlNode item in items)
            //{
            //    XmlElement guid = item["title"];
            //    Console.WriteLine(guid.InnerText);
            //}
            doTest(rss);            
        }

        private static void doTest(string rss)
        {
            SqlConnection conn = FoeServerDb.OpenDb();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "insert into RssFeeds (Code, Rss, DtLastUpdated) values (@code, @rss, @dtLastUpdated)";


            cmd.Parameters.Add("@code", SqlDbType.NVarChar, 10);
            cmd.Parameters.Add("@rss", SqlDbType.NVarChar, -1);
            cmd.Parameters.Add("@dtLastUpdated", SqlDbType.DateTime);
            cmd.Prepare();
            cmd.Parameters["@code"].Value = "CKXX";
            cmd.Parameters["@rss"].Value = rss;
            cmd.Parameters["@dtLastUpdated"].Value = DateTime.Now;
            cmd.ExecuteNonQuery();

            conn.Close();           
        }
    }
}
