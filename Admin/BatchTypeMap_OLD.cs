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
    public partial class BatchTypeMap_OLD : Form
    {
        public BatchTypeMap_OLD()
        {
            InitializeComponent();
            fsModule.CheckFormAccess(this.Name);
        }

        private void BatchTypeMap_Load(object sender, EventArgs e)
        {
            this.batchLinesTableAdapter.Fill(this.detroitDataSet.BatchLines);

            this.batchTypeStationsTableAdapter.Fill(this.detroitDataSet.BatchTypeStations);
            this.mapPointsTableAdapter.Fill(this.detroitDataSet.MapPoints);
            this.assembLineTableAdapter.Fill(this.detroitDataSet.AssembLine);
            this.batchTypeMapTableAdapter.Fill(this.detroitDataSet.BatchTypeMap);
            this.batchTypeTableAdapter.Fill(this.detroitDataSet.BatchType);

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
            btnSave_Click(sender, e);
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.batchTypeMapTableAdapter.Update(this.detroitDataSet.BatchTypeMap);
            this.mapPointsTableAdapter.Update(this.detroitDataSet.MapPoints);
            this.batchTypeStationsTableAdapter.Fill(this.detroitDataSet.BatchTypeStations);
            //this.mapPartsTableAdapter.Update(this.detroitDataSet.MapParts);
        }


        private void dataGridView1_Enter(object sender, EventArgs e)
        {
            this.batchTypeMapTableAdapter.Update(this.detroitDataSet.BatchTypeMap);
            this.batchTypeStationsTableAdapter.Fill(this.detroitDataSet.BatchTypeStations);
        }

        private void dataGridView1_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["IsMain"].Value = 1;
        }

        private void dataGridView2_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["NextLineAuto"].Value = 1;
           
        }

        private void dataGridView2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.ColumnIndex == 2)) 
            {
                string aNextLineId = ((DataGridView)sender).CurrentCell.Value.ToString();
                if (aNextLineId == "") 
                {
                    ((DataGridView)sender).CurrentRow.Cells["NextLineAuto"].Value = 0;
                }
               
            }

       }



        private FormSecurityModule fsModule = new FormSecurityModule();

        private void setFormReadMode()
        {
            this.dataGridView1.ReadOnly = true;
            this.dataGridView2.ReadOnly = true;
            //this.dataGridView3.ReadOnly = true;
            this.btnOk.Enabled = false;
            this.btnSave.Enabled = false;
        }
        private void setFormWriteMode() { }

        private void dataGridView2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && this.dataGridView2.CurrentCell.ColumnIndex == 2 && this.dataGridView2.CurrentCell.ReadOnly == false)
            {
                this.dataGridView2.CurrentCell.Value = DBNull.Value;
            }
        }



    }
}
