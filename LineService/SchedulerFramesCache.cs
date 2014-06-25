using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace LineService
{
    class SchedulerFramesCache
    {
        private static DataRow[] rows;

        public DataRow[] Rows
        {
            get { return rows; }
            set { rows = value; }
        }
    }
}
