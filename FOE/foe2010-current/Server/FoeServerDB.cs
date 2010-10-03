using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using Microsoft.Win32;

namespace Foe.Server
{

    public class FoeServerDb
    {
        private static string _dbServer = null;
        private static string _dbUser = null;
        private static string _dbPass = null;
        private static string _dbName = null;

        //private static string _connStr = "Data Source=vm-xp\\sqlserverdev;Initial Catalog=FOE2;Persist Security Info=True;User ID=foe;Password=foe";
        private static string _connStr = null;
        
        /// <summary>
        /// Get the SqlConnection object. It will automatically open DB connection if it's not present.
        /// </summary>
        /// <returns>The active SqlConnection object, which can be used to communicate with DB server.</returns>
        public static SqlConnection OpenDb()
        {
            // check if _conn is null
            //if (_conn == null) {
            //    _conn = new SqlConnection(_connStr);
            //    _conn.Open();
            //}
            //else if (_conn.State != ConnectionState.Open)
            //{
            //    _conn.Open();
            //}
            //return _conn;

            // Check if _connStr is null, if so, load configuration
            if (_connStr == null)
            {
                GetConfig();
            }

            SqlConnection conn = new SqlConnection(_connStr);
            conn.Open();

            return conn;
        }

        /// <summary>
        /// Wrapper for the SqlDataReader.GetByte() function.
        /// </summary>
        /// <param name="reader">The SqlDataReader object where data will be retrieved.</param>
        /// <param name="columnName">The name of the column to retrieve.</param>
        /// <returns>The data value.</returns>
        public static byte? GetByte(SqlDataReader reader, string columnName)
        {
            int ord = reader.GetOrdinal(columnName);
            return (reader.IsDBNull(ord) ? null : (byte?)reader.GetByte(ord));
        }

        public static short? GetInt16(SqlDataReader reader, string columnName)
        {
            int ord = reader.GetOrdinal(columnName);
            return (reader.IsDBNull(ord) ? null : (short?)reader.GetInt16(ord));
        }

        public static int? GetInt32(SqlDataReader reader, string columnName)
        {
            int ord = reader.GetOrdinal(columnName);
            return (reader.IsDBNull(ord) ? null : (int?)reader.GetInt32(ord));
        }

        public static string GetString(SqlDataReader reader, string columnName)
        {
            int ord = reader.GetOrdinal(columnName);
            return (reader.IsDBNull(ord) ? null : reader.GetString(ord));
        }

        public static DateTime? GetDateTime(SqlDataReader reader, string columnName)
        {
            int ord = reader.GetOrdinal(columnName);
            return (reader.IsDBNull(ord) ? null : (DateTime?)reader.GetDateTime(ord));
        }

        private static void GetConfig()
        {
            string subKey = "Software\\BBG\\Foe";
            RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(subKey);
            if (key == null)
            {
                throw new Exception("Foe Server is not configured properly. Please run server installation again.");
            }

            _dbServer = key.GetValue("DbServer").ToString();
            _dbUser = key.GetValue("DbUser").ToString();
            _dbPass = key.GetValue("DbPass").ToString();
            _dbName = key.GetValue("DbName").ToString();
            _connStr = "Data Source=" + _dbServer + ";Initial Catalog=" + _dbName +
                ";Persist Security Info=True;User ID=" + _dbUser + ";Password=" + _dbPass;
        }
    }
}
