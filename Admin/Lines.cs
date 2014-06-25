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
    public partial class LinesForm : Form
    {
        private FormSecurityModule fsModule = new FormSecurityModule();
        private LogProvider logProvider;

        public LinesForm()
        {
            InitializeComponent();
            fsModule.CheckFormAccess(this.Name);
        }

        public LinesForm(LogProvider logProvider)
        {
            InitializeComponent();
            this.fsModule.CheckFormAccess(this.Name);
            this.logProvider = logProvider;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
      
            this.stationTableAdapter.Fill(this.detroitDataSet.Station);
            this.assembLineTableAdapter.Fill(this.detroitDataSet.AssembLine);

            if (fsModule.AccessMode == FormAssessMode.Write) 
            {
                this.setFormWriteMode();
            }
            else if (fsModule.AccessMode == FormAssessMode.Read) 
            { 
                this.setFormReadMode();
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.logRows(this.detroitDataSet.Station);

            this.assembLineTableAdapter.Update(this.detroitDataSet.AssembLine);
            this.stationTableAdapter.Update(this.detroitDataSet.Station);
            this.Close();
        }

        private void logRows(DataTable dataTable) 
        {
            for(int i = 0; i < dataTable.Rows.Count; i++) 
            {
                DataRow row = dataTable.Rows[i];
                if (row.RowState == DataRowState.Added | row.RowState == DataRowState.Modified ) 
                {
                    //this.logProvider.LogAlert(0, "Application", this.GetType().ToString(), dataTable.TableName.ToString(),
                    //    row.RowState.ToString() + ": "
                    //    + row[0].ToString() + ", " + row[1].ToString() 
                    //    , User.Name);
                } 
                else if ( row.RowState == DataRowState.Deleted )
                {
                    //this.logProvider.LogAlert(0, "Application", this.GetType().ToString(), dataTable.TableName.ToString(),
                    //    row.RowState.ToString() + ": "
                    //    + row.ToString()
                    //    , User.Name);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.assembLineTableAdapter.Update(this.detroitDataSet.AssembLine);
            this.stationTableAdapter.Update(this.detroitDataSet.Station);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView2_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            //e.Row.Cells["IsMain"].Value = 1;
            e.Row.Cells["BufferSize"].Value = 0;
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }


        private void setFormReadMode() 
        {
            this.dataGridView1.ReadOnly = true;
            this.dataGridView2.ReadOnly = true;
            this.btnOk.Enabled = false;
            this.btnSave.Enabled = false;
        }

        private void setFormWriteMode() { }

        private void dataGridView2_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
        }





    }
}
