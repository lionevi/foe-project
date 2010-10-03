namespace Foe
{
    partial class FoeReader
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
            this.components = new System.ComponentModel.Container();
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.miSubscribe = new System.Windows.Forms.ToolStripMenuItem();
            this.miSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.sendNewsRequestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkNewsUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miEmailAccount = new System.Windows.Forms.ToolStripMenuItem();
            this.miHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.miAboutFoe = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tssStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.wbFeedDisplay = new System.Windows.Forms.WebBrowser();
            this.timerCheckUpdate = new System.Windows.Forms.Timer(this.components);
            this.menuStripMain.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miSubscribe,
            this.miSettings,
            this.miHelp});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new System.Drawing.Size(297, 24);
            this.menuStripMain.TabIndex = 0;
            this.menuStripMain.Text = "menuStrip1";
            // 
            // miSubscribe
            // 
            this.miSubscribe.Name = "miSubscribe";
            this.miSubscribe.Size = new System.Drawing.Size(70, 20);
            this.miSubscribe.Text = "Subscribe";
            // 
            // miSettings
            // 
            this.miSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sendNewsRequestToolStripMenuItem,
            this.checkNewsUpdatesToolStripMenuItem,
            this.miEmailAccount});
            this.miSettings.Name = "miSettings";
            this.miSettings.Size = new System.Drawing.Size(61, 20);
            this.miSettings.Text = "Settings";
            // 
            // sendNewsRequestToolStripMenuItem
            // 
            this.sendNewsRequestToolStripMenuItem.Name = "sendNewsRequestToolStripMenuItem";
            this.sendNewsRequestToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.sendNewsRequestToolStripMenuItem.Text = "Send News Request";
            this.sendNewsRequestToolStripMenuItem.Click += new System.EventHandler(this.sendNewsRequestToolStripMenuItem_Click);
            // 
            // checkNewsUpdatesToolStripMenuItem
            // 
            this.checkNewsUpdatesToolStripMenuItem.Name = "checkNewsUpdatesToolStripMenuItem";
            this.checkNewsUpdatesToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.checkNewsUpdatesToolStripMenuItem.Text = "Check News Updates";
            this.checkNewsUpdatesToolStripMenuItem.Click += new System.EventHandler(this.checkNewsUpdatesToolStripMenuItem_Click);
            // 
            // miEmailAccount
            // 
            this.miEmailAccount.Name = "miEmailAccount";
            this.miEmailAccount.Size = new System.Drawing.Size(185, 22);
            this.miEmailAccount.Text = "Email Account";
            this.miEmailAccount.Click += new System.EventHandler(this.miEmailAccount_Click);
            // 
            // miHelp
            // 
            this.miHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miAboutFoe});
            this.miHelp.Name = "miHelp";
            this.miHelp.Size = new System.Drawing.Size(44, 20);
            this.miHelp.Text = "Help";
            // 
            // miAboutFoe
            // 
            this.miAboutFoe.Name = "miAboutFoe";
            this.miAboutFoe.Size = new System.Drawing.Size(129, 22);
            this.miAboutFoe.Text = "About Foe";
            this.miAboutFoe.Click += new System.EventHandler(this.miAboutFoe_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 454);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(297, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tssStatus
            // 
            this.tssStatus.Name = "tssStatus";
            this.tssStatus.Size = new System.Drawing.Size(26, 17);
            this.tssStatus.Text = "Idle";
            // 
            // wbFeedDisplay
            // 
            this.wbFeedDisplay.AllowWebBrowserDrop = false;
            this.wbFeedDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbFeedDisplay.IsWebBrowserContextMenuEnabled = false;
            this.wbFeedDisplay.Location = new System.Drawing.Point(0, 24);
            this.wbFeedDisplay.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbFeedDisplay.Name = "wbFeedDisplay";
            this.wbFeedDisplay.Size = new System.Drawing.Size(297, 430);
            this.wbFeedDisplay.TabIndex = 2;
            // 
            // timerCheckUpdate
            // 
            this.timerCheckUpdate.Interval = 5000;
            this.timerCheckUpdate.Tick += new System.EventHandler(this.timerCheckUpdate_Tick);
            // 
            // FoeReader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(297, 476);
            this.Controls.Add(this.wbFeedDisplay);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStripMain);
            this.MainMenuStrip = this.menuStripMain;
            this.MinimumSize = new System.Drawing.Size(250, 38);
            this.Name = "FoeReader";
            this.Text = "Foe";
            this.Shown += new System.EventHandler(this.FoeReader_Shown);
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStripMenuItem miSubscribe;
        private System.Windows.Forms.ToolStripMenuItem miSettings;
        private System.Windows.Forms.ToolStripMenuItem miHelp;
        private System.Windows.Forms.ToolStripMenuItem miAboutFoe;
        private System.Windows.Forms.ToolStripMenuItem miEmailAccount;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tssStatus;
        private System.Windows.Forms.WebBrowser wbFeedDisplay;
        private System.Windows.Forms.Timer timerCheckUpdate;
        private System.Windows.Forms.ToolStripMenuItem checkNewsUpdatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendNewsRequestToolStripMenuItem;

    }
}