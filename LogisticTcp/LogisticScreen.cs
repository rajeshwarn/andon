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

namespace LogisticTcp
{
    

    public partial class LogisticScreen : Form
    {
        private const string LogisticEndPointName = "NetTcpBinding_ILogistic";
        
        private int lineId;
        protected Size formSize;
        private Hashtable checkedRows = new Hashtable();

        private bool connected1;
        private bool connected2;
        
        private ServiceReference1.LogisticClient myLineClient1;
        private ServiceReference1.LogisticClient myLineClient2;

        //private ServiceReference1.LogisticCollectorClient myCollector;

        //x private ServiceReference1.Frame frame = new ServiceReference1.Frame();
        private FormTimer myFormTimer;
        protected LogProvider myLog = new LogProvider(LogType.File, "station_log.txt", true);

        
        public LogisticScreen()
        {
            InitializeComponent();
            this.formSize = this.Size;
            this.lineId = Convert.ToInt32(Properties.Settings.Default.LineId);

            this.myLineClient1 = new ServiceReference1.LogisticClient("NetTcpBinding_ILogistic1");
            this.myLineClient2 = new ServiceReference1.LogisticClient("NetTcpBinding_ILogistic2");
            ////this.myCollector = new ServiceReference1.LogisticCollectorClient("NetTcpBinding_ILogisticCollector");

            this.myFormTimer = new FormTimer();
            this.myFormTimer.CounterTick += new myEventHandler(timerHandler);
            this.laProduct.Text = ""; // tbd

            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlackScreen_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.BlackScreen_MouseMove);

        }

        private void laTime_DoubleClick(object sender, EventArgs e)
        {
            this.Close();
        }
        private Point last;
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
                this.Location = loc;
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

        private void timerHandler()
        {
            this.laTime.Text = DateTime.Now.ToLongTimeString().ToString();
            try
            {

                if (!this.connected1)
                {
                    this.myLineClient1 = new ServiceReference1.LogisticClient("NetTcpBinding_ILogistic1");
                    this.connected1 = true;
                }

                if (!this.connected2)
                {
                    this.myLineClient2 = new ServiceReference1.LogisticClient("NetTcpBinding_ILogistic2");
                    this.connected2 = true;
                }

                ServiceReference1.StationRealtimeData[] stationData1 = new ServiceReference1.StationRealtimeData[0];
                ServiceReference1.StationRealtimeData[] stationData2 = new ServiceReference1.StationRealtimeData[0];

                try 
                {
                    stationData1 = myLineClient1.ReadLogisticRealTimeData(1);
                }
                catch (Exception ex)
                {
                    this.myLog.LogAlert(AlertType.Error, this.GetType().ToString(), ex.Message.ToString());
                }

                try 
                {
                    stationData2 = myLineClient2.ReadLogisticRealTimeData(1);
                }
                catch (Exception ex)
                {
                    this.myLog.LogAlert(AlertType.Error, this.GetType().ToString(), ex.Message.ToString());
                }


                int largeValue = 999999;
                int taktCounter1 = (stationData1.FirstOrDefault(p => p.Key.Equals("T")) != null)
                    ? Convert.ToInt32(stationData1.FirstOrDefault(p => p.Key.Equals("T")).Value) : largeValue;
                int taktCounter2 = (stationData2.FirstOrDefault(p => p.Key.Equals("T")) != null)
                    ? Convert.ToInt32(stationData2.FirstOrDefault(p => p.Key.Equals("T")).Value) : largeValue;
                int taktCounter = Math.Min(taktCounter1, taktCounter2);

                this.laMem.Text = formatCounter(taktCounter);

                this.laFrame.Text = (stationData1.FirstOrDefault(p => p.Key.Equals("F")) != null)
                    ? (stationData1.FirstOrDefault(p => p.Key.Equals("F")).Value).ToString() : "";

               
                this.fillStationGrid();
                this.fillTailGrid();

                this.fillBatchesList();
                this.fillNextBatchInfo();

                this.laMessage.Text = ""; // empty string if connected
            }

            catch (System.ServiceModel.EndpointNotFoundException e)
            {
                this.myLog.LogAlert(AlertType.Error, this.GetType().ToString(), e.Message.ToString());
                this.laMessage.Text = "Connecting " + new String('.', (this.myFormTimer.Counter % 4));
            }
            catch (System.TimeoutException e)
            {
                this.myLog.LogAlert(AlertType.Error, this.GetType().ToString(), e.Message.ToString());
                this.laMessage.Text = "Connecting " + new String('.', (this.myFormTimer.Counter % 4));
            }
            catch (Exception e1)
            {
                this.myLog.LogAlert(AlertType.Error, this.GetType().ToString(), e1.Message.ToString());
            }

        }

         private void fillBatchesList()
        {
            List<string> batchNames = new List<string>();
            List<ServiceReference1.LogistTailElem> tmpList = new List<ServiceReference1.LogistTailElem>();

            try
            {
                tmpList.AddRange(myLineClient1.GetBatchesOnLine());
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, ex.Source.ToString(), ex.Message.ToString());
                this.connected1 = false;
                if (this.myLineClient1 != null)
                {
                    this.myLineClient1.Abort();
                }
            }

            try
            {
                tmpList.AddRange(myLineClient2.GetBatchesOnLine());
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, ex.Source.ToString(), ex.Message.ToString());
                this.connected2 = false;
                if (this.myLineClient2 != null)
                {
                    this.myLineClient2.Abort();
                }
            }

            ServiceReference1.LogistTailElem[] newBatches = tmpList.ToArray();

            string batchString = "";
            foreach (ServiceReference1.LogistTailElem batch in newBatches)
            {
                if (!batchNames.Contains(batch.BatchName)) 
                {
                    batchNames.Add(batch.BatchName);
                    batchString += ", " + batch.BatchName + "(" + batch.BatchType + ")";
                }
                
            }
            if (batchString.Length > 0)
            {
                batchString = batchString.Substring(1, batchString.Length - 1);
            }
            this.laProduct.Text = "Batches online: " + batchString;

        }

        private void fillStationGrid()
        {

            List<string> keyNames = new List<string>();
            List<ServiceReference1.LogistRequestElem> tmpList = new List<ServiceReference1.LogistRequestElem>();

            try
            {
                tmpList.AddRange(myLineClient1.GetLogisticRequests());
            }
            catch (Exception ex)
            {
                this.connected1 = false;
                if (this.myLineClient1 != null)
                {
                    this.myLineClient1.Abort();
                }
                this.myLog.LogAlert(AlertType.Error, ex.Source.ToString(), ex.Message.ToString());
            }

            try
            {
                tmpList.AddRange(myLineClient2.GetLogisticRequests());
            }
            catch (Exception ex)
            {
                this.connected2 = false;
                if (this.myLineClient2 != null)
                {
                    this.myLineClient2.Abort();
                }
                this.myLog.LogAlert(AlertType.Error, ex.Source.ToString(), ex.Message.ToString());
            }

            
            // code below is keeped from release for one LineService
            //
            ServiceReference1.LogistRequestElem[] newRequests = tmpList.ToArray();

            this.dgrRequests.Rows.Clear();
            for (int i = 0; i < newRequests.Count(); i++) 
            {
                string checkedField = "";
                string rowKey = newRequests[i].Address + "-" + newRequests[i].PartName;
                if (this.checkedRows.Contains(rowKey))
                {
                    checkedField = "Ok";
                    this.checkedRows[rowKey] = 1;
                }
                this.dgrRequests.Rows.Add(
                    newRequests[i].Address,
                    newRequests[i].PartName,
                    this.formatCounter(newRequests[i].WaitingTime),
                    checkedField,
                    newRequests[i].OrderNum
                );
            }

            string[] keys = new string[this.checkedRows.Count];
            this.checkedRows.Keys.CopyTo(keys, 0);
            foreach(string key in keys)
            {
                if (((int)this.checkedRows[key]) == 0)
                {
                    this.checkedRows.Remove(key);
                }
                else 
                {
                    this.checkedRows[key] = 0;
                }
            } 

            DataGridViewColumn sortCol = this.dgrRequests.Columns["OrderNum"];
            if(sortCol != null) 
            {
                this.dgrRequests.Sort(sortCol, ListSortDirection.Ascending);
            }
            
        }

        private void fillTailGrid()
        {

            List<string> keyNames = new List<string>();
            List<ServiceReference1.LogistTailElem> tmpList = new List<ServiceReference1.LogistTailElem>();

            try
            {
                tmpList.AddRange(myLineClient1.GetTails());
            }
            catch (Exception ex)
            {
                this.connected1 = false;
                if (this.myLineClient1 != null)
                {
                    this.myLineClient1.Abort();
                }
                this.myLog.LogAlert(AlertType.Error, ex.Source.ToString(), ex.Message.ToString());
            }

            try
            {
                tmpList.AddRange(myLineClient2.GetTails());
            }
            catch (Exception ex)
            {
                this.connected2 = false;
                if (this.myLineClient2 != null)
                {
                    this.myLineClient2.Abort();
                }
                this.myLog.LogAlert(AlertType.Error, ex.Source.ToString(), ex.Message.ToString());
            }


            // code below is keeped from release for one LineService
            //
            ServiceReference1.LogistTailElem[] newTails = tmpList.ToArray();

            this.dgrTails.Rows.Clear();
            for (int i = 0; i < newTails.Count(); i++)
            {
                if (newTails[i] != null)
                {
                    string rowKey = newTails[i].BatchName + "-" + newTails[i].BatchType;
                    this.dgrTails.Rows.Add(
                        newTails[i].BatchName,
                        newTails[i].BatchType,
                        newTails[i].TailStationName
                    );
                }
            }
        }

        private void fillNextBatchInfo() 
        {
            try
            {
                ServiceReference1.LogisticInfo info = myLineClient1.GetLogisticInfo();
                this.laNextBatchValue.Text = info.NextBatchName.ToString();
                this.laTaktsTillNextBatchValue.Text = info.TaktsTillNextBatch.ToString();
            }
            catch (Exception ex) 
            {
                this.myLog.LogAlert(AlertType.Error, this.GetType().ToString(), ex.Message.ToString());
            }
        }


        private void getLogisticsRealTimeData() 
        { 
            //ServiceReference1.StationRealtimeData[]
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
                negative_indicator = "R ";
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
        private void dgrRequests_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                this.myLog.LogAlert(AlertType.System, this.GetType().ToString(), "--------------------------------------------------------" );
                this.myLog.LogAlert(AlertType.System, this.GetType().ToString(), "dgrRequests_CellDoubleClick() call" );


                

                foreach (DictionaryEntry row in this.checkedRows) 
                {
                    this.myLog.LogAlert(AlertType.System, this.GetType().ToString(), "checkedRow: " + row.Key );  
                }

                string rowKey = ((DataGridView)sender).CurrentRow.Cells["Station"].Value.ToString() + "-" +
                ((DataGridView)sender).CurrentRow.Cells["Parts"].Value.ToString();

                //rowKey = rowKey.Replace(" ", ".");
                this.myLog.LogAlert(AlertType.System, this.GetType().ToString(), "rowKey: " + rowKey);  
                
                if (!this.checkedRows.Contains(rowKey))
                {
                    this.checkedRows.Add(rowKey, 0);
                    this.myLog.LogAlert(AlertType.System, this.GetType().ToString(), "add row: " + rowKey);  
                }

                foreach (DictionaryEntry row in this.checkedRows)
                {
                    this.myLog.LogAlert(AlertType.System, this.GetType().ToString(), "checkedRow: " + row.Key);
                }

            }
            catch (Exception ex) 
            {
                this.myLog.LogAlert(AlertType.Error, this.GetType().ToString(), ex.Message.ToString());
            }
        }


        protected void resizeControls(object owner, float kx, float ky)
        {
            if (Object.ReferenceEquals(owner.GetType(), typeof(Microsoft.VisualBasic.PowerPacks.ShapeContainer))) 
            {
                foreach (Microsoft.VisualBasic.PowerPacks.Shape ctrl in ((Microsoft.VisualBasic.PowerPacks.ShapeContainer)owner).Shapes) 
                {
                    ((Microsoft.VisualBasic.PowerPacks.LineShape)ctrl).X1 = (int)(((Microsoft.VisualBasic.PowerPacks.LineShape)ctrl).X1 * kx);
                    ((Microsoft.VisualBasic.PowerPacks.LineShape)ctrl).X2 = (int)(((Microsoft.VisualBasic.PowerPacks.LineShape)ctrl).X2 * kx);
                    ((Microsoft.VisualBasic.PowerPacks.LineShape)ctrl).Y1 = (int)(((Microsoft.VisualBasic.PowerPacks.LineShape)ctrl).Y1 * kx);
                    ((Microsoft.VisualBasic.PowerPacks.LineShape)ctrl).Y2 = (int)(((Microsoft.VisualBasic.PowerPacks.LineShape)ctrl).Y2 * kx);                  
                }
            
            }
            else if (Object.ReferenceEquals(owner.GetType(), typeof(Label))) 
            {
                Label lab = (Label)owner;
                int oldX = lab.Location.X;
                int oldY = lab.Location.Y;

                //lab.Location = new Point((int)(oldX * kx), (int)(oldY * ky));
                //lab.Size = new Size((int)(lab.Size.Width * kx), (int)(lab.Size.Height * ky));

                lab.Font = new Font(lab.Font.Name, lab.Font.Size * Math.Min(kx, ky), lab.Font.Style, lab.Font.Unit);


            } 
            else 
            {
                foreach (object ctrl in ((Control)owner).Controls)
                {
                    if (Object.ReferenceEquals(ctrl.GetType(), typeof(Label)))
                    {
                        Label lab = (Label)ctrl;
                        int oldX = lab.Location.X;
                        int oldY = lab.Location.Y;
                        lab.Location = new Point((int)(oldX * kx), (int)(oldY * ky));
                        lab.Size = new Size((int)(lab.Size.Width * kx), (int)(lab.Size.Height * ky));
                        lab.Font = new Font(lab.Font.Name, lab.Font.Size * Math.Min(kx, ky), lab.Font.Style, lab.Font.Unit);
                    }
                    else if (Object.ReferenceEquals(ctrl.GetType(), typeof(DataGridView)))
                    {
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



                        foreach (DataGridViewColumn column in dgrv.Columns) 
                        {
                            if (column.AutoSizeMode != DataGridViewAutoSizeColumnMode.Fill)
                            {
                                column.Width = (int)(column.Width * kx);
                            }
                        }

                        //resizeControls(ctrl, kx, ky);
                    }
                    else
                    {
                        int oldX = ((Control)ctrl).Location.X;
                        int oldY = ((Control)ctrl).Location.Y;
                        ((Control)ctrl).Location = new Point((int)(oldX * kx), (int)(oldY * ky));
                        ((Control)ctrl).Size = new Size((int)(((Control)ctrl).Size.Width * kx), (int)(((Control)ctrl).Size.Height * ky));
                        resizeControls(ctrl, kx, ky);
                    }
                }
            }
        }
        private void panel1_DoubleClick(object sender, EventArgs e)
        {
            this.Close();
        }
        private void LogisticScreen_Load(object sender, EventArgs e)
        {
        }

        private void laTime_DoubleClick_1(object sender, EventArgs e)
        {
            this.Close();
        }

    }



    public class LogistRequestElem 
    {
        public string StationName;
        public string PartName;
        public string WaitingTime;
    }
    public class LogistTailElem
    {
        public string BatchName;
        public string BatchType;
        public string TailStationName;
    }

    public class LogisticScreenScalable : LogisticScreen 
    {
        public LogisticScreenScalable(Size formSize)
            : base()
        {
            this.formSize = formSize;
            this.Load += new System.EventHandler(this.LogisticScreenScalable_Load);
        }

        private void LogisticScreenScalable_Load(object sender, EventArgs e) 
        {
            try
            {
                //this.Location = new Point(0, 0);
                if (this.formSize.Width != 0 && this.formSize.Height != 0)
                {
                    //float kx = 1.667F; //(int)((this.Size.Width / this.formSize.Width) * 1000) / 1000;
                    //float ky = 1.667F; //(int)((this.Size.Height / this.formSize.Height) * 1000) / 1000;

                    float old_W = this.Size.Width;
                    float old_H = this.Size.Height;

                    float new_W = this.formSize.Width;
                    float new_H = this.formSize.Height;

                    float tmpW = (new_W / old_W) * 1000;
                    tmpW = (float)((int)tmpW);

                    float tmpH = (new_H / old_H) * 1000;
                    tmpH = (float)((int)tmpH);


                    float kx = tmpW / 1000;
                    float ky = tmpH / 1000;


                    this.Location = new Point(0, 0);
                    
                    resizeControls(this, kx, ky);
                    
                    this.Size = new Size(this.formSize.Width, this.formSize.Height);
                    this.formSize = this.Size;
                }

            }
            catch  (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.GetType().ToString(), ex.Message.ToString());
            }
        }



    }


}
