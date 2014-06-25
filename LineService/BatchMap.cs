using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Collections;
using System.Data;
using AppLog;
using System.Runtime.Serialization;

namespace LineService
{
    [Serializable]
    [DataContract]
    public class BatchMapItem 
    {
        private LineStation lineStation;
        private List<BatchMapItem> assistLineStaions = new List<BatchMapItem>();

        public LineStation LineStation { get { return this.lineStation; } }
        public List<BatchMapItem> AssistLineStations { get { return this.assistLineStaions; } }
        public BatchMapItem(LineStation lineStation) 
        {
            this.lineStation = lineStation;
        }
    }


    public class BatchMap : LinkedList<BatchMapItem>
    {
        private Line line;
        private int takt;
        private int nextLineId = 0;
        private int nextLineAuto = 0;
        private Batch owner;
        private DetroitDataSet detroitDataSet;
        private DetroitDataSetTableAdapters.BatchMapPointTableAdapter batchMapPointTableAdapter;
        private DetroitDataSetTableAdapters.BatchTypeMapTableAdapter batchTypeMapTableAdapter;
        private LogProvider myLog; 

        public int NextLineId { get { return this.nextLineId; } }
        public int NextLineAuto { get { return this.nextLineAuto; } }
        public int Takt { get { return this.takt; } }

        public BatchMap(Batch owner, Line line)
            : this(owner, line, null)
        { }

        public BatchMap(Batch owner, Line line, LogProvider logProvider) 
        {
            // ------------------------------------------
            // Method description:
            // ------------------------------------------
            // get BatchType from Detroit
            // get BatchTypeMap from Detroit
            // get MapPoints by BatchTypeMapId from Detroit 
            // in routine :
            //      get MapPoint
            //      get station object by StationId
            //      create new BatchMapItem
            //      if it's main station add new BatchMapItem to the BatchMap list
            //      if it's assistant station add it to the exact main station
            //      (exact station == last elem in list, because adapter's rows ordered "step, isMain DESC") 
            // ------------------------------------------
            this.owner = owner;
            this.line = line;
            this.myLog = logProvider;
            int line_capacity = line.GetStations().Count();

            
            // connect to database
            this.detroitDataSet = new DetroitDataSet();
            this.batchMapPointTableAdapter = new DetroitDataSetTableAdapters.BatchMapPointTableAdapter();
            this.batchMapPointTableAdapter.FillByBatchTypeId(this.detroitDataSet.BatchMapPoint, owner.TypeId, line.Id);

            this.batchTypeMapTableAdapter = new DetroitDataSetTableAdapters.BatchTypeMapTableAdapter();
            this.batchTypeMapTableAdapter.FillBy(this.detroitDataSet.BatchTypeMap, owner.TypeId, line.Id);

            if( this.detroitDataSet.BatchTypeMap.Rows.Count > 0) 
            {
                object objNextLineId = this.detroitDataSet.BatchTypeMap.Rows[0]["NextLineId"];
                object objNextLineAuto = this.detroitDataSet.BatchTypeMap.Rows[0]["NextLineAuto"];
                object objTakt = this.detroitDataSet.BatchTypeMap.Rows[0]["Takt"];
                if (objNextLineId != null && objNextLineId != DBNull.Value && objTakt != DBNull.Value)
                {
                    this.nextLineId = Convert.ToInt32(objNextLineId);
                    this.takt = Convert.ToInt32(objTakt);
                    if (objNextLineAuto != null && objNextLineAuto != DBNull.Value)
                    {
                        this.nextLineAuto = Convert.ToInt32(objNextLineAuto);
                    }
                }
                else
                {
                    this.nextLineId = 0;
                    this.nextLineAuto = 0;
                    this.takt = 1;  //tbd ?!
                }
            }
            

            for (int i = 0; i <= this.detroitDataSet.BatchMapPoint.Rows.Count - 1; i++)
            {
                // read key data from databse
                DataRow mapPointRow = this.detroitDataSet.BatchMapPoint.Rows[i];
                int stationId = (int)mapPointRow["StationId"];
                int isMain = (int)mapPointRow["IsMain"];

                // find station object on AssemblyLine
                LineStation enStation = (LineStation)line.GetStation(stationId);
                
                // ALWAYS CHECK null VALUES !!!
                if (enStation == null) 
                { 
                    //this.myLog.LogAlert(AppLog.AlertType.System, this.GetType().ToString(), "Map constructor can't find station object (id=" + stationId.ToString() + ") in AssemblyLine");
                }

                BatchMapItem newMapItem = new BatchMapItem(enStation);

                if (isMain == 1) 
                {
                    this.AddLast(newMapItem);
                    //this.myLog.LogAlert(AppLog.AlertType.System, this.GetType().ToString(), "Batch "+ owner.Name + ", add map item: " + enStation.Name);
                }
                else 
                { 
                    // handle assist station, 
                    // link it to the last main staion            
                    BatchMapItem mainStationMapItem = this.Last();
                    mainStationMapItem.AssistLineStations.Add(newMapItem);
                    //this.myLog.LogAlert(AppLog.AlertType.System, this.GetType().ToString(), "Batch " + owner.Name + ", add assist map item: " + enStation.Name + " to main: " + mainStationMapItem.LineStation.Name);
                }
            }
        }
    }
}
