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
    public partial class ProductBuffer : Form
    {
        public ProductBuffer()
        {
            InitializeComponent();
            fsModule.CheckFormAccess(this.Name);
        }

        public ProductBuffer(string databaseConnectionString) 
        {
            Properties.Settings.ChangeConnectionString(databaseConnectionString);
            InitializeComponent();
            fsModule.CheckFormAccess(this.Name);
        }


        private void ProductBuffer_Load(object sender, EventArgs e)
        {
            this.assembLineTableAdapter.Fill(this.detroitDataSet.AssembLine);
            this.batchTypeTableAdapter.Fill(this.detroitDataSet.BatchType);
            this.productBufferTableAdapter.Fill(this.detroitDataSet.ProductBuffer);

            if (fsModule.AccessMode == FormAssessMode.Write)
            {
                this.setFormWriteMode();
            }
            else if (fsModule.AccessMode == FormAssessMode.Read)
            {
                this.setFormReadMode();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            this.productBufferTableAdapter.Fill(this.detroitDataSet.ProductBuffer);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //this.productBufferTableAdapter.Update(this.detroitDataSet.ProductBuffer);
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
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

    }
}
