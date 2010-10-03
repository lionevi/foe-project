using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foe.Common;
using Foe.Server;
using System.Data.SqlClient;

namespace Foe.Server
{
    public static class FoeServerRegistry
    {
        public static void Set(string name, string value)
        {
            SqlConnection conn = FoeServerDb.OpenDb();

            // Set the server registry
            SqlCommand cmd = conn.CreateCommand();

            // check if the registry name exists
            string regValue = Get(name);

            if (regValue != null)
            {
                // Registry exists, we'll update the value
                cmd.CommandText = "update Registry set Value=@value where Name=@name";
            }
            else
            {
                // Registry doesn't exist, we'll create one
                cmd.CommandText = "insert into Registry (Name, Value) values (@name, @value)";
            }
            cmd.Parameters.Add("@name", System.Data.SqlDbType.NVarChar, 50);
            cmd.Parameters.Add("@value", System.Data.SqlDbType.NVarChar, 512);
            cmd.Prepare();
            cmd.Parameters["@name"].Value = name;
            cmd.Parameters["@value"].Value = value;

            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static string Get(string name)
        {
            string value = null;
            SqlConnection conn = FoeServerDb.OpenDb();

            // Get registry value
            SqlCommand checkCmd = conn.CreateCommand();
            checkCmd.CommandText = "select Value from Registry where Name=@name";
            checkCmd.Parameters.Add("@name", System.Data.SqlDbType.NVarChar, 50);
            checkCmd.Prepare();
            checkCmd.Parameters["@name"].Value = name;
            SqlDataReader reader = checkCmd.ExecuteReader();
            if (reader.Read())
            {
                value = reader.GetString(reader.GetOrdinal("Value"));
            }
            reader.Close();
            conn.Close();

            return value;
        }
    }
}
