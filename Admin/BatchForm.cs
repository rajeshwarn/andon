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
    public partial class BatchForm : Form
    {
        public BatchForm()
        {
            InitializeComponent();
            fsModule.CheckFormAccess(this.Name);
        }

        private void BatchForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'detroitDataSet.BatchType' table. You can move, or remove it, as needed.
            this.batchTypeTableAdapter.Fill(this.detroitDataSet.BatchType);
            this.batchTableAdapter.Fill(this.detroitDataSet.Batch);

            int rowsCount = this.dataGridView1.Rows.Count;
            if (rowsCount > 0)
            {
                this.dataGridView1.CurrentCell = this.dataGridView1.Rows[rowsCount - 1].Cells[0];
            }

            if (fsModule.AccessMode == FormAssessMode.Write)
            {
                this.setFormWriteMode();
            }
            else if (fsModule.AccessMode == FormAssessMode.Read)
            {
                this.setFormReadMode();
            }
        }

        private void batchBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.batchTableAdapter.Update(this.detroitDataSet.Batch);
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

        private void dataGridView1_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells[4].Value = "New";
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // if BatchType changed - change Capacity from it
            if ((e.ColumnIndex == 1))
            {
                int defaultCapacity = 0;
                defaultCapacity = (int)(((DataRowView)this.batchTypeBindingSource.Current).Row["Capacity"]);
                ((DataGridView)sender).CurrentRow.Cells["Capacity"].Value = defaultCapacity;
            }
        }


        private FormSecurityModule fsModule = new FormSecurityModule();

        private void setFormReadMode()
        {
            this.dataGridView1.ReadOnly = true;
            //this.dataGridView2.ReadOnly = true;
            //this.dataGridView3.ReadOnly = true;
            this.btnOk.Enabled = false;
            this.btnSave.Enabled = false;
        }
        private void setFormWriteMode() { }

    }
}
