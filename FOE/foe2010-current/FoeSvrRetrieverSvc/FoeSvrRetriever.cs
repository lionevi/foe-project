using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace Foe.Server.FoeSvrRetrieverSvc
{
    static class FoeSvrRetriever
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new FoeSvrRetrieverSvc() 
			};
            ServiceBase.Run(ServicesToRun);
        }
    }
}
