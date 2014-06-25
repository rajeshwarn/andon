using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Security
{
    public partial class Logon : Form
    {
        private SecurityMgr owner;
        public Logon(SecurityMgr owner)
        {
            InitializeComponent();
            this.owner = owner;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Logon_Load(object sender, EventArgs e)
        {
            this.ActiveControl = this.tbUserName;
            this.AcceptButton = this.btnOk;
            this.CancelButton = this.btnCancel;

            this.tbUserName.Text = "admin";
            this.tbUserPassword.Text = "1";
        }
    }
}
