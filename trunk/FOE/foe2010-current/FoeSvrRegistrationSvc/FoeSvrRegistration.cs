using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace FoeSvrRegistrationSvc
{
    static class FoeSvrRegistration
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new FoeSvrRegistrationSvc() 
			};
            ServiceBase.Run(ServicesToRun);
        }
    }
}
