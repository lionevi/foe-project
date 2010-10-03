using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Foe.Client;
using Foe.Common;

namespace Foe
{
    public partial class EmailInfo : Form
    {
        private DialogResult _result = DialogResult.Cancel;

        public EmailInfo()
        {
            InitializeComponent();

            // Try to load existing email address, SMTP and POP3 information
            FoeClientRegistryEntry smtpServer = FoeClientRegistry.GetEntry("smtpserver");
            FoeClientRegistryEntry smtpPort = FoeClientRegistry.GetEntry("smtpport");
            FoeClientRegistryEntry smtpRequireAuth = FoeClientRegistry.GetEntry("smtpauthrequired");
            FoeClientRegistryEntry smtpRequireSsl = FoeClientRegistry.GetEntry("smtpsslenabled");
            FoeClientRegistryEntry smtpUsername = FoeClientRegistry.GetEntry("smtpusername");
            FoeClientRegistryEntry smtpPassword = FoeClientRegistry.GetEntry("smtppassword");

            FoeClientRegistryEntry popServer = FoeClientRegistry.GetEntry("popserver");
            FoeClientRegistryEntry popPort = FoeClientRegistry.GetEntry("popport");
            FoeClientRegistryEntry popRequireSsl = FoeClientRegistry.GetEntry("popsslenabled");
            FoeClientRegistryEntry popUsername = FoeClientRegistry.GetEntry("popusername");
            FoeClientRegistryEntry popPassword = FoeClientRegistry.GetEntry("poppassword");

            FoeClientRegistryEntry userEmail = FoeClientRegistry.GetEntry("useremail");
            
            // Pre-fill info
            tbxEmail.Text = ((userEmail == null) ? "" : userEmail.Value);

            tbxSmtpServer.Text = ((smtpServer == null) ? "" : smtpServer.Value);
            tbxSmtpPort.Text = ((smtpPort == null) ? "" : smtpPort.Value);
            cbxSmtpRequireAuth.Checked = ((smtpRequireAuth == null) || (smtpRequireAuth.Value.ToUpper()=="T"));
            cbxSmtpRequireSsl.Checked = ((smtpRequireSsl != null) && (smtpRequireSsl.Value.ToUpper() == "T"));
            tbxSmtpUsername.Text = ((smtpUsername == null) ? "" : smtpUsername.Value);
            tbxSmtpPassword.Text = ((smtpPassword == null) ? "" : smtpPassword.Value);

            tbxPopServer.Text = ((popServer == null) ? "" : popServer.Value);
            tbxPopPort.Text = ((popPort == null) ? "" : popPort.Value);
            cbxPopRequireSsl.Checked = ((popRequireSsl != null) && (popRequireSsl.Value.ToUpper() == "T"));
            tbxPopUsername.Text = ((popUsername == null) ? "" : popUsername.Value);
            tbxPopPassword.Text = ((popPassword == null) ? "" : popPassword.Value);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Check if all information are provided
            string missingFields = CheckInputs();
            if (missingFields != null)
            {
                MessageBox.Show("The following fields are not valid.\r\n" + missingFields,
                    "Foe", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Save info database and close form
            try
            {
                SaveInfo();
                _result = DialogResult.OK;
                Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to save information. Please make sure you have all the application files in the same directory, and " +
                    "you have sufficient (read and write) privileges for all the files.", "Foe", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Changes will not be saved. Are you sure you want to exit?", "Foe", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Close();
            }
        }

        private void cbxSmtpRequireAuth_CheckedChanged(object sender, EventArgs e)
        {
            // If checked, we'll enable the remaining SMTP inputs
            cbxSmtpRequireSsl.Enabled = cbxSmtpRequireAuth.Checked;
            tbxSmtpUsername.Enabled = cbxSmtpRequireAuth.Checked;
            tbxSmtpPassword.Enabled = cbxSmtpRequireAuth.Checked;
            lblSmtpUsername.Enabled = cbxSmtpRequireAuth.Checked;
            lblSmtpPassword.Enabled = cbxSmtpRequireAuth.Checked;
        }

        private string CheckInputs()
        {
            string invalidFields = "";

            // Check email address
            if ((tbxEmail.Text.Trim().Length < 3) || (tbxEmail.Text.IndexOf('@') < 0))
            {
                invalidFields += "Email Address\r\n";
            }

            // Check SMTP server name
            if ((tbxSmtpServer.Text.Trim().Length == 0))
            {
                invalidFields += "SMTP Server Name\r\n";
            }

            // Check SMTP port
            try
            {
                int smtpPort = Convert.ToInt32(tbxSmtpPort.Text.Trim());
            }
            catch (Exception)
            {
                invalidFields += "SMTP Port\r\n";
            }

            // Check SMTP user name
            if (cbxSmtpRequireAuth.Checked && (tbxSmtpUsername.Text.Trim().Length == 0))
            {
                invalidFields += "SMTP User Name\r\n";
            }

            // Check SMTP password
            if (cbxSmtpRequireAuth.Checked && (tbxSmtpPassword.Text.Trim().Length == 0))
            {
                invalidFields += "SMTP Password\r\n";
            }

            // Check POP3 server name
            if ((tbxPopServer.Text.Trim().Length == 0))
            {
                invalidFields += "POP3 Server Name\r\n";
            }

            // Check POP3 port
            try
            {
                int smtpPort = Convert.ToInt32(tbxPopPort.Text.Trim());
            }
            catch (Exception)
            {
                invalidFields += "POP3 Port\r\n";
            }

            // Check POP3 user name
            if (tbxPopUsername.Text.Trim().Length == 0)
            {
                invalidFields += "POP3 User Name\r\n";
            }

            // Check POP3 password
            if (tbxPopPassword.Text.Trim().Length == 0)
            {
                invalidFields += "POP3 Password\r\n";
            }

            if (invalidFields.Length == 0)
            {
                return null;
            }

            return invalidFields;
        }

        private void SaveInfo()
        {
            // Save user email address
            FoeClientRegistry.SetEntry("useremail", tbxEmail.Text.Trim());

            // Save SMTP Info
            FoeClientRegistry.SetEntry("smtpserver", tbxSmtpServer.Text.Trim().ToLower());
            FoeClientRegistry.SetEntry("smtpport", tbxSmtpPort.Text.Trim());
            FoeClientRegistry.SetEntry("smtpauthrequired", (cbxSmtpRequireAuth.Checked ? "T" : "F"));
            FoeClientRegistry.SetEntry("smtpsslenabled", (cbxSmtpRequireSsl.Checked ? "T" : "F"));
            FoeClientRegistry.SetEntry("smtpusername", tbxSmtpUsername.Text.Trim());
            FoeClientRegistry.SetEntry("smtppassword", tbxSmtpPassword.Text);

            // Save POP3 info
            FoeClientRegistry.SetEntry("popserver", tbxPopServer.Text.Trim().ToLower());
            FoeClientRegistry.SetEntry("popport", tbxPopPort.Text.Trim());
            FoeClientRegistry.SetEntry("popsslenabled", (cbxPopRequireSsl.Checked ? "T" : "F"));
            FoeClientRegistry.SetEntry("popusername", tbxPopUsername.Text.Trim());
            FoeClientRegistry.SetEntry("poppassword", tbxPopPassword.Text);
        }

        private void EmailInfo_FormClosed(object sender, FormClosedEventArgs e)
        {
            DialogResult = _result;
        }
    }
}
