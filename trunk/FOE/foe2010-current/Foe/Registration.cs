using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Foe.Common;
using Foe.Client;

namespace Foe
{
    public partial class Registration : Form
    {
        private DialogResult _result = DialogResult.Cancel;

        public Registration()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to cancel setup?", "Foe", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Close();
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            // Disable Register button
            btnRegister.Enabled = false;
            btnEmailSettings.Enabled = false;

            // Update status
            try
            {
                lblStatus.Text = "Sending registration request...";
                FoeClientRegistration.SendRegistration();
                lblStatus.Text = "Registration request sent. Will check for status in 15 seconds.";

                // Start timer to check registration status
                timerCheckRegistration.Start();
            }
            catch (Exception)
            {
                MessageBox.Show("Error sending registration request. Make sure your computer is connected to the Internet and the SMTP servers " +
                    "settings are correct. You can click the Email Settings button to reconfigure SMTP and POP3 settings.", 
                    "Foe", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // re-enable buttons
                btnRegister.Enabled = true;
                btnEmailSettings.Enabled = true;

                // Update status
                lblStatus.Text = "Error sending registration request.";
            }
        }

        private void timerCheckRegistration_Tick(object sender, EventArgs e)
        {
            try
            {
                // Stop timer
                timerCheckRegistration.Stop();

                // Check messages
                lblStatus.Text = "Checking registration status...";
                FoeClientMessage.DownloadMessages();
                
                // Check if user ID is now set
                FoeClientRegistryEntry userId = FoeClientRegistry.GetEntry("userid");
                if ((userId == null) || (userId.Value.Trim().Length == 0))
                {
                    lblStatus.Text = "[" + DateTime.Now.ToString("HH:mm:ss") + "] registration still processing. Wait 15 seconds.";
                    timerCheckRegistration.Start();
                    return;
                }

                // Registration is completed
                lblStatus.Text = "Registration completed.";
                _result = DialogResult.OK;
                Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error checking registration request. Make sure your computer is connected to the Internet and the POP3 servers " +
                    "settings are correct. You can click the Email Settings button to reconfigure SMTP and POP3 settings.",
                    "Foe", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // re-enable buttons
                btnRegister.Enabled = true;
                btnEmailSettings.Enabled = true;

                // Update status
                lblStatus.Text = "Error checking registration request.";
            }
        }

        private void btnEmailSettings_Click(object sender, EventArgs e)
        {
            EmailInfo emailForm = new EmailInfo();
            emailForm.ShowDialog();
        }

        private void Registration_FormClosed(object sender, FormClosedEventArgs e)
        {
            DialogResult = _result;
        }
    }
}
