using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foe.Server;
using System.Data.SqlClient;

namespace Foe.Server
{
    /// <summary>
    /// Used for AutoSubscription. Maps to the AutoSubscriptions table.
    /// </summary>
    public class FoeServerAutoSubscriber
    {
        public int Id { get; set; }
        public string UserEmail { get; set; }
        public string CatalogCode { get; set; }
        public string RequestId { get; set; }
        public DateTime DtSubscribed { get; set; }
        public DateTime? DtLastUpdated { get; set; }
    }

    public static class FoeServerAutoSubscribe
    {
        public static void Add(string userEmail, string catalogCode, string requestId)
        {
            SqlConnection conn = FoeServerDb.OpenDb();

            // First delete any existing subsubscription by the same user for the same catalog code
            SqlCommand delCmd = conn.CreateCommand(); // Deletion command
            delCmd.CommandText = "delete from AutoSubscriptions where UserEmail=@userEmail and CatalogCode=@catalogCode";
            delCmd.Parameters.Add("@userEmail", System.Data.SqlDbType.NVarChar, 256);
            delCmd.Parameters.Add("@catalogCode", System.Data.SqlDbType.NVarChar, 10);
            delCmd.Prepare();
            delCmd.Parameters["@userEmail"].Value = userEmail;
            delCmd.Parameters["@catalogCode"].Value = catalogCode;
            delCmd.ExecuteNonQuery();
            
            DateTime now = DateTime.Now;

            // Add the new subscription
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = 
                "insert into AutoSubscriptions (UserEmail, CatalogCode, RequestId, DtSubscribed, DtLastUpdated) " +
                "values (@userEmail, @catalogCode, @requestId, @dtSubscribed, @dtLastUpdated)";
            cmd.Parameters.Add("@userEmail", System.Data.SqlDbType.NVarChar, 256);
            cmd.Parameters.Add("@catalogCode", System.Data.SqlDbType.NVarChar, 10);
            cmd.Parameters.Add("@requestId", System.Data.SqlDbType.NVarChar, 128);
            cmd.Parameters.Add("@dtSubscribed", System.Data.SqlDbType.DateTime);
            cmd.Parameters.Add("@dtLastUpdated", System.Data.SqlDbType.DateTime);
            cmd.Prepare();
            cmd.Parameters["@userEmail"].Value = userEmail;
            cmd.Parameters["@catalogCode"].Value = catalogCode;
            cmd.Parameters["@requestId"].Value = requestId;
            cmd.Parameters["@dtSubscribed"].Value = now;
            cmd.Parameters["@dtLastUpdated"].Value = now;
            cmd.ExecuteNonQuery();
        }

        public static void Update(string userEmail, string catalogCode, string requestId)
        {
            SqlConnection conn = FoeServerDb.OpenDb();

            // Update existing subsubscription by the same user for the same catalog code
            SqlCommand cmd = conn.CreateCommand(); // Update command
            cmd.CommandText = 
                "update AutoSubscriptions set dtLastUpdated=@dtLastUpdated " +
                "where UserEmail=@userEmail and CatalogCode=@catalogCode";
            cmd.Parameters.Add("@dtLastUpdated", System.Data.SqlDbType.DateTime);
            cmd.Parameters.Add("@userEmail", System.Data.SqlDbType.NVarChar, 256);
            cmd.Parameters.Add("@catalogCode", System.Data.SqlDbType.NVarChar, 10);
            cmd.Prepare();
            cmd.Parameters["@dtLastUpdated"].Value = DateTime.Now;
            cmd.Parameters["@userEmail"].Value = userEmail;
            cmd.Parameters["@catalogCode"].Value = catalogCode;
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Return a FoeServerAutoSubscriber object extract from next record in the SqlDataReader object provided.
        /// </summary>
        /// <param name="reader">SqlDataReader object that contains the query results.</param>
        /// <returns>The FoeServerAutoSubscriber object representing the next record in the SqlDataReader object.</returns>
        public static FoeServerAutoSubscriber GetNextAutoSubscriberFromSqlReader(SqlDataReader reader)
        {
            FoeServerAutoSubscriber sub = null;
            if ((reader != null) && (reader.Read()))
            {
                sub = new FoeServerAutoSubscriber();
                sub.Id = (int)FoeServerDb.GetInt32(reader, "Id");
                sub.UserEmail = FoeServerDb.GetString(reader, "UserEmail");
                sub.CatalogCode = FoeServerDb.GetString(reader, "CatalogCode");
                sub.RequestId = FoeServerDb.GetString(reader, "RequestId");
                sub.DtSubscribed = (DateTime)FoeServerDb.GetDateTime(reader, "DtSubscribed");
                sub.DtLastUpdated = FoeServerDb.GetDateTime(reader, "DtLastUpdated");
            }

            return sub;
        }
    }
}
