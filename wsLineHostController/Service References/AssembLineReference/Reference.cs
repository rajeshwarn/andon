﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AndonApplication.AssembLineReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="AssembLineReference.IAssembLine")]
    public interface IAssembLine {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAssembLine/Execute", ReplyAction="http://tempuri.org/IAssembLine/ExecuteResponse")]
        void Execute();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAssembLine/Terminate", ReplyAction="http://tempuri.org/IAssembLine/TerminateResponse")]
        void Terminate();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAssembLine/Move", ReplyAction="http://tempuri.org/IAssembLine/MoveResponse")]
        void Move();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAssembLine/GetState", ReplyAction="http://tempuri.org/IAssembLine/GetStateResponse")]
        int GetState();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAssembLine/GetCounter", ReplyAction="http://tempuri.org/IAssembLine/GetCounterResponse")]
        int GetCounter();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAssembLine/GetStations", ReplyAction="http://tempuri.org/IAssembLine/GetStationsResponse")]
        LineService.LineStation[] GetStations();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAssembLine/RemoveStation", ReplyAction="http://tempuri.org/IAssembLine/RemoveStationResponse")]
        void RemoveStation(string LineStationRemoveId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAssembLine/SubmitStation", ReplyAction="http://tempuri.org/IAssembLine/SubmitStationResponse")]
        void SubmitStation(LineService.LineStation NewLineStation);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAssembLine/PushStationButton", ReplyAction="http://tempuri.org/IAssembLine/PushStationButtonResponse")]
        void PushStationButton(int StationIndex, int ButtonIndex);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAssembLine/ReadStationButton", ReplyAction="http://tempuri.org/IAssembLine/ReadStationButtonResponse")]
        string ReadStationButton(int StationIndex, int ButtonIndex);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IAssembLineChannel : AndonApplication.AssembLineReference.IAssembLine, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class AssembLineClient : System.ServiceModel.ClientBase<AndonApplication.AssembLineReference.IAssembLine>, AndonApplication.AssembLineReference.IAssembLine {
        
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
        
        public void Terminate() {
            base.Channel.Terminate();
        }
        
        public void Move() {
            base.Channel.Move();
        }
        
        public int GetState() {
            return base.Channel.GetState();
        }
        
        public int GetCounter() {
            return base.Channel.GetCounter();
        }
        
        public LineService.LineStation[] GetStations() {
            return base.Channel.GetStations();
        }
        
        public void RemoveStation(string LineStationRemoveId) {
            base.Channel.RemoveStation(LineStationRemoveId);
        }
        
        public void SubmitStation(LineService.LineStation NewLineStation) {
            base.Channel.SubmitStation(NewLineStation);
        }
        
        public void PushStationButton(int StationIndex, int ButtonIndex) {
            base.Channel.PushStationButton(StationIndex, ButtonIndex);
        }
        
        public string ReadStationButton(int StationIndex, int ButtonIndex) {
            return base.Channel.ReadStationButton(StationIndex, ButtonIndex);
        }
    }
}
