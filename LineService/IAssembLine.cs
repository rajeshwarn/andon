using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace LineService
{
    [ServiceContract]
    interface IAssembLine
    {
        [OperationContract]
        void Execute();
        [OperationContract]
        void Terminate();
        [OperationContract]
        void ResetTimer();
        [OperationContract]
        void Move();

        [OperationContract]
        void FinishStation(string stationName);

        [OperationContract]
        int GetState();


        [OperationContract]
        int GetCounter();

        [OperationContract]
        void SetCounter(int value);
        
        [OperationContract]
        void SetCounterForever(int value);

        [OperationContract]
        List<LineStationBase> GetStations();

        [OperationContract]
        LineStationBase[] GetStationsArray();
 
        [OperationContract]
        void RemoveStation(string LineStationRemoveId);


        [OperationContract]
        int PushStationButton(int StationIndex, string ControlKey);
        [OperationContract]
        string ReadStationButton(int StationIndex, string ControlKey);
        [OperationContract]
        string ReadStationName(int StationIndex);

        [OperationContract]
        void SetOPCMode(bool mode, string OPCServerName);

        [OperationContract]
        bool GetOPCMode();

        [OperationContract]
        string ReadProduct(int StationIndex);

        [OperationContract]
        Frame ReadFrame();

        [OperationContract]
        ProductBase[] GetProductStock();

        [OperationContract]
        ProductBase[] GetProductBuffer();

        [OperationContract]
        ProductBase[] GetStationBuffer(int stationId);

        //[OperationContract]
        //int ReadStationTimer(int StationIndex, string timer_key);

        [OperationContract]
        StationRealtimeData[] ReadRealTimeData(int StationIndex);

        [OperationContract]
        StationRealtimeData[] ReadRealTimeDataForLine(int StationIndex);

        [OperationContract]
        LogistTailElem[] GetLogisticTails();


        [OperationContract]
        int GetSumStopTime();

        [OperationContract]
        void RestoreLine();

        [OperationContract]
        void SetOPCRestarted();

        [OperationContract]
        void SetStationDayPlan(string stationName, int amount);

        [OperationContract]
        void SetStationMonthPlan(string stationName, int amount);

        [OperationContract]
        void SetStationDayFact(string stationName, int amount);

        [OperationContract]
        void SetStationMonthFact(string stationName, int amount);

        [OperationContract]
        List<LogMessage> GetMemLogData();

        [OperationContract]
        ClientInstruction GetClientInstruction();

        [OperationContract]
        void SetClientInstruction(int Mode, string ContentUrl);




    }

    [DataContract]
    public class ClientInstruction 
    {
        [DataMember] public int Mode;
        [DataMember] public string ContentUrl;
    }


    [DataContract]
    public class StationRealtimeData
    {
        protected string myKey;
        protected string myValue;

        [DataMember]
        public string Key 
        {
            get { return myKey; }
            set { myKey = value; }
        }

        [DataMember]
        public string Value
        {
            get { return myValue; }
            set { myValue = value; }
        }
    }

    [Serializable]
    [DataContract]
    public class LineStationBase
    {
        protected int id;
        protected string name;
        protected string currentProductName;

        [DataMember]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [DataMember]
        public string CurrentProductName
        {
            get { return currentProductName; }
            set { currentProductName = value; }
        }

    }


    [DataContract]
    public class ProductBase
    {
        protected int id;
        protected string name;

        [DataMember]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

    }

    [DataContract]
    public class Frame
    {
        protected string name;
        protected int type;

        [DataMember]
        public string Name {get {return this.name;} set { this.name = value;} }
        
        [DataMember]
        public int Type { get { return this.type; } set { this.type = value; } }


    }


    [DataContract]
    public class LogMessage 
    {
        [DataMember]
        public int AlertType;

        [DataMember]
        public string Line;

        [DataMember]
        public string ObjectType;

        [DataMember]
        public string ObjName;

        [DataMember]
        public string MessageString;

        [DataMember]
        public string UserName;

        [DataMember]
        public DateTime EventTime;
    }


}
