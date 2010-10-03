namespace Foe
{
    partial class EmailInfo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmailInfo));
            this.lblIntro = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbxEmail = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbxSmtpPassword = new System.Windows.Forms.TextBox();
            this.tbxSmtpUsername = new System.Windows.Forms.TextBox();
            this.lblSmtpPassword = new System.Windows.Forms.Label();
            this.lblSmtpUsername = new System.Windows.Forms.Label();
            this.cbxSmtpRequireSsl = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbxSmtpRequireAuth = new System.Windows.Forms.CheckBox();
            this.tbxSmtpPort = new System.Windows.Forms.TextBox();
            this.tbxSmtpServer = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tbxPopPassword = new System.Windows.Forms.TextBox();
            this.tbxPopUsername = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cbxPopRequireSsl = new System.Windows.Forms.CheckBox();
            this.tbxPopPort = new System.Windows.Forms.TextBox();
            this.tbxPopServer = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblIntro
            // 
            this.lblIntro.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblIntro.Location = new System.Drawing.Point(13, 13);
            this.lblIntro.Name = "lblIntro";
            this.lblIntro.Size = new System.Drawing.Size(379, 70);
            this.lblIntro.TabIndex = 0;
            this.lblIntro.Text = resources.GetString("lblIntro.Text");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Email Address";
            // 
            // tbxEmail
            // 
            this.tbxEmail.Location = new System.Drawing.Point(19, 103);
            this.tbxEmail.Name = "tbxEmail";
            this.tbxEmail.Size = new System.Drawing.Size(373, 20);
            this.tbxEmail.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbxSmtpPassword);
            this.groupBox1.Controls.Add(this.tbxSmtpUsername);
            this.groupBox1.Controls.Add(this.lblSmtpPassword);
            this.groupBox1.Controls.Add(this.lblSmtpUsername);
            this.groupBox1.Controls.Add(this.cbxSmtpRequireSsl);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cbxSmtpRequireAuth);
            this.groupBox1.Controls.Add(this.tbxSmtpPort);
            this.groupBox1.Controls.Add(this.tbxSmtpServer);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 129);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(380, 153);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SMTP Server";
            // 
            // tbxSmtpPassword
            // 
            this.tbxSmtpPassword.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxSmtpPassword.Location = new System.Drawing.Point(196, 120);
            this.tbxSmtpPassword.Name = "tbxSmtpPassword";
            this.tbxSmtpPassword.PasswordChar = '*';
            this.tbxSmtpPassword.Size = new System.Drawing.Size(178, 20);
            this.tbxSmtpPassword.TabIndex = 9;
            // 
            // tbxSmtpUsername
            // 
            this.tbxSmtpUsername.Location = new System.Drawing.Point(6, 121);
            this.tbxSmtpUsername.Name = "tbxSmtpUsername";
            this.tbxSmtpUsername.Size = new System.Drawing.Size(178, 20);
            this.tbxSmtpUsername.TabIndex = 8;
            // 
            // lblSmtpPassword
            // 
            this.lblSmtpPassword.AutoSize = true;
            this.lblSmtpPassword.Location = new System.Drawing.Point(197, 104);
            this.lblSmtpPassword.Name = "lblSmtpPassword";
            this.lblSmtpPassword.Size = new System.Drawing.Size(53, 13);
            this.lblSmtpPassword.TabIndex = 7;
            this.lblSmtpPassword.Text = "Password";
            // 
            // lblSmtpUsername
            // 
            this.lblSmtpUsername.AutoSize = true;
            this.lblSmtpUsername.Location = new System.Drawing.Point(6, 105);
            this.lblSmtpUsername.Name = "lblSmtpUsername";
            this.lblSmtpUsername.Size = new System.Drawing.Size(60, 13);
            this.lblSmtpUsername.TabIndex = 6;
            this.lblSmtpUsername.Text = "User Name";
            // 
            // cbxSmtpRequireSsl
            // 
            this.cbxSmtpRequireSsl.AutoSize = true;
            this.cbxSmtpRequireSsl.Location = new System.Drawing.Point(9, 85);
            this.cbxSmtpRequireSsl.Name = "cbxSmtpRequireSsl";
            this.cbxSmtpRequireSsl.Size = new System.Drawing.Size(91, 17);
            this.cbxSmtpRequireSsl.TabIndex = 5;
            this.cbxSmtpRequireSsl.Text = "Requires SSL";
            this.cbxSmtpRequireSsl.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(304, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Port";
            // 
            // cbxSmtpRequireAuth
            // 
            this.cbxSmtpRequireAuth.AutoSize = true;
            this.cbxSmtpRequireAuth.Checked = true;
            this.cbxSmtpRequireAuth.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxSmtpRequireAuth.Location = new System.Drawing.Point(9, 62);
            this.cbxSmtpRequireAuth.Name = "cbxSmtpRequireAuth";
            this.cbxSmtpRequireAuth.Size = new System.Drawing.Size(139, 17);
            this.cbxSmtpRequireAuth.TabIndex = 3;
            this.cbxSmtpRequireAuth.Text = "Requires Authentication";
            this.cbxSmtpRequireAuth.UseVisualStyleBackColor = true;
            this.cbxSmtpRequireAuth.CheckedChanged += new System.EventHandler(this.cbxSmtpRequireAuth_CheckedChanged);
            // 
            // tbxSmtpPort
            // 
            this.tbxSmtpPort.Location = new System.Drawing.Point(303, 36);
            this.tbxSmtpPort.Name = "tbxSmtpPort";
            this.tbxSmtpPort.Size = new System.Drawing.Size(71, 20);
            this.tbxSmtpPort.TabIndex = 2;
            // 
            // tbxSmtpServer
            // 
            this.tbxSmtpServer.Location = new System.Drawing.Point(6, 36);
            this.tbxSmtpServer.Name = "tbxSmtpServer";
            this.tbxSmtpServer.Size = new System.Drawing.Size(288, 20);
            this.tbxSmtpServer.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Server Name";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tbxPopPassword);
            this.groupBox2.Controls.Add(this.tbxPopUsername);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.cbxPopRequireSsl);
            this.groupBox2.Controls.Add(this.tbxPopPort);
            this.groupBox2.Controls.Add(this.tbxPopServer);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(12, 288);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(380, 133);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "POP3 Server";
            // 
            // tbxPopPassword
            // 
            this.tbxPopPassword.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxPopPassword.Location = new System.Drawing.Point(196, 98);
            this.tbxPopPassword.Name = "tbxPopPassword";
            this.tbxPopPassword.PasswordChar = '*';
            this.tbxPopPassword.Size = new System.Drawing.Size(178, 20);
            this.tbxPopPassword.TabIndex = 8;
            // 
            // tbxPopUsername
            // 
            this.tbxPopUsername.Location = new System.Drawing.Point(6, 98);
            this.tbxPopUsername.Name = "tbxPopUsername";
            this.tbxPopUsername.Size = new System.Drawing.Size(178, 20);
            this.tbxPopUsername.TabIndex = 7;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(197, 80);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 13);
            this.label9.TabIndex = 6;
            this.label9.Text = "Password";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 82);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "User Name";
            // 
            // cbxPopRequireSsl
            // 
            this.cbxPopRequireSsl.AutoSize = true;
            this.cbxPopRequireSsl.Location = new System.Drawing.Point(9, 62);
            this.cbxPopRequireSsl.Name = "cbxPopRequireSsl";
            this.cbxPopRequireSsl.Size = new System.Drawing.Size(91, 17);
            this.cbxPopRequireSsl.TabIndex = 4;
            this.cbxPopRequireSsl.Text = "Requires SSL";
            this.cbxPopRequireSsl.UseVisualStyleBackColor = true;
            // 
            // tbxPopPort
            // 
            this.tbxPopPort.Location = new System.Drawing.Point(303, 35);
            this.tbxPopPort.Name = "tbxPopPort";
            this.tbxPopPort.Size = new System.Drawing.Size(71, 20);
            this.tbxPopPort.TabIndex = 3;
            // 
            // tbxPopServer
            // 
            this.tbxPopServer.Location = new System.Drawing.Point(6, 36);
            this.tbxPopServer.Name = "tbxPopServer";
            this.tbxPopServer.Size = new System.Drawing.Size(285, 20);
            this.tbxPopServer.TabIndex = 2;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(304, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(26, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Port";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Server Name";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(207, 427);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(185, 40);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(12, 427);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(185, 40);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // EmailInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 476);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tbxEmail);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblIntro);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "EmailInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Foe - Email Configurations";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.EmailInfo_FormClosed);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblIntro;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbxEmail;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbxSmtpServer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cbxSmtpRequireSsl;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cbxSmtpRequireAuth;
        private System.Windows.Forms.TextBox tbxSmtpPort;
        private System.Windows.Forms.TextBox tbxSmtpPassword;
        private System.Windows.Forms.TextBox tbxSmtpUsername;
        private System.Windows.Forms.Label lblSmtpPassword;
        private System.Windows.Forms.Label lblSmtpUsername;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox tbxPopServer;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbxPopPassword;
        private System.Windows.Forms.TextBox tbxPopUsername;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox cbxPopRequireSsl;
        private System.Windows.Forms.TextBox tbxPopPort;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}