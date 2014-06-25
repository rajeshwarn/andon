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
    public partial class UncompletedProducts : Form
    {
        public UncompletedProducts()
        {
            InitializeComponent();
            fsModule.CheckFormAccess(this.Name);
        }

        public UncompletedProducts(string databaseConnectionString)
        {
            Properties.Settings.ChangeConnectionString(databaseConnectionString);
            InitializeComponent();
            fsModule.CheckFormAccess(this.Name);
        }

        private void UncompletedProducts_Load(object sender, EventArgs e)
        {
            this.assembLineTableAdapter.Fill(this.detroitDataSet.AssembLine);
            this.batchTypeTableAdapter.Fill(this.detroitDataSet.BatchType);
            this.uncompletedProductTableAdapter.Fill(this.detroitDataSet.UncompletedProduct);

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
            this.uncompletedProductTableAdapter.Fill(this.detroitDataSet.UncompletedProduct);
        }

        private void btnFixup_Click(object sender, EventArgs e)
        {
            DialogResult dResult = MessageBox.Show("Would you like to complete this chassis \n and move it to the next assembly line?", "Fixup and Move?", MessageBoxButtons.YesNo);
            if (dResult == DialogResult.Yes) 
            { 
                this.FixupAndMove();
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.uncompletedProductTableAdapter.Update(this.detroitDataSet.UncompletedProduct);
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FixupAndMove() 
        {
            // fixup ...
            DataRowView curProduct = (DataRowView)this.uncompletedProductBindingSource.Current;
            if (curProduct != null)
            {
                curProduct.Row["TimeRecord"] = DateTime.Now;
                curProduct.Row["State"] = "completed";

                // move ...
                this.uncompletedProductTableAdapter.fixupAndMoveProductInBuffer(Convert.ToInt32(curProduct.Row["Id"]));

                // refresh ...
                this.btnRefresh_Click(this, EventArgs.Empty);
            }
        }




        private FormSecurityModule fsModule = new FormSecurityModule();

        private void setFormReadMode()
        {
            this.dataGridView1.ReadOnly = true;
            this.btnOk.Enabled = false;
            this.btnFixup.Enabled = false;
        }
        private void setFormWriteMode() { }
    }
}
