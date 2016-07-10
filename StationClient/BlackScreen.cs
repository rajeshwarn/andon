using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AppLog;
using System.Collections;
using System.IO.Compression;
using AppLog;

using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using Newtonsoft.Json;

namespace StationClient
{
    public enum BSFlag { 
        Finished    = 0x0001, 
        Late        = 0x0100, 
        Blocked     = 0x0010, 
        Free        = 0x0200, 
        LiveLate    = 0x0400, 
        SumLate     = 0x0800, 
        Outgo       = 0x0020 
    }

    public partial class BlackScreen : Form
    {
        private LogProvider myLog = new LogProvider(LogType.File, "station_log.txt", true);
        private int stationIndex;
        private int lineId;
        private Size formSize;
        private ServiceReference2.AssembLineClient myLineClient;
        private ServiceReference2.Frame frame = new ServiceReference2.Frame();
        private FormTimer myFormTimer;

        private ServiceReference2.StationRealtimeData[] stationData;

        private int stopTimeCounter = 0;
        private bool connected = false;

        private MediaClient.MediaPlayer mplayer;

        public BlackScreen()
        {
            try
            {
                InitializeComponent();
                this.myFormTimer = new FormTimer();
                this.myFormTimer.CounterTick += new myEventHandler(timerHandler);
                this.stationIndex = Convert.ToInt32(Properties.Settings.Default.StationIndex);
                this.fLogMessage("Connect to Station #" + this.stationIndex.ToString() + " ...");
                this.laStation.Text = "Station ID: " + stationIndex.ToString();

                this.laFinishBtnValue.TextChanged += new EventHandler(finishButtonPressed_handler);
                this.laStopBtnValue.TextChanged += new EventHandler(stopButtonPressed_handler);
                this.laHelpBtnValue.TextChanged += new EventHandler(helpButtonPressed_handler);
                this.laBitState.TextChanged += new EventHandler(laBitState_TextChanged);

                this.formSize = this.Size;

                this.timerStop.Tick += new EventHandler(timerStop_Tick);
                this.myLineClient = new ServiceReference2.AssembLineClient();
                this.connected = true;
                this.laBitState.Text = "0";

                mplayer = new MediaClient.MediaPlayer(this);
                this.lineId = Properties.Settings.Default.LineId;
                
                initTimers();
                imqttInit();
                instruction = new ClientInstruction();
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AppLog.AlertType.Error, "Client", ex.Message);
            }
        }

        private void initTimers()
        {
            timers = new Dictionary<string, int>();
            timers.Add("LIVE", 0);
            timers.Add("TIMER_SumLate", 0);
            timers.Add("TIMER_STOPLAST", 0);
            timers.Add("TIMER_STOP", 0);
            timers.Add("T", 0);
            timers.Add("TIMER_HELP", 0);
        }

        public BlackScreen(int stationIndex, string endpoint_confname, string endpoint_address) 
        {
            try
            {
                InitializeComponent();
                this.Load -= this.BlackScreen_Load;
                this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlackScreen_MouseDown);
                this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.BlackScreen_MouseMove);

                this.myFormTimer = new FormTimer();
                this.myFormTimer.CounterTick += new myEventHandler(timerHandler);
                this.stationIndex = stationIndex; //Convert.ToInt32(Properties.Settings.Default.StationIndex);
                this.fLogMessage("Connect to Station #" + this.stationIndex.ToString() + " ...");
                this.laStation.Text = "Station ID: " + stationIndex.ToString();

                this.laFinishBtnValue.TextChanged += new EventHandler(finishButtonPressed_handler);
                this.laStopBtnValue.TextChanged += new EventHandler(stopButtonPressed_handler);
                this.laHelpBtnValue.TextChanged += new EventHandler(helpButtonPressed_handler);
                this.laBitState.TextChanged += new EventHandler(laBitState_TextChanged);

                this.timerStop.Tick += new EventHandler(timerStop_Tick);
                this.myLineClient = new ServiceReference2.AssembLineClient(endpoint_confname, endpoint_address);
                this.connected = true;
                this.laBitState.Text = "0";

                mplayer = new MediaClient.MediaPlayer(this);
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AppLog.AlertType.Error, "Client", ex.Message);
            }
        }

        private void fLogMessage(string message) 
        {
            //this.tbxStationLog.Text = this.tbxStationLog.Text + message + " ";
        }

        private string formatCounter(int counter) 
        { 
            // format int counter to "#:#0:00" mask

            string result = "NA";
            string negative_indicator = "";

            int seconds = counter;
            if (seconds < 0)
            {
                seconds = -seconds;
                negative_indicator = "-";
            }

            int minutes = Convert.ToInt32(seconds / 60);
            int hours = Convert.ToInt32(minutes / 60);

            seconds = seconds - minutes * 60;
            minutes = minutes - hours * 60;
            string stSesonds = "0" + seconds.ToString();
            string stMinutes;
            string stHours;

            if (hours > 0)
            {
                stMinutes = "0" + minutes.ToString() + ":";
                stMinutes = stMinutes.Substring(stMinutes.Length - 3);
                stHours = hours.ToString() + ":";
            }
            else
            {
                stMinutes = minutes.ToString() + ":";
                stHours = "";
            }

            result = negative_indicator + stHours + stMinutes + stSesonds.Substring(stSesonds.Length - 2);
            return result;
        }

        // Connect if not connected
        private bool checkConnection() 
        {
            if (!this.connected) {
                this.myLineClient = new ServiceReference2.AssembLineClient();
                this.connected = (this.myLineClient != null);
            }
            return this.connected;
        }

        // What to show: takt, video, pic?
        private int checkViewMode() {
            int result = 0;
            ServiceReference2.ClientInstruction instruction = myLineClient.GetClientInstruction();
            switch (instruction.Mode) {
                case 1:
                    mplayer.PlayVideo("http://localhost:8080/shark.flv");
                    break;
                case 2:
                    mplayer.ShowPicture("http://localhost:8080/pic.jpg");
                    break;
                default:
                    mplayer.ResetView();
                    break;
            }
            result = instruction.Mode;
            return result;
        }

        private int checkViewMode(ClientInstruction instruction)
        {
            int result = 0;
            switch (instruction.Mode) {
                case 1:
                    mplayer.PlayVideo(instruction.ContentUrl);
                    // http://localhost:8080/shark.flv
                    break;
                case 2:
                    mplayer.ShowPicture(instruction.ContentUrl);
                    // http://localhost:8080/pic.jpg
                    break;
                default:
                    mplayer.ResetView();
                    break;
            }
            result = instruction.Mode;
            return result;
        }

        private int calcCounter()
        {
            int counter = 0;
            int taktCounter = timers["T"];

            int codeWord = this.getButtonsWord();
            if ((codeWord & 0x0008) == 0x0008) { //stop     
                counter = timers["TIMER_STOPLAST"];
            }
            else if ((((BSFlag)codeWord & BSFlag.Late) == BSFlag.Late) &
                     (((BSFlag)codeWord & BSFlag.LiveLate) == BSFlag.LiveLate) &
                     (((BSFlag)codeWord & BSFlag.Finished) == 0)) {
                counter = timers["LIVE"];
                int tmp = timers["TIMER_SumLate"];
                counter = -tmp;
            }
            else if (((((BSFlag)codeWord & BSFlag.Late) == BSFlag.Late) &
                      (((BSFlag)codeWord & BSFlag.LiveLate) == BSFlag.LiveLate) &
                      (((BSFlag)codeWord & BSFlag.Finished) == BSFlag.Finished)) |
                     ((((BSFlag)codeWord & (BSFlag.Finished | BSFlag.LiveLate)) == (BSFlag.Finished | BSFlag.LiveLate)) & 
                      (((BSFlag)codeWord & ~BSFlag.Late & ~BSFlag.Outgo) == (BSFlag)codeWord))) {
                counter = -timers["TIMER_STOP"];
            }
            else if (((BSFlag)codeWord & BSFlag.Late) == BSFlag.Late) {
                counter = taktCounter;
            }
            else {
                counter = taktCounter;
            }
            return counter;
        }

        private void timerHandler()
        {
            // UI thread

            checkViewMode(instruction);
            // still pocess in background
            // what to show: takt, video, pic? 

        }

        private void timerHandler2()
        {
            this.laTime.Text = DateTime.Now.ToLongTimeString().ToString();
            try
            {
                if (!this.checkConnection()) return;  // connect if disconnected  
                if (this.checkViewMode() != 0) return; // what to show: takt, video, pic?

                this.stationData = myLineClient.ReadRealTimeDataForLine(this.stationIndex);

                ///refactor 3.0.10 - bbegin
                timers["LIVE"] = Convert.ToInt32(stationData.FirstOrDefault(p => p.Key.Equals("LIVE")).Value);
                timers["T"] = Convert.ToInt32(stationData.First(p => p.Key.Equals("T")).Value);
                timers["TIMER_STOP"] = Convert.ToInt32(stationData.First(p => p.Key.Equals("TIMER_STOP")).Value);
                timers["TIMER_HELP"] = Convert.ToInt32(stationData.First(p => p.Key.Equals("TIMER_HELP")).Value);
                timers["TIMER_STOPLAST"] = Convert.ToInt32(stationData.First(p => p.Key.Equals("TIMER_STOPLAST")).Value);
                timers["TIMER_SumLate"] = Convert.ToInt32(stationData.FirstOrDefault(p => p.Key.Equals("TIMER_SumLate")).Value);
                ///refactor 3.0.10 - end

                //done
                this.laMem.Text = formatCounter(this.calcCounter());
                //done
                this.laTimeLeft.Text = formatCounter(timers["T"]);
                //done
                this.laLive.Text = formatCounter(timers["LIVE"]);
                
                //done
                this.laStation.Text = "Station ID: " + stationData.First(p => p.Key.Equals("S")).Value;
                //done
                this.laProduct.Text = "Batch: " + stationData.First(p => p.Key.Equals("B")).Value;
                //done
                this.laFrame.Text = stationData.First(p => p.Key.Equals("F")).Value;

                //done
                this.laSumstopValue.Text = formatCounter(timers["TIMER_STOP"]);
                //done
                this.laHelpTimeValue.Text = formatCounter(timers["TIMER_HELP"]);

                ServiceReference2.StationRealtimeData tmpObj;
                int dayPlan;
                int dayFact;
                tmpObj = stationData.FirstOrDefault(p => p.Key.Equals("REGP.D"));
                dayPlan = (tmpObj != null) ? Convert.ToInt32(tmpObj.Value) : 99;

                tmpObj = stationData.FirstOrDefault(p => p.Key.Equals("REGF.D"));
                dayFact = (tmpObj != null) ? Convert.ToInt32(tmpObj.Value) : 99;
                //done
                this.laDayPlan.Text = "DPF: " + dayPlan.ToString() + "/" + dayFact.ToString();

                int monthPlan;
                int monthFact;
                tmpObj = stationData.FirstOrDefault(p => p.Key.Equals("REGP.M"));
                monthPlan = (tmpObj != null) ? Convert.ToInt32(tmpObj.Value) : 99;

                tmpObj = stationData.FirstOrDefault(p => p.Key.Equals("REGF.M"));
                monthFact = (tmpObj != null) ? Convert.ToInt32(tmpObj.Value) : 99;
                //done
                this.laMonthPlan.Text = "MPF: " + monthPlan.ToString() + "/" + monthFact.ToString();
                //done
                this.laPlanValue.Text = (dayFact - dayPlan).ToString();          // plan fact difference
                //done
                this.laMonthplanValue.Text = (monthFact - monthPlan).ToString(); // plan fact difference
                //done
                this.laMessage.Text = "";                                        // empty string if connected

                //done
                this.laFinishBtnValue.Text = stationData.First(p => p.Key.Equals("FINISH")).Value.ToString();
                //done
                this.laStopBtnValue.Text = stationData.First(p => p.Key.Equals("STOP")).Value.ToString();
                //done
                this.laHelpBtnValue.Text = stationData.First(p => p.Key.Equals("HELP")).Value.ToString();

                int newBitWord = Convert.ToInt32(stationData.FirstOrDefault(p => p.Key.Equals("BST")).Value.ToString());
                newBitWord = newBitWord & (int)~BSFlag.Blocked; // useless bit "Blocked"
                //done
                this.laBitState.Text = newBitWord.ToString();
                //done
                this.laBitState2.Text = "BS: " + newBitWord.ToString("X");
            }
            catch (System.ServiceModel.EndpointNotFoundException e) {
                this.connected = false;
                this.myLineClient.Abort();
                this.myLog.LogAlert(AlertType.System, this.GetType().ToString(), e.Message.ToString());
                this.laMessage.Text = "Connecting " + new String('.', (this.myFormTimer.Counter % 4));
                this.myLog.LogAlert(AppLog.AlertType.Error, "Client", e.Message);
            }
            catch (System.TimeoutException e) {
                this.connected = false;
                this.myLineClient.Abort();
                this.myLog.LogAlert(AlertType.System, this.GetType().ToString(), e.Message.ToString());
                this.laMessage.Text = "Connecting " + new String('.', (this.myFormTimer.Counter % 4));
                this.myLog.LogAlert(AppLog.AlertType.Error, "Client", e.Message);
            }
            catch (Exception e1) {
                this.connected = false;
                if (this.myLineClient != null) {
                    this.myLineClient.Abort();
                }
                this.myLog.LogAlert(AlertType.System, this.GetType().ToString(), e1.Message.ToString());
                this.myLog.LogAlert(AppLog.AlertType.Error, "Client", e1.Message);
            }

        }

        private Color getButtonColor(string state, int button_type) 
        {
            Color result = Color.Gray;
            this.laMem.ForeColor = Color.White;
            this.laProduct.ForeColor = Color.White;
            this.laStation.ForeColor = Color.White;
            //this.BackColor = Color.Black;
            this.label1.ForeColor = Color.White;
            this.label2.ForeColor = Color.White;
            this.label3.ForeColor = Color.White;
            if (state == "On") 
            {
                switch (button_type)
                {
                    case 1:
                        result = Color.LightGreen;
                        this.laMem.ForeColor = Color.LightGreen;
                        this.laProduct.ForeColor = Color.White;
                        this.laStation.ForeColor = Color.White;
                        //this.BackColor = Color.Black;
                        this.label1.ForeColor = Color.White;
                        this.label2.ForeColor = Color.White;
                        this.label3.ForeColor = Color.White;
                        break;
                    case 2:
                        result = Color.Red;
                        this.laMem.ForeColor = Color.Black;
                        this.laProduct.ForeColor = Color.Black;
                        this.laStation.ForeColor = Color.Black;
                        this.label1.ForeColor = Color.Black;
                        this.label2.ForeColor = Color.Black;
                        this.label3.ForeColor = Color.Black;
                        //this.BackColor = Color.Red;
                        break;
                    case 3:
                        result = Color.Yellow;
                        this.laMem.ForeColor = Color.Yellow;
                        //this.laProduct.ForeColor = Color.Black;
                        //this.laStation.ForeColor = Color.Black;
                        //this.label1.ForeColor = Color.Black;
                        //this.label2.ForeColor = Color.Black;
                        //this.label3.ForeColor = Color.Black;
                        //this.BackColor = Color.Yellow;
                        break;
                    default:
                        result = Color.White;
                        this.laMem.ForeColor = Color.White;
                        this.laProduct.ForeColor = Color.White;
                        this.laStation.ForeColor = Color.White;
                        //this.BackColor = Color.Black;
                        this.label1.ForeColor = Color.White;
                        this.label2.ForeColor = Color.White;
                        this.label3.ForeColor = Color.White;
                        break;
                }
            }
            return result;
        }

        private void finishButtonPressed_handler(object sender, EventArgs e) 
        {
            this.paBlack.BackColor = this.applyColorSchema(this.getButtonsWord());
            if (((Label)sender).Text == "1")
            {
                this.laFinish.ForeColor = Color.LightGreen;
            }
            else
            {
                this.laFinish.ForeColor = Color.DarkGray;
            }
        }

        private void stopButtonPressed_handler(object sender, EventArgs e)
        {
            this.paBlack.BackColor = this.applyColorSchema(this.getButtonsWord());
            
            if (((Label)sender).Text == "1") {
                this.laStop.ForeColor = Color.Red;
                this.stopTimeCounter = 0;
                this.timerStop.Start();
            }
            else {
                this.laStop.ForeColor = Color.DarkGray;
                this.timerStop.Stop();
            }
        }

        private void helpButtonPressed_handler(object sender, EventArgs e)
        {
            this.paBlack.BackColor = this.applyColorSchema(this.getButtonsWord());
            
            if (((Label)sender).Text == "1") {
                this.laHelp.ForeColor = Color.Yellow;
            }
            else {
                this.laHelp.ForeColor = Color.DarkGray;
            }
        }

        private void laBitState_TextChanged(object sender, EventArgs e)
        {
            // 0000 0001 0000 - 8..5th bit 
            int codeword = this.getButtonsWord();

            this.paBlack.BackColor = this.applyColorSchema(codeword);
        }

        private void laTime_DoubleClick(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        Point last;

        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) {
                last = MousePosition;
            }
        }

        private void Form_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) {
                Point cur = MousePosition;
                int dx = cur.X - last.X;
                int dy = cur.Y - last.Y;
                Point loc = new Point(Location.X + dx, Location.Y + dy);
                Location = loc;
                last = cur;
            }
        }

        private void BlackScreen_MouseDown(object sender, MouseEventArgs e)
        {
            this.Form_MouseDown(sender, e);
        }

        private void BlackScreen_MouseMove(object sender, MouseEventArgs e)
        {
            this.Form_MouseMove(sender, e);
        }

        private int getButtonsWord() 
        { 
            int intWord = 
                (Convert.ToInt32(this.laBitState.Text) & 0xFFF0) +
                + Convert.ToInt32(this.laStopBtnValue.Text)* 8
                + Convert.ToInt32(this.laHelpBtnValue.Text) * 4
                + Convert.ToInt32(this.frame.Type.ToString()) * 2
                + Convert.ToInt32(this.laFinishBtnValue.Text) * 1;
            return intWord;
        }

        private Color applyColorSchema(int codeWord) 
        {
            Color result = Color.Black;
            BSFlag redState = (BSFlag.Late | BSFlag.LiveLate);
            BSFlag greenState = (BSFlag.Finished | BSFlag.Outgo);

            if (((BSFlag)codeWord & BSFlag.Free) == BSFlag.Free) {
                result = Color.DimGray; // free and do nothing
            }
            else if ((codeWord & 0x0008) == 0x0008) {
                result = Color.DarkRed; // stop
            }
            else if ((((BSFlag)codeWord & redState) == redState) | 
                     ((((BSFlag)codeWord & (BSFlag.Finished | BSFlag.LiveLate)) == (BSFlag.Finished | BSFlag.LiveLate)) & 
                      (((BSFlag)codeWord & ~BSFlag.Late & ~BSFlag.Outgo) == (BSFlag)codeWord))) {
                result = Color.Red;
            }
            else if (((((BSFlag)codeWord & ~BSFlag.Finished & ~BSFlag.Late) == (BSFlag)codeWord) & 
                      (((BSFlag)codeWord & BSFlag.LiveLate) == BSFlag.LiveLate)) | 
                     ((((BSFlag)codeWord & (BSFlag.Finished | BSFlag.Late)) == (BSFlag.Finished | BSFlag.Late)) & 
                      (((BSFlag)codeWord & ~BSFlag.LiveLate) == (BSFlag)codeWord)) | 
                     ((((BSFlag)codeWord & BSFlag.Finished) == BSFlag.Finished) & 
                      (((BSFlag)codeWord & ~BSFlag.Late & ~BSFlag.Outgo & ~BSFlag.LiveLate) == (BSFlag)codeWord))) {
                result = Color.Orange;
            }
            else if ((codeWord & 4) == 4) {
                result = Color.Goldenrod; // help
            }
            else if ((codeWord & 2) == 2) {
                // this.paBlack.BackColor = Color.Black;                 // non-working frame
            }
            else if (((BSFlag)codeWord & greenState) == greenState) {
                result = Color.ForestGreen; // finish
            }
            else {
                result = Color.Black; // default
            }
            return result;
        }

        private void BlackScreen_Load(object sender, EventArgs e)
        {
            this.Location = new Point(0, 0);
            Size size = new Size(1600, 900);
            this.Size = size;

            if (this.formSize.Width != 0 && this.formSize.Height != 0)  {
                float kx = 1.667F; //(int)((this.Size.Width / this.formSize.Width) * 1000) / 1000;
                float ky = 1.667F; //(int)((this.Size.Height / this.formSize.Height) * 1000) / 1000;
                resizeControls(this, kx, ky);
                this.formSize = this.Size;
            }
        }

        private void timerStop_Tick(object sender, EventArgs e) 
        {
            this.stopTimeCounter++;
        }

        private void BlackScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.myFormTimer.TimerDestroy();
            
            if (client != null && client.IsConnected) 
                client.Disconnect();

            if (clientTi != null && clientTi.IsConnected)
                clientTi.Disconnect();

            mqttLogger.Dispose();
        }

        private void resizeControls(object owner, float kx, float ky)
        {
            if (Object.ReferenceEquals(owner.GetType(), typeof(Microsoft.VisualBasic.PowerPacks.ShapeContainer))) {
                foreach (Microsoft.VisualBasic.PowerPacks.Shape ctrl in ((Microsoft.VisualBasic.PowerPacks.ShapeContainer)owner).Shapes)  {
                    ((Microsoft.VisualBasic.PowerPacks.LineShape)ctrl).X1 = (int)(((Microsoft.VisualBasic.PowerPacks.LineShape)ctrl).X1 * kx);
                    ((Microsoft.VisualBasic.PowerPacks.LineShape)ctrl).X2 = (int)(((Microsoft.VisualBasic.PowerPacks.LineShape)ctrl).X2 * kx);
                    ((Microsoft.VisualBasic.PowerPacks.LineShape)ctrl).Y1 = (int)(((Microsoft.VisualBasic.PowerPacks.LineShape)ctrl).Y1 * kx);
                    ((Microsoft.VisualBasic.PowerPacks.LineShape)ctrl).Y2 = (int)(((Microsoft.VisualBasic.PowerPacks.LineShape)ctrl).Y2 * kx);
                }
            }
            else if (Object.ReferenceEquals(owner.GetType(), typeof(Label))) {
                Label lab = (Label)owner;
                int oldX = lab.Location.X;
                int oldY = lab.Location.Y;

                //lab.Location = new Point((int)(oldX * kx), (int)(oldY * ky));
                //lab.Size = new Size((int)(lab.Size.Width * kx), (int)(lab.Size.Height * ky));
                lab.Font = new Font(lab.Font.Name, lab.Font.Size * Math.Min(kx, ky), lab.Font.Style, lab.Font.Unit);
            }
            else {
                foreach (object ctrl in ((Control)owner).Controls) {
                    if (Object.ReferenceEquals(ctrl.GetType(), typeof(Label))) {
                        Label lab = (Label)ctrl;
                        int oldX = lab.Location.X;
                        int oldY = lab.Location.Y;
                        lab.Location = new Point((int)(oldX * kx), (int)(oldY * ky));
                        lab.Size = new Size((int)(lab.Size.Width * kx), (int)(lab.Size.Height * ky));
                        lab.Font = new Font(lab.Font.Name, lab.Font.Size * Math.Min(kx, ky), lab.Font.Style, lab.Font.Unit);
                    }
                    else if (Object.ReferenceEquals(ctrl.GetType(), typeof(DataGridView))) {
                        int oldX = ((Control)ctrl).Location.X;
                        int oldY = ((Control)ctrl).Location.Y;
                        ((Control)ctrl).Location = new Point((int)(oldX * kx), (int)(oldY * ky));
                        ((Control)ctrl).Size = new Size((int)(((Control)ctrl).Size.Width * kx), (int)(((Control)ctrl).Size.Height * ky));

                        DataGridView dgrv = (DataGridView)ctrl;
                        dgrv.Font = new Font(dgrv.Font.Name, dgrv.Font.Size * Math.Min(kx, ky), dgrv.Font.Style, dgrv.Font.Unit);
                        dgrv.ColumnHeadersHeight = (int)(dgrv.ColumnHeadersHeight * ky);
                        dgrv.ColumnHeadersDefaultCellStyle.Font = new Font(dgrv.ColumnHeadersDefaultCellStyle.Font.Name,
                            dgrv.ColumnHeadersDefaultCellStyle.Font.Size * Math.Min(kx, ky),
                            dgrv.ColumnHeadersDefaultCellStyle.Font.Style,
                            dgrv.ColumnHeadersDefaultCellStyle.Font.Unit);
                        dgrv.RowTemplate.Height = (int)(dgrv.RowTemplate.Height * ky);
                        dgrv.DefaultCellStyle.Font = new Font(dgrv.DefaultCellStyle.Font.Name,
                            dgrv.DefaultCellStyle.Font.Size * Math.Min(kx, ky),
                            dgrv.DefaultCellStyle.Font.Style,
                            dgrv.DefaultCellStyle.Font.Unit);

                        foreach (DataGridViewColumn column in dgrv.Columns) {
                            if (column.AutoSizeMode != DataGridViewAutoSizeColumnMode.Fill) {
                                column.Width = (int)(column.Width * kx);
                            }
                        }
                    }   
                    else {
                        int oldX = ((Control)ctrl).Location.X;
                        int oldY = ((Control)ctrl).Location.Y;
                        ((Control)ctrl).Location = new Point((int)(oldX * kx), (int)(oldY * ky));
                        ((Control)ctrl).Size = new Size((int)(((Control)ctrl).Size.Width * kx), (int)(((Control)ctrl).Size.Height * ky));
                        resizeControls(ctrl, kx, ky);
                    }
                }
            }
        }

    }
}
