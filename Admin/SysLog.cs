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
            //
            this.Load += new EventHandler(this.Log_Load);
        }

        protected void Log_Load(object sender, EventArgs e)
        {
            this.logTableAdapter.FillAll(this.detroitDataSet.Log);
            this.dataGridView1.Sort(this.dataGridView1.Columns[0], ListSortDirection.Descending);
        }
    }
}
