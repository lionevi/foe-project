using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foe.Common;
using System.Data.SqlClient;

namespace Foe.Server
{
    public static class FoeServerLog
    {
        public enum LogType { Message, Warning, Error, NetworkError, DBError };

        private static SqlConnection _writeConn = null;

        public static void Add(string processName, LogType type, string detail)
        {
            // Check if DB write connection is alive
            if ((_writeConn == null) || (_writeConn.State != System.Data.ConnectionState.Open))
            {
                _writeConn = FoeServerDb.OpenDb();
            }

            // Add log to DB
            SqlCommand cmd = _writeConn.CreateCommand();
            cmd.CommandText = "insert into Logs (DtCreated, Process, Type, Detail) values (@dtCreated, @process, @type, @detail)";
            cmd.Parameters.Add("@dtCreated", System.Data.SqlDbType.DateTime);
            cmd.Parameters.Add("@process", System.Data.SqlDbType.NVarChar, 50);
            cmd.Parameters.Add("@type", System.Data.SqlDbType.NVarChar, 10);
            cmd.Parameters.Add("@detail", System.Data.SqlDbType.NVarChar, -1);
            cmd.Prepare();
            cmd.Parameters["@dtCreated"].Value = DateTime.Now;
            cmd.Parameters["@process"].Value = processName;
            cmd.Parameters["@type"].Value = LogTypeToString(type);
            cmd.Parameters["@detail"].Value = detail;
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// This is for something more serious such as database connection errors.
        /// </summary>
        /// <param name="processName">The name of the process.</param>
        /// <param name="type">Type of the log (e.g. "Network", "DB", etc.)</param>
        /// <param name="detail">The detail nature of the error.</param>
        public static void AddEventLog(string processName, string type, string detail)
        {

        }

        public static string LogTypeToString(LogType type)
        {
            switch (type)
            {
                case LogType.Message:
                    return "Message";
                case LogType.Error:
                    return "Error";
                case LogType.NetworkError:
                    return "NetworkError";
                case LogType.DBError:
                    return "DBError";
                case LogType.Warning:
                    return "Warning";
                default:
                    return "Unknown";
            }
        }
    }
}
