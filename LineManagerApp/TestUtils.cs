using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wsLineHostController;
using Security;

namespace LineManagerApp
{
    class TestUtils
    {
        private int lineId;
        public LineManager testLineManager;

        public TestUtils(int LineId) 
        { 
            this.lineId = LineId;
        }

        public void StartHostLineService()
        {

            if (this.testLineManager == null)
            {
                this.testLineManager = new LineManager(this.lineId);
            }

            if (!this.testLineManager.IsServiceRunning)
            {
                this.testLineManager.RunService();
            }
        }

        public void StopHostLineService() 
        {
            if (this.testLineManager != null)
            {
                if (this.testLineManager.IsServiceRunning)
                {
                    this.testLineManager.BreakService();
                }
            }   
        }

    }
}
