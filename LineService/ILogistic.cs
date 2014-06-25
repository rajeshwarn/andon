using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace LineService
{
    [ServiceContract]
    interface ILogistic
    {

        [OperationContract]
        LogistRequestElem[] GetLogisticRequests();

        [OperationContract]
        LogistTailElem[] GetTails();

        [OperationContract]
        LogistTailElem[] GetBatchesOnLine();

        [OperationContract]
        LogisticInfo GetLogisticInfo();

        [OperationContract]
        StationRealtimeData[] ReadLogisticRealTimeData(int stationIndex);

     }



    [DataContract]
    public class LogistRequestElem
    {
        [DataMember]
        public string StationName;
        [DataMember]
        public string PartName;
        [DataMember]
        public int WaitingTime;
        [DataMember]
        public int StationIndex;
        [DataMember]
        public string Key;
        [DataMember]
        public int OrderNum;
        [DataMember]
        public string Address;
    }

    [DataContract]
    public class LogistTailElem
    {
        [DataMember]
        public string BatchName;
        [DataMember]
        public string BatchType;
        [DataMember]
        public string TailStationName;
        [DataMember]
        public int TailStationIndex;
    }

    [DataContract]
    public class LogisticInfo
    {
        [DataMember]
        public string NextBatchName;
        [DataMember]
        public int TaktsTillNextBatch;
        [DataMember]
        public string LineId;

    }


}
