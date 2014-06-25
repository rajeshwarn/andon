using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using AppLog;

namespace LineService
{
    class LineQueue : Queue<Batch>
    {
        private string lineName = "default";
        private DetroitDataSet detroitDataSet;
        private DetroitDataSetTableAdapters.LineQueueExtTableAdapter lineQueueTableAdapter;
        private DetroitDataSetTableAdapters.BatchTableAdapter batchTableAdapter;
        private Hashtable cache = new Hashtable();
        private LogProvider myLog;

        public LineQueue(DetroitDataSet detroit, LogProvider logProvider) 
        {
            this.myLog = logProvider;
            this.detroitDataSet = detroit;
            
            //...
            // connect to database
            try
            {
                this.myLog.LogAlert(AlertType.System, this.lineName, this.GetType().ToString(), "LineQueue()",
                    "Connecting to the database ... " + LineService.Properties.Settings.Default.DetroitConnectionString.ToString()
                    , "system");

                this.lineQueueTableAdapter = new DetroitDataSetTableAdapters.LineQueueExtTableAdapter();
                this.lineQueueTableAdapter.FillByLineId(this.detroitDataSet.LineQueueExt, this.detroitDataSet.LineId);
                this.myLog.LogAlert(AlertType.System, this.lineName, this.GetType().ToString(), "LineQueue()",
                    "Connection string: " + this.lineQueueTableAdapter.Connection.ConnectionString.ToString()
                    , "system");

                // read Bathes from LineQueue in routine
                // add every Batch

                for (int i = 0; i <= this.detroitDataSet.LineQueueExt.Rows.Count - 1; i++)
                {
                    DataRow queueRow = this.detroitDataSet.LineQueueExt.Rows[i];
                    Batch newBatch = new Batch(
                        null
                        , queueRow["Nummer"].ToString() 
                        , queueRow["BatchType_Name"].ToString()
                        , (int)queueRow["Capacity"]
                        , (int)queueRow["BatchId"]
                        , (int)queueRow["BatchTypeId"]
                        , (int)queueRow["Length"]
                        , (int)queueRow["Takt"]
                    );

                    newBatch.PutInQueue();
                    this.Enqueue(newBatch);
                    this.cache.Add(newBatch.Id.ToString(), newBatch);
                }
            }
            catch (System.Data.SqlClient.SqlException e) 
            {
                this.myLog.LogAlert(AlertType.Error, this.lineName, this.GetType().ToString(), "LineQueue()",
                    e.Message + ", Line number = " + e.LineNumber.ToString()
                    , "system");
            }

        }
        public new Batch Dequeue() 
        {
            // Remove batch from database, from LineQueue 
            // (in debug mode set "_deleted" instead of deleting)

            Batch firstBatch = this.Peek();
            if (firstBatch != null) 
            {
                //this.lineQueueTableAdapter.UpdateStatus("_deleted", firstBatch.Id);
                this.lineQueueTableAdapter.DeleteByBatchId(firstBatch.Id);
            }

            base.Dequeue();
            this.cache.Remove(firstBatch.Id.ToString());
            return firstBatch;
        }

        public void Refresh() 
        {
            this.cache.Clear();
            base.Clear();

            this.lineQueueTableAdapter.FillByLineId(this.detroitDataSet.LineQueueExt, this.detroitDataSet.LineId);
            for (int i = 0; i <= this.detroitDataSet.LineQueueExt.Rows.Count - 1; i++)
            {
                DataRow queueRow = this.detroitDataSet.LineQueueExt.Rows[i];
                
                Batch newBatch = new Batch(
                        null
                        , queueRow["Nummer"].ToString()
                        , queueRow["BatchType_Name"].ToString()
                        , (int)queueRow["Capacity"]
                        , (int)queueRow["BatchId"]
                        , (int)queueRow["BatchTypeId"]
                        , (int)queueRow["Length"]
                        , (int)queueRow["Takt"]
                    );
                    newBatch.PutInQueue();
                    this.Enqueue(newBatch);
                    this.cache.Add(newBatch.Id.ToString(), newBatch);
                
            }
        }

        public bool ContainsBatch(string batchIdKey) 
        {
            bool result = cache.Contains(batchIdKey);
            return result;
        }

    }
}
