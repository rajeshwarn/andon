using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.ComponentModel;
using System.Collections;
using System.Timers;

namespace AppLog
{
    public enum LogType{ File = 1, SQL = 2 }

    public enum AlertType { System = 0, Info = 1, Error = 2, Warning = 3 }

    public class Message : Object 
    {
        public AlertType AlertType;
        public string Line;
        public string ObjectType; 
        public string ObjName; 
        public string MessageString;
        public string UserName;
        public DateTime EventTime;

        public Message(AlertType alertType, string line, string objectType, string objName, string message, string userName, DateTime eventTime) 
        { 
            AlertType = alertType;
            Line = line;
            ObjectType = objectType;
            ObjName = objName;
            MessageString = message;
            UserName = userName;
            EventTime = eventTime;
        }
    }

    public class LogProvider : Object
    {
        private LogType logType;
        public SqlConnection cn;
        private string fileLogPath;
        private StreamWriter w;
        private Object thisLock = new Object();
        private Queue<Message> messageBuffer = new Queue<Message>();
        private int bufferSize = 100000;
        private string datePatt = @"dd/MM/yyyy HH:mm:ss.fff";
        private string sqlDatePatt = @"yyyy-MM-dd HH:mm:ss.fff";
        private string connString = "";
        private Timer logTimer;
        private int timerInt = 10000;

        private void initSQLLog(string connectionString)
        {
            this.connString = connectionString;
            Console.WriteLine("Connecting to datasource: " + connectionString);

            string cnStr = connectionString;
            if (cnStr == "" | cnStr.Length == 0)
            {
                cnStr = ConfigurationManager.AppSettings["cnStr"];
            }

            this.connString = cnStr;
        }

        private void initFileLog(string Path, bool Overwrite)
        {
            this.fileLogPath = "log.txt";
            if (Path != "")
            {
                this.fileLogPath = Path;
            }
            if (Overwrite)
            {
                w = new StreamWriter(this.fileLogPath);
                w.WriteLine("*** Andon log file ***");
                w.WriteLine("============================");
                w.WriteLine("" + DateTime.Now.ToString());
                w.Close();
            }
            else
            {
                w = File.AppendText(this.fileLogPath);

                w.WriteLine("");
                w.WriteLine("*** Andon log file ***");
                w.WriteLine("============================");
                w.WriteLine("> system started: " + DateTime.Now.ToString());
                w.WriteLine("");

                w.Close();
            }

        }

        public LogProvider()
        {
            this.logTimer = new Timer(timerInt);
            this.logTimer.Elapsed += new ElapsedEventHandler(logTimer_handler);
            this.logTimer.Enabled = true;
            initSQLLog(""); // by default
        }

        public LogProvider(LogType LogType, string Path, bool Overwrite)
        {
            this.logTimer = new Timer(timerInt);
            this.logTimer.Enabled = true;
            this.logTimer.Elapsed += new ElapsedEventHandler(logTimer_handler);

            this.logType = LogType;
            switch (LogType)
            {
                case LogType.File:
                    initFileLog(Path, Overwrite);
                    break;
                case LogType.SQL:
                    initSQLLog(Path);
                    this.initFileLog("log_system.txt", false);
                    break;
                default:
                    initFileLog(Path, Overwrite);
                    this.logType = LogType.File;
                    break;
            }
        }

        private  void logTimer_handler(object sender, EventArgs e) 
        {
            this.sql_handler();
        }

        private void sql_handler() 
        {
            if (this.logType == LogType.SQL)
            {
                SqlCommand cmd;
                try
                {
                    this.cn = new SqlConnection();
                    this.cn.ConnectionString = this.connString;

                    Console.WriteLine(DateTime.Now.ToString(datePatt) + " - trying to open connection ...");
                    this.cn.Open();
                    Console.WriteLine(DateTime.Now.ToString(datePatt) + " - connection opened.");

                    if (this.cn.State == ConnectionState.Open)
                    {
                        cmd = new SqlCommand("", this.cn);
                        this.writeBufferToDatabase(cmd);
                    }

                    if (this.cn != null && this.cn.State == ConnectionState.Open)
                    {
                        this.cn.Close();
                        this.cn.Dispose();
                        Console.WriteLine(DateTime.Now.ToString(datePatt) + " - close connection.");
                    }
                }
                catch (Exception ex)
                {
                    this.LogFileAlert(AlertType.Error, "NA", ex.TargetSite.ToString(), ex.Source.ToString(), ex.ToString(), "system");
                }
            }        
        }

        private void writeBufferToDatabase(SqlCommand cmd)
        {
            try
            {

                if (this.messageBuffer == null)
                    this.messageBuffer = new Queue<Message>();

                // read and try to write into SQL all messages from Log buffer


                while (this.messageBuffer.Count > 0)
                {
                    // get first row from buffer
                    Message aMessage = this.messageBuffer.Peek();
                    if (aMessage == null)
                        throw new NullReferenceException("messageBuffer.Peek() returns null");

                    if (aMessage.EventTime == null)
                        aMessage.EventTime = new DateTime(DateTime.Now.Ticks);

                    if (aMessage.Line == null)
                        aMessage.Line = "0";

                    if (aMessage.ObjectType == null)
                        aMessage.ObjectType = "null";

                      // make SQL command
                    string sql = string.Format("insert into Log" +
                        "([TimeStamp], [AlertType],[Line],[ObjectType],[ObjectNum],[Message],[UserName]) values " +
                        "('{0}', {1}, '{2}', '{3}', '{4}', '{5}', '{6}')"
                        , aMessage.EventTime.ToString(sqlDatePatt)
                        , (int)(aMessage.AlertType)
                        , aMessage.Line
                        , aMessage.ObjectType
                        , aMessage.ObjName
                        , aMessage.MessageString
                        , aMessage.UserName
                        );

                    if (cmd == null)
                        throw new NullReferenceException("Input variable 'cmd' is null");

                    cmd.CommandText = sql;

                    int rowsNumber = 0;
                    if (this.cn != null && this.cn.State == ConnectionState.Open)
                    {
                        rowsNumber = cmd.ExecuteNonQuery();
                    }

                    // if no exception then delete row from  buffer
                    if (rowsNumber > 0 && this.messageBuffer.Count > 0)
                    {
                        this.messageBuffer.Dequeue();
                    }
                }
            }
            catch (NullReferenceException ex) 
            {
                this.LogSQLAlert(AlertType.Error, "NA", ex.TargetSite.ToString(), ex.Source.ToString(), ex.ToString(), "system");

                this.LogFileAlert(AlertType.Error, "NA", ex.TargetSite.ToString(), ex.Source.ToString(), ex.ToString(), "system");
                this.LogFileAlert(AlertType.Error, "NA", ex.TargetSite.ToString(), ex.Source.ToString(), ex.InnerException.ToString(), "system");
                this.LogFileAlert(AlertType.Error, "NA", ex.TargetSite.ToString(), ex.Source.ToString(), ex.Data.ToString(), "system");
                this.LogFileAlert(AlertType.Error, "NA", ex.TargetSite.ToString(), ex.Source.ToString(), ex.StackTrace.ToString(), "system");
            }

            catch (Exception ex)
            {
                this.LogFileAlert(AlertType.Error, "NA", ex.TargetSite.ToString(), ex.Source.ToString(), ex.ToString(), "system");
            }
        }
        
        public void LogAlert(AlertType alertType, string objName, string message)
        {
            this.LogAlert(alertType, objName, message, "NA");
        }

        public void LogAlert(AlertType alertType, string objName, string message, string userName)
        {
            try
            {
                switch (logType)
                {
                    case LogType.File:
                        LogFileAlert(alertType, "NA", "NA", objName, message, userName);
                        break;
                    case LogType.SQL:
                        LogSQLAlert(alertType, "NA", "NA", objName, message, userName);
                        break;
                    default:

                        break;
                }

                
            }
            catch (Exception ex)
            {
                this.LogFileAlert(AlertType.Error, "NA", "NA", this.GetType().ToString(), ex.Message, "system");
            }
        }

        public void LogAlert(AlertType alertType, string lineName, string objType, string objName, string message, string userName)
        {
            try
            {
                switch (logType)
                {
                    case LogType.File:
                        LogFileAlert(alertType, lineName, objType, objName, message, userName);
                        break;
                    case LogType.SQL:
                        LogSQLAlert(alertType, lineName, objType, objName, message, userName);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                this.LogFileAlert(AlertType.Error, "NA", "NA", this.GetType().ToString(), ex.Message, "system");
            }

        }



        protected virtual void LogSQLAlert(AlertType alertType, string line, string objectType, string objName, string message, string userName)
        {
            // refine message text
            message = message.Replace("'", "''");
            int length = message.Length;
            if (length > 2048)
            {
                length = 2048;
            }
            message = message.Substring(0, length);

            // add message to the buffer fist
            Message messageObj = new Message(alertType, line, objectType, objName, message, userName, DateTime.Now);
            if (this.messageBuffer.Count < this.bufferSize)
            {
                this.messageBuffer.Enqueue(messageObj);
            }
            else
            {
                this.LogFileAlert(AlertType.Error, "NA", "NA", this.GetType().ToString(), "MessageBuffer size exceeded. Count="+this.messageBuffer.Count.ToString() , "system");  
            }

            //SqlCommand cmd;
            //try
            //{
            //    //Console.WriteLine(DateTime.Now.ToString(datePatt) + " - open connection for logging... , buffer = " + this.messageBuffer.Count.ToString());
            //    if (this.cn != null && this.cn.State == ConnectionState.Open)
            //    {
            //        this.cn.Close();
            //    }

            //    if (this.cn == null) {
            //        this.initSQLLog(this.connString);
            //    }

            //    if (this.cn != null && this.cn.State == ConnectionState.Closed) {
            //        this.cn.Open(); 
            //    }

            //    cmd = new SqlCommand("", this.cn);

            //    // read and try to write into SQL all messages from Log buffer
            //    while (this.messageBuffer.Count > 0) 
            //    {
            //        // get first row from buffer
            //        Message aMessage = this.messageBuffer.Peek();
            //        //Console.WriteLine("aMessage.EventTime.ToString(sqlDatePatt) = " + aMessage.EventTime.ToString(sqlDatePatt));

            //        // make SQL command
            //        string sql = string.Format("insert into Log" +
            //            "([TimeStamp], [AlertType],[Line],[ObjectType],[ObjectNum],[Message],[UserName]) values " +
            //            "('{0}', {1}, '{2}', '{3}', '{4}', '{5}', '{6}')"
            //            , aMessage.EventTime.ToString(sqlDatePatt)
            //            , (int)(aMessage.AlertType)
            //            , aMessage.Line
            //            , aMessage.ObjectType
            //            , aMessage.ObjName
            //            , aMessage.MessageString
            //            , aMessage.UserName
            //            );
            //        cmd.CommandText = sql;
            //        //Console.WriteLine(sql);

            //        int rowsNumber = 0;
            //        if (this.cn != null && this.cn.State == ConnectionState.Open) {
            //            rowsNumber = cmd.ExecuteNonQuery();
            //        }
 
            //        // if no exception then delete row from  buffer
            //        if (rowsNumber > 0) {
            //            this.messageBuffer.Dequeue();
            //        }
            //        else {
            //            //this.LogFileAlert(AlertType.Error, "NA", "NA", this.GetType().ToString(), 
            //            //    "Failed to write into SQL Log table! LogBuffer="+this.messageBuffer.Count.ToString(), "system");
            //        }
            //    }

            //    //if (cmd != null) cmd.Dispose();
            //    if (this.cn != null) this.cn.Close();

            //    //Console.WriteLine(DateTime.Now.ToString(datePatt) + " - close connection.");
            //}
            //catch (Exception ex)
            //{
            //    this.LogFileAlert(AlertType.Error, "NA", ex.TargetSite.ToString(), ex.Source.ToString(), ex.ToString(), "system");
            //}
        }

        private void LogSQLAlert_v1(AlertType alertType, string line, string objectType, string objName, string message, string userName)
        {
            message = message.Replace("'", "''");
            int length = message.Length;
            if (length > 2048)
            {
                length = 2048;
            }
            message = message.Substring(0, length);

            string sql = string.Format("insert into Log" +
                "([AlertType],[Line],[ObjectType],[ObjectNum],[Message],[UserName]) values " +
                "({0}, '{1}', '{2}', '{3}', '{4}', '{5}')"
                , (int)alertType
                , line
                , objectType
                , objName
                , message
                , userName
                );
            SqlCommand cmd = new SqlCommand(sql, this.cn);
            try
            {
                if (cn.State == ConnectionState.Closed)
                {
                    this.cn.Open();
                }
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                this.LogFileAlert(AlertType.Error, "NA", "NA", this.GetType().ToString(), ex.Message, "system");
            }
        }

        protected void LogFileAlert(AlertType alertType, string line, string objectType, string objName, string message, string userName)
        {
            try
            {
                lock (this.thisLock)
                {
                    StreamWriter myWriter = File.AppendText(this.fileLogPath);
                    string logString = string.Format(DateTime.Now.ToString() + " : {0}, {1}", objName, message);
                    myWriter.WriteLine(logString);
                    myWriter.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("LogFileAlert() Exception: " + ex.Message);
                //this.LogFileAlert(AlertType.Error, "NA", "NA", this.GetType().ToString(), ex.Message, "system");
            }
        }
    }
    



}
