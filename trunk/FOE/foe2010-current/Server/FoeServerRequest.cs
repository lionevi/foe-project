using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Foe.Common;
using Foe.Server;

namespace Foe.Server
{
    public class FoeRequester
    {
        public int Id { get; set; }
        public RequestType Type { get; set; }
        public string UserEmail { get; set; }
        public string RequestId { get; set; }
        public string ProcessorEmail { get; set; }
        public string RequestMessage { get; set; }
        public DateTime DtReceived { get; set; }
        public DateTime? DtProcessed { get; set; }
        public string Status { get; set; }
    }

    public class FoeServerRequest
    {
        private SqlConnection _conn = null;  // For read (SELECT)
        private SqlDataReader _reader = null;
        private string _processorEmail = null;
        private RequestType _requestType = RequestType.Unknown;

        public FoeServerRequest(RequestType requestType, string processorEmail)
        {
            _requestType = requestType;
            _processorEmail = processorEmail;
        }
        
        public static string RequestTypeToString(RequestType requestType)
        {
            if (requestType == RequestType.Registration)
            {
                return "REGISTE";
            }
            else if (requestType == RequestType.Content)
            {
                return "CONTENT";
            }
            else if (requestType == RequestType.Catalog)
            {
                return "CATALOG";
            }
            else if (requestType == RequestType.Feed)
            {
                return "FEED";
            }

            return "UNKNOWN";
        }

        public static RequestType StringToRequestType(string requestType)
        {
            switch (requestType)
            {
                case "REGISTE":
                    return RequestType.Registration;
                case "CONTENT":
                    return RequestType.Content;
                case "CATALOG":
                    return RequestType.Catalog;
                case "FEED":
                    return RequestType.Feed;
                default:
                    return RequestType.Unknown;
            }
        }

        private void LoadRequests()
        {
            // Close the connection if for whatever reason it's not null
            if (_conn != null)
            {
                _conn.Close();
            }

            // Get all pending requests from the DB
            _conn = FoeServerDb.OpenDb();
            SqlCommand cmd = _conn.CreateCommand();
            cmd.CommandText = "select * from Requests where RequestType=@requestType and Status='P' and ProcessorEmail=@processorEmail";
            cmd.Parameters.Add("@processorEmail", System.Data.SqlDbType.NVarChar, 256);
            cmd.Parameters.Add("@requestType", System.Data.SqlDbType.NVarChar, 10);
            cmd.Prepare();
            cmd.Parameters["@processorEmail"].Value = _processorEmail;
            cmd.Parameters["@requestType"].Value = RequestTypeToString(_requestType);
            cmd.Prepare();

            _reader = cmd.ExecuteReader();
        }

        /// <summary>
        /// Get the next available request.
        /// </summary>
        /// <returns>The next available request. If no more request is available, null will be returned.</returns>
        public FoeRequester GetNextRequest()
        {
            FoeRequester req = null;            

            // check if we are currently connected to DB
            if ((_conn != null) && (_conn.State != System.Data.ConnectionState.Open))
            {
                _conn.Close();
            }
            
            LoadRequests();

            // Load request messages
            while (_reader.Read())
            {       
                req = new FoeRequester();
                req.Id = (int)FoeServerDb.GetInt32(_reader, "Id");
                req.Type = _requestType;
                req.UserEmail = FoeServerDb.GetString(_reader, "UserEmail");
                req.RequestId = FoeServerDb.GetString(_reader, "RequestId");
                req.ProcessorEmail = FoeServerDb.GetString(_reader, "ProcessorEmail");
                req.RequestMessage = FoeServerDb.GetString(_reader, "RequestMessage");
                req.DtReceived = (DateTime)FoeServerDb.GetDateTime(_reader, "DtReceived");
                req.DtProcessed = null;
                req.Status = FoeServerDb.GetString(_reader, "Status");
                
            }

            return req;
        }

        public void UpdateRequestStatus(FoeRequester req, string status)
        {
            try
            {
                // Update the status of a request
                SqlConnection conn = FoeServerDb.OpenDb();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "update Requests set Status=@status, DtProcessed=@dtProcessed where Id=@id";
                cmd.Parameters.Add("@status", System.Data.SqlDbType.NChar, 1);
                cmd.Parameters.Add("@dtProcessed", System.Data.SqlDbType.DateTime);
                cmd.Parameters.Add("@id", System.Data.SqlDbType.Int);
                cmd.Prepare();
                cmd.Parameters["@status"].Value = status;
                cmd.Parameters["@dtProcessed"].Value = DateTime.Now;
                cmd.Parameters["@id"].Value = req.Id;
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception except)
            {
                throw except;
            }
        }

        public void Close()
        {
            try
            {
                // Close the reader
                if (_reader != null)
                {
                    _reader.Close();
                }
            }
            catch (Exception)
            {
                // Error closing reader? Ignore.
            }

            try
            {
                // Close the read SqlConnection object
                if (_conn != null)
                {
                    _conn.Close();
                }
            }
            catch (Exception)
            {
                // Error closing DB connection? Umm ... ignore.
            }
            
        }
    }
}
