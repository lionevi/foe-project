namespace FSRetrieverSvc
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
            this.FoeSvrRetriverSvcProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.FoeSvrRetrieverSvcServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // FoeSvrRetriverSvcProcessInstaller
            // 
            this.FoeSvrRetriverSvcProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.FoeSvrRetriverSvcProcessInstaller.Password = null;
            this.FoeSvrRetriverSvcProcessInstaller.Username = null;
            // 
            // FoeSvrRetrieverSvcServiceInstaller
            // 
            this.FoeSvrRetrieverSvcServiceInstaller.Description = "Responsible for download email messages and categorize requests.";
            this.FoeSvrRetrieverSvcServiceInstaller.ServiceName = "Foe Server Retriever Service";
            this.FoeSvrRetrieverSvcServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.FoeSvrRetriverSvcProcessInstaller,
            this.FoeSvrRetrieverSvcServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller FoeSvrRetriverSvcProcessInstaller;
        private System.ServiceProcess.ServiceInstaller FoeSvrRetrieverSvcServiceInstaller;
    }
}