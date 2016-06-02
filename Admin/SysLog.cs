using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Admin
{
    class SysLog : Log
    {
        public SysLog() :base() {
            this.Load += new EventHandler(this.Log_Load);
            base.btnRefresh.Click += new EventHandler(this.btnRefresh_Click);
        }

        protected void Log_Load(object sender, EventArgs e)
        {
            this.logTableAdapter.FillByDate(this.detroitDataSet.Log, this.dtPickerBegin.Value, this.dtPickerEnd.Value);
            this.dataGridView1.Sort(this.dataGridView1.Columns[0], ListSortDirection.Descending);
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            this.logTableAdapter.FillByDate(this.detroitDataSet.Log, this.dtPickerBegin.Value, this.dtPickerEnd.Value);
        }
    }
}
