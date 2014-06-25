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
    public partial class Stations : Form
    {

        private FormSecurityModule fsModule = new FormSecurityModule();
        private LogProvider logProvider;


        public Stations()
        {
            InitializeComponent();
            this.fsModule.CheckFormAccess(this.Name);
            this.logProvider = logProvider;
        }

        private void Stations_Load(object sender, EventArgs e)
        {
            this.assembLineTableAdapter.Fill(this.detroitDataSet1.AssembLine);
            this.stationTableAdapter.Fill(this.detroitDataSet.Station);

            if (fsModule.AccessMode == FormAssessMode.Write)
            {
                this.setFormWriteMode();
            }
            else if (fsModule.AccessMode == FormAssessMode.Read)
            {
                this.setFormReadMode();
            }
        }

        private void dataGridView1_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["BufferSize"].Value = 0;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.stationTableAdapter.Update(this.detroitDataSet.Station);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.stationTableAdapter.Update(this.detroitDataSet.Station);
            this.Close();
        }


        private void setFormReadMode()
        {
            this.dataGridView1.ReadOnly = true;
            //this.dataGridView2.ReadOnly = true;
            this.btnOk.Enabled = false;
            this.btnSave.Enabled = false;
        }

        private void setFormWriteMode() { }
    }
}
