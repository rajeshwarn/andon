using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using LineService;
//using netTcpLibrary;
using System.Reflection;

namespace netTcpApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost serviceHost = null;
            LineService.AssembLine myLine0 = null;
            
            string opcServerName = Properties.Settings.Default.OPCServer;
            int LineId = Properties.Settings.Default.LineId;
            Uri serviceNetTcpAddress = new Uri(Properties.Settings.Default.serviceNetTcpAddress); 

            try
            {
                myLine0 = new LineService.AssembLine();
                myLine0.Init(LineId);

                Assembly exeAssembly = Assembly.GetEntryAssembly();
                AssemblyName exeName = exeAssembly.GetName();
                Version exeVersion = exeName.Version;
                string fullVersion = exeVersion.ToString(4);
                string appName = exeName.Name;

                AssemblyName[] libs = exeAssembly.GetReferencedAssemblies();
                //string libName = lib.Name;
                //string libVersion = lib.Assembly.GetName().Version.ToString(4);

                Console.WriteLine("Atria Ltd. (c) 2011-2013");
                Console.WriteLine("Andon " + appName + " v." + fullVersion);
                foreach (AssemblyName lib in libs) 
                {
                    Console.WriteLine(" - " + lib.Name + " v." + lib.Version.ToString(4));
                }
                
                //
                Console.WriteLine("Service " + "Assembly line ##" + LineId.ToString());
                Console.WriteLine("--starting on " + serviceNetTcpAddress.ToString() + " ...");

              

                serviceHost = new ServiceHost(myLine0, serviceNetTcpAddress);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                serviceHost.Abort();
            }


            try
            {
                serviceHost.Open();
                myLine0.SetOPCMode(true, opcServerName);

                Console.WriteLine("Try to restore takt counter, Takt = " + myLine0.GetCounter().ToString() + " sec");
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
