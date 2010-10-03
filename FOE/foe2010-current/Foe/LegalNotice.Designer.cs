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
            this.tbxLegalNotice = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnAgree
            // 
            this.btnAgree.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAgree.Location = new System.Drawing.Point(210, 314);
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
            this.btnDisagree.Location = new System.Drawing.Point(12, 314);
            this.btnDisagree.Name = "btnDisagree";
            this.btnDisagree.Size = new System.Drawing.Size(180, 40);
            this.btnDisagree.TabIndex = 2;
            this.btnDisagree.Text = "I disagree";
            this.btnDisagree.UseVisualStyleBackColor = true;
            this.btnDisagree.Click += new System.EventHandler(this.btnDisagree_Click);
            // 
            // tbxLegalNotice
            // 
            this.tbxLegalNotice.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxLegalNotice.BackColor = System.Drawing.Color.White;
            this.tbxLegalNotice.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxLegalNotice.Location = new System.Drawing.Point(12, 12);
            this.tbxLegalNotice.Multiline = true;
            this.tbxLegalNotice.Name = "tbxLegalNotice";
            this.tbxLegalNotice.ReadOnly = true;
            this.tbxLegalNotice.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxLegalNotice.Size = new System.Drawing.Size(378, 292);
            this.tbxLegalNotice.TabIndex = 3;
            this.tbxLegalNotice.TabStop = false;
            // 
            // LegalNotice
            // 
            this.AcceptButton = this.btnAgree;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnDisagree;
            this.ClientSize = new System.Drawing.Size(402, 366);
            this.Controls.Add(this.tbxLegalNotice);
            this.Controls.Add(this.btnDisagree);
            this.Controls.Add(this.btnAgree);
            this.MinimumSize = new System.Drawing.Size(410, 400);
            this.Name = "LegalNotice";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Foe - Legal Notice";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.LegalNotice_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAgree;
        private System.Windows.Forms.Button btnDisagree;
        private System.Windows.Forms.TextBox tbxLegalNotice;
    }
}