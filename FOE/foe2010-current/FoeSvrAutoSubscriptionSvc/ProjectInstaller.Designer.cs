namespace FoeSvrAutoSubscriptionSvc
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
            this.FoeSvrAutoSubscriptionSvcServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.FoeSvrAutoSubscriptionSvcServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // FoeSvrAutoSubscriptionSvcServiceProcessInstaller
            // 
            this.FoeSvrAutoSubscriptionSvcServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.FoeSvrAutoSubscriptionSvcServiceProcessInstaller.Password = null;
            this.FoeSvrAutoSubscriptionSvcServiceProcessInstaller.Username = null;
            // 
            // FoeSvrAutoSubscriptionSvcServiceInstaller
            // 
            this.FoeSvrAutoSubscriptionSvcServiceInstaller.Description = "Responsible for delivery updated RSS feeds to active users.";
            this.FoeSvrAutoSubscriptionSvcServiceInstaller.ServiceName = "Foe Server Auto Subscription Service";
            this.FoeSvrAutoSubscriptionSvcServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.FoeSvrAutoSubscriptionSvcServiceProcessInstaller,
            this.FoeSvrAutoSubscriptionSvcServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller FoeSvrAutoSubscriptionSvcServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller FoeSvrAutoSubscriptionSvcServiceInstaller;
    }
}