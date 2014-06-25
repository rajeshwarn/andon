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
    public partial class Parts : Form
    {
        public Parts()
        {
            InitializeComponent();
            fsModule.CheckFormAccess(this.Name);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Parts_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'detroitDataSet.Station' table. You can move, or remove it, as needed.
            this.stationTableAdapter.Fill(this.detroitDataSet.Station);
            this.partsTableAdapter.Fill(this.detroitDataSet.Parts);
            
            
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
            this.partsTableAdapter.Update(this.detroitDataSet.Parts);
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
