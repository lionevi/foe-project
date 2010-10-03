using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using Foe.Common;

namespace Foe.Client
{
    public static class FoeClientRequest
    {
        public static string GenerateId()
        {
            string id = RandomString.Generate(6);

            while (Get(id) != null)
            {
                // We need to generate another because this one is being used
                id = RandomString.Generate(6);
            }

            return id;
        }

        public static void Add(FoeClientRequestItem req)
        {
            SQLiteConnection conn = FoeClientDb.Open();
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = "insert into requests(id, type, dtrequested) values (@id, @type, @dtrequested)";
            cmd.Parameters.Add("@id", System.Data.DbType.String, 128);
            cmd.Parameters.Add("@type", System.Data.DbType.String, 10);
            cmd.Parameters.Add("@dtrequested", System.Data.DbType.DateTime);
            cmd.Prepare();

            // execute query
            cmd.Parameters["@id"].Value = req.Id;
            cmd.Parameters["@type"].Value = req.Type;
            cmd.Parameters["@dtrequested"].Value = req.DtRequested;
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static FoeClientRequestItem Get(string requestId)
        {
            SQLiteConnection conn = FoeClientDb.Open();
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from requests where id=@id";
            cmd.Parameters.Add("@id", System.Data.DbType.String, 128);
            cmd.Prepare();
            cmd.Parameters["@id"].Value = requestId;
            
            // Read request 
            FoeClientRequestItem req = null;
            SQLiteDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                req = new FoeClientRequestItem();
                req.Id = requestId;
                req.Type = reader.GetString(reader.GetOrdinal("type"));
                req.DtRequested = reader.GetDateTime(reader.GetOrdinal("dtrequested"));
            }
            reader.Close();
            conn.Close();

            return req;
        }

        public static void DeleteAll()
        {
            SQLiteConnection conn = FoeClientDb.Open();
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = "delete from requests";
            cmd.Prepare();

            // execute query
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static void DeleteOldRequest(TimeSpan span)
        {
            DateTime timeline = DateTime.Now.Subtract(span);

            SQLiteConnection conn = FoeClientDb.Open();
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = "delete from requests where dtrequested<=@timeline";
            cmd.Parameters.Add("@timeline", System.Data.DbType.Time);
            cmd.Prepare();
            cmd.Parameters["@timeline"].Value = timeline;

            // execute query
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
