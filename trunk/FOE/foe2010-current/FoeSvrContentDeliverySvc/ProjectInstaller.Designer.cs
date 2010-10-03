namespace FoeSvrContentDeliverySvc
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
            this.FoeSvrContentDeliverySvcServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.FoeSvrContentDeliverySvcServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // FoeSvrContentDeliverySvcServiceProcessInstaller
            // 
            this.FoeSvrContentDeliverySvcServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.FoeSvrContentDeliverySvcServiceProcessInstaller.Password = null;
            this.FoeSvrContentDeliverySvcServiceProcessInstaller.Username = null;
            // 
            // FoeSvrContentDeliverySvcServiceInstaller
            // 
            this.FoeSvrContentDeliverySvcServiceInstaller.Description = "Responsible for responding to users\' content requests. Delivering content to user" +
                "s and add users to the AutoSubscription list.";
            this.FoeSvrContentDeliverySvcServiceInstaller.ServiceName = "Foe Server Content Delivery Service";
            this.FoeSvrContentDeliverySvcServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.FoeSvrContentDeliverySvcServiceProcessInstaller,
            this.FoeSvrContentDeliverySvcServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller FoeSvrContentDeliverySvcServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller FoeSvrContentDeliverySvcServiceInstaller;
    }
}