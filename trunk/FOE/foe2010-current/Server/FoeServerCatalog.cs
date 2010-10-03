using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Foe.Server;
using Foe.Common;
using System.Xml;
using System.Net;

namespace Foe.Server
{
    public class FoeServerRssFeed
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Rss { get; set; }
        public DateTime DtLastUpdated { get; set; }
    }

    public class FoeServerCatalog
    {
        /// <summary>
        /// Get all catalog items available.
        /// </summary>
        /// <returns>A list of catalog items that are available.</returns>
        public static List<CatalogItem> GetCatalog()
        {
            List<CatalogItem> items = new List<CatalogItem>();

            // get catalog items from DB
            SqlConnection conn = FoeServerDb.OpenDb();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from CatalogRss order by Name";
            
            // get items
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                // create a new item and populate the data
                CatalogItem item = new CatalogItem();
                item.Code = FoeServerDb.GetString(reader, "Code");
                item.ContentType = FoeServerDb.GetString(reader, "ContentType");
                item.Description = FoeServerDb.GetString(reader, "Description");
                item.Location = FoeServerDb.GetString(reader, "Location");

                // add to list
                items.Add(item);
            }

            reader.Close();
            conn.Close();

            return items;
        }

        /// <summary>
        /// Get catalog item that matches the catalog code.
        /// </summary>
        /// <param name="code">Catalog code</param>
        /// <returns>CatalogItem object that contains the details of a catalog item. If no matching catalog code is found, null will be returned.</returns>
        public static CatalogItem GetCatalogItem(string code)
        {
            CatalogItem item = null;

            // get catalog items from DB
            SqlConnection conn = FoeServerDb.OpenDb();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from CatalogRss where Code=@code";
            cmd.Parameters.Add("@code", SqlDbType.NVarChar, 10);
            cmd.Prepare();
            cmd.Parameters["@code"].Value = code;

            // get item
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                // populate the data
                item = new CatalogItem();
                item.Code = FoeServerDb.GetString(reader, "Code");
                item.ContentType = FoeServerDb.GetString(reader, "ContentType");
                item.Description = FoeServerDb.GetString(reader, "Description");
                item.Location = FoeServerDb.GetString(reader, "Location");
            }

            reader.Close();
            conn.Close();

            return item;
        }

        public static void DeleteCatalogItem(string code)
        {
            // get catalog items from DB
            SqlConnection conn = FoeServerDb.OpenDb();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "delete from CatalogRss where Code=@code";
            cmd.Parameters.Add("@code", SqlDbType.NVarChar, 10);
            cmd.Prepare();
            cmd.Parameters["@code"].Value = code;

            // delete item
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static void UpdateCatalogItem(CatalogItem item)
        {
            // get catalog items from DB
            SqlConnection conn = FoeServerDb.OpenDb();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText =
                "update CatalogRss set " +
                "ContentType=@contentType, " +
                "Description=@description, " +
                "Location=@location " +
                "where Code=@code";
            cmd.Parameters.Add("@code", SqlDbType.NVarChar, 10);
            cmd.Parameters.Add("@contentType", SqlDbType.NVarChar, 32);
            cmd.Parameters.Add("@description", SqlDbType.NVarChar, 512);
            cmd.Parameters.Add("@location", SqlDbType.NVarChar, 512);
            cmd.Prepare();

            // populate data
            cmd.Parameters["@code"].Value = item.Code;
            cmd.Parameters["@contentType"].Value = item.ContentType;
            cmd.Parameters["@description"].Value = item.Description;
            cmd.Parameters["@location"].Value = item.Location;

            // get item
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static void AddCatalogItem(CatalogItem item)
        {
            // get catalog items from DB
            SqlConnection conn = FoeServerDb.OpenDb();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText =
                "insert into CatalogRss (Code, ContentType, Description, Location) " +
                "values (@code, @contentType, @description, @location)";
            cmd.Parameters.Add("@code", SqlDbType.NVarChar, 10);
            cmd.Parameters.Add("@contentType", SqlDbType.NVarChar, 32);
            cmd.Parameters.Add("@description", SqlDbType.NVarChar, 512);
            cmd.Parameters.Add("@location", SqlDbType.NVarChar, 512);
            cmd.Prepare();

            // populate data
            cmd.Parameters["@code"].Value = item.Code;
            cmd.Parameters["@contentType"].Value = item.ContentType;
            cmd.Parameters["@description"].Value = item.Description;
            cmd.Parameters["@location"].Value = item.Location;

            // get item
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        /// <summary>
        /// Get the RSS cache.
        /// </summary>
        /// <param name="catalogCode">Catalog code representing the RSS</param>
        public static FoeServerRssFeed GetRssFeedCache(string catalogCode)
        {
           FoeServerRssFeed feed = null;

            SqlConnection conn = FoeServerDb.OpenDb();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from RssFeeds where Code=@code";
            cmd.Parameters.Add("@code", SqlDbType.NVarChar, 10);
            cmd.Prepare();
            cmd.Parameters["@code"].Value = catalogCode;

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                feed = new FoeServerRssFeed();
                feed.Id = (int)FoeServerDb.GetInt32(reader, "Id");
                feed.Rss = FoeServerDb.GetString(reader, "Rss");
                feed.Code = FoeServerDb.GetString(reader, "Code");
                feed.DtLastUpdated = (DateTime)FoeServerDb.GetDateTime(reader, "DtLastUpdated");
            }
            reader.Close();
            conn.Close();

            return feed;
        }
        /// <summary>
        /// Get the RSS cache.
        /// </summary>
        /// <param name="catalogCode">Catalog code representing the RSS</param>
        public static string GetRssCache(string catalogCode)
        {
            string rss = null;

            SqlConnection conn = FoeServerDb.OpenDb();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select Rss from RssFeeds where Code=@code";
            cmd.Parameters.Add("@code", SqlDbType.NVarChar, 10);
            cmd.Prepare();
            cmd.Parameters["@code"].Value = catalogCode;

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                rss = FoeServerDb.GetString(reader, "Rss");
            }
            reader.Close();
            conn.Close();

            return rss;
        }


        /// <summary>
        /// Add a RSS cache.
        /// </summary>
        /// <param name="catalogCode">The catalog code representing the RSS feed.</param>
        /// <param name="rss">The actual RSS to be cached.</param>
        public static void AddRssCache(string catalogCode, string rss)
        {
            SqlConnection conn = FoeServerDb.OpenDb();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "insert into RssFeeds (Code, Rss, DtLastUpdated) values (@code, @rss, @dtLastUpdated)";


            cmd.Parameters.Add("@code", SqlDbType.NVarChar, 10);
            cmd.Parameters.Add("@rss", SqlDbType.NVarChar, -1);
            cmd.Parameters.Add("@dtLastUpdated", SqlDbType.DateTime);
            cmd.Prepare();
            cmd.Parameters["@code"].Value = catalogCode;
            cmd.Parameters["@rss"].Value = rss;
            cmd.Parameters["@dtLastUpdated"].Value = DateTime.Now;
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        /// <summary>
        /// Update a RSS cache.
        /// </summary>
        /// <param name="catalogCode">The catalog code representing the RSS feed.</param>
        /// <param name="rss">The actual RSS to be cached.</param>
        public static void UpdateRssCache(string catalogCode, string rss)
        {
            
            SqlConnection conn = FoeServerDb.OpenDb();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "update RssFeeds set Rss=@rss,DtLastUpdated=@dtLastUpdated where Code=@code";
            

            cmd.Parameters.Add("@code", SqlDbType.NVarChar, 10);
            cmd.Parameters.Add("@rss", SqlDbType.NVarChar, -1);
            cmd.Parameters.Add("@dtLastUpdated", SqlDbType.DateTime);
            cmd.Prepare();
            cmd.Parameters["@code"].Value = catalogCode;
            cmd.Parameters["@rss"].Value = rss;
            cmd.Parameters["@dtLastUpdated"].Value = DateTime.Now;
            cmd.ExecuteNonQuery();  

            conn.Close();
        }

        public static void SendRssCache(string catalogCode, string userEmail, string requestId)
        {
            SendRssCache(catalogCode, userEmail, requestId, false);
        }

        public static void SendRssCache(string catalogCode, string userEmail, string requestId, bool isAutoSubscription)
        {
            // Load RSS
            string rss = FoeServerCatalog.GetRssCache(catalogCode);
            if (rss == null)
            {
                throw new Exception("RSS feed " + catalogCode + " does not exist.");
            }

            // Load User info
            FoeUser user = FoeServerUser.GetUser(userEmail);
            if (user == null)
            {
                throw new Exception("User " + userEmail + " does not exist.");
            }

            // Prepare Foe Message
            /*
            string rssBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(feed.Rss));
            FoeMessage foeMessage = new FoeMessage();
            foeMessage.Add(new FoeMessageItem("CatalogCode", feed.Code));
            foeMessage.Add(new FoeMessageItem("Rss", rssBase64));
            */            
            
            FoeDebug.Print("Generated Foe Message.");
            
            // Send reply to user
            try
            {
                FoeServerMessage.SendMessage(
                    FoeServerMessage.GetDefaultSmtpServer(),
                    FoeServerRegistry.Get("ProcessorEmail"),
                    userEmail,
                    SubjectGenerator.ReplySubject(RequestType.Content, catalogCode, requestId, FoeServerUser.GetUser(userEmail).UserId),
                    rss);

                FoeDebug.Print("Sent reply to user.");

                // Add user to auto-subscription list
                if (!isAutoSubscription)
                {
                    FoeServerAutoSubscribe.Add(userEmail, catalogCode, requestId);
                }
                else
                {
                    // If the caller function is just processing AutoSubscription, then 
                    // we don't want to recreate the subscription, simply update the
                    // current subscription.
                    FoeServerAutoSubscribe.Update(userEmail, catalogCode, requestId);
                }

                FoeDebug.Print("Added user to Auto Subscription.");
            }
            catch (Exception except)
            {
                FoeDebug.Print("FoeServerCatalog: Error sending email.");
                FoeDebug.Print(except.ToString());
            }
        }      
    }
}
