using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Security;

namespace Security
{
    public partial class NewUser : Form
    {
        private SecurityMgr owner;
        public NewUser(SecurityMgr owner)
        {
            InitializeComponent();
            this.owner = owner;
            fsModule.CheckFormAccess(this.Name);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.owner.CreateNewUser(tbUserName.Text, tbUserPassword.Text, tbSalt.Text);
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void NewUser_Load(object sender, EventArgs e)
        {
            this.ActiveControl = this.tbUserName;
            this.AcceptButton = this.btnOk;
            this.CancelButton = this.btnCancel;

            if (fsModule.AccessMode == FormAssessMode.Write)
            {
                this.setFormWriteMode();
            }
            else if (fsModule.AccessMode == FormAssessMode.Read)
            {
                this.setFormReadMode();
            }
        }

        private FormSecurityModule fsModule = new FormSecurityModule();
        private void setFormReadMode()
        {
            //this.dataGridView1.ReadOnly = true;
            this.btnOk.Enabled = false;
            //this.btnSave.Enabled = false;
        }
        private void setFormWriteMode() { }
    }
}
