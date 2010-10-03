using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using Foe.Common;

namespace Foe.Client
{
    public static class FoeClientRegistry
    {
        /// <summary>
        /// Get registry key.
        /// </summary>
        /// <param name="registryName">Registry key name.</param>
        /// <returns>FoeClientRegistryEntry object. Returns null if key is not available.</returns>
        public static FoeClientRegistryEntry GetEntry(string registryName)
        {
            FoeClientRegistryEntry entry = null;
            SQLiteConnection conn = FoeClientDb.Open();

            // Connect to database and prepare command
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from registry where name=@name";
            cmd.Parameters.Add("@name", System.Data.DbType.String, 128);
            cmd.Prepare();
            cmd.Parameters["@name"].Value = registryName;

            // read registry entry
            SQLiteDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                entry = new FoeClientRegistryEntry();
                entry.Name = registryName;
                entry.Value = reader.GetString(reader.GetOrdinal("value"));
            }
            reader.Close();
            conn.Close();
            
            return entry;
        }

        public static void SetEntry(string registryName, string registryValue)
        {
            // first delete existing registry key
            DeleteEntry(registryName);

            // Insert new key
            SQLiteConnection conn = FoeClientDb.Open();
            SQLiteCommand cmd = conn.CreateCommand();
            cmd = conn.CreateCommand();
            cmd.CommandText = "insert into registry (name, value) values (@name, @value)";
            cmd.Parameters.Add("@name", System.Data.DbType.String, 128);
            cmd.Parameters.Add("@value", System.Data.DbType.String, 512);
            cmd.Prepare();
            cmd.Parameters["@name"].Value = registryName;
            cmd.Parameters["@value"].Value = registryValue;
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public static void DeleteEntry(string registryName)
        {
            SQLiteConnection conn = FoeClientDb.Open();

            // first delete existing registry key
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = "delete from registry where name=@name";
            cmd.Parameters.Add("@name", System.Data.DbType.String, 128);
            cmd.Prepare();
            cmd.Parameters["@name"].Value = @registryName;
            cmd.ExecuteNonQuery();
        }
    }
}
