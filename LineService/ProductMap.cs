using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppLog;

namespace LineService
{
    public enum MapStepResult { Undefined = 0, Ok = 1, Fail = 2 }
    public enum MapStepState { New = 0, InProgress = 1, Done = 2 }

    public class MapItem
    {
        public string Operation;
        public string LineStationKey;
        public int StepNum;
        private MapStepState state; // State: 0 - has not been done, 1 - in progress, 2 - done.
        private MapStepResult result; 
        private DateTime lastStateTime;
        private LineStation lineStation;
        private List<MapItem> assistLineStaions = new List<MapItem>();
 
        public MapItem(string oper, LineStation lineStation, string lineStationKey, int stepNum)
        {
            this.lineStation = lineStation; 
            this.Operation = oper;
            this.LineStationKey = lineStationKey;
            this.StepNum = stepNum;
            this.state = MapStepState.New;
            this.result = MapStepResult.Undefined;
            this.lastStateTime = new DateTime(2000, 1, 1, 0, 0, 0);
        }

        public MapStepState State { get { return this.state; } }
        public MapStepResult Result { get { return this.result; } }

        public LineStation LineStation { get { return this.lineStation; } }
        public DateTime LastStateTime { get { return this.lastStateTime; } }
        public List<MapItem> AssistLineStations { get { return this.assistLineStaions; } }

        public void Start() 
        {
            this.state = MapStepState.InProgress;
            this.lastStateTime = DateTime.Now;
        }
        public void Finish() 
        {
            this.state = MapStepState.Done;
            this.lastStateTime = DateTime.Now;
            if (this.result == MapStepResult.Undefined) 
            {
                this.result = MapStepResult.Ok;
            }
        }
        public void Fail() 
        {
            this.result = MapStepResult.Fail;
        }

    }

    public class ProductMap : LinkedList<MapItem>
    {
        private Product owner;
        private LinkedListNode<MapItem> marker;
        private LogProvider myLog; 

        public int NextLineId { get { return this.owner.Owner.Map.NextLineId;  } }
        public int NextLineAuto { get { return this.owner.Owner.Map.NextLineAuto; } }
        public int Takt { get { return this.owner.Owner.Map.Takt; } }
    
        public ProductMap(Product owner, BatchMap batchMap, LogProvider logProvider) 
        {
            this.owner = owner;
            this.myLog = logProvider;

            //            
            // copy from Batch Mapt
            marker = null;
            int i = 0;
            LinkedListNode<BatchMapItem> enBatchMapItem = batchMap.First;
            while (enBatchMapItem != null)
            {
                // create main item in product map 
                MapItem newProductMapItem = new MapItem("NA", enBatchMapItem.Value.LineStation, enBatchMapItem.Value.LineStation.Name, ++i);
                this.AddLast(newProductMapItem);

                //this.myLog.LogAlert(AppLog.AlertType.System, this.GetType().ToString(), "Product " + owner.Name + ", add map item: " + enBatchMapItem.Value.LineStation.Name);

                // check and create assistant items for current main item in product map
                List<BatchMapItem> assistLineStations = enBatchMapItem.Value.AssistLineStations;
                int assist_count = assistLineStations.Count;
                if (assist_count > 0)
                {
                    // read all elements in assistant list
                    for (int j = 0; j < assist_count; j++)
                    {
                        // get first element and move it to the end
                        BatchMapItem enBatchMapAssistItem = assistLineStations.First();
                        assistLineStations.RemoveAt(0);
                        assistLineStations.Add(enBatchMapAssistItem);

                        // create new assist map item for product 
                        MapItem newAssisttMapItem = new MapItem("NA", enBatchMapAssistItem.LineStation, enBatchMapAssistItem.LineStation.Name, j);
                        newProductMapItem.AssistLineStations.Add(newAssisttMapItem);

                        //this.myLog.LogAlert(AppLog.AlertType.System, this.GetType().ToString(), "Product " + owner.Name + ", add assist map item: " + enBatchMapAssistItem.LineStation.Name);
                    }
                }

                enBatchMapItem = enBatchMapItem.Next;
            }
        }

        public LinkedListNode<MapItem> Marker { get { return this.marker; } set { this.marker = value; } }
 
    }
}
