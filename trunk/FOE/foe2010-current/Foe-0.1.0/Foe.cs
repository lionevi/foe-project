using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Foe.Common;
using Foe.Client;

namespace Foe
{
    static class Foe
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Check if this is the first time setup is run.
            // If so, we'll display the legal notice.
            if (IsFirstRun())
            {
                LegalNotice legalForm = new LegalNotice();
                Application.Run(legalForm);
                if (legalForm.DialogResult == DialogResult.Cancel)
                {
                    return;
                }
            }

            // Check if email is configured
            if (!IsEmailConfigured())
            {
                Setup setupForm = new Setup();
                Application.Run(setupForm);

                // Check the DialogResult for setupForm
                if (setupForm.DialogResult == DialogResult.Cancel)
                {
                    MessageBox.Show("Please come back again when you have an email account ready.");
                    return;
                }

                // Open the email information form
                EmailInfo emailInfoForm = new EmailInfo();
                Application.Run(emailInfoForm);
                if (emailInfoForm.DialogResult == DialogResult.Cancel)
                {
                    // User cancelled setup, exit program
                    return;
                }
            }

            // Check if we need to register user
            if (!IsRegistered())
            {
                Registration regForm = new Registration();
                Application.Run(regForm);

                // Check if regForm returns OK
                if (regForm.DialogResult == DialogResult.Cancel)
                {
                    // Exit program
                    return;
                }
            }

            // Setup complete, set FirstRun to "F" (false)
            FoeClientRegistry.SetEntry("firstrun", "F");

            // Start Foe client
            FoeReader foeReader = new FoeReader();
            Application.Run(foeReader);
        }

        static bool IsEmailConfigured()
        {
            SmtpServer smtp = FoeClientMessage.GetSmtpServer();
            PopServer pop = FoeClientMessage.GetPopServer();

            if ((smtp == null) || (pop==null))
            {
                return false;
            }

            return true;
        }

        static bool IsRegistered()
        {
            FoeClientRegistryEntry userIdKey = FoeClientRegistry.GetEntry("userid");
            if (userIdKey == null)
            {
                return false;
            }

            return true;
        }

        static bool IsFirstRun()
        {
            FoeClientRegistryEntry firstRunKey = FoeClientRegistry.GetEntry("firstrun");
            if ((firstRunKey == null) || (firstRunKey.Value == "T"))
            {
                return true;
            }

            return false;
        }
    }
}
