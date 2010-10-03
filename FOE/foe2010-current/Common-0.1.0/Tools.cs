#define DEBUG

using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Foe.Common;


namespace Foe.Common
{
    public static class RandomString
    {
        private static Random _rand = new Random();

        /// <summary>
        /// Generate random string with a specific length.
        /// </summary>
        /// <param name="length">Number of characters to generate.</param>
        /// <returns>Randomly generated string</returns>
        public static string Generate(int length)
        {
            // verification code is 32 characters long
            // valid character are 0-9, A-Z, and a-z
            string verificationCode = "";

            for (int i = 0; i < length; i++)
            {
                int c = _rand.Next(61);
                if (c < 10)
                {
                    // it's a number
                    verificationCode += Convert.ToChar(c + 48);
                }
                else if (c < 36)
                {
                    // it's upper case char
                    verificationCode += Convert.ToChar(c + 55);
                }
                else
                {
                    // it's lower case char
                    verificationCode += Convert.ToChar(c + 61);
                }
            }

            return verificationCode;
        }
    }

    public static class MD5Hash
    {

        public static string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }

    public static class SubjectGenerator
    {
        public static string RequestSubject(RequestType requestType, string requestId, string userId)
        {
            string typeString = "Unknown";
            if (requestType == RequestType.Content)
            {
                typeString = "Request";
            }
            else if (requestType == RequestType.Registration)
            {
                typeString = "Register";
            }

            return (typeString + " " + requestId + " by " + userId);
        }

        public static string ReplySubject(RequestType requestType, string requestId, string userId)
        {
            return ("Re: " + RequestSubject(requestType, requestId, userId));
        }
    }

    public static class FoeDebug
    {
        public static void Print(string message)
        {
#if DEBUG
            Console.WriteLine("{0} > {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), message);
#endif
        }
    }
}
