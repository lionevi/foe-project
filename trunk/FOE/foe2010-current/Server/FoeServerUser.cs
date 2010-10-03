using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Foe.Common;
using Foe.Server;

namespace Foe.Server
{
    public class FoeServerUser
    {
        private static Random _rand = new Random();

        /// <summary>
        /// Get user information in an FoeUser object.
        /// </summary>
        /// <param name="userEmail">user's email address</param>
        /// <returns>A FoeUser object populated with user info. If user is not found, then null is returned.</returns>
        public static FoeUser GetUser(string userEmail)
        {
            FoeUser user = null;

            // open connection to FOE DB
            SqlConnection conn = FoeServerDb.OpenDb();
            SqlCommand cmd = conn.CreateCommand();

            // get user
            string sql = "select * from Users where Email=@email";
            cmd.CommandText = sql;
            cmd.Parameters.Add("@email", SqlDbType.NVarChar, 256);
            cmd.Prepare();
            cmd.Parameters["@email"].Value = userEmail.Trim().ToLower();
            SqlDataReader reader = cmd.ExecuteReader();

            // see if user exists
            if (reader.HasRows)
            {
                reader.Read();

                // create a new user object;
                user = new FoeUser();

                // populate user information
                user.Id = FoeServerDb.GetInt32(reader, "Id");
                user.Email = FoeServerDb.GetString(reader, "Email");
                user.UserId = FoeServerDb.GetString(reader, "UserId");
                user.DtCreated = FoeServerDb.GetDateTime(reader, "DtCreated");
                user.ProcessorEmail = FoeServerDb.GetString(reader, "ProcessorEmail");
            }

            reader.Close();
            conn.Close();

            return user;
        }

        /// <summary>
        /// Register new user.
        /// </summary>
        /// <param name="email">User's email address</param>
        /// <returns>The user object containing the new user's information</returns>
        public static FoeUser RegisterUser(string email)
        {
            FoeUser user = GetUser(email);

            if (user == null)
            {
                // create user info
                user = new FoeUser();

                user.Id = null;
                user.Email = email.Trim().ToLower();
                user.UserId = GenerateUserId(email);
                user.DtCreated = DateTime.Now;
                user.ProcessorEmail = AssignProcessorEmail(email);

                // add user to database
                AddUser(user);
            }

            return user;
        }

        /// <summary>
        /// Generate user ID for a new user base on the email address and a random number.
        /// </summary>
        /// <param name="email">Email address of the new user.</param>
        /// <returns>New user ID</returns>
        public static string GenerateUserId(string email)
        {
            int length = _rand.Next(6, 10);
            string userId = RandomString.Generate(length);

            return userId;
        }

        /// <summary>
        /// Assign a mail process to the user.
        /// </summary>
        /// <param name="email">User's email address</param>
        /// <returns>The email address of the mail processor</returns>
        public static string AssignProcessorEmail(string email)
        {
            // get the processor from registry
            return FoeServerRegistry.Get("ProcessorEmail");
        }

        /// <summary>
        /// Add user to db
        /// </summary>
        /// <param name="user">The user object containing the user info.</param>
        public static void AddUser(FoeUser user)
        {
            SqlConnection conn = FoeServerDb.OpenDb();
            SqlCommand cmd = conn.CreateCommand();
            
            cmd.CommandText =
                "insert into Users (Email, UserId, DtCreated, ProcessorEmail) " +
                "values (@email, @userId, @dtCreated, @processorEmail)";

            cmd.Parameters.Add("@email", SqlDbType.NVarChar, 256);
            cmd.Parameters.Add("@userId", SqlDbType.NVarChar, 128);
            cmd.Parameters.Add("@dtCreated", SqlDbType.DateTime);
            cmd.Parameters.Add("@processorEmail", SqlDbType.NVarChar, 256);

            cmd.Prepare();

            // add user to db
            cmd.Parameters["@email"].Value = user.Email;
            cmd.Parameters["@userId"].Value = user.UserId;
            cmd.Parameters["@dtCreated"].Value = user.DtCreated;
            cmd.Parameters["@processorEmail"].Value = user.ProcessorEmail;

            // execute command
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public static void UpdateUser(FoeUser user)
        {
            SqlConnection conn = FoeServerDb.OpenDb();
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText =
                "update Users " +
                "set " +
                "Email=@email, " +
                "UserId=@userId, " + 
                "DtCreated=@dtCreated, " + 
                "ProcessorEmail=@processorEmail " +
                "where Id=@id";

            cmd.Parameters.Add("@id", SqlDbType.Int);
            cmd.Parameters.Add("@email", SqlDbType.NVarChar, 256);
            cmd.Parameters.Add("@userId", SqlDbType.NVarChar, 128);
            cmd.Parameters.Add("@dtCreated", SqlDbType.DateTime);
            cmd.Parameters.Add("@processorEmail", SqlDbType.NVarChar, 256);

            cmd.Prepare();

            // add user to db
            cmd.Parameters["@id"].Value = user.Id;
            cmd.Parameters["@email"].Value = user.Email;
            cmd.Parameters["@userId"].Value = user.UserId;
            cmd.Parameters["@dtCreated"].Value = user.DtCreated;
            cmd.Parameters["@processorEmail"].Value = user.ProcessorEmail;

            // execute command
            cmd.ExecuteNonQuery();

            conn.Close();
        }
    }
}
