namespace Foe
{
    partial class Setup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Setup));
            this.tbxIntroduction = new System.Windows.Forms.TextBox();
            this.btnReady = new System.Windows.Forms.Button();
            this.btnNotReady = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbxIntroduction
            // 
            this.tbxIntroduction.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbxIntroduction.Location = new System.Drawing.Point(13, 13);
            this.tbxIntroduction.Multiline = true;
            this.tbxIntroduction.Name = "tbxIntroduction";
            this.tbxIntroduction.ReadOnly = true;
            this.tbxIntroduction.Size = new System.Drawing.Size(389, 194);
            this.tbxIntroduction.TabIndex = 0;
            this.tbxIntroduction.Text = resources.GetString("tbxIntroduction.Text");
            // 
            // btnReady
            // 
            this.btnReady.Location = new System.Drawing.Point(215, 213);
            this.btnReady.Name = "btnReady";
            this.btnReady.Size = new System.Drawing.Size(187, 41);
            this.btnReady.TabIndex = 1;
            this.btnReady.Text = "Yes, I\'m ready.";
            this.btnReady.UseVisualStyleBackColor = true;
            this.btnReady.Click += new System.EventHandler(this.btnReady_Click);
            // 
            // btnNotReady
            // 
            this.btnNotReady.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnNotReady.Location = new System.Drawing.Point(12, 213);
            this.btnNotReady.Name = "btnNotReady";
            this.btnNotReady.Size = new System.Drawing.Size(187, 41);
            this.btnNotReady.TabIndex = 2;
            this.btnNotReady.Text = "I don\'t have an account, yet.";
            this.btnNotReady.UseVisualStyleBackColor = true;
            this.btnNotReady.Click += new System.EventHandler(this.btnNotReady_Click);
            // 
            // Setup
            // 
            this.AcceptButton = this.btnReady;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnNotReady;
            this.ClientSize = new System.Drawing.Size(414, 266);
            this.Controls.Add(this.btnNotReady);
            this.Controls.Add(this.btnReady);
            this.Controls.Add(this.tbxIntroduction);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Setup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Foe Setup";
            this.Shown += new System.EventHandler(this.Setup_Shown);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Setup_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbxIntroduction;
        private System.Windows.Forms.Button btnReady;
        private System.Windows.Forms.Button btnNotReady;

    }
}