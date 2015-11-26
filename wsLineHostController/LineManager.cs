using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.ServiceModel;
using System.Net;
using AppLog;

namespace wsLineHostController
{

    public class lmEventArgs : EventArgs
    {
        public  int Counter;
    }

    public class LineManager : Object
    {
        private int lineId;
        private ServiceReference2.AssembLineClient myLineClient;
        public LineService.AssembLine myLine0;
        private ServiceHost LineServiceHost;
        //private List<ServiceHost> LineServiceHosts = new List<ServiceHost>();
        private FormTimer myFormTimer;
        private string textTime = "0";
        private bool isRunning = false;
        private bool isServiceRunning = false;
        public string sURL = "http://localhost/";
        private bool opcMode = true;
        private LogProvider myLog = new LogProvider(LogType.File, "andon_log.txt", false);

        private void timerHandler(object sender, EventArgs e)
        {
            textTime = myLine0.GetCounter().ToString();
            lmEventArgs myLineEvent = new lmEventArgs();
            myLineEvent.Counter = Convert.ToInt32(textTime);
            this.TaktTick(this, myLineEvent);
        }

        private string getHostIP() {
            string currIP4 = "";
            string currHostName = Dns.GetHostName();
            //Console.WriteLine("Curr host name:" + currHostName);

            IPHostEntry ipEntry = Dns.GetHostEntry(currHostName);
            IPAddress[] addrs = ipEntry.AddressList;
            foreach (IPAddress currIPAddress in addrs)
            {
                Byte[] bytes = currIPAddress.GetAddressBytes();
                if (bytes.Count() == 4)
                {
                    //Console.Write("Current Host IPV4 Address: ");
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        currIP4 = currIP4 + bytes[i].ToString();
                        if (i < 3) 
                        {
                            currIP4 = currIP4 + ".";
                        }
                        //Console.Write(bytes[i] + " ");
                    }
                    //Console.WriteLine();
                    break;
                }
                else
                {
                    //Console.Write("Current Host IPV6 Address: ");
                }
            }

            return currIP4;
        }

        private string getHostName()
        {
            string currHostName = Dns.GetHostName();
            return currHostName;        
        }

        private void SetupLine()
        {
            //for (int i = 1; i < 4; i++) {
            //    LineService.LineStation myStation = new LineService.LineStation(i.ToString());
            //    myLine0.SubmitStation(myStation);
            //}
            myLine0.SetOPCMode(true, Properties.Settings.Default.OPCServer);
            //myLog.LogAlert(AppLog.AlertType.System, this.GetType().ToString(), "OPC mode is 'ON'");            
        }

        public LineManager(int LineId)
        {
            this.lineId = LineId;
            this.myFormTimer = new FormTimer();
            this.myFormTimer.CounterTick += new EventHandler(timerHandler);
            this.sURL = "http://" + this.getHostName() + ":8000/LineService" + this.lineId.ToString();
            myLog.LogAlert(AppLog.AlertType.System, this.GetType().ToString(), "Uri = " + this.sURL);  
            myLine0 = new LineService.AssembLine();
            myLine0.Init(LineId);
        }

        public bool IsRunning { get{return this.isRunning;}}
        public bool IsServiceRunning { get { return this.isServiceRunning; } }
        public delegate void LineEventHandler(object sender, lmEventArgs le);
        public event LineEventHandler TaktTick;
        public string TestCounter { get { return this.textTime; } }


        public void RunService()
        {
            myLog.LogAlert(AppLog.AlertType.System, this.GetType().ToString(), "AssemblyLine Service starting : " + this.sURL);            
            // Runs assembly line
            this.isServiceRunning = true;
            //this.LineServiceHost = new ServiceHost(typeof(LineService.AssembLine), new Uri(sURL));
            LineServiceHost = new ServiceHost(myLine0, new Uri(this.sURL));
            LineServiceHost.Open();
            this.SetupLine();
       }
        public void BreakService()
        {
            // Stops assembly line
            LineServiceHost.Close();
            isServiceRunning = false;
        }
        public void Run()
        {
            // Runs assembly line
            isRunning = true;
            myLine0.Execute();
            //myFormTimer.TimerStart();
        }
        public void Break()
        {
            // Stops assembly line
            this.myFormTimer.TimerStop();
            myLine0.Terminate();
            this.isRunning = false;
        }
        public int PushButton(int StationIndex, string ControlKey)
        {
            return myLine0.PushStationButton(StationIndex, ControlKey);
        }

        public bool OPCMode
        {
            get { return this.opcMode;}
            set 
            {
                this.myLine0.SetOPCMode(value, Properties.Settings.Default.OPCServer);
                this.opcMode = value;
            }
        }
        public void Move()
        {
            this.myLine0.Move();
        }

    }
}
