namespace FoeSvrRssUpdaterSvc
{
    partial class ProjectInstaller
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.FoeSvrRssUpdaterSvcServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.FoeSvrRssUpdaterSvcServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // FoeSvrRssUpdaterSvcServiceProcessInstaller
            // 
            this.FoeSvrRssUpdaterSvcServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.FoeSvrRssUpdaterSvcServiceProcessInstaller.Password = null;
            this.FoeSvrRssUpdaterSvcServiceProcessInstaller.Username = null;
            // 
            // FoeSvrRssUpdaterSvcServiceInstaller
            // 
            this.FoeSvrRssUpdaterSvcServiceInstaller.Description = "Responsible for updating RSS feeds and store a copy of each feed in database.";
            this.FoeSvrRssUpdaterSvcServiceInstaller.ServiceName = "Foe Server RSS Updater Service";
            this.FoeSvrRssUpdaterSvcServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.FoeSvrRssUpdaterSvcServiceProcessInstaller,
            this.FoeSvrRssUpdaterSvcServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller FoeSvrRssUpdaterSvcServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller FoeSvrRssUpdaterSvcServiceInstaller;
    }
}