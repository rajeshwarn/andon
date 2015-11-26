using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using wsLineHostController;
using AppLog;
using System.ServiceModel;
using PlannerLib;


namespace wsLineHostController
{
    public partial class LineHostController : ServiceBase
    {
        private int lineId;
        private LineManager wsLineManager;
        private Planner planner;
        private System.Timers.Timer plannerTimer = new System.Timers.Timer();
        private LogProvider myLog = new LogProvider(LogType.File, "andon_log.txt", true);

        //private ServiceHost SimLineServiceHost;

        public LineHostController(int lineId)
        {
            InitializeComponent();
            this.lineId = lineId;

            // The name of the service that appears in the Registry


            this.ServiceName = "AssemblyLineService" + this.lineId.ToString();
            // Allow an administrator to stop (and restart) the service
            this.CanStop = true;
            // Report Start and Stop events to the Windows event log
            this.AutoLog = true;
            this.plannerTimer.Interval = 1000 * 60 * 5;  // every 5 minutes
            this.plannerTimer.Elapsed += new System.Timers.ElapsedEventHandler(plannerTimer_Elapsed);
        }

        private void plannerTimer_Elapsed(object sender, EventArgs e) 
        {
            if (DateTime.Today.Hour == 0 & DateTime.Today.Minute < 11)
            {
                this.planner = new Planner(Properties.Settings.Default.LineId, "");
                this.planner.RunSimulation();
  
                if (DateTime.Today.Day == 1)
                {
                   this.planner.FillScheduleFramesByDefault(DateTime.Today);
                   this.planner.MakeProductPlan(PlanMode.Month);
                   //this.myLog.LogAlert(0, this.ServiceName, "Month-Planner has been started.");
                }
                this.planner.MakeProductPlan(PlanMode.Day);
                //this.myLog.LogAlert(0, this.ServiceName, "Day-Planner has been started.");
                
            }
        }

        protected override void OnStart(string[] args)
        {
            int lineId = this.lineId;
            this.plannerTimer.Start();
            this.wsLineManager = new LineManager(lineId);
            if (!wsLineManager.IsServiceRunning)
            {
                this.wsLineManager.RunService();
            }

            this.startSimLine(wsLineManager.myLine0, wsLineManager.sURL+"/sim");
            this.myLog.LogAlert(0, this.ServiceName, "Service and LineManager started.");

        }

        protected override void OnStop()
        {
            this.plannerTimer.Stop();
            if (wsLineManager.IsServiceRunning)
            {
                this.wsLineManager.BreakService();
            }
        }

        private void startSimLine(LineService.AssembLine line, string sURL) 
        {
            //SimLine.SimLine simLine = new SimLine.SimLine();

            // copy objects

            //simLine.Id = line.Id;
            //simLine. 


            //myLog.LogAlert(AppLog.AlertType.System, this.GetType().ToString(), "SimLine Service starting : " + sURL);
            //SimLineServiceHost = new ServiceHost(simLine, new Uri(sURL));
            //SimLineServiceHost.Open();
        }

        
    }
}
