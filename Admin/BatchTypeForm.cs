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
    public partial class BatchTypeForm : Form
    {
        public BatchTypeForm()
        {
            InitializeComponent();
            fsModule.CheckFormAccess(this.Name);
        }

        private void BatchTypeForm_Load(object sender, EventArgs e)
        {
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.batchTypeTableAdapter.Update(this.detroitDataSet.BatchType);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.batchTypeTableAdapter.Update(this.detroitDataSet.BatchType);
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // first of all save modified data!
            this.batchTypeTableAdapter.Update(this.detroitDataSet.BatchType);

            // create a new copy of selected batch with map 
            DataRowView curLine = (DataRowView)this.batchTypeBindingSource.Current;
            int orign_BatchId = Convert.ToInt32(curLine.Row["Id"]);
            int? new_BatchId = 0;
            if (orign_BatchId > 0)
            {
                this.batchTypeTableAdapter.CopyBatchType(orign_BatchId, ref new_BatchId);
                this.batchTypeTableAdapter.Fill(this.detroitDataSet.BatchType);
                int indx = this.batchTypeBindingSource.Find("Id", (int)new_BatchId);
                this.batchTypeBindingSource.Position = indx;
            }

        }

        private FormSecurityModule fsModule = new FormSecurityModule();
        private void setFormReadMode()
        {
            this.dataGridView1.ReadOnly = true;
            this.btnOk.Enabled = false;
            this.btnSave.Enabled = false;
        }
        private void setFormWriteMode() { }

        private void BatchTypeForm_Shown(object sender, EventArgs e)
        {
            string filter = "Archived <> 1";
            this.batchTypeBindingSource.Filter = filter;
            this.scrollToTheLastRow();
        }

        private void cbxShowArchived_CheckedChanged(object sender, EventArgs e)
        {
            string filter = "";
            if(!(sender as CheckBox).Checked) 
            {
                filter = "Archived <> 1";
            }
            this.batchTypeBindingSource.Filter = filter;
  
            if (this.dataGridView1.Rows.Count > 0)
            {
                this.scrollToTheLastRow();
            }
        }

        private void scrollToTheLastRow() 
        {
            this.dataGridView1.ClearSelection();//If you want

            int nRowIndex = dataGridView1.Rows.Count - 1;
            int nColumnIndex = 1;

            dataGridView1.Rows[nRowIndex].Selected = true;
            dataGridView1.Rows[nRowIndex].Cells[nColumnIndex].Selected = true;

            //In case if you want to scroll down as well.
            dataGridView1.FirstDisplayedScrollingRowIndex = nRowIndex; 
        }

        private void dataGridView1_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["Archived"].Value = 0;
        }

    }
}
