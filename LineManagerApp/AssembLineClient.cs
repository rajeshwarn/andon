//------------------------------------------------------------------------------
// Writen manualy
//------------------------------------------------------------------------------

namespace LineManagerApp.ServiceReference2 {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="LineStation", Namespace="http://schemas.datacontract.org/2004/07/LineService")]
    [System.SerializableAttribute()]

    public partial class LineStation : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string idField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string id {
            get {
                return this.idField;
            }
            set {
                if ((object.ReferenceEquals(this.idField, value) != true)) {
                    this.idField = value;
                    this.RaisePropertyChanged("id");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference2.IAssembLine")]
    public interface IAssembLine {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAssembLine/Execute", ReplyAction="http://tempuri.org/IAssembLine/ExecuteResponse")]
        void Execute();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAssembLine/GetCounter", ReplyAction="http://tempuri.org/IAssembLine/GetCounterResponse")]
        int GetCounter();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAssembLine/GetStations", ReplyAction="http://tempuri.org/IAssembLine/GetStationsResponse")]
        LineManagerApp.ServiceReference2.LineStation[] GetStations();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAssembLine/Move", ReplyAction="http://tempuri.org/IAssembLine/MoveResponse")]
        void Move();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAssembLine/RemoveStation", ReplyAction="http://tempuri.org/IAssembLine/RemoveStationResponse")]
        void RemoveStation(string LineStationRemoveId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAssembLine/SubmitStation", ReplyAction="http://tempuri.org/IAssembLine/SubmitStationResponse")]
        void SubmitStation(LineManagerApp.ServiceReference2.LineStation NewLineStation);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAssembLine/Terminate", ReplyAction="http://tempuri.org/IAssembLine/TerminateResponse")]
        void Terminate();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAssembLine/GetState", ReplyAction="http://tempuri.org/IAssembLine/GetStateResponse")]
        int GetState();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAssembLine/PushStationButton", ReplyAction="http://tempuri.org/IAssembLine/PushStationButtonResponse")]
        void PushStationButton(int StationIndex, int ButtonIndex);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAssembLine/ReadStationButton", ReplyAction="http://tempuri.org/IAssembLine/ReadStationButtonResponse")]
        string ReadStationButton(int StationIndex, int ButtonIndex);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IAssembLine/SetOPCMode", ReplyAction = "http://tempuri.org/IAssembLine/SetOPCMode")]
        void SetOPCMode(bool mode, string OPCServerName);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IAssembLine/GetOPCMode", ReplyAction = "http://tempuri.org/IAssembLine/GetOPCMode")]
        bool GetOPCMode();

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IAssembLine/ReadProduct", ReplyAction = "http://tempuri.org/IAssembLine/ReadProductResponse")]
        string ReadProduct(int StationIndex);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IAssembLineChannel : LineManagerApp.ServiceReference2.IAssembLine, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class AssembLineClient : System.ServiceModel.ClientBase<LineManagerApp.ServiceReference2.IAssembLine>, LineManagerApp.ServiceReference2.IAssembLine {
        
        public AssembLineClient() {
        }
        
        public AssembLineClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public AssembLineClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public AssembLineClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public AssembLineClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void Execute() {
            base.Channel.Execute();
        }
        
        public int GetCounter() {
            return base.Channel.GetCounter();
        }
        
        public LineManagerApp.ServiceReference2.LineStation[] GetStations() {
            return base.Channel.GetStations();
        }
        
        public void Move() {
            base.Channel.Move();
        }
        
        public void RemoveStation(string LineStationRemoveId) {
            base.Channel.RemoveStation(LineStationRemoveId);
        }
        
        public void SubmitStation(LineManagerApp.ServiceReference2.LineStation NewLineStation) {
            base.Channel.SubmitStation(NewLineStation);
        }
        
        public void Terminate() {
            base.Channel.Terminate();
        }
        
        public int GetState() {
            return base.Channel.GetState();
        }
        
        public void PushStationButton(int StationIndex, int ButtonIndex) {
            base.Channel.PushStationButton(StationIndex, ButtonIndex);
        }
        
        public string ReadStationButton(int StationIndex, int ButtonIndex) {
            return base.Channel.ReadStationButton(StationIndex, ButtonIndex);
        }

        public void SetOPCMode(bool mode, string OPCServerName)        {
            base.Channel.SetOPCMode(mode, OPCServerName);
        }

        public bool GetOPCMode()        {
            return base.Channel.GetOPCMode();
        }

        public string ReadProduct(int StationIndex)        {
            return base.Channel.ReadProduct(StationIndex);
        }

    }
}
