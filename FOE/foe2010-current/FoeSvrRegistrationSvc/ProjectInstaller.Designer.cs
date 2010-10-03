namespace FoeSvrRegistrationSvc
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
            this.FoeSvrRegistrationSvcServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.FoeSvrRegistrationSvcServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // FoeSvrRegistrationSvcServiceProcessInstaller
            // 
            this.FoeSvrRegistrationSvcServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.FoeSvrRegistrationSvcServiceProcessInstaller.Password = null;
            this.FoeSvrRegistrationSvcServiceProcessInstaller.Username = null;
            // 
            // FoeSvrRegistrationSvcServiceInstaller
            // 
            this.FoeSvrRegistrationSvcServiceInstaller.Description = "Responsible for registering new users and send User ID and Processor Email to the" +
                " new user.";
            this.FoeSvrRegistrationSvcServiceInstaller.ServiceName = "Foe Server Registration Service";
            this.FoeSvrRegistrationSvcServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.FoeSvrRegistrationSvcServiceProcessInstaller,
            this.FoeSvrRegistrationSvcServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller FoeSvrRegistrationSvcServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller FoeSvrRegistrationSvcServiceInstaller;
    }
}