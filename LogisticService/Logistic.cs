using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace LogisticService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Logistic : ILogistic, ILogisticCollector
    {
        private List<LogistBatch> batchesOnLines;
        private LogisticInfo logisticInfo;

        public Logistic() 
        {
            this.batchesOnLines = new List<LogistBatch>();
            this.logisticInfo = new LogisticInfo() { LineId = "", NextBatchName = "", TaktsTillNextBatch = 0 };
        }

        public LogistBatch[] GetBatchesOnLine()
        {
            ////LogistBatch[] result;
            ////int arraySize = 2;
            ////result = new LogistBatch[arraySize];
            ////string testRtData = DateTime.Now.Second.ToString();
            ////result[0] = new LogistBatch() { BatchName = "A00" + testRtData };
            ////result[1] = new LogistBatch() { BatchName = "B000" + testRtData };
            ////return result;

            ////return this.batchesOnLines.ToArray();
            return null; 
        }



        public void RenewBatchesOnLine(LogistBatch[] lineBatches)
        {
            // just test connection
            // LogistBatch item = new LogistBatch() { BatchName = "AAA", BatchType = "TC" };
            // this.batchesOnLines.Add(item);

            // clear all batches for this line from Logistic
            if (lineBatches.Count() > 0)
            {
                this.batchesOnLines.RemoveAll(p => p.LineId.Equals(lineBatches[0].LineId));
            }

            // write new batches
            foreach (LogistBatch batch in lineBatches)
            {
                this.batchesOnLines.Add(batch);
            }

        }

        public LogistRequestElem[] GetLogisticRequests()
        {
            throw new NotImplementedException();
        }

        public LogistTailElem[] GetLogisticTails()
        {
            throw new NotImplementedException();
        }

        public LogisticInfo GetLogisticInfo()
        {
            return this.logisticInfo;
        }


        public void RenewNextBatch(LogisticInfo info)
        {
            if (Convert.ToInt16(this.logisticInfo.LineId) == 1) 
            {
                this.logisticInfo = info;
            }
            
        }
    }
}
