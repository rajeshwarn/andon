using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using AppLog;
using System.Timers;
using LineService;

namespace wsOPCStarter
{
    public partial class OPCStarter : ServiceBase
    {
        private LogProvider log;
        private Timer timer;
        private LineOPCProvider opcProvider;
        private int counter;
        private bool isOPC_Ready;
        private bool isOPC_Connecting;
        private string opcServerName;

        public OPCStarter()
        {
            InitializeComponent();
        }

        private void Init() 
        {
            this.log = new LogProvider(LogType.File, "OPCStarter_log.txt", true);
            this.log.LogAlert(AlertType.System, "checkOPCState()", "OPCStarter starting ....");

            this.timer = new Timer();
            this.timer.Interval = 20000;
            this.timer.Elapsed += new ElapsedEventHandler(timerHandler);
            this.timer.Enabled = true;

            this.opcProvider = new LineOPCProvider(-1, this.log);

            this.isOPC_Ready = false;
            this.isOPC_Connecting = false;
            this.opcServerName = Properties.Settings.Default.OPCServerName; // "Matrikon.OPC.Modbus.1";        
        }

        private void timerHandler(object sender, EventArgs e) 
        {
            this.checkOPCState();
        }

        private void checkOPCState()
        {
            try
            {
                this.counter++;
                string currentOPCState = this.opcProvider.State.ToString();
                this.log.LogAlert(AlertType.System, "checkOPCState()", "Read OPC state = " + currentOPCState);
                this.isOPC_Ready = true;
            }
            catch (Exception ex)
            {
                // "OPC server is anavalable.";
                this.log.LogAlert(AlertType.System, "checkOPCState()", ex.Message);
                this.isOPC_Ready = false;
            }
            finally
            {
                if (!this.isOPC_Ready && !this.isOPC_Connecting)
                {
                    // "Trying to reconnect ...";
                    this.log.LogAlert(AlertType.System, "checkOPCState()", "Trying to reconnect ...");
                    this.isOPC_Connecting = true;
                    this.reconnect();
                }
            }
        }

        public void reconnect()
        {
            try
            {
                //this.opcProvider = new LineOPCProvider(this.log);
                this.opcProvider.OPCConnect(this.opcServerName);
                this.isOPC_Connecting = false;
                this.log.LogAlert(AlertType.System, "Reconnect()", "OPC reconnected OK.");
            }
            catch (System.TimeoutException ex)
            {
                this.log.LogAlert(AlertType.System, "Reconnect()", "Failed to send message to LineService.");
                this.log.LogAlert(AlertType.System, "Reconnect()", ex.Message);
            }
            catch (System.ServiceModel.ActionNotSupportedException ex) 
            {
                this.log.LogAlert(AlertType.System, "Reconnect()", "Failed to send message to LineService.");
                this.log.LogAlert(AlertType.System, "Reconnect()", ex.ToString());
            }
            catch (Exception ex)
            {
                this.log.LogAlert(AlertType.System, "Reconnect()", "Failed to reconnect.");
                this.log.LogAlert(AlertType.System, "Reconnect()", ex.ToString());
                this.log.LogAlert(AlertType.System, "Reconnect()", ex.Message);
            }

        }


        protected override void OnStart(string[] args)
        {
            this.Init();
            //this.timer.Enabled = true;
        }

        protected override void OnStop()
        {
            //this.timer.Enabled = false;
        }
    }
}
