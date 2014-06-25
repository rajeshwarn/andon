using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using LogisticService;
using System.Reflection;

namespace netTcpLogisticApplication
{
    class Program
    {
        static void Main(string[] args)
        {

            ServiceHost serviceHost = null;
            LogisticService.Logistic myLogistic = null;

            Uri serviceNetTcpAddress = new Uri(Properties.Settings.Default.serviceNetTcpAddress);
            Uri collectorServiceNetTcpAddress = new Uri("net.tcp://localhost:10000/Logistic");

            try
            {
                myLogistic = new LogisticService.Logistic();

                Assembly exeAssembly = Assembly.GetEntryAssembly();
                AssemblyName exeName = exeAssembly.GetName();
                Version exeVersion = exeName.Version;
                string fullVersion = exeVersion.ToString(4);
                string appName = exeName.Name;

                Console.WriteLine("Atria Ltd. (c) 2011-2013");
                Console.WriteLine("Andon " + appName + " v." + fullVersion);
                Console.WriteLine("Service " + "Logistic ##");
                Console.WriteLine("--starting on " + serviceNetTcpAddress.ToString() + " ...");
                
                serviceHost = new ServiceHost(myLogistic, serviceNetTcpAddress);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                serviceHost.Abort();
            }


            try
            {
                Console.ReadLine();
                serviceHost.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                serviceHost.Abort();
            }

        }
    }
}
