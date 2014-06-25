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
    public partial class Stock : Form
    {

        public Stock()
        {
            InitializeComponent();
            fsModule.CheckFormAccess(this.Name);
        }

        public Stock(string databaseConnectionString)
        {
            Properties.Settings.ChangeConnectionString(databaseConnectionString);
            InitializeComponent();
            fsModule.CheckFormAccess(this.Name);
        }

        private void Stock_Load(object sender, EventArgs e)
        {
            this.productStockTableAdapter.Fill(this.detroitDataSet.ProductStock);
            this.dataGridView1.Sort(this.dataGridView1.Columns[0], ListSortDirection.Descending);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            this.productStockTableAdapter.Fill(this.detroitDataSet.ProductStock);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private FormSecurityModule fsModule = new FormSecurityModule();

        private void setFormReadMode()
        {
            this.dataGridView1.ReadOnly = true;
            //this.dataGridView2.ReadOnly = true;
            //this.dataGridView3.ReadOnly = true;
            this.btnOk.Enabled = false;
            //this.btnSave.Enabled = false;
        }
        private void setFormWriteMode() { }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.CurrentCell != null)
            {
                DataGridViewColumn currentViewColumn = this.dataGridView1.CurrentCell.OwningColumn;

                // the first column is a datetime column, it doesn't support "like %" comparision!!
                if (currentViewColumn.Index > 0)
                {
                    string columnName = currentViewColumn.DataPropertyName;
                    string filterSelect = columnName + " like '%" + this.tbxFilter.Text + "%'";
                    this.productStockBindingSource.Filter = filterSelect;
                    if (this.dataGridView1.Rows.Count > 0) 
                    { 
                        this.dataGridView1.ClearSelection();
                        this.dataGridView1.CurrentCell = this.dataGridView1[currentViewColumn.Index, 0];  
                    }
                }
            }

        }

        private void tbxFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) 
            {
                this.btnFilter_Click(sender, new EventArgs());
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
