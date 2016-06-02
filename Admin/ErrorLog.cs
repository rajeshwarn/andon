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
    public partial class ErrorLog : Form
    {
        private Timer timer;
        public string LineId; 
        public ErrorLog()
        {
            InitializeComponent();
            fsModule.CheckFormAccess(this.Name);
            //this.timer = new Timer();
            //this.timer.Interval = 500;
            //this.timer.Enabled = true;
            //this.timer.Tick += new EventHandler(timer_Tick);
        }

        public ErrorLog(string databaseConnectionString)
        {
            Properties.Settings.ChangeConnectionString(databaseConnectionString);
            InitializeComponent();
            fsModule.CheckFormAccess(this.Name);
            //this.timer = new Timer();
            //this.timer.Interval = 500;
            //this.timer.Enabled = true;
            //this.timer.Tick += new EventHandler(timer_Tick);
        }

        protected void Log_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'detroitDataSet.ErrorLog' table. You can move, or remove it, as needed.
            this.errorLogTableAdapter.FillByLine(this.detroitDataSet.ErrorLog);
            //this.dataGridView1.Sort(this.dataGridView1.Columns[0], ListSortDirection.Descending);
     
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.errorLogTableAdapter.Update(this.detroitDataSet.ErrorLog);
            this.Close();
        }

        private FormSecurityModule fsModule = new FormSecurityModule();

        private void setFormReadMode()
        {
            //this.dataGridView1.ReadOnly = true;
            //this.dataGridView2.ReadOnly = true;
            //this.dataGridView3.ReadOnly = true;
            this.btnOk.Enabled = false;
            //this.btnSave.Enabled = false;
        }
        private void setFormWriteMode() { }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            //this.logTableAdapter.Fill(this.detroitDataSet.Log);
        }

        private void btnCheckAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgv in this.dataGridView1.Rows)
            {
                dgv.Cells["AcknowMark"].Value = true;
            }
        }


        //private void timer_Tick(object sender, EventArgs e) 
        //{
        //    if (this.chbAutoRefresh.Checked)
        //    {
        //        this.btnRefresh_Click(sender, e);
        //    }
        //}

    }
}
