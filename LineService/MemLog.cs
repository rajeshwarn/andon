using System;
using System.Collections.Generic;
using System.Linq;
using AppLog;

namespace LineService
{

    public class MemLog : LogProvider
    {
        private DataSet1.MemLogDataTable memErrorDataTable;
        private Queue<LogMessage> memErrorLogData;
        private int memLogSize = 300;

        public MemLog (LogType LogType, string Path, bool Overwrite) 
            :base(LogType, Path, Overwrite)
        {
            this.memErrorLogData = new Queue<LogMessage>();
            this.memErrorDataTable = new DataSet1.MemLogDataTable();
        }

        protected override void LogSQLAlert(AlertType alertType, string line, string objectType, string objName, string message, string userName)
        {
            try {
                if (line == null)
                {
                    line = "NA";
                }

 	            base.LogSQLAlert(alertType, line, objectType, objName, message, userName);

                LogMessage messageObj = new LogMessage()
                {
                    AlertType = (int)alertType,
                    Line = line,
                    ObjectType = objectType,
                    ObjName = objName,
                    MessageString = message,
                    UserName = userName,
                    EventTime = DateTime.Now
                };


                if (alertType == AlertType.Info)
                {
                    this.writeToQueue(messageObj);
                    this.writeToDatatable(messageObj);
                }

            }
            catch (Exception ex)
            {
                this.LogFileAlert(AlertType.Error, "NA", ex.TargetSite.ToString(), ex.Source.ToString(), ex.ToString(), "system");
            }


        }

        private void writeToDatatable(LogMessage messageObj)
        {
            try
            {
                if (this.memErrorDataTable.Count > this.memLogSize)
                {
                   DataSet1.MemLogRow aRow = this.memErrorDataTable.FirstOrDefault();
                   if (aRow != null) 
                   {
                       aRow.Delete();
                       this.memErrorDataTable.AcceptChanges();
                   }
                }

                this.memErrorDataTable.AddMemLogRow(  messageObj.EventTime,
                                                    2,
                                                    messageObj.Line,
                                                    messageObj.ObjectType,
                                                    messageObj.ObjName,
                                                    messageObj.MessageString,
                                                    messageObj.UserName,
                                                    "",
                                                    0);
            }
            catch (Exception ex)
            {
                this.LogFileAlert(AlertType.Error, "NA", ex.TargetSite.ToString(), ex.Source.ToString(), ex.ToString(), "system");
            }
        }

        private void writeToQueue(LogMessage messageObj) 
        { 
            try {
                if (this.memErrorLogData.Count < this.memLogSize)
                {
                    this.memErrorLogData.Enqueue(messageObj);
                }
                else
                {
                    this.memErrorLogData.Dequeue();
                    this.memErrorLogData.Enqueue(messageObj);
                }
            }
            catch (Exception ex) {
                this.LogFileAlert(AlertType.Error, "NA", ex.TargetSite.ToString(), ex.Source.ToString(), ex.ToString(), "system");
            }
        }

        public DataSet1.MemLogDataTable ErrorData { get { return this.memErrorDataTable;  } }
        public List<LogMessage> ErrorList { get { return this.memErrorLogData.ToList(); } }
            
    }
}
