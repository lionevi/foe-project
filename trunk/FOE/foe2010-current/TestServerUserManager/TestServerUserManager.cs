using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foe.Server;
using Foe.Common;

namespace TestFoeServerUser
{
    class TestFoeServerUser
    {
        private static string SeparatorLine = "-------------------------------------------------";
        private static string sampleEmail = "foeclient@coderation.com";

        static void Main(string[] args)
        {
            TestGenerateUserId();
            TestRegisterUser();
            TestGetUser();
            TestAssignProcessorEmail();
            TestGetUser();
            TestUpdateUser();
            TestGetUser();

            // Test completed
            PrintFooter();
        }

        static void PrintTitle(string title)
        {
            Console.WriteLine();
            Console.WriteLine(title);
            Console.WriteLine(SeparatorLine);
        }

        static void PrintFooter()
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(SeparatorLine);
            Console.WriteLine(SeparatorLine);
            Console.WriteLine("Test completed.");
        }

        static void TestGenerateUserId()
        {
            // Test GenerateUserId()
            PrintTitle("Testing GenerateUserId()");

            string email = sampleEmail;

            for (int i = 0; i < 20; i++)
            {
                Console.WriteLine(i.ToString() + ": " + FoeServerUser.GenerateUserId(email));
            }
        }

        static void TestRegisterUser()
        {
            // Test RegisterUser()
            try
            {
                PrintTitle("Testing RegisterUser()");
                FoeServerUser.RegisterUser(sampleEmail);
                Console.WriteLine("User registered.");
            }
            catch (Exception except)
            {
                Console.WriteLine(except.ToString());
            }
        }

        static void TestGetUser()
        {
            // Test GetUser()
            try
            {
                PrintTitle("Testing GetUser()");

                FoeUser user = FoeServerUser.GetUser(sampleEmail);
                if (user == null)
                {
                    Console.WriteLine("Cannot find " + sampleEmail);
                }
                else
                {
                    Console.WriteLine("User found.");
                    Console.WriteLine("ID:                " + user.Id);
                    Console.WriteLine("Email:             " + user.Email);
                    Console.WriteLine("User ID:           " + user.UserId);
                    Console.WriteLine("Date Created:      " + user.DtCreated.ToString());
                    Console.WriteLine("Verification Code: " + user.VerificationCode);
                    Console.WriteLine("Is Verified:       " + (user.IsVerified ? "true" : "false"));
                    Console.WriteLine("Date Verified:     " + ((user.DtVerified == null) ? "NULL" : user.DtVerified.ToString()));
                    Console.WriteLine("Processor Email:   " + user.ProcessorEmail);
                }
            }
            catch (Exception except)
            {
                Console.WriteLine(except.ToString());
            }
        }

        static void TestAssignProcessorEmail()
        {
            // Test AssignProcessorEmail
            PrintTitle("Testing AssignProcessorEmail()");

            Console.WriteLine("Processor email: " + FoeServerUser.AssignProcessorEmail(sampleEmail));
        }

        static void TestUpdateUser()
        {
            // Test UpdateUser()
            try
            {
                PrintTitle("Test UpdateUser()");
                Console.WriteLine("Loading user info for " + sampleEmail);
                FoeUser updateUser = FoeServerUser.GetUser(sampleEmail);
                Console.WriteLine("Resetting IsVerified and clear DtVerified.");
                updateUser.IsVerified = false;
                updateUser.DtVerified = null;
                Console.WriteLine("Calling UpdateUser().");
                FoeServerUser.UpdateUser(updateUser);
                Console.WriteLine("Done.");
            }
            catch (Exception except)
            {
                Console.WriteLine(except.ToString());
            }
        }
    }
}
