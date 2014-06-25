using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LineService
{
    class Planner
    {
        int lineId;

        public Planner(int lineId) 
        {
            this.lineId = lineId;
        }

        public Dictionary<string, PlanRegisterRecord> MakeDayPlan() 
        {
            return null;
        }

        public Dictionary<string, PlanRegisterRecord> MakeMonthPlan()
        {
            return null;
        }
    }
}
