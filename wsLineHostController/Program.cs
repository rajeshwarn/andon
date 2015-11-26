using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace wsLineHostController
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            int lineId = Convert.ToInt32(Properties.Settings.Default.LineId);

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new LineHostController(lineId)
			};
            ServiceBase.Run(ServicesToRun);
        }
    }
}
