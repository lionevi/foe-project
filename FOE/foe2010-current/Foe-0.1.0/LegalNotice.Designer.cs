namespace Foe
{
    partial class LegalNotice
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
            this.btnAgree = new System.Windows.Forms.Button();
            this.btnDisagree = new System.Windows.Forms.Button();
            this.wbLegalNotice = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // btnAgree
            // 
            this.btnAgree.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAgree.Location = new System.Drawing.Point(202, 310);
            this.btnAgree.Name = "btnAgree";
            this.btnAgree.Size = new System.Drawing.Size(180, 40);
            this.btnAgree.TabIndex = 1;
            this.btnAgree.Text = "I agree";
            this.btnAgree.UseVisualStyleBackColor = true;
            this.btnAgree.Click += new System.EventHandler(this.btnAgree_Click);
            // 
            // btnDisagree
            // 
            this.btnDisagree.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDisagree.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnDisagree.Location = new System.Drawing.Point(12, 310);
            this.btnDisagree.Name = "btnDisagree";
            this.btnDisagree.Size = new System.Drawing.Size(180, 40);
            this.btnDisagree.TabIndex = 2;
            this.btnDisagree.Text = "I disagree";
            this.btnDisagree.UseVisualStyleBackColor = true;
            this.btnDisagree.Click += new System.EventHandler(this.btnDisagree_Click);
            // 
            // wbLegalNotice
            // 
            this.wbLegalNotice.AllowWebBrowserDrop = false;
            this.wbLegalNotice.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.wbLegalNotice.IsWebBrowserContextMenuEnabled = false;
            this.wbLegalNotice.Location = new System.Drawing.Point(12, 12);
            this.wbLegalNotice.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbLegalNotice.Name = "wbLegalNotice";
            this.wbLegalNotice.ScriptErrorsSuppressed = true;
            this.wbLegalNotice.Size = new System.Drawing.Size(370, 292);
            this.wbLegalNotice.TabIndex = 3;
            // 
            // LegalNotice
            // 
            this.AcceptButton = this.btnAgree;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnDisagree;
            this.ClientSize = new System.Drawing.Size(394, 362);
            this.Controls.Add(this.wbLegalNotice);
            this.Controls.Add(this.btnDisagree);
            this.Controls.Add(this.btnAgree);
            this.MinimumSize = new System.Drawing.Size(410, 400);
            this.Name = "LegalNotice";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Foe - Legal Notice";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.LegalNotice_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAgree;
        private System.Windows.Forms.Button btnDisagree;
        private System.Windows.Forms.WebBrowser wbLegalNotice;
    }
}