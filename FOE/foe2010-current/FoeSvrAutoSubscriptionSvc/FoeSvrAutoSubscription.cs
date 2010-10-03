using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace FoeSvrAutoSubscriptionSvc
{
    static class FoeSvrAutoSubscription
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new FoeSvrAutoSubscriptionSvc() 
			};
            ServiceBase.Run(ServicesToRun);
        }
    }
}
