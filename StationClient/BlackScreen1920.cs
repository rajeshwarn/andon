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

namespace StationClient
{
    public partial class BlackScreen1920 : Form
    {
        private int stationIndex;
        private ServiceReference2.AssembLineClient myLineClient;
        private ServiceReference2.Frame frame = new ServiceReference2.Frame();
        private FormTimer myFormTimer;
        private LogProvider myLog = new LogProvider(LogType.File, "station_log.txt", true);
        private int buttonsWord;
        private int stopTimeCounter = 0;
         

        public BlackScreen1920()
        {
            InitializeComponent();
            this.myLineClient = new ServiceReference2.AssembLineClient();
            this.myFormTimer = new FormTimer();
            this.myFormTimer.CounterTick += new myEventHandler(timerHandler);
            this.stationIndex = Convert.ToInt32(Properties.Settings.Default.StationIndex);
            this.fLogMessage("Connect to Station #" + this.stationIndex.ToString() + " ...");
            this.laStation.Text = "Station ID: " + stationIndex.ToString();

            this.laFinishBtnValue.TextChanged += new EventHandler(finishButtonPressed_handler);
            this.laStopBtnValue.TextChanged += new EventHandler(stopButtonPressed_handler);
            this.laHelpBtnValue.TextChanged += new EventHandler(helpButtonPressed_handler);


            this.timerStop.Tick += new EventHandler(timerStop_Tick);
        }

        public BlackScreen1920(int stationIndex, string endpoint_confname, string endpoint_address) 
        {

            InitializeComponent();
            this.myLineClient = new ServiceReference2.AssembLineClient(endpoint_confname, endpoint_address);
            this.myFormTimer = new FormTimer();
            this.myFormTimer.CounterTick += new myEventHandler(timerHandler);
            this.stationIndex = Convert.ToInt32(Properties.Settings.Default.StationIndex);
            this.fLogMessage("Connect to Station #" + this.stationIndex.ToString() + " ...");
            this.laStation.Text = "Station ID: " + stationIndex.ToString();

            this.laFinishBtnValue.TextChanged += new EventHandler(finishButtonPressed_handler);
            this.laStopBtnValue.TextChanged += new EventHandler(stopButtonPressed_handler);
            this.laHelpBtnValue.TextChanged += new EventHandler(helpButtonPressed_handler);


            this.timerStop.Tick += new EventHandler(timerStop_Tick);

            this.stationIndex = stationIndex; 
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
                negative_indicator = "- ";
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

        private void timerHandler()
        {
            this.laTime.Text = DateTime.Now.ToLongTimeString().ToString();
            try
            {
                //x ServiceReference2.StationRealtimeData[] stationData = myLineClient.ReadRealTimeData(this.stationIndex);

                int counter = 0;
                int taktCounter = myLineClient.GetCounter();
                if ((this.getButtonsWord() & 8) == 8)
                {
                    counter = this.stopTimeCounter;
                }
                else
                {
                    counter = taktCounter;
                }

                this.laStation.Text = "Station ID: " + myLineClient.ReadStationName(this.stationIndex).ToString();
                this.laProduct.Text = "Batch: " + myLineClient.ReadProduct(this.stationIndex);

                //x this.frame = myLineClient.ReadFrame();
                //x this.laFrame.Text = this.frame.Name;

                //x this.laFinishBtnValue.Text = myLineClient.ReadStationButton(this.stationIndex, "FINISH").ToString();
                //x this.laStopBtnValue.Text = myLineClient.ReadStationButton(this.stationIndex, "STOP").ToString();
                //x this.laHelpBtnValue.Text = myLineClient.ReadStationButton(this.stationIndex, "HELP").ToString();

                //this.laButton1.ForeColor = getButtonColor(this.laFinishBtnValue.Text, 1);
                //this.laButton2.ForeColor = getButtonColor(this.laStopBtnValue.Text, 2);
                //this.laButton3.ForeColor = getButtonColor(this.laHelpBtnValue.Text, 3);

                this.laMessage.Text = ""; // empty string if connected
                this.laMem.Text = formatCounter(counter);
                this.laTimeLeft.Text = formatCounter(taktCounter);
                //x this.laSumstopValue.Text = formatCounter(Convert.ToInt32(stationData.First(p => p.Key.Equals("TIMER_STOP")).Value));
                //x this.laHelpTimeValue.Text = formatCounter(Convert.ToInt32(stationData.First(p => p.Key.Equals("TIMER_HELP")).Value));

                //x this.laPlanValue.Text = Convert.ToInt32(stationData.First(p => p.Key.Equals("GAP_DAY")).Value).ToString();
                //x this.laMonthplanValue.Text = Convert.ToInt32(stationData.First(p => p.Key.Equals("GAP_MONTH")).Value).ToString();
            }

            catch (System.ServiceModel.EndpointNotFoundException e)
            {
                this.myLog.LogAlert(AlertType.System, this.GetType().ToString(), e.Message.ToString());
                this.laMessage.Text = "Connecting " + new String('.', (this.myFormTimer.Counter % 4));
            }
            catch (System.TimeoutException e)
            {
                this.myLog.LogAlert(AlertType.System, this.GetType().ToString(), e.Message.ToString());
                this.laMessage.Text = "Connecting " + new String('.', (this.myFormTimer.Counter % 4));
            }
            catch (Exception e1) 
            {
                this.myLog.LogAlert(AlertType.System, this.GetType().ToString(), e1.Message.ToString());
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

        private void btnFinish_Click(object sender, EventArgs e)
        {
            this.myLineClient.PushStationButton(this.stationIndex, "FINISH");
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            this.myLineClient.PushStationButton(this.stationIndex, "STOP");
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            this.myLineClient.PushStationButton(this.stationIndex, "HELP");
        }



        private void finishButtonPressed_handler(object sender, EventArgs e) 
        {
            this.applyColorSchema(this.getButtonsWord());
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
            this.applyColorSchema(this.getButtonsWord());
            if (((Label)sender).Text == "1")
            {
                this.laStop.ForeColor = Color.Red;
                this.stopTimeCounter = 0;
                this.timerStop.Start();
            }
            else
            {
                this.laStop.ForeColor = Color.DarkGray;
                this.timerStop.Stop();
            }
        }

        private void helpButtonPressed_handler(object sender, EventArgs e)
        {
            this.applyColorSchema(this.getButtonsWord());
            if (((Label)sender).Text == "1")
            {
                this.laHelp.ForeColor = Color.Yellow;
            }
            else
            {
                this.laHelp.ForeColor = Color.DarkGray;
            }
        }




        private void laTime_DoubleClick(object sender, EventArgs e)
        {
            this.Close();
        }


        Point last;

        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                last = MousePosition;
            }
        }

        private void Form_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point cur = MousePosition;
                int dx = cur.X - last.X;
                int dy = cur.Y - last.Y;
                Point loc = new Point(Location.X + dx, Location.Y + dy);
                Location = loc;
                last = cur;
            }
        }

        private void BlackScreen1920_MouseDown(object sender, MouseEventArgs e)
        {
            this.Form_MouseDown(sender, e);
        }

        private void BlackScreen1920_MouseMove(object sender, MouseEventArgs e)
        {
            this.Form_MouseMove(sender, e);
        }

        private int getButtonsWord() 
        { 
            int intWord = 
                Convert.ToInt32(this.laStopBtnValue.Text)* 8
                + Convert.ToInt32(this.laHelpBtnValue.Text) * 4
                + Convert.ToInt32(this.frame.Type.ToString()) * 2
                + Convert.ToInt32(this.laFinishBtnValue.Text) * 1;
            return intWord;
        }

        private void applyColorSchema(int codeWord) 
        { 
            if ((codeWord & 8) == 8)
            {
                // stop
                this.paBlack.BackColor = Color.DarkRed;
            } 
            else if ((codeWord & 4) == 4)
            {
                // help
                this.paBlack.BackColor = Color.Peru;
            }
            else if ((codeWord & 2) == 2)
            {
                // non-working frame
                this.paBlack.BackColor = Color.Black;
            }
            else if ((codeWord & 1) == 1)
            {
                // finish
            } 
            else
            {
                // default
                this.paBlack.BackColor = Color.Black;
            }


        }

        private void BlackScreen1920_Load(object sender, EventArgs e)
        {
        }

        private void timerStop_Tick(object sender, EventArgs e) 
        {
            this.stopTimeCounter++;
        }

        private void laPlanValue_Click(object sender, EventArgs e)
        {

        }

        private void BlackScreen1920_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.myFormTimer.TimerDestroy();
        }



    }
}
