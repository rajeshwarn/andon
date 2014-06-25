using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Admin
{
    public partial class WorkScheduleRepForm : Form
    {
        public WorkScheduleRepForm()
        {
            InitializeComponent();
        }

        private void WorkScheduleRepForm_Load(object sender, EventArgs e)
        {

            this.repWS.RefreshReport();
        }
    }
}
