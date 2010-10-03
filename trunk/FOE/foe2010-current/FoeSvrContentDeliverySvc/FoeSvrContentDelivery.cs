using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace FoeSvrContentDeliverySvc
{
    static class FoeSvrContentDelivery
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new FoeSvrContentDeliverySvc() 
			};
            ServiceBase.Run(ServicesToRun);
        }
    }
}
