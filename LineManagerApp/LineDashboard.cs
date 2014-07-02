using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using AppLog;
using System.Xml;
using System.Configuration;
using System.Reflection;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using Security;
using System.Windows.Forms;
using System.Drawing;

namespace LineManagerApp
{

    public enum BSFlag { Finished = 0x0001, Late = 0x0100, Hungry = 0x0010, Free = 0x0200, LiveLate = 0x0400, SumLate = 0x0800, Outgo = 0x0020 }

    public enum ButtonState { On = 1, Off = 0 }

    public partial class LineDashboard : Form
    {
        private int lineId = 0;
        private FormTimer myFormTimer;
        private LineServiceReference.AssembLineClient myLineClient;
        private LineServiceReference.LineStationBase[] lineStations = null;
        private LineServiceReference.ProductBase[] stock = null;
        private LineServiceReference.ProductBase[] stationBuffer = null;
        private LineServiceReference.ProductBase[] productBuffer = null;
        private LineServiceReference.ProductBase[] productOutbox = null;
        private Hashtable controlHashtable;
        private Hashtable controlsLocation;
        private int moveCounter = -1;
        private int eventCounter = -1;
        private LineServiceReference.StationRealtimeData[] stationData;
        
        private bool connected;
        private string endpoint_address; 
        private string endpoint_confname;

        private Hashtable formSettings;

        private int gridSize = 10;

        private LogProvider myLog;

        public LineDashboard(int LineId, LogProvider logProvider)
        {
            InitializeComponent();
            this.myLog = logProvider;
            fsModule.CheckFormAccess(this.Name + LineId.ToString());

            this.lineId = LineId;
            this.cbOPCMode.Checked = true;
            this.myFormTimer = new FormTimer();
            //--------------------------------------------

            int basePort = Properties.Settings.Default.BasePort;
            string servicePort = ":" + (basePort + this.lineId).ToString();
            string serviceName = "/LineService" + this.lineId.ToString();

            this.endpoint_address = Properties.Settings.Default.EndpointAddress + servicePort + serviceName;
            this.endpoint_confname = Properties.Settings.Default.EndpointName;

            try
            {
                this.myLineClient = new LineServiceReference.AssembLineClient(endpoint_confname, endpoint_address);
                this.myFormTimer.CounterTick += new EventHandler(timerHandler);
                
                this.stock = this.myLineClient.GetProductStock();
                this.productBuffer = this.myLineClient.GetProductBuffer();
                this.lineStations = this.myLineClient.GetStationsArray();

                this.controlsLocation = new Hashtable();
                this.formSettings = new Hashtable();
                this.DrawLineObjects();
             }
            catch (System.ServiceModel.EndpointNotFoundException e)
            {
                this.myLog.LogAlert(AlertType.System, "0", e.TargetSite.ToString(), e.Source.ToString(), e.Message.ToString() + endpoint_address, "NA");
                MessageBox.Show("There is no connection. Please, try later.", "Connecting to line service ...", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Asterisk);
                throw new LineConnectionException(e.Message.ToString()); 
            }
            catch (System.TimeoutException e)
            {
                this.myLog.LogAlert(AlertType.System, "0", e.TargetSite.ToString(), e.Source.ToString(), e.Message.ToString() + endpoint_address, "NA");
                MessageBox.Show("There is no connection. Please, try later.", "Connecting to line service ...", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Asterisk);
                throw new LineConnectionException(e.Message.ToString()); 
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void DrawLineObjects()
        {
            int Xpos = 12;
            int Xtab = 80;
            int Ypos = 300;
            int YoffsetGroup = 142;
            int YoffsetLine = 16;
            int YposInterval = 30;
            int XposInterval = 100;

            int productFieldLength = 77;
            int bufferFieldLingth = 77;
            int columnsMagrin = 12;

            int indButtonSizeWidth = 23;
            int indButtonSizeHeight = 23;

            // create stations block
            for (int i = 0; i < this.lineStations.Count(); i++)
            {
                GroupBox gbxStation = new GroupBox();
                gbxStation.Name = "gbxStation_"  + i.ToString();
                gbxStation.Text = this.lineStations[i].Name;
                gbxStation.Location = new Point(Xpos + ((int)(i / 10))*XposInterval, Ypos + YposInterval * i);
                gbxStation.Size = new Size(Xtab * 5, YoffsetGroup);
                gbxStation.Padding = new Padding(5, 12, 5, 5);
                this.Controls.Add(gbxStation);
                gbxStation.BringToFront();

                Panel paColorPanel = new Panel();
                paColorPanel.Name = "paColorPanel_" + i.ToString();
                paColorPanel.Size = new Size( 
                        (gbxStation.Size.Width - (gbxStation.Padding.Left + gbxStation.Padding.Right)),
                        (gbxStation.Size.Height - (gbxStation.Padding.Top + gbxStation.Padding.Bottom))
                );
                paColorPanel.Location = new Point(gbxStation.Padding.Left, gbxStation.Padding.Top);
                paColorPanel.BackColor = SystemColors.ControlLight;
                paColorPanel.Padding = new Padding(12, 3, 12, 3);
                gbxStation.Controls.Add(paColorPanel);

                Label laProduct = new Label();
                laProduct.Name = "laProduct_" + i.ToString();
                laProduct.Text = "Product";
                laProduct.Location = new Point(paColorPanel.Padding.Left, paColorPanel.Padding.Top);
                laProduct.AutoSize = true;
                paColorPanel.Controls.Add(laProduct);
                
                TextBox tbxProduct = new TextBox();
                tbxProduct.Name = "tbxProduct_" + i.ToString();
                tbxProduct.Text = this.lineStations[i].CurrentProductName;
                tbxProduct.Location = new Point(paColorPanel.Padding.Left, paColorPanel.Padding.Top + YoffsetLine);
                tbxProduct.Size = new Size(productFieldLength, 20);
                tbxProduct.BorderStyle = BorderStyle.Fixed3D; // .FixedSingle;
                tbxProduct.ReadOnly = false;
                paColorPanel.Controls.Add(tbxProduct);

                Label laBuffer = new Label();
                laBuffer.Name = "laBuffer_" + i.ToString();
                laBuffer.Text = "Buffer";
                laBuffer.Location = new Point(tbxProduct.Location.X + tbxProduct.Width + columnsMagrin, paColorPanel.Padding.Top);
                laBuffer.AutoSize = true;
                paColorPanel.Controls.Add(laBuffer);

                //create buffer listbox
                ListBox lbxBuffer = new ListBox();
                lbxBuffer.Name = "lbxBuffer_" + i.ToString();
                lbxBuffer.Location = new Point(tbxProduct.Location.X + tbxProduct.Width + columnsMagrin, paColorPanel.Padding.Top + YoffsetLine);
                lbxBuffer.Size = new Size(bufferFieldLingth, YoffsetLine * 3);
                lbxBuffer.BorderStyle = BorderStyle.FixedSingle;
                paColorPanel.Controls.Add(lbxBuffer);


                // hide !!!!!
                lbxBuffer.Visible = false;
                laBuffer.Visible = false;


                // add indicator buttons
                int secondLineY = lbxBuffer.Location.Y + lbxBuffer.Size.Height;
                int thirdLineY = secondLineY + 28;
                int indButtonsInterval = indButtonSizeWidth + 4;

                Button btnF = new Button();
                btnF.Name = "btnFinish_" + i.ToString(); 
                btnF.Size = new Size(indButtonSizeWidth, indButtonSizeHeight);
                btnF.Location = new Point(tbxProduct.Location.X, secondLineY);
                btnF.Text = "F";
                paColorPanel.Controls.Add(btnF);
                btnF.Click += new EventHandler(btnF_Click);

                Button btnS = new Button();
                btnS.Name = "btnStop_" + i.ToString(); 
                btnS.Size = new Size(indButtonSizeWidth, indButtonSizeHeight);
                btnS.Location = new Point(tbxProduct.Location.X + indButtonsInterval * 1, secondLineY);
                btnS.Text = "S";
                paColorPanel.Controls.Add(btnS);

                Button btnH = new Button();
                btnH.Name = "btnHelp_" + i.ToString(); 
                btnH.Size = new Size(indButtonSizeWidth, indButtonSizeHeight);
                btnH.Location = new Point(tbxProduct.Location.X + indButtonsInterval * 2, secondLineY);
                btnH.Text = "H";
                paColorPanel.Controls.Add(btnH);

                Button btnP1 = new Button();
                btnP1.Name = "btnPart1_" + i.ToString(); 
                btnP1.Size = new Size(indButtonSizeWidth, indButtonSizeHeight);
                btnP1.Location = new Point(tbxProduct.Location.X, thirdLineY);
                btnP1.Text = "P";
                paColorPanel.Controls.Add(btnP1);

                Button btnP2 = new Button();
                btnP2.Name = "btnPart2_" + i.ToString();
                btnP2.Size = new Size(indButtonSizeWidth, indButtonSizeHeight);
                btnP2.Location = new Point(tbxProduct.Location.X + indButtonsInterval * 1, thirdLineY);
                btnP2.Text = "P";
                paColorPanel.Controls.Add(btnP2);

                Button btnU = new Button();
                btnU.Name = "btnFail_" + i.ToString();
                btnU.Size = new Size(indButtonSizeWidth, indButtonSizeHeight);
                btnU.Location = new Point(tbxProduct.Location.X + indButtonsInterval * 2, thirdLineY);
                btnU.Text = "U";
                paColorPanel.Controls.Add(btnU);

                Button btnP = new Button();
                btnP.Name = "btnPlan_" + i.ToString();
                btnP.Size = new Size(indButtonSizeWidth, indButtonSizeHeight);
                btnP.Location = new Point(tbxProduct.Location.X + indButtonsInterval * 5, 5);
                btnP.Text = "P";
                paColorPanel.Controls.Add(btnP);
                btnP.Click += new EventHandler(btnP_Click);

                Label laTail = new Label();
                laTail.Name = "laTail_" + i.ToString();
                //laTail.Text = "Tail: 34001"; 
                laTail.Text = "---";
                laTail.Location = new Point(lbxBuffer.Location.X, secondLineY);
                laTail.Font = new Font(laTail.Font, FontStyle.Bold);
                paColorPanel.Controls.Add(laTail);

                Label laPlanFakt = new Label();
                laPlanFakt.Name = "laPlanFakt_" + i.ToString();
                laPlanFakt.Text = "P/F: 0/0";
                laPlanFakt.Location = new Point(lbxBuffer.Location.X, thirdLineY); ;
                //laPlanFakt.Font = new Font(laPlanFakt.Font, FontStyle.Bold);
                paColorPanel.Controls.Add(laPlanFakt);

                Label laLive = new Label();
                laLive.Name = "laLive_" + i.ToString();
                laLive.Text = "0";
                laLive.AutoSize = true;
                laLive.Location = new Point(tbxProduct.Location.X, tbxProduct.Location.Y + 22);
                paColorPanel.Controls.Add(laLive);

                Label laLiveTakt = new Label();
                laLiveTakt.Name = "laLiveTakt_" + i.ToString();
                laLiveTakt.Text = "0:0";
                laLiveTakt.Location = new Point(lbxBuffer.Location.X, laLive.Location.Y);
                paColorPanel.Controls.Add(laLiveTakt);

                //correct size of panel and group box
                paColorPanel.Width = lbxBuffer.Location.X + lbxBuffer.Size.Width + paColorPanel.Padding.Right;
                paColorPanel.Height = thirdLineY + btnU.Height + paColorPanel.Padding.Bottom;
                gbxStation.Width = paColorPanel.Location.X + paColorPanel.Width + gbxStation.Padding.Right;
                gbxStation.Height = paColorPanel.Height + gbxStation.Padding.Top + gbxStation.Padding.Bottom;

                lbxBuffer.BeginUpdate();
                this.stationBuffer = this.myLineClient.GetStationBuffer(this.lineStations[i].Id);
                if (this.stationBuffer != null)
                {
                    for (int k = this.stationBuffer.Count(); k > 0; k--)
                    {
                        lbxBuffer.Items.Add(this.stationBuffer[k - 1].Name);
                    }
                }
                lbxBuffer.EndUpdate();

                gbxStation.MouseMove += new MouseEventHandler(aPanel_MouseMove);
                gbxStation.MouseDown += new MouseEventHandler(aPanel_MouseDown);
                gbxStation.MouseUp += new MouseEventHandler(aPanel_MouseUp);
                gbxStation.MouseDoubleClick += new MouseEventHandler(openStationWindow);


            }


            int Xpos_stock = Xpos + Xtab * 5 + 40;
            int X_margin = 10;
            int Y_top_margin = 20;
            int Y_bottom_margin = 10;
            int boxes_Y_lines = 7;


            // ------------------------------------------------------
            // create stock listbox
            // ------------------------------------------------------
            GroupBox gbxStock = new GroupBox();
            gbxStock.Name = "gbxStock";
            gbxStock.Text = "Products on stock:";
            gbxStock.Location = new Point(Xpos_stock, Ypos);
            gbxStock.Size = new Size(Xtab * 2 + X_margin * 2, YoffsetLine * boxes_Y_lines + Y_top_margin + Y_bottom_margin - 6);
            this.Controls.Add(gbxStock);

            ListBox lbxStock = new ListBox();
            lbxStock.Name = "lbxStock";
            lbxStock.Location = new Point(X_margin, Y_top_margin);
            lbxStock.Size = new Size(Xtab * 2, YoffsetLine * boxes_Y_lines);
            lbxStock.BorderStyle = BorderStyle.FixedSingle;
            gbxStock.Controls.Add(lbxStock);
            
            lbxStock.BeginUpdate();
            for (int i = this.stock.Count(); i > 0; i--) 
            {
                lbxStock.Items.Add(this.stock[i-1].Name);
            }
            lbxStock.EndUpdate();

            gbxStock.MouseMove += new MouseEventHandler(aPanel_MouseMove);
            gbxStock.MouseDown += new MouseEventHandler(aPanel_MouseDown);
            gbxStock.MouseUp += new MouseEventHandler(aPanel_MouseUp);


            // ------------------------------------------------------
            // create productBuffer listbox
            // ------------------------------------------------------
            GroupBox gbxProductBuffer = new GroupBox();
            gbxProductBuffer.Name = "gbxProductBuffer";
            gbxProductBuffer.Text = "Products in buffer:";
            gbxProductBuffer.Location = new Point(Xpos_stock + 200, Ypos);
            gbxProductBuffer.Size = new Size(Xtab * 2 + X_margin * 2, YoffsetLine * boxes_Y_lines + Y_top_margin + Y_bottom_margin - 6);
            this.Controls.Add(gbxProductBuffer);

            ListBox lbxProductBuffer = new ListBox();
            lbxProductBuffer.Name = "lbxProductBuffer";
            lbxProductBuffer.Location = new Point(X_margin, Y_top_margin);
            lbxProductBuffer.Size = new Size(Xtab * 2, YoffsetLine * boxes_Y_lines);
            lbxProductBuffer.BorderStyle = BorderStyle.FixedSingle;
            gbxProductBuffer.Controls.Add(lbxProductBuffer);

            lbxProductBuffer.BeginUpdate();
            for (int i = this.productBuffer.Count(); i > 0; i--)
            {
                lbxProductBuffer.Items.Add(this.productBuffer[i - 1].Name);
            }
            lbxProductBuffer.EndUpdate();

            gbxProductBuffer.MouseMove += new MouseEventHandler(aPanel_MouseMove);
            gbxProductBuffer.MouseDown += new MouseEventHandler(aPanel_MouseDown);
            gbxProductBuffer.MouseUp += new MouseEventHandler(aPanel_MouseUp);



            // ------------------------------------------------------
            // gbxControlPanel
            // ------------------------------------------------------

            this.gbxControlPanel.MouseMove += new MouseEventHandler(aPanel_MouseMove);
            this.gbxControlPanel.MouseDown += new MouseEventHandler(aPanel_MouseDown);
            this.gbxControlPanel.MouseUp += new MouseEventHandler(aPanel_MouseUp);


            this.ClientSize = new Size(gbxStock.Location.X + gbxStock.Size.Width + 20, Ypos + YoffsetGroup * (this.lineStations.Count()) + YoffsetLine);
        }

        void btnP_Click(object sender, EventArgs e)
        {
            
            PlanDialog editDialog = new PlanDialog();

            // get data for the station
            string stationName = ((Button)sender).Parent.Parent.Text;
            int stationIndex = 0;
            LineServiceReference.LineStationBase station = null;
            for (int i = 0; i < this.lineStations.Count(); i++)
            {
                if (this.lineStations[i].Name == stationName)
                {
                    station = this.lineStations[i];
                    stationIndex = i+1;
                }
            }
            if (station == null) return; 
            LineServiceReference.StationRealtimeData[] stationData = this.myLineClient.ReadRealTimeData(stationIndex);
            if (stationData == null) return;

            string buttonKeyPlan = "REGP.D"; 
            string buttonKeyFakt = "REGF.D"; 
            int tmpDayPlan = Convert.ToInt32(stationData.FirstOrDefault(p => p.Key.Equals(buttonKeyPlan)).Value);
            int tmpDayFakt = Convert.ToInt32(stationData.FirstOrDefault(p => p.Key.Equals(buttonKeyFakt)).Value);
            string buttonKeyPlanM = "REGP.M";
            string buttonKeyFaktM = "REGF.M";
            int tmpMonthPlan = Convert.ToInt32(stationData.FirstOrDefault(p => p.Key.Equals(buttonKeyPlanM)).Value);
            int tmpMonthFakt = Convert.ToInt32(stationData.FirstOrDefault(p => p.Key.Equals(buttonKeyFaktM)).Value);

            //fill data form controls
            editDialog.dataGridView1.Rows.Add(new object[2] { "Day plan", tmpDayPlan.ToString() });
            editDialog.dataGridView1.Rows.Add(new object[2] { "Day fact", tmpDayFakt.ToString() });
            editDialog.dataGridView1.Rows.Add(new object[2] { "Month plan", tmpMonthPlan.ToString() });
            editDialog.dataGridView1.Rows.Add(new object[2] { "Month fact", tmpMonthFakt.ToString() });

            // Show editDialog as a modal dialog and determine if DialogResult = OK. 
            if (editDialog.ShowDialog(this) == DialogResult.OK)
            {
                // Read the contents of testDialog's TextBox. 
                //this.txtResult.Text = testDialog.TextBox1.Text;

                int newDayPlan = Convert.ToInt16(editDialog.dataGridView1.Rows[0].Cells[1].Value);
                if (newDayPlan != tmpDayPlan) 
                {
                    this.myLineClient.SetStationDayPlan(stationName, newDayPlan);
                }

                int newDayFact = Convert.ToInt16(editDialog.dataGridView1.Rows[1].Cells[1].Value);
                if (newDayFact != tmpDayFakt)
                {
                    this.myLineClient.SetStationDayFact(stationName, newDayFact);
                }

                int newMonthPlan = Convert.ToInt16(editDialog.dataGridView1.Rows[2].Cells[1].Value);
                if (newMonthPlan != tmpMonthPlan)
                {
                    this.myLineClient.SetStationMonthPlan(stationName, newMonthPlan);
                }

                int newMonthFakt = Convert.ToInt16(editDialog.dataGridView1.Rows[3].Cells[1].Value);
                if (newMonthFakt != tmpMonthFakt)
                {
                    this.myLineClient.SetStationMonthFact(stationName, newMonthFakt);
                }
            }
            else
            {
                //this.txtResult.Text = "Cancelled";
            }
            editDialog.Dispose();


            //if (InputBox("Edit DAY plan", "Set DAY plan amount for this Tact.", ref value) == DialogResult.OK) 
            //{
            //    string stationName = ((Button)sender).Parent.Parent.Text;
            //    int amount = Convert.ToInt16(value);
            //    //MessageBox.Show(stationName + " = " + amount.ToString());
            //    this.myLineClient.SetStationDayPlan(stationName, amount);
            //}

            //if (InputBox("Edit MONTH plan", "Set MONTH plan amount for this Tact.", ref value) == DialogResult.OK)
            //{
            //    string stationName = ((Button)sender).Parent.Parent.Text;
            //    int amount = Convert.ToInt16(value);
            //    //MessageBox.Show(stationName + " = " + amount.ToString());
            //    this.myLineClient.SetStationMonthPlan(stationName, amount);
            //}

            //if (InputBox("Edit DAY fact", "Set DAY fact amount for this Tact.", ref value) == DialogResult.OK)
            //{
            //    string stationName = ((Button)sender).Parent.Parent.Text;
            //    int amount = Convert.ToInt16(value);
            //    //MessageBox.Show(stationName + " = " + amount.ToString());
            //    this.myLineClient.SetStationDayFact(stationName, amount);
            //}

            //if (InputBox("Edit MONTH fact", "Set MONTH fact amount for this Tact.", ref value) == DialogResult.OK)
            //{
            //    string stationName = ((Button)sender).Parent.Parent.Text;
            //    int amount = Convert.ToInt16(value);
            //    //MessageBox.Show(stationName + " = " + amount.ToString());
            //    this.myLineClient.SetStationMonthFact(stationName, amount);
            //}
        }

        void btnF_Click(object sender, EventArgs e)
        {

            string stationName = ((Button)sender).Parent.Parent.Text;
            this.myLineClient.FinishStation(stationName);
        }




        private void RefreshDrawObjects() 
        {
            for (int i = 0; i < this.lineStations.Count(); i++)
            {
                Control curControl = this.GetControlByName("tbxProduct_" + i.ToString());
                curControl.Text = this.lineStations[i].CurrentProductName;

                this.stationBuffer = this.myLineClient.GetStationBuffer(this.lineStations[i].Id);
                ListBox lbxBuffer = (ListBox)this.GetControlByName("lbxBuffer_" + i.ToString());
                lbxBuffer.BeginUpdate();
                lbxBuffer.Items.Clear();
                if (stationBuffer != null)
                {
                    for (int k = this.stationBuffer.Count(); k > 0; k--)
                    {
                        lbxBuffer.Items.Add(this.stationBuffer[k - 1].Name);
                    }
                }
                lbxBuffer.EndUpdate();
            }

            this.stock = this.myLineClient.GetProductStock();
            ListBox lbxStock = (ListBox)this.GetControlByName("lbxStock");
            lbxStock.BeginUpdate();
            lbxStock.Items.Clear();
            for (int i = this.stock.Count(); i > 0; i--)
            {
                lbxStock.Items.Add(stock[i - 1].Name);
            }
            lbxStock.EndUpdate();

            this.productBuffer = this.myLineClient.GetProductBuffer();
            ListBox lbxProductBuffer = (ListBox)this.GetControlByName("lbxProductBuffer");
            lbxProductBuffer.BeginUpdate();
            lbxProductBuffer.Items.Clear();
            //for (int i = this.productBuffer.Count(); i > 0; i--)
            //{
            //    lbxProductBuffer.Items.Add(productBuffer[i - 1].Name);
            //}
            for (int i = 0; i < this.productBuffer.Count(); i++)
            {
                lbxProductBuffer.Items.Add(productBuffer[i].Name);
            }
            lbxProductBuffer.EndUpdate();
        
        }

        private string formatTaktTime(int lineCounter) 
        {
            string result = "NA";
            string negative_indicator = "";

            int seconds = lineCounter;
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


        private void timerHandler(object sender, EventArgs e) 
        {
            //------------------------------------------------
            // Read timer from Line Service
            //

            string freezedTakt = "";

            try
            {
                if (!this.connected)
                {
                    this.myLineClient = new LineServiceReference.AssembLineClient(this.endpoint_confname, this.endpoint_address);
                    this.connected = true;
                }


                this.laTimeValue.Text = DateTime.Now.ToLongTimeString();

                this.stationData = this.myLineClient.ReadRealTimeDataForLine(1);
                //this.laTakt.Text = this.formatTaktTime(this.myLineClient.GetCounter());
                this.laTakt.Text = this.formatTaktTime(Convert.ToInt32(stationData.FirstOrDefault(p => p.Key.Equals("T")).Value));
                this.laMessage.Text = "";               
                    
                freezedTakt = ((stationData.FirstOrDefault(p => p.Key.Equals("FRZ")) != null) ? 
                     stationData.FirstOrDefault(p => p.Key.Equals("FRZ")).Value.ToString() : "0");
                this.laFreezedTakt.Text = (freezedTakt == "0") ? "(auto)" : "(set to: " + freezedTakt + " min)";

 

                this.fillButtonsState();

                //LineServiceReference.Frame frame = myLineClient.ReadFrame();
                //this.laFrameValue.Text = frame.Name ?? "NA";
                this.laFrameValue.Text = (stationData.FirstOrDefault(p => p.Key.Equals("F")) != null) ? 
                    stationData.FirstOrDefault(p => p.Key.Equals("F")).Value : "NA";

                this.laPlanValue.Text = stationData.FirstOrDefault(p => p.Key.Equals("GAP_DAY")).Value;

                int newEventCounter = Convert.ToInt32(stationData.FirstOrDefault(p => p.Key.Equals("EVENT_COUNTER")).Value);
                if (newEventCounter > this.eventCounter)
                {
                    this.eventCounter = newEventCounter;
                    this.lineStations = this.myLineClient.GetStationsArray();
                    this.RefreshDrawObjects();
                    this.fillTails();
                }
                //this.laMoveCounter.Text = this.moveCounter.ToString();

                this.laSumStopValue.Text = this.formatTaktTime(Convert.ToInt32(stationData.FirstOrDefault(p => p.Key.Equals("SS")).Value));


                //------------------------------------------------
            }
            catch (System.ServiceModel.EndpointNotFoundException e1)
            {
                this.connected = false;
                if (this.myLineClient != null)
                {
                    this.myLineClient.Abort();
                }

                this.myLog.LogAlert(AlertType.System, this.GetType().ToString(), e1.Message.ToString() + this.myLineClient.Endpoint.Address);
                this.laMessage.Text = "Connecting " + new String('.', (this.myFormTimer.Counter % 4));
            }
            catch (System.TimeoutException e2)
            {
                this.connected = false;
                if (this.myLineClient != null)
                {
                    this.myLineClient.Abort();
                }

                this.myLog.LogAlert(AlertType.System, this.GetType().ToString(), e2.Message.ToString() + this.myLineClient.Endpoint.Address);
                this.laMessage.Text = "Connecting " + new String('.', (this.myFormTimer.Counter % 4));
            }
             catch (CommunicationObjectFaultedException e2)
            {
                this.connected = false;
                if (this.myLineClient != null)
                {
                    this.myLineClient.Abort();
                }

                this.myLog.LogAlert(AlertType.System, this.GetType().ToString(), e2.Message.ToString() + this.myLineClient.Endpoint.Address);
                this.laMessage.Text = "Connecting " + new String('.', (this.myFormTimer.Counter % 4));
            }
            catch (CommunicationException e2) 
            {
                this.connected = false;
                if (this.myLineClient != null)
                {
                    this.myLineClient.Abort();
                }


                this.myLog.LogAlert(AlertType.System, this.GetType().ToString(), e2.Message.ToString() + this.myLineClient.Endpoint.Address);
                this.laMessage.Text = "Connecting " + new String('.', (this.myFormTimer.Counter % 4));
            }
            catch(Exception e2)
            {
                this.connected = false;
                if (this.myLineClient != null)
                {
                    this.myLineClient.Abort();
                }


                this.myLog.LogAlert(AlertType.System, this.GetType().ToString(), e2.Message.ToString() + this.myLineClient.Endpoint.Address);
                this.laMessage.Text = "Connecting " + new String('.', (this.myFormTimer.Counter % 4));
            }
        }

        private void LineDashboard_Load(object sender, EventArgs e)
        {
            try
            {

                //-----------------------------------------------
                // Fill data controls from database 
                this.assembLineTableAdapter.Fill(this.dataSet11.AssembLine);
                DataSet1.AssembLineRow assembLineRow = this.dataSet11.AssembLine.FindById(this.lineId);
                if (assembLineRow != null)
                {
                    this.laLineName.Text = assembLineRow["Name"].ToString();
                }
                //-----------------------------------------------
                // Start from timer for reading service data 
                this.myFormTimer.TimerStart();

                // create table of all controls
                this.controlHashtable = new Hashtable();
                foreach (Control control in this.Controls)
                {
                    this.controlHashtable.Add(control.Name, control);
                    if (control.HasChildren)
                    {
                        foreach (Control child in control.Controls)
                        {
                            this.controlHashtable.Add(child.Name, child);
                            if (child.HasChildren)
                            {
                                foreach (Control child2 in child.Controls)
                                {
                                    this.controlHashtable.Add(child2.Name, child2);
                                }
                            }
                        }
                    }

                }

                this.restoreCtrlsLocation();
                //this.WindowState = FormWindowState.Maximized;

                if (fsModule.AccessMode == FormAssessMode.Write)
                {
                    this.setFormWriteMode();
                }
                else if (fsModule.AccessMode == FormAssessMode.Read)
                {
                    this.setFormReadMode();
                }
            }
            catch (Exception ex) 
            { 
                this.myLog.LogAlert(AlertType.System, this.GetType().ToString(), ex.TargetSite.ToString(), ex.Source.ToString(), ex.Message.ToString(), "userName");
            }
        }

        private Control GetControlByName(string name)
        {
            return this.controlHashtable[name] as Control;
        }

        private void btnLineStart_Click(object sender, EventArgs e)
        {
            int lineState = this.myLineClient.GetState();
            if (lineState == 0 | lineState == 1)
            {
                this.myLineClient.Execute();
                this.lineStations = this.myLineClient.GetStationsArray();
                this.RefreshDrawObjects();
            }
          }
        private void btnLineStop_Click(object sender, EventArgs e)
        {
            int lineState = this.myLineClient.GetState();
            if (1==1) //(lineState == 2)
            {
                this.myLineClient.Terminate();
            }
        }
        private void cbOPCMode_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chbox = sender as CheckBox;
            //this.fmLineManager.OPCMode = chbox.Checked;
            //this.paButtons.Enabled = !chbox.Checked;
         }
        private void btnLineMove_Click(object sender, EventArgs e)
        {
            int lineState = this.myLineClient.GetState();
            if (lineState == 2)
            {
                btnLineMove.Enabled = false;
                Timer timer = new Timer();
                timer.Interval = 2000;
                timer.Tick += new EventHandler((_s, _e) =>
                {
                    timer.Stop();
                    timer.Dispose();

                    btnLineMove.Enabled = true;
                });
                timer.Start();



                this.myLineClient.Move();
                this.lineStations = this.myLineClient.GetStationsArray();
                this.RefreshDrawObjects();
            }
        }

        private void LineDashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.myFormTimer.TimerStop();
            this.myLineClient.Close();
            this.WindowState = FormWindowState.Normal;
        }

        private Point last;

        private void aPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                last.X = MousePosition.X;
                last.Y = MousePosition.Y;
            }
        }
        private void aPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point cur = MousePosition;
                int dx = cur.X - this.last.X;
                int dy = cur.Y - this.last.Y;
                Point loc = new Point(
                    ((Control)sender).Location.X + dx,
                    ((Control)sender).Location.Y + dy                    
                );
                ((Control)sender).Location = loc;
                this.last = cur;
            }
        }
        private void aPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point loc = new Point(
                                    (int)(((Control)sender).Location.X / this.gridSize) * this.gridSize,
                                    (int)(((Control)sender).Location.Y / this.gridSize) * this.gridSize
                                );
                ((Control)sender).Location = loc;

                string key = ((Control)sender).Name;
                if (this.controlsLocation != null && this.controlsLocation.Contains(key)) 
                {
                    this.controlsLocation[key] = loc;
                } 
                else 
                {
                    this.controlsLocation.Add(key, loc);
                }
            }
        }

        private void saveCtrlsLocation() 
        { 
            //file stream states the saved binary
            string fileName = "Locations" + this.lineId.ToString() + ".dat";
            string fileName2 = "FormSettings" + this.lineId.ToString() + ".dat";
            FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
            FileStream fs2 = new FileStream(fileName2, FileMode.OpenOrCreate, FileAccess.Write);
            try
            {
                Hashtable a = new Hashtable();
                a = this.controlsLocation;
                ////BinaryFormatter formatter = new BinaryFormatter();
                ////formatter.Serialize(fs, a);
                SoapFormatter formatter = new SoapFormatter();
                formatter.Serialize(fs, a);

                Hashtable b = new Hashtable();
                b = this.formSettings;
                BinaryFormatter formatter2 = new BinaryFormatter();
                formatter2.Serialize(fs2, b);

            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
        }
        private void restoreCtrlsLocation() 
        {
            // Open the file containing the data that you want to deserialize.
            string fileName = "Locations" + this.lineId.ToString() + ".dat";
            string fileName2 = "FormSettings" + this.lineId.ToString() + ".dat";

            try
            ////{

            ////    FileStream fs = new FileStream(fileName, FileMode.Open);
            ////    BinaryFormatter formatter = new BinaryFormatter();

            ////    // Deserialize the hashtable from the file and 
            ////    // assign the reference to the local variable.
            ////    this.controlsLocation.Clear();
            ////    this.controlsLocation = (Hashtable)formatter.Deserialize(fs);
            ////    this.applyLocationsFromTable(this.controlsLocation, this.controlHashtable);
            ////}
            {

                FileStream fs = new FileStream(fileName, FileMode.Open);
                SoapFormatter formatter = new SoapFormatter();

                // Deserialize the hashtable from the file and 
                // assign the reference to the local variable.
                this.controlsLocation.Clear();
                this.controlsLocation = (Hashtable)formatter.Deserialize(fs);
                this.applyLocationsFromTable(this.controlsLocation, this.controlHashtable);
            }
            catch (System.IO.FileNotFoundException e)
            {
                this.myLog.LogAlert(AlertType.System, this.GetType().ToString(), e.TargetSite.ToString(), e.Source.ToString(), e.Message.ToString(), "userName");
            }
            catch (SerializationException e)
            {
                this.myLog.LogAlert(AlertType.System, this.GetType().ToString(), e.TargetSite.ToString(), e.Source.ToString(), e.Message.ToString(), "userName");
                throw;
            }
            catch (Exception e) 
            { 
                this.myLog.LogAlert(AlertType.System, this.GetType().ToString(), e.TargetSite.ToString(), e.Source.ToString(), e.Message.ToString(), "userName");
            }
      

            try 
            {
                FileStream fs2 = new FileStream(fileName2, FileMode.Open);

                BinaryFormatter formatter2 = new BinaryFormatter();
                this.formSettings = (Hashtable)formatter2.Deserialize(fs2);
                this.applyFormSettingsFromTable();            
            }
            catch (System.IO.FileNotFoundException e)
            {
                // file not found
                this.myLog.LogAlert(AlertType.System, this.GetType().ToString(), e.TargetSite.ToString(), e.Source.ToString(), e.Message.ToString(), "userName");
            }
            catch (SerializationException e)
            {
                this.myLog.LogAlert(AlertType.System, this.GetType().ToString(), e.TargetSite.ToString(), e.Source.ToString(), e.Message.ToString(), "userName");
                throw;
            }
            catch (Exception e)
            {
                this.myLog.LogAlert(AlertType.System, this.GetType().ToString(), e.TargetSite.ToString(), e.Source.ToString(), e.Message.ToString(), "userName");
            }

        }
        private void applyLocationsFromTable(Hashtable locations, Hashtable controls)
        {
            foreach (DictionaryEntry de in locations) 
            {
                if (controls.Contains(de.Key)) 
                {
                    ((Control)controls[de.Key]).Location = (Point)de.Value;
                }
            }
        }

        private void applyFormSettingsFromTable() 
        {
            this.Size = new Size((int)this.formSettings["XSize"], (int)this.formSettings["YSize"]); 
            this.WindowState = (FormWindowState)this.formSettings["WindowState"];
            this.Location = new Point((int)this.formSettings["Xpos"], (int)this.formSettings["YPos"]);
        }


        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.saveCtrlsLocation();
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            this.restoreCtrlsLocation();
        }

        private void openStationWindow(object sender, MouseEventArgs e) 
        {
            string controlName = ((Control)sender).Name;
            string key = controlName.Substring(controlName.IndexOf("_") + 1);
            //MessageBox.Show(key);

            int stationIndex = Convert.ToInt16(key);
            string endpoint_address = Properties.Settings.Default.EndpointAddress + ":" 
                + (Convert.ToInt32(Properties.Settings.Default.BasePort) + lineId).ToString() 
                + "/LineService" + lineId.ToString();
            string endpoint_confname = Properties.Settings.Default.EndpointName;

            Form stationForm = new StationClient.BlackScreen(stationIndex + 1, endpoint_confname, endpoint_address);
            stationForm.FormClosed += new FormClosedEventHandler(stationForm_FormClosed);
            stationForm.Show();
        }
        private void stationForm_FormClosed(Object sender, FormClosedEventArgs e) 
        {
            ((Form)sender).Dispose();
        }


        private void fillTails()
        {
            LineServiceReference.LogistTailElem[] newTails = this.myLineClient.GetLogisticTails();

            for (int j = 0; j < this.lineStations.Count(); j++)
            {
                string key = "laTail_" + j;
                Control laTail = (Control)this.controlHashtable[key];
                if (laTail != null)
                {
                    laTail.Text = "";
                }
            }

            for (int i = 0; i < newTails.Count(); i++)
            {
                string key = "laTail_" + ((int)(newTails[i].TailStationIndex)-1).ToString();
                Control laTail = (Control)this.controlHashtable[key];
                if (laTail != null)
                {
                    laTail.Text = "Tail: " + newTails[i].BatchName; // +"-" + newTails[i].BatchType;
                }
            }

        }
        private void fillButtonsState_old() 
        {
            string buttonKey;
            string key;
            Control button = null;

            for (int i = 0; i < this.lineStations.Count(); i++) 
            {
                LineServiceReference.StationRealtimeData[] stationData = myLineClient.ReadRealTimeData(i + 1);
            
                buttonKey = "FINISH";
                key = "btnFinish_" + i.ToString();
                button = (Control)this.controlHashtable[key];
                if (button != null) 
                {
                    int state = Convert.ToInt32(stationData.FirstOrDefault(p => p.Key.Equals(buttonKey)).Value);
                    button.BackColor = this.getButtonColor(buttonKey, (ButtonState)state);
                }

                buttonKey = "STOP";
                key = "btnStop_" + i.ToString();
                button = (Control)this.controlHashtable[key];
                if (button != null)
                {
                    int state = Convert.ToInt32(stationData.FirstOrDefault(p => p.Key.Equals(buttonKey)).Value);
                    button.BackColor = this.getButtonColor(buttonKey, (ButtonState)state);
                }

                buttonKey = "HELP";
                key = "btnHelp_" + i.ToString();
                button = (Control)this.controlHashtable[key];
                if (button != null)
                {
                    int state = Convert.ToInt32(stationData.FirstOrDefault(p => p.Key.Equals(buttonKey)).Value);
                    button.BackColor = this.getButtonColor(buttonKey, (ButtonState)state);
                }

                buttonKey = "PART1";
                key = "btnPart1_" + i.ToString();
                button = (Control)this.controlHashtable[key];
                if (button != null)
                {
                    int state = Convert.ToInt32(stationData.FirstOrDefault(p => p.Key.Equals(buttonKey)).Value);
                    button.BackColor = this.getButtonColor(buttonKey, (ButtonState)state);
                }

                buttonKey = "PART2";
                key = "btnPart2_" + i.ToString();
                button = (Control)this.controlHashtable[key];
                if (button != null)
                {
                    int state = Convert.ToInt32(stationData.FirstOrDefault(p => p.Key.Equals(buttonKey)).Value);
                    button.BackColor = this.getButtonColor(buttonKey, (ButtonState)state);
                }

                buttonKey = "FAIL";
                key = "btnFail_" + i.ToString();
                button = (Control)this.controlHashtable[key];
                if (button != null)
                {
                    int state = Convert.ToInt32(stationData.FirstOrDefault(p => p.Key.Equals(buttonKey)).Value);
                    button.BackColor = this.getButtonColor(buttonKey, (ButtonState)state);
                }
            }
            
        }
        private void fillButtonsState()
        {
            string buttonKey;
            string key;
            Control button = null;

            try
            {

                for (int stationIndex = 0; stationIndex < this.lineStations.Count(); stationIndex++)
                {
                    int bitState = 0;

                    buttonKey = "FI" + (stationIndex + 1).ToString();
                    key = "btnFinish_" + stationIndex.ToString();
                    button = (Control)this.controlHashtable[key];
                    if (button != null)
                    {
                        int state = Convert.ToInt32(stationData.FirstOrDefault(p => p.Key.Equals(buttonKey)).Value);
                        button.BackColor = this.getButtonColor(buttonKey, (ButtonState)state);

                        bitState = ((bitState & ~(int)BSFlag.Finished) | (state * 1));
                    }

                    buttonKey = "ST" + (stationIndex + 1).ToString();
                    key = "btnStop_" + stationIndex.ToString();
                    button = (Control)this.controlHashtable[key];
                    if (button != null)
                    {
                        int state = Convert.ToInt32(stationData.FirstOrDefault(p => p.Key.Equals(buttonKey)).Value);
                        button.BackColor = this.getButtonColor(buttonKey, (ButtonState)state);

                        bitState = (bitState & ~0x0008) | (state * 8);
                    }

                    buttonKey = "HE" + (stationIndex + 1).ToString();
                    key = "btnHelp_" + stationIndex.ToString();
                    button = (Control)this.controlHashtable[key];
                    if (button != null)
                    {
                        int state = Convert.ToInt32(stationData.FirstOrDefault(p => p.Key.Equals(buttonKey)).Value);
                        button.BackColor = this.getButtonColor(buttonKey, (ButtonState)state);

                        bitState = (bitState & ~0x0004) | (state * 4);
                    }

                    buttonKey = "P1" + (stationIndex + 1).ToString();
                    key = "btnPart1_" + stationIndex.ToString();
                    button = (Control)this.controlHashtable[key];
                    if (button != null)
                    {
                        int state = Convert.ToInt32(stationData.FirstOrDefault(p => p.Key.Equals(buttonKey)).Value);
                        button.BackColor = this.getButtonColor(buttonKey, (ButtonState)state);
                    }

                    buttonKey = "P2" + (stationIndex + 1).ToString();
                    key = "btnPart2_" + stationIndex.ToString();
                    button = (Control)this.controlHashtable[key];
                    if (button != null)
                    {
                        int state = Convert.ToInt32(stationData.FirstOrDefault(p => p.Key.Equals(buttonKey)).Value);
                        button.BackColor = this.getButtonColor(buttonKey, (ButtonState)state);
                    }

                    buttonKey = "FA" + (stationIndex + 1).ToString();
                    key = "btnFail_" + stationIndex.ToString();
                    button = (Control)this.controlHashtable[key];
                    if (button != null)
                    {
                        int state = Convert.ToInt32(stationData.FirstOrDefault(p => p.Key.Equals(buttonKey)).Value);
                        button.BackColor = this.getButtonColor(buttonKey, (ButtonState)state);
                    }

                    buttonKey = "BST" + (stationIndex + 1).ToString();
                    key = "paColorPanel_" + stationIndex.ToString();
                    Control panel = (Control)this.controlHashtable[key];
                    if (panel != null)
                    {
                        int tmpState = Convert.ToInt32(stationData.FirstOrDefault(p => p.Key.Equals(buttonKey)).Value);
                        bitState = (bitState & 0x000F) | (tmpState & 0xFFF0);
                        panel.BackColor = this.getBackpanColor(bitState);
                    }


                    string buttonKeyPlan = "REGP.D" + (stationIndex + 1).ToString();
                    string buttonKeyFakt = "REGF.D" + (stationIndex + 1).ToString();
                    key = "laPlanFakt_" + stationIndex.ToString();
                    Control label = (Control)this.controlHashtable[key];
                    if (label != null)
                    {
                        int tmpDayPlan = Convert.ToInt32(stationData.FirstOrDefault(p => p.Key.Equals(buttonKeyPlan)).Value);
                        int tmpDayFakt = Convert.ToInt32(stationData.FirstOrDefault(p => p.Key.Equals(buttonKeyFakt)).Value);
                        label.Text = "P/F: " + tmpDayPlan.ToString() + "/" + tmpDayFakt.ToString();
                    }

                    string stationKeyLive = "LIVE" + (stationIndex + 1).ToString();
                    key = "laLive_" + stationIndex.ToString();
                    Control laLive = (Control)this.controlHashtable[key];
                    if (label != null)
                    {
                        string st = formatTaktTime(Convert.ToInt32(stationData.FirstOrDefault(p => p.Key.Equals(stationKeyLive)).Value));
                        laLive.Text = "live: " + st;
                    }


                    string stationKeyLiveTakt = "LIVTA" + (stationIndex + 1).ToString();
                    key = "laLiveTakt_" + stationIndex.ToString();
                    Control laLiveTakt = (Control)this.controlHashtable[key];
                    if (label != null)
                    {
                        string st = formatTaktTime(Convert.ToInt32(stationData.FirstOrDefault(p => p.Key.Equals(stationKeyLiveTakt)).Value));
                        laLiveTakt.Text = "takt: " + st;
                    }

                }

            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, "fillButtonState()", ex.Message);
            }

        }

        private Color getBackpanColor_v2_1_171(int codeWord) 
        {
            Color result = SystemColors.ControlLight;


            if ((codeWord & (int)BSFlag.Free) == (int)BSFlag.Free)
            {
                // free and do nothing
                result = Color.Silver;
            }
            else if ((codeWord & 8) == 8)
            {
                // stop
                result = Color.Tomato;
            }
            else if ((codeWord & 0x0111) == 0x0111)
            {
                // finished, late and next is busy
                result = Color.Thistle;
            }
            else if ((codeWord & 0x0210) == 0x0210)
            {
                // late and free
                result = Color.Silver;
            }
            else if (((codeWord & (int)BSFlag.LiveLate) == (int)BSFlag.LiveLate) & ((codeWord & (int)BSFlag.Late) == (int)BSFlag.Late))
            {
                // liveLate and and not finish
                result = Color.LightCoral;
            }
            else if ((codeWord & (int)BSFlag.Late) == (int)BSFlag.Late)
            {
                // late and not finish
                result = Color.Moccasin;
            }
            else if ((codeWord & 4) == 4)
            {
                // help
                result = Color.PaleGoldenrod;
            }
            else if ((codeWord & 2) == 2)
            {
                // non-working frame
                // this.paBlack.BackColor = Color.Black;
            }
            else if ((codeWord & (int)BSFlag.Finished) == (int)BSFlag.Finished)
            {
                // finish
                result = Color.PaleGreen;
            }
            else
            {
                // default
                result = SystemColors.ControlLight;
            }

            return result;
        }

        private Color getBackpanColor(int codeWord)
        {
            Color result = SystemColors.ControlLight;


            BSFlag redState = (BSFlag.Late | BSFlag.LiveLate);
            BSFlag greenState = (BSFlag.Finished | BSFlag.Outgo);

            if (((BSFlag)codeWord & BSFlag.Free) == BSFlag.Free)
            {
                // free and do nothing
                result = Color.Silver;
            }
            else if ((codeWord & 0x0008) == 0x0008)
            {
                // stop
                result = Color.Tomato;
            }

            else if (
                (((BSFlag)codeWord & redState) == redState) 
                
                | ((((BSFlag)codeWord & (BSFlag.Finished | BSFlag.LiveLate)) == (BSFlag.Finished | BSFlag.LiveLate))
                    & (((BSFlag)codeWord & ~BSFlag.Late & ~BSFlag.Outgo) == (BSFlag)codeWord))
            )
            {
                // late, live > takt
                result = Color.LightCoral;
            }

            else if (
                ((((BSFlag)codeWord & ~BSFlag.Finished & ~BSFlag.Late) == (BSFlag)codeWord)
                    & (((BSFlag)codeWord & BSFlag.LiveLate) == BSFlag.LiveLate))

                | ((((BSFlag)codeWord & (BSFlag.Finished | BSFlag.Late)) == (BSFlag.Finished | BSFlag.Late))
                    & (((BSFlag)codeWord & ~BSFlag.LiveLate) == (BSFlag)codeWord))

                | ((((BSFlag)codeWord & BSFlag.Finished) == BSFlag.Finished)
                    & (((BSFlag)codeWord & ~BSFlag.Late & ~BSFlag.Outgo & ~BSFlag.LiveLate) == (BSFlag)codeWord))
            )
            {
                result = Color.Moccasin;
            }

            else if ((codeWord & 4) == 4)
            {
                // help
                result = Color.PaleGoldenrod;
            }
            else if ((codeWord & 2) == 2)
            {
                // non-working frame
                // this.paBlack.BackColor = Color.Black;
            }
            else if (((BSFlag)codeWord & greenState) == greenState)
            {
                // finish
                result = Color.PaleGreen;
            }
            else
            {
                // default
                result = SystemColors.ControlLight; 
            }


            return result;
        }

        private System.Drawing.Color getButtonColor(string key, ButtonState state) 
        {
            System.Drawing.Color result = SystemColors.ControlLight;
            if (state == ButtonState.On) 
            {
                
                key = key.Substring(0, 2);

                if (key == "FI") { result = Color.Lime; }
                if (key == "ST") { result = Color.Tomato; }
                if (key == "HE") { result = Color.Gold; }
                if (key == "P1") { result = Color.White; }
                if (key == "P2") { result = Color.White; }
                if (key == "FA") { result = Color.Tomato; }
            }

            return result;
        }



        private FormSecurityModule fsModule = new FormSecurityModule();
        private void setFormReadMode()
        {
            //this.dataGridView1.ReadOnly = true;
            //this.dataGridView2.ReadOnly = true;
            this.btnLineStart.Enabled = false;
            this.btnLineStop.Enabled = false;
            this.btnLineMove.Enabled = false;
            //this.btnSave.Enabled = false;
        }
        private void setFormWriteMode() { }

        private void button7_Click(object sender, EventArgs e)
        {
            this.RefreshDrawObjects();
        }

        private void LineDashboard_RegionChanged(object sender, EventArgs e)
        {
            int i = 1;
        }

        private void LineDashboard_ResizeEnd(object sender, EventArgs e)
        {
            this.formSettings.Clear();
            this.formSettings.Add("XSize", this.Size.Width);
            this.formSettings.Add("YSize", this.Size.Height);
            this.formSettings.Add("WindowState", this.WindowState);
            this.formSettings.Add("Xpos", this.Location.X);
            this.formSettings.Add("YPos", this.Location.Y);
        }

        private void LineDashboard_LocationChanged(object sender, EventArgs e)
        {
            this.formSettings.Clear();
            this.formSettings.Add("XSize", this.Size.Width);
            this.formSettings.Add("YSize", this.Size.Height);
            this.formSettings.Add("WindowState", this.WindowState);
            this.formSettings.Add("Xpos", this.Location.X);
            this.formSettings.Add("YPos", this.Location.Y);
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            this.myLineClient.RestoreLine();
        }

        private void LineDashboard_Shown(object sender, EventArgs e)
        {
            this.fillTails();
        }

        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.myLineClient.ResetTimer();
        }

        private void laTakt_DoubleClick(object sender, EventArgs e)
        {

        }

        private void btnLogisticScreen_Click(object sender, EventArgs e)
        {
            this.openLogisticWindow(this, e);
        }

        private void openLogisticWindow(object sender, EventArgs e)
        {
            ////string endpoint_address = Properties.Settings.Default.EndpointAddress + ":"
            ////    + (Convert.ToInt32(Properties.Settings.Default.BasePort) + lineId).ToString()
            ////    + "/LineService" + lineId.ToString();
            ////string endpoint_confname = Properties.Settings.Default.EndpointName;

            Form logisticForm = new LogisticTcp.LogisticScreen();
            logisticForm.FormClosed += new FormClosedEventHandler(stationForm_FormClosed);
            logisticForm.Show();
        }

        private void setCurrentTaktToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string value = "0";
            if (InputBox("Edit takt", "Set current takt value in SECONDS.", ref value) == DialogResult.OK)
            {
                int amount = Convert.ToInt32(value);
                this.myLineClient.SetCounter(amount);
            }
        }

        private void setTaktToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string value = "0";
            if (InputBox("Edit takt", "Set takt value PERMANENTLY in minutes. \n To cancel this settings, select menu \"Takt timer.../ Reset to auto\"", ref value) == DialogResult.OK)
            {
                int amount = Convert.ToInt32(value);
                this.myLineClient.SetCounterForever(amount);
            }
        }

        private void resetToAutoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("To reset next takt to auto value press OK button, to avoid press Cancel.", "Reset takt", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk ) == DialogResult.OK)
            {
                this.myLineClient.SetCounterForever(0);
            }
        }

        public LineServiceReference.LogMessage[] errorList = new LineServiceReference.LogMessage[0];

        private void testMemLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //this.errorList = this.myLineClient.GetMemLogData();
            //this.dataGridView1.DataSource = this.errorList;
        }
        
    }


    [Serializable()]
    public class LineConnectionException : System.Exception
    {
        public LineConnectionException() : base() { }
        public LineConnectionException(string message) : base(message) { }
        public LineConnectionException(string message, System.Exception inner) : base(message, inner) { }
        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client. 
        protected LineConnectionException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) { }
    }




}
