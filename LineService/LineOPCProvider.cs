using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OPCAutomation;
using System.Configuration;
using LineService.DetroitDataSetTableAdapters;
using System.Data;
using AppLog;
using System.Timers;
using MOXA_CSharp_MXIO;

namespace LineService
{
    public enum MXIO_ModuleType { E1212 = 4626, E2210 = 8720 }

    public enum OPCControlType { Button = 1, Lamp = 2 }

    public class VarEventArgs : EventArgs
    {
        public int varIndex;
    }

    public class VarItem 
    {
        public VarItem()
        { 
            //...
        }

        public string name;
        public int array_index;
        public int station_index;
        public string button_key;
        public bool bool_value;
        public bool old_bool_value;
        public int servHandle;
        public OPCControlType type; // 1 = button, 2 = lamp
        public string hash_key;
        public string MXIO_ipAddr;
        public int MXIO_channel;
        public int MXIO_moduleType;
    }

    public class LineOPCProvider
    {
        // ----------------------------------------------
        //
        //  private section
        //
        //

        private int id;
        private OPCServer myOPCServer;
        private OPCBrowser myOPCBrowser;
        private OPCGroups myOPCGroups;
        private OPCGroup myOPCGroup;
        private string OPCServerName;
        private LogProvider myLog;
        private Timer timer;
        private DateTime timeOPCStarted;


        private void myOPCItems_Fill(List<VarItem> OPCVariables, OPCItems items)
        {
            for (int i = 0; i < OPCVariables.Count; i++)
            {
                VarItem myVarItem = OPCVariables[i];
                OPCItem myItem = items.AddItem(myVarItem.name, myVarItem.array_index);

                // Check if the OPC variable consist in OPC server's configuration !!!
                if (myItem == null)
                {
                    this.myLog.LogAlert(AlertType.Error, this.id.ToString(), this.GetType().ToString(), "myOPCItems_Fill()",
                        "Exception: unknown variable '" + myVarItem.name + "' !!"
                        , "system");
                    OPCVariables.Remove(OPCVariables[i]);
                }
                else
                {
                    OPCVariables[i].servHandle = myItem.ServerHandle;

                    this.myLog.LogAlert(AlertType.System, this.id.ToString(), this.GetType().ToString(), "myOPCItems_Fill()",
                        this.OPCServerName +", myVarItem (name; arr_idx) = "
                        + myVarItem.name.ToString()
                        + ", " + myVarItem.array_index.ToString()
                        + ", myItem.ServerHandle=" + myItem.ServerHandle.ToString()
                        + ", myItem.Quality=" + myItem.Quality.ToString()
                        + ", myItem.ItemID=" + myItem.ItemID.ToString()
                        // + ", myItem.Value=" + myItem.Value.ToString()
                        , "system"
                        );
                }

            }
        }
        private void OPCGroup_DataChange(int TransactionID, int NumItems, ref Array ClientHandles, ref Array ItemValues, ref Array Qualities, ref Array TimeStamps)
        {
            for (int i = 1; i <= NumItems; i++)
            {
                int varIndex = Convert.ToInt32(ClientHandles.GetValue(i));

                VarItem currentItem = this.OPCVariables.FirstOrDefault(p => p.array_index.Equals(varIndex));

                if (currentItem != null && currentItem.type == OPCControlType.Button)
                {
                    VarItem myVarItem = currentItem;
                    bool myBoolValue = Convert.ToBoolean(ItemValues.GetValue(i));
                    if (myVarItem.bool_value != myBoolValue)
                    {
                        myVarItem.old_bool_value = myVarItem.bool_value;
                        myVarItem.bool_value = myBoolValue;
                        if (myVarItem.bool_value == true)
                        {

                            VarEventArgs myArgs = new VarEventArgs();
                            myArgs.varIndex = myVarItem.array_index;

                            ////int buttonIndex = this.OPCVariables.IndexOf(currentItem);
                            ////VarItem myVarSignalItem = this.OPCVariables[buttonIndex + 1];

                            // this maqnaged by the line itself !!!
                            //bool bntSignal = myOPCGroup.OPCItems.GetOPCItem((int)myVarSignalItem.servHandle).Value;
                            //myOPCGroup.OPCItems.GetOPCItem((int)myVarSignalItem.servHandle).Write(!bntSignal);

                            this.ImpDataChanged(this, myArgs);
                        }
                    }

                }
            }
        }
        private void timerHandler(object sender, EventArgs e)
        {
            this.checkOPCState();
        }
        private void checkOPCState()
        {
            try
            {
                DateTime lastTimeOPCStarted = this.myOPCServer.StartTime;
                if (DateTime.Compare(lastTimeOPCStarted, this.timeOPCStarted) > 0) 
                {
                    this.ServerRestarted(this, new EventArgs());
                    this.myLog.LogAlert(AlertType.System, this.id.ToString(), this.GetType().ToString(), "checkOPCState()", 
                        "Read OPC new start time = " + lastTimeOPCStarted.ToString(), "system");                    
                }
            }
            catch (Exception ex)
            {
                // "OPC server is anavalable.";
                this.myLog.LogAlert(AlertType.System, this.id.ToString(), this.GetType().ToString(), "checkOPCState()", 
                    ex.Message, "system");
                this.OPCDisconnect();
                this.OPCConnect(this.OPCServerName);
            }
        }
        private void set_MXDO_Signal(int station, string control_key, bool signal)
        {
            try
            {
                string tmpStr = "";
                this.myLog.LogAlert(AlertType.System, this.id.ToString(), this.GetType().ToString(), "Set_MXDO_Signal()",
                                    String.Format("Method started ... Station num={0}, key={1}, signal={2}", station.ToString(), control_key, signal.ToString()), "system");

                for (int i = 0; i < (this.OPCVariables.Count - 1); i++)
                {

                    this.myLog.LogAlert(AlertType.System, this.id.ToString(), this.GetType().ToString(), "Set_MXDO_Signal()",
                                                   String.Format("OPCVariables[{0}].station_index={1}, button_key={2}",
                                                   i.ToString(), this.OPCVariables[i].station_index.ToString(), this.OPCVariables[i].button_key.ToString()), "system");

                    if (this.OPCVariables[i].station_index == station & this.OPCVariables[i].button_key == control_key)
                    {
                        tmpStr = this.OPCVariables[i + 1].button_key.ToString();


                        byte channel = (byte)(this.OPCVariables[i + 1].MXIO_channel);
                        string iPAddr = this.OPCVariables[i + 1].MXIO_ipAddr;
                        int moduleType = this.OPCVariables[i + 1].MXIO_moduleType;
                        UInt32 timeout = 300;
                        uint bVal = Convert.ToUInt32(signal);

                        this.myLog.LogAlert(AlertType.System, this.id.ToString(), this.GetType().ToString(), "Set_MXDO_Signal()",
                                String.Format("IPAddr: {0}, moduleType={1}, channel={2}, bVal={3}", iPAddr, moduleType.ToString(), channel.ToString(), bVal.ToString()), "system");

                        if (iPAddr != "")
                        {


                            this.myLog.LogAlert(AlertType.System, this.id.ToString(), this.GetType().ToString(), "Set_MXDO_Signal()",
                                "Try: " + iPAddr + ", DO" + channel.ToString() + " = " + bVal.ToString(), "system");

                            this.eXX_DO_Write(iPAddr, 502, timeout, channel, bVal, moduleType);

                            this.myLog.LogAlert(AlertType.System, this.id.ToString(), this.GetType().ToString(), "Set_MXDO_Signal()",
                                "Success: " + iPAddr + ", DO" + channel.ToString() + " = " + bVal.ToString(), "system");

                            break;



                        }
                        else 
                        {
                            this.myLog.LogAlert(AlertType.Error, this.id.ToString(), this.GetType().ToString(), "Set_MXDO_Signal()",
                            String.Format("IPAddr is EMPTY for Station num={0}, key={1}, signal={2}", station.ToString(), control_key, signal.ToString())
                            , "system");
                        }
                    }
                    i++;
                }
            }
            catch (Exception e3)
            {
                this.myLog.LogAlert(AlertType.Error, this.id.ToString(), this.GetType().ToString(), "Set_MXDO_Signal()",
                    e3.ToString()
                    , "system");
            }
        }
        private void eXX_DO_Write(string IPAddr, UInt16 Port, UInt32 Timeout, byte Channel, uint Value, int ModuleType)
        {
            Int32[] hConnection = new Int32[16];
            string Password = "";
            int ret = 0;

            try
            {
                this.myLog.LogAlert(AlertType.System, this.id.ToString(), this.GetType().ToString(), "e1K_DO_Write()", "IPAddr = " + IPAddr.ToString(), "system");
                int init = MXIO_CS.MXEIO_Init();

                if ((MXIO_ModuleType)ModuleType == MXIO_ModuleType.E1212) 
                {
                    ret = MXIO_CS.MXEIO_E1K_Connect(System.Text.Encoding.UTF8.GetBytes(IPAddr), Port, Timeout, hConnection, System.Text.Encoding.UTF8.GetBytes(Password));
                    ret = MXIO_CS.E1K_DO_Writes(hConnection[0], Channel, 1, Value);
                    MXIO_CS.MXEIO_Disconnect(hConnection[0]);
                }
                else if ((MXIO_ModuleType)ModuleType == MXIO_ModuleType.E2210) 
                {
                    byte bytSlot = 0;
                    ret = MXIO_CS.MXEIO_Connect(System.Text.Encoding.UTF8.GetBytes(IPAddr), Port, Timeout, hConnection);
                    ret = MXIO_CS.DO_Writes(hConnection[0], bytSlot, Channel, 1, Value);
                    MXIO_CS.MXEIO_Disconnect(hConnection[0]);                
                }

                MXIO_CS.MXEIO_Exit();

                if (ret != 0) 
                {
                    this.myLog.LogAlert(AlertType.Error, this.id.ToString(), this.GetType().ToString(), "e1K_DO_Write()",
                        "Error code, ret = " + ret.ToString(), "system");
                }
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.id.ToString(), this.GetType().ToString(), "e1K_DO_Write()",
                    ex.ToString(), "system");
            }

        }
         

        public List<VarItem> OPCVariables = new List<VarItem>();
        public int State { get { return this.myOPCServer.ServerState; } }
       
        public LineOPCProvider(int id, LogProvider logProvider) 
        {
            this.id = id;
            this.myLog = logProvider;
            
            this.timer = new Timer();
            this.timer.Interval = 60000;
            this.timer.Elapsed += new ElapsedEventHandler(timerHandler);
            this.timer.Enabled = true;
        }

        public bool OPCConnect(string OPCServerName)
        {
            bool result = false;
            try
            {
                //this.OPCServerName = Properties.Settings.Default.OPCServer.ToString();
                //this.OPCServerName = "Matrikon.OPC.Modbus.1";
                //this.OPCServerName = "Matrikon.OPC.Simulation.1";
                this.OPCServerName = OPCServerName;
                this.myLog.LogAlert(AppLog.AlertType.System, this.id.ToString(), this.GetType().ToString(), "OPCConnect()",
                    "OPC Server connecting :  " + this.OPCServerName.ToString()
                    ,"system");

                this.myOPCServer = new OPCServer();
                this.myOPCServer.Connect(OPCServerName, "");

                this.myLog.LogAlert(AppLog.AlertType.System, this.id.ToString(), this.GetType().ToString(), "OPCConnect()",
                    ""
                    + ", ServerName =" + this.myOPCServer.ServerName.ToString()
                    + ", ServerNode =" + this.myOPCServer.ServerNode.ToString()
                    + ", ServerState =" + this.myOPCServer.ServerState.ToString()
                    + ", LocaleID =" + this.myOPCServer.LocaleID.ToString()
                    , "system"
                        );   
                

                this.myOPCGroups = myOPCServer.OPCGroups;
                this.myOPCGroup = myOPCGroups.Add("OPCGroup1");

                this.myOPCGroup.UpdateRate = 200;
                this.myOPCGroup.IsActive = true;
                this.myOPCGroup.IsSubscribed = true;

                this.myOPCItems_Fill(this.OPCVariables, this.myOPCGroup.OPCItems);
                this.myOPCGroup.DataChange += new DIOPCGroupEvent_DataChangeEventHandler(this.OPCGroup_DataChange);

                if (this.myOPCServer.ServerState > -1000) 
                {
                    this.timeOPCStarted = this.myOPCServer.StartTime;
                }
                

                result = true;
            }
            catch (Exception e2)
            {
                this.myLog.LogAlert(AppLog.AlertType.Error, this.id.ToString(), this.GetType().ToString(), "OPCConnect()",
                    "Exception: " + e2.ToString()
                    , "system");
            }
            return result;
        }
        public bool OPCDisconnect()
        {
            bool result = true;
            if (this.myOPCServer != null)
            {
                this.myOPCServer.Disconnect();
            }
            return result;
        }
        public event EventHandler ServerRestarted;
        public event EventHandler ImpDataChanged;


        public void ResetButtonSignal(int station, string control_key) 
        {
            string buttonHashKey = station.ToString() + "-" + control_key;
            try
            {
                this.set_MXDO_Signal(station, control_key, false);
            }
            catch (Exception ex) 
            {
                this.myLog.LogAlert(AlertType.Error, this.id.ToString(), this.GetType().ToString(), "ResetButtonSignal()",
                    ex.Message, "system");
            }
        }
        //public void SetButtonSignal(int station, string control_key, bool value)
        //{
        //    string buttonHashKey = station.ToString() + "-" + control_key;
        //    try
        //    {
        //        this.set_MXDO_Signal(station, control_key, value);
        //    }
        //    catch (Exception ex)
        //    {
        //        this.myLog.LogAlert(AlertType.Error, this.id.ToString(), this.GetType().ToString(), "ResetButtonSignal()",
        //            ex.Message, "system");
        //    }
        //}
        public bool ReadVariableValue(int servHandle) 
        {
            bool result = false;
            try
            {
                OPCItem opc_item_s = this.myOPCGroup.OPCItems.GetOPCItem(servHandle);

                short source = 0; // OPC_DS_CACHE or OPC_DS_DEVICE
                object value = null;
                object quality = null;
                object timestamp = null;

                if (opc_item_s != null)
                {
                    //opc_item_s.Read(source, out value, out quality, out timestamp);
                    value = opc_item_s.Value;

                    result = Convert.ToBoolean(value);
                }
                else 
                {
                    this.myLog.LogAlert(AlertType.Error, this.id.ToString(), this.GetType().ToString(), "ReadVariableValue()",
                        "opc_item_s is null, servHandle = " + servHandle.ToString(), "system");
                }
                return result;
            }
            catch (Exception ex) 
            {
                this.myLog.LogAlert(AlertType.Error, this.id.ToString(), this.GetType().ToString(), "ReadVariableValue()",
                    ex.Message, "system");
                return result;
            }
        }
        public void WriteVariableValue(VarItem varItem, bool value)
        {
            try
            {
                if (this.OPCServerName.Contains("Simulation"))
                {
                    int servHandle = varItem.servHandle;
                    OPCItem opc_item_s = this.myOPCGroup.OPCItems.GetOPCItem(servHandle);
                    opc_item_s.Write(value);
                }
                else
                {
                    this.eXX_DO_Write(varItem.MXIO_ipAddr, 502, 300, Convert.ToByte(varItem.MXIO_channel), Convert.ToUInt32(value), varItem.MXIO_moduleType);
                }
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.id.ToString(), this.GetType().ToString(), "WriteVariableValue()",
                    ex.Message, "system");
            }
        }



    }


    public enum TagQuality { Bad = 0, Good = 192 }

    public class NodeEventArgs : EventArgs 
    {
        public MoxaIPNode Node;
    }
    public class TagEventArgs : EventArgs 
    {
        public MoxaTag Tag;
    }

    public class MoxaIPNode 
    {
        MXIO_ModuleType moduleType;
        string IPAddr;
        UInt16 port;
        UInt32 timeout;
        bool isAlive;
     
        
        public List<MoxaTag> Tags;

        public MoxaIPNode(string IPAddr, UInt16 Port, UInt32 Timeout, MXIO_ModuleType ModuleType) 
        {
            this.Tags = new List<MoxaTag>();
        }

        public void Connect() {}
        public void Disconnect() {}
        public bool IsAlive 
            { get { return this.isAlive;  } }
    }

    public class MoxaTag 
    {
        string name;
        MoxaIPNode node;
        byte register;
        byte chanel;
        string path;
        int pollingInterval;
        TagQuality quality;
        object value;


        public TagQuality Quality { get; set; }

    }

    public class MoxaIPProvider 
    {
        List<MoxaIPNode> nodes;
        List<MoxaTag> tags;
        Timer bitTimer;
        double bitInterval;
        Timer pollTimer;
        double pollInterval;

        public MoxaIPProvider() 
        {
            this.nodes = new List<MoxaIPNode>();
            this.tags = new List<MoxaTag>();
            this.bitInterval = 1000;
            this.bitTimer = new Timer(bitInterval);
            this.bitTimer.Enabled = false;
            this.bitTimer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            this.pollInterval = 300;
            this.pollTimer = new Timer(pollInterval);
            this.pollTimer.Enabled = false;
            this.pollTimer.Elapsed += new ElapsedEventHandler(pollTimer_Elapsed);

            this.nodeIsNotResponding += new EventHandler<NodeEventArgs>(MoxaIPProvider_nodeIsNotResponding);
            
            MoxaIPNode newNode = new MoxaIPNode("192.168.21.2", 501, 300, MXIO_ModuleType.E1212);
            this.nodes.Add(newNode);

            MoxaTag newTag = new MoxaTag();
            this.tags.Add(newTag);
        }


        public void Connect() 
        {
            foreach (MoxaIPNode node in this.nodes) 
            {
                node.Connect();
            }
            this.bitTimer.Enabled = true;
        }
        public void Disconnect() 
        {
            this.bitTimer.Enabled = false;
            foreach (MoxaIPNode node in this.nodes)
            {
                node.Disconnect();
            }            
        }
        public void AddNode(MoxaTag node) { }
        public void AddTag(MoxaTag tag) { }
        public event EventHandler<TagEventArgs> OnDataChanged;
        public MoxaIPNode GetNodeByName(string name) 
        {
            return null;
        }
        public List<MoxaTag> Tags 
        { 
            get { return this.tags; } 
        }




        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            foreach (MoxaIPNode node in this.nodes)
            {
                TagQuality lastQuality = TagQuality.Bad;
                if (node.IsAlive)
                {
                    lastQuality = TagQuality.Good;
                }
                else 
                {
                    if (this.nodeIsNotResponding != null)
                        this.nodeIsNotResponding(this, new NodeEventArgs { Node = node });
                }
                foreach (MoxaTag tag in node.Tags)
                {
                    tag.Quality = lastQuality;
                }
            }
        }
        private void pollTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            foreach (MoxaTag tag in this.tags) 
            {
                if (tag.Quality == TagQuality.Good) 
                {
                    this.recallTagValue(tag);
                }
            }
        }
        private event EventHandler<NodeEventArgs> nodeIsNotResponding;
        private void MoxaIPProvider_nodeIsNotResponding(object sender, NodeEventArgs e)
        {
            e.Node.Disconnect();
            e.Node.Connect();
        }
        private void recallTagValue(MoxaTag tag) 
        { 
            // read value, compare with current, rise event "OnChanged"

            if (this.OnDataChanged != null) 
            {
                this.OnDataChanged(this, new TagEventArgs { Tag = tag });
            }
        }


    }
}
