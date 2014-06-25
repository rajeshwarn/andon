using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Security;

namespace Admin
{
    public partial class ScheduleFramesDefault : Form
    {
        public ScheduleFramesDefault()
        {
            InitializeComponent();
            fsModule.CheckFormAccess(this.Name);
        }

        private void ScheduleFramesDefault_Load(object sender, EventArgs e)
        {
            this.schedulerFrameDefaultTableAdapter.ClearBeforeFill = true;
            this.schedulerFrameDefaultTableAdapter.Fill(this.detroitDataSet.SchedulerFrameDefault);
            
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
            //this.detroitDataSet.SchedulerFrameDefault.AcceptChanges();
            this.schedulerFrameDefaultTableAdapter.Update(this.detroitDataSet.SchedulerFrameDefault);
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
            this.schedulerFrameDefaultTableAdapter.Update(this.detroitDataSet.SchedulerFrameDefault);

            // get max OrderNum and Finish time
            DetroitDataSet.SchedulerFrameDefaultDataTable tmpTable = this.schedulerFrameDefaultTableAdapter.GetData();
            int num = 1;
            DateTime startDate = DateTime.Today;
            if (tmpTable.Rows.Count != 0)
            {
                DataRow maxRow = tmpTable.Rows[tmpTable.Rows.Count-1];
                num = (int)maxRow["OrderNum"] + 1;
                startDate = (DateTime)maxRow["Finish"];
            }
            e.Row.Cells["OrderNum"].Value = num;
            e.Row.Cells["Start"].Value = startDate;
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




        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private FormSecurityModule fsModule = new FormSecurityModule();

        private void setFormReadMode()
        {
            this.dgrFrames.ReadOnly = true;
            this.btnOk.Enabled = false;
            this.button1.Enabled = false;
        }
        private void setFormWriteMode() { }



    }
}
