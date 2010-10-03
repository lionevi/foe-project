using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace Foe.Client
{
    public static class FoeClientCatalog
    {
        public static List<FoeClientCatalogItem> GetAll()
        {
            List<FoeClientCatalogItem> catalog = new List<FoeClientCatalogItem>();

            // Open data and prepare command
            SQLiteConnection conn = FoeClientDb.Open();
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from catalog order by code";
            cmd.Prepare();
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                // Get catalog item from database
                FoeClientCatalogItem item = new FoeClientCatalogItem();
                item.Code = reader.GetString(reader.GetOrdinal("code"));
                string isSubscribed = reader.GetString(reader.GetOrdinal("issubscribed"));
                item.IsSubscribed = ((isSubscribed == "T") ? true : false);                
                item.DtLastUpdated = reader.GetDateTime(reader.GetOrdinal("dtlastupdated"));

                catalog.Add(item);
            }
            reader.Close();
            conn.Close();

            return catalog;
        }

        public static FoeClientCatalogItem Get(string catalogCode)
        {
            FoeClientCatalogItem item = null;

            // Connect to DB
            SQLiteConnection conn = FoeClientDb.Open();
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from catalog where code=@code";
            cmd.Parameters.Add("@code", System.Data.DbType.String, 10);
            cmd.Prepare();
            cmd.Parameters["@code"].Value = catalogCode;
            SQLiteDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                // Get catalog item from database
                item = new FoeClientCatalogItem();
                item.Code = reader.GetString(reader.GetOrdinal("code"));
                item.IsSubscribed = ((reader.GetString(reader.GetOrdinal("issubscribed")) == "T") ? true : false);                
                item.DtLastUpdated = reader.GetDateTime(reader.GetOrdinal("dtlastupdated"));
            }
            reader.Close();
            conn.Close();

            return item;
        }

        public static void SetSubscription(string catalogCode, bool isSubscribe)
        {
            // Connect to DB
            SQLiteConnection conn = FoeClientDb.Open();
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = "update catalog set issubscribed=@issubscribed where code=@code";
            cmd.Parameters.Add("@issubscribed", System.Data.DbType.String, 1);
            cmd.Parameters.Add("@code", System.Data.DbType.String, 10);
            cmd.Prepare();
            cmd.Parameters["@issubscribed"].Value = (isSubscribe ? "T" : "F");
            cmd.Parameters["@code"].Value = catalogCode;
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static void DeleteAll()
        {
            SQLiteConnection conn = FoeClientDb.Open();
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = "delete from catalog";
            cmd.Prepare();

            // execute query
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static void Delete(string catalogCode)
        {
            // Connect to DB
            SQLiteConnection conn = FoeClientDb.Open();
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = "delete from catalog where code=@code";
            cmd.Parameters.Add("@code", System.Data.DbType.String, 10);
            cmd.Prepare();
            cmd.Parameters["@code"].Value = catalogCode;
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static void Add(FoeClientCatalogItem item)
        {
            // First delete existing item if it exists
            Delete(item.Code);

            // Connect to DB
            SQLiteConnection conn = FoeClientDb.Open();
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText =
                "insert into catalog (code, issubscribed, dtlastupdated)" +
                "values (@code, @issubscribed, @dtlastupdated)";            
            cmd.Parameters.Add("@issubscribed", System.Data.DbType.String, 1);
            cmd.Parameters.Add("@code", System.Data.DbType.String, 10);
            cmd.Parameters.Add("@dtlastupdated", System.Data.DbType.Time);
            cmd.Prepare();
            
            cmd.Parameters["@issubscribed"].Value = (item.IsSubscribed ? "T" : "F");
            cmd.Parameters["@code"].Value = item.Code;
            cmd.Parameters["@dtlastupdated"].Value = item.DtLastUpdated;
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
