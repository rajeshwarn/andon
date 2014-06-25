using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace LogisticService
{
    [ServiceContract]
    interface ILogistic
    {
        [OperationContract]
        LogistRequestElem[] GetLogisticRequests();

        [OperationContract]
        LogistTailElem[] GetLogisticTails();

        [OperationContract]
        LogistBatch[] GetBatchesOnLine();

        [OperationContract]
        LogisticInfo GetLogisticInfo();
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

    [DataContract]
    public class LogistBatch
    {
        [DataMember]
        public string BatchName;
        [DataMember]
        public string BatchType;
        [DataMember]
        public string LineId;
    }




}
