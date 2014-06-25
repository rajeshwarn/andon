using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Security;
using System.Collections;
using Security;

namespace Security
{
    public partial class UserPermitions : Form
    {
        private SecurityMgr securityMgr;
        public UserPermitions(SecurityMgr owner)
        {
            InitializeComponent();
            this.securityMgr = owner;
            fsModule.CheckFormAccess(this.Name);
        }

        private void UserPermitions_Load(object sender, EventArgs e)
        {
            this.usersTableAdapter.Fill(this.detroit.Users);
            this.clbUsersPermissions.Items.AddRange(this.securityMgr.GetPermissionsList());

            this.cbUser.SelectedValue = User.Name;
            Hashtable permissions = this.securityMgr.GetUserPermissions(this.cbUser.Text);
            for (int i = 0; i < this.clbUsersPermissions.Items.Count; i++) 
            {
                if (permissions.ContainsKey(this.clbUsersPermissions.Items[i])) 
                {
                    this.clbUsersPermissions.SetItemChecked(i, true);
                }
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

        private void btnDeselectAll1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.clbUsersPermissions.Items.Count; i++)
            {
                this.clbUsersPermissions.SetItemChecked(i, false);
            }
        }

        private void btnSelectAll1_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < this.clbUsersPermissions.Items.Count; i++)
            {
                this.clbUsersPermissions.SetItemChecked(i, true);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.btnSave_Click(sender, e);
            this.Close();
        }

        private void cbUser_SelectedValueChanged(object sender, EventArgs e)
        {
            
            this.btnDeselectAll1_Click(sender, e);

            if (this.cbUser.Text != "")
            {
                Hashtable permissions = this.securityMgr.GetUserPermissions(this.cbUser.Text);
                for (int i = 0; i < this.clbUsersPermissions.Items.Count; i++)
                {
                    if (permissions.ContainsKey(this.clbUsersPermissions.Items[i]))
                    {
                        this.clbUsersPermissions.SetItemChecked(i, true);
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string[] checkedItems = new string[clbUsersPermissions.CheckedItems.Count];
            clbUsersPermissions.CheckedItems.CopyTo(checkedItems, 0);
            this.securityMgr.SetPermitions(this.cbUser.Text, checkedItems);
            this.securityMgr.ReloadUserPermissions();
        }




        private FormSecurityModule fsModule = new FormSecurityModule();

        private void setFormReadMode()
        {
            this.clbUsersPermissions.Enabled = false;
            this.btnSelectAll1.Enabled = false;
            this.btnDeselectAll1.Enabled = false;
            this.btnOk.Enabled = false;
            this.btnSave.Enabled = false;
        }
        private void setFormWriteMode() { }
    }
}
