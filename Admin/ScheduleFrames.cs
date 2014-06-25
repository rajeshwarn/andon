using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Security;
using AppLog;

namespace Admin
{
    public partial class ScheduleFrames : Form
    {
        private DateTime copyFramesDate;
        private int copyLineId;
        private DetroitDataSetTableAdapters.SchedulerFrameDefaultTableAdapter schedulerFrameDefaultTableAdapter = new DetroitDataSetTableAdapters.SchedulerFrameDefaultTableAdapter();
        private LogProvider systemLog;

        public ScheduleFrames()
        {
            InitializeComponent();
            fsModule.CheckFormAccess(this.Name);
        }

        public ScheduleFrames(LogProvider systemLog) : this()
        {
            this.systemLog = systemLog;
        }

        private void ScheduleFrames_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'detroitDataSet.AssembLine' table. You can move, or remove it, as needed.
            this.assembLineTableAdapter.Fill(this.detroitDataSet.AssembLine);
            // TODO: This line of code loads data into the 'detroitDataSet.SchedulerFrame' table. You can move, or remove it, as needed.
   //         this.schedulerFrameTableAdapter.Fill(this.detroitDataSet.SchedulerFrame);
            // TODO: This line of code loads data into the 'detroitDataSet.SchedulerFrameDefault' table. You can move, or remove it, as needed.
            this.schedulerFrameTableAdapter.FillByCurrDate(this.detroitDataSet.SchedulerFrame, DateTime.Today);
            this.dpkDate.Value = DateTime.Today;

            if (fsModule.AccessMode == FormAssessMode.Write)
            {
                this.setFormWriteMode();
            }
            else if (fsModule.AccessMode == FormAssessMode.Read)
            {
                this.setFormReadMode();
            }


            DataTable data = new DataTable();
            data.Columns.Add(new DataColumn("Value", typeof(int)));
            data.Columns.Add(new DataColumn("Description", typeof(string)));
            
            data.Rows.Add(1, "Work");
            data.Rows.Add(0, "Pause");

            this.type.DataSource = data;
            this.type.ValueMember = "Value";
            this.type.DisplayMember = "Description";

            //this.type.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.schedulerFrameTableAdapter.Update(this.detroitDataSet.SchedulerFrame);
            this.Close();
        }

        private void dgrFrames_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow curRow = ((DataGridView)sender).CurrentRow;

            // calculate Finish time
            if (e.ColumnIndex == 4) 
            {
                if (curRow.Cells["Start"].Value.ToString() != "" & curRow.Cells["Length"].Value.ToString() != "") 
                {
                    int length = Convert.ToInt32(curRow.Cells["Length"].Value);
                    DateTime finish_time = Convert.ToDateTime(curRow.Cells["Start"].Value);
                    finish_time = finish_time.AddMinutes(length);
                    curRow.Cells["Finish"].Value = finish_time; // Convert.ToDateTime("24.04.2012 10:00");
                }

                
            }
        }

        private void dgrFrames_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            // save data before insert a new row
            this.schedulerFrameTableAdapter.Update(this.detroitDataSet.SchedulerFrame);

            // get max OrderNum and Finish time
            DetroitDataSet.SchedulerFrameDataTable tmpTable = this.schedulerFrameTableAdapter.GetDataBy(this.dpkDate.Value);

            if (tmpTable.Rows.Count > 0)
            {
                DataRow maxRow = tmpTable.Rows[0];
                e.Row.Cells["OrderNum"].Value = (int)maxRow["OrderNum"] + 1;
                e.Row.Cells["Start"].Value = maxRow["Finish"];
                e.Row.Cells["type"].Value = 0;
            }
            else 
            { 
                //
                e.Row.Cells["OrderNum"].Value = 1;
                e.Row.Cells["Start"].Value = Convert.ToDateTime("0:00:00");
                e.Row.Cells["type"].Value = 0;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataGridViewRow curRow = this.dgrFrames.CurrentRow;
            int curIndex = this.dgrFrames.CurrentRow.Index;
            int maxIndex = this.dgrFrames.RowCount;

            DateTime prevFinish = Convert.ToDateTime(curRow.Cells["Finish"].Value);

            for (int i = curIndex + 1; i < maxIndex -1; i++) 
            {
                DataGridViewRow updRow = this.dgrFrames.Rows[i];
                if (updRow != null)
                {
                    updRow.Cells["Start"].Value = prevFinish;
                    prevFinish = prevFinish.AddMinutes((int)updRow.Cells["Length"].Value);
                    updRow.Cells["Finish"].Value = prevFinish;
                }
            }
           
        }

        private void dpkDate_ValueChanged(object sender, EventArgs e)
        {
            DateTime curDate = Convert.ToDateTime("2012-01-01");
            curDate = ((DateTimePicker)sender).Value;
            this.schedulerFrameTableAdapter.FillByCurrDate(this.detroitDataSet.SchedulerFrame, curDate);
        }

        private void btnCopyFrames_Click(object sender, EventArgs e)
        {
            this.copyFramesDate = this.dpkDate.Value;
            this.copyLineId = this.getLineId();
        }

        private void btnPasteFrames_Click(object sender, EventArgs e)
        {
            DateTime curDate = Convert.ToDateTime("2012-01-01");
            curDate = this.dpkDate.Value;

            int lineId = this.getLineId();
            if(this.copyLineId > 0 && lineId > 0)
            {
                this.schedulerFrameTableAdapter.schedulerFramesPaste(this.copyFramesDate, curDate, this.copyLineId, lineId);
                this.schedulerFrameTableAdapter.FillByCurrDate(this.detroitDataSet.SchedulerFrame, curDate);
                this.dpkDate.Focus();
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }



        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.detroitDataSet.SchedulerFrame.EndInit();
            this.assembLineSchedulerFrameBindingSource.EndEdit();
            this.schedulerFrameTableAdapter.Update(this.detroitDataSet.SchedulerFrame);
            this.dpkDate.Value = this.dpkDate.Value.AddDays(-1);
        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.detroitDataSet.SchedulerFrame.EndInit();
            this.assembLineSchedulerFrameBindingSource.EndEdit();
            this.schedulerFrameTableAdapter.Update(this.detroitDataSet.SchedulerFrame);
            this.dpkDate.Value = this.dpkDate.Value.AddDays(+1);
        }

        private void btnFillDefault_Click(object sender, EventArgs e)
        {
            this.schedulerFrameDefaultTableAdapter.Fill(this.detroitDataSet.SchedulerFrameDefault);

            DateTime curDate = new DateTime(DateTime.Today.Year, 1, 1);
            
            curDate = this.dpkDate.Value;
            if (this.systemLog != null) 
            {
                this.systemLog.LogAlert(AlertType.System, "btnFillDefault_Click()", "this.dpkDate.Value = " + curDate.ToString());
            }

            int lineId = this.getLineId();
            if (lineId > 0)
            {
                this.schedulerFrameDefaultTableAdapter.schedulerFramesPasteDefault(curDate, 1, lineId);
                this.schedulerFrameTableAdapter.FillByCurrDate(this.detroitDataSet.SchedulerFrame, curDate);
                this.dpkDate.Focus();
            }
        }




        private FormSecurityModule fsModule = new FormSecurityModule();

        private void setFormReadMode()
        {
            this.dgrFrames.ReadOnly = true;
            this.btnOk.Enabled = false;
            this.button1.Enabled = false;
            this.btnFillDefault.Enabled = false;
            this.btnCopyFrames.Enabled = false;
            this.btnPasteFrames.Enabled = false;
        }
        private void setFormWriteMode() { }


        private int getLineId() 
        {
            int lineId = 0;
            DataRowView lineRow = (DataRowView)this.assembLineBindingSource.Current;
            if (lineRow != null)
            {
                lineId = Convert.ToInt32(lineRow.Row["Id"]);
            }
            return lineId;
        }

    }
}
