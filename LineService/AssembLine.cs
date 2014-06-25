using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using OPCAutomation;
using System.Configuration;
using LineService.DetroitDataSetTableAdapters;
using System.Data;
using AppLog;
using System.Collections;
using System.Timers;
using System.Runtime.Serialization;
using System.Security.Permissions;


namespace LineService
{
    enum PlanMode { Day = 1, Month = 2 }
    enum StationProductAction { Add = 1, Remove = 0 }
    public enum ProductState { InBuffer = 0, InStation = 1, InStationBuff = 2, Uncompleted = 3, InStock = 4, WaitForNextLine = 5, Undefined = 99 }
    public enum LineType { Synchro = 0, Asynchro = 1 }

    [Serializable]
    struct LineParams 
    {
        public int LineCounter;
        public int MoveCounter;
        public int LineState;
        public int EventCounter;
        public int DayTaktNo;
        public int MonthTaktNo;
    }

    public class BatchesOnLine : Queue<Batch> 
    {
        public event EventHandler OnListChange;

        new public void Enqueue(Batch item) 
        {
            base.Enqueue(item);

            if(this.OnListChange != null)
                this.OnListChange(this, new EventArgs());
        }

        new public Batch Dequeue()
        {
            Batch result;
            result = base.Dequeue();
            if (this.OnListChange != null) 
                this.OnListChange(this, new EventArgs());
            return result;
        } 
    }



    public class ProductsOnLine : Queue<Product> { }
    public class BatchStock : Queue<Batch> { }
    public class LineFrame : Frame 
    {
        public LineFrame() 
        {
            this.Name = "na";
            this.Type = 0;
        }
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class AssembLine :  Line,  IAssembLine, ILogistic
    {
        protected int id;                       // unid of the line
        protected string name;                  // logical name
        private int state = 0;                  // { 0 - Stopped, 1 - Ready, 2 - Runnung,  3 - Terminated, 4 - Error };
        private bool opcMode = true;            // "false" - virtual, data values from LineManager; "true" - real, data values from OPC Server
        private int defaultTaktDuration = 0;


        private int dayGap = 0;
        private int monthGap = 0;
        private int moveCounter = 0;
        private int eventCounter = 0;

        private bool isMoveInProgress = false;
        private bool isOPCAvailable = false;
        private bool isOPCRestarted = false;

        private LineType lineType = LineType.Synchro;
        
        private List<TimedLineStation> lineStations = new List<TimedLineStation>();
        private Dispatcher dispatcher;
        private Supplier supplier;
        private LogType logType;
        private MemLog myLog; //LogProvider myLog;
        private LineOPCProvider OPCProvider;
        private TimeManager timeManager;

        private Hashtable logisticRequestTable = new Hashtable();
        private Hashtable logisticTailTable = new Hashtable();
    
        // private LogisticService.LogisticCollectorClient logisticCollector = new LogisticService.LogisticCollectorClient("NetTcpBinding_ILogisticCollector");
        
        // Database section
        private DetroitDataSet detroitDataSet ; //

        private StationTableAdapter stationTableAdapter;
        private ControlTableAdapter controlTableAdapter;
        
        private LineSnapshotTableAdapter lineSnapshotTableAdapter = new LineSnapshotTableAdapter();
        private DetroitDataSet.LineSnapshotDataTable lineSnapshotTable; //= new DetroitDataSet.LineSnapshotDataTable();

       
        private void OPCInit(List<TimedLineStation> lineStations, List<VarItem> opcVariables)
        {
            this.myLog.LogAlert(AlertType.System, this.name, this.GetType().ToString(), "OPCInit()",
                "OPCInit started ...", "system");

            // for each station in the line:
            // - read list of controls from Database by line_id
            // - add variables to "items" array
            int i = 0;
            int k = 0;

            foreach (TimedLineStation myStation in this.lineStations) 
            {
                i++; // station index
                int j = 0;
                foreach (object myObject in myStation.StationControls.Values) 
                {
                    Button myButton = (Button)myObject;
                    j++; // button index
                    k++; // index of the variable in the group

                    VarItem myVarItem = new VarItem();
                    myVarItem.name = myButton.VarName;
                    myVarItem.array_index = k;
                    myVarItem.station_index = i;
                    myVarItem.button_key = myButton.Name;
                    myVarItem.type = OPCControlType.Button;
                    myVarItem.hash_key = myVarItem.station_index.ToString() + "-" + myVarItem.button_key.ToString();
                    myVarItem.MXIO_channel = 99;
                    myVarItem.MXIO_ipAddr = "";
                    myVarItem.MXIO_moduleType = 0;
                    opcVariables.Add(myVarItem);
                    
                    k++; // index of the OPC Variable in the group

                    VarItem myVarSignalItem = new VarItem();
                    myVarSignalItem.name = myButton.VarSignalName;
                    myVarSignalItem.array_index = k;
                    myVarSignalItem.station_index = i;
                    //myVarSignalItem.button_index = j;
                    myVarSignalItem.button_key = myButton.Name;
                    myVarSignalItem.type = OPCControlType.Lamp;
                    myVarSignalItem.hash_key = myVarSignalItem.station_index.ToString() + "-" + myVarSignalItem.button_key.ToString();
                    myVarSignalItem.MXIO_channel = myButton.Channel;
                    myVarSignalItem.MXIO_ipAddr = myButton.IPAddr;
                    myVarSignalItem.MXIO_moduleType = myButton.ModuleType;
                    opcVariables.Add(myVarSignalItem);
                }
                myStation.OPCProvider = this.OPCProvider;
            }
            this.myLog.LogAlert(AlertType.System, this.name, this.GetType().ToString(), "OPCInit()",
                "OPCInit finished.", "system");
        }
        private void ButtonPressed(object sender, EventArgs e) 
        {
            try
            {
                // write action into log
                this.myLog.LogAlert(AlertType.System, this.id.ToString(), this.GetType().ToString(), "ButtonPressed()",
                    "Method started", "system");

                // pointer to apropriate variable of Button in array of OPC items
                int buttonIndex = ((VarEventArgs)e).varIndex;
                // pointer to apropriate variable of Lamp in array of OPC items
                int signalIndex = buttonIndex + 1;

                // try to switch-on or switch-off the lamp ofthe button
                this.checkButtonsLamp(buttonIndex, signalIndex);

                //disabled since v.1 
                //this.checkButtonsOnLine();
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.Id.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }

        private void checkButtonsLamp(int buttonIndex, int signalIndex)
        {
            try
            {

                int buttonState = 0;

                // get Button variable from array of OPC items
                VarItem varButtonItem = this.OPCProvider.OPCVariables.Find(p => p.array_index.Equals(buttonIndex));

                // get Signal variable from array of OPC items
                VarItem myVarSignalItem = this.OPCProvider.OPCVariables.Find(p => p.array_index.Equals(signalIndex));

                // try to "push" the Button for station object
                if (varButtonItem != null)
                {
                    buttonState = this.PushStationButton(varButtonItem.station_index, varButtonItem.button_key);
                    this.myLog.LogAlert(AlertType.System, this.id.ToString(), this.GetType().ToString(), "checkButtonsLamp()",
                        varButtonItem.name + " Button pushed = " + buttonState.ToString(), "system");

                    if (varButtonItem.button_key == "FINISH")
                    {
                        this.freezeButtonTemporary(varButtonItem.station_index, varButtonItem.button_key);
                    }
                }

                if (myVarSignalItem != null && (varButtonItem != null))
                {
                    // hit the lamp output!
                    // read curren state of the Lamp and invert it
                    bool signalState = Convert.ToBoolean(buttonState);

                    //this.OPCProvider.WriteVariableValue((int)myVarSignalItem.servHandle, signalState);
                    this.OPCProvider.WriteVariableValue(myVarSignalItem, signalState);
                    bool faktiskSignalState = (bool)(this.OPCProvider.ReadVariableValue((int)myVarSignalItem.servHandle));

                    ////// Press button second time ?!   
                    ////// 
                    ////if (signalState != faktiskSignalState)
                    ////{
                    ////    this.OPCProvider.WriteVariableValue(myVarSignalItem, signalState);
                    ////    faktiskSignalState = (bool)(this.OPCProvider.ReadVariableValue((int)myVarSignalItem.servHandle));
                    ////}

                    ////if (signalState != faktiskSignalState)
                    ////{
                    ////    signalState = faktiskSignalState;
                    ////    this.PushStationButton(varButtonItem.station_index, varButtonItem.button_key);
                    ////}
                    this.myLog.LogAlert(AlertType.System, this.id.ToString(), this.GetType().ToString(), "checkButtonsLamp()",
                        myVarSignalItem.name + " Lamp on = " + signalState, "system");
                }

            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.Id.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }
        private void checkButtonsOnLine()
        {

            try
            {
                //this.myLog.LogAlert(AlertType.System, "checkButtonsOnLine()", "Method started");

                int busy_stations = 0;
                int finish_pushed = 0;
                int stop_pushed = 0;

                for (int i = 0; i < this.lineStations.Count; i++)
                {
                    if (this.lineStations[i].CurrentProduct != null)
                    {
                        busy_stations++;
                        if (this.lineStations[i].ReadButton("FINISH") == "1")
                        {
                            finish_pushed++;
                        }
                        if (this.lineStations[i].ReadButton("STOP") == "1")
                        {
                            stop_pushed++;
                        }
                    }

                }
                if ((busy_stations > 0) & (busy_stations == finish_pushed) & (stop_pushed == 0))
                {
                    this.Move();
                }
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.Id.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }
        private void resetButtons()
        {
            try
            {
                for (int i = 0; i < this.lineStations.Count; i++)
                {
                    this.lineStations[i].ResetControls(ResetControlsType.All);
                }
            }
            catch (Exception ex) 
            {
                this.myLog.LogAlert(AlertType.Error, "resetButtons()", ex.Message);
            }
        }
        private void resetSignals() 
        {
            try
            {
                Button aButton;
                for (int i = 0; i < this.lineStations.Count; i++)
                {
                    aButton = (Button)this.lineStations[i].StationControls["FINISH"];
                    if (aButton != null)
                    {
                        aButton.Reset();
                        this.OPCProvider.ResetButtonSignal(i + 1, aButton.Name);
                    }

                    aButton = (Button)this.lineStations[i].StationControls["STOP"];
                    if (aButton != null)
                    {
                        aButton.Reset();
                        this.OPCProvider.ResetButtonSignal(i + 1, aButton.Name);
                    }

                    aButton = (Button)this.lineStations[i].StationControls["HELP"];
                    if (aButton != null)
                    {
                        aButton.Reset();
                        this.OPCProvider.ResetButtonSignal(i + 1, aButton.Name);
                    }

                    aButton = (Button)this.lineStations[i].StationControls["FAIL"];
                    if (aButton != null)
                    {
                        aButton.Reset();
                        this.OPCProvider.ResetButtonSignal(i + 1, aButton.Name);
                    }

                    aButton = (Button)this.lineStations[i].StationControls["PART1"];
                    if (aButton != null)
                    {
                        aButton.Reset();
                        this.OPCProvider.ResetButtonSignal(i + 1, aButton.Name);
                    }

                    aButton = (Button)this.lineStations[i].StationControls["PART2"];
                    if (aButton != null)
                    {
                        aButton.Reset();
                        this.OPCProvider.ResetButtonSignal(i + 1, aButton.Name);
                    }

                }

            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.Id.ToString(), ex.Source.ToString(), ex.TargetSite.ToString(),
                       "resetSignals()", ex.Message);
            }        
        }
       
     
        public override int Id { get { return this.id; } }
        public string Name { get { return this.name; } }
        public int MoveCounter { get { return this.moveCounter; } }




        public AssembLine()
        {
            this.logType = LogType.SQL;
            this.myLog = new MemLog(this.logType, Properties.Settings.Default.DetroitConnectionString.ToString(), false);

            //this.OPCProvider = new LineOPCProvider(this.id, this.myLog);
            //this.OPCProvider.ServerRestarted += new EventHandler(this.handlerOPCProvider_ServerRestarted);

            //this.OPCProvider.ImpDataChanged += new EventHandler(this.ButtonPressed);

            this.lineType = (LineType)(Convert.ToInt16(Properties.Settings.Default.LineType));
       }
        public void Init(int id)
        {
            int MAIN_STATION_TYPE = 1;
            //int DEFAULT_BUFFER_SIZE = 0;
            this.id = id;

            this.OPCProvider = new LineOPCProvider(this.id, this.myLog);
            this.OPCProvider.ServerRestarted += new EventHandler(this.handlerOPCProvider_ServerRestarted);
            this.OPCProvider.ImpDataChanged += new EventHandler(this.ButtonPressed);

            this.myLog.LogAlert(AlertType.System, this.Id.ToString(), this.GetType().ToString(), "Init()",
                "Connecting to the database ... "
                + LineService.Properties.Settings.Default.DetroitConnectionString.ToString(), "system");

            // read from database stations and controls for the line
            this.detroitDataSet = new DetroitDataSet(this.id);
            this.stationTableAdapter = new DetroitDataSetTableAdapters.StationTableAdapter();
            this.controlTableAdapter = new DetroitDataSetTableAdapters.ControlTableAdapter();
            this.stationTableAdapter.FillByLineId(this.detroitDataSet.Station, this.id);
            this.lineSnapshotTable = this.detroitDataSet.LineSnapshot;

            // init line fields from database
            DetroitDataSetTableAdapters.AssembLineTableAdapter lineTableAdapter = new AssembLineTableAdapter();
            lineTableAdapter.Fill(detroitDataSet.AssembLine);
            DataRow[] rows = detroitDataSet.AssembLine.Select("Id = " + this.id.ToString());
            if (rows.Count() > 0)
            {
                this.name = rows[0]["Name"].ToString();
            }
            else
            {
                throw new Exception(String.Format("Line with ID={0} isn't found in database.", this.id));
            }

            this.myLog.LogAlert(AlertType.System, this.Id.ToString(), this.GetType().ToString(), "Init()", 
                "Reading stations and buttons from database ...", "system");

            for (int i = 0; i <= this.detroitDataSet.Station.Rows.Count - 1; i++)
            {
                DataRow stationRow = this.detroitDataSet.Station.Rows[i];
                TimedLineStation myStation = new TimedLineStation(
                    Convert.ToInt32(stationRow["Id"])
                    , MAIN_STATION_TYPE
                    , stationRow["Name"].ToString()
                    , (int)stationRow["BufferSize"]
                    , i + 1
                    , this
                    , this.myLog
                    , null
                );
                this.myLog.LogAlert(AlertType.System, this.Id.ToString(), this.GetType().ToString(), "Init()", 
                    "Adding station: id = " + stationRow["Id"].ToString(), "system");

                // add new Line Station !!!
                lineStations.Add(myStation);


                // add controls to the station from database
                this.detroitDataSet.EnforceConstraints = false;
                this.controlTableAdapter.FillByStationId(this.detroitDataSet.Control, Convert.ToInt32(stationRow["Id"]));
                for (int j = 0; j <= this.detroitDataSet.Control.Rows.Count - 1; j++)
                {
                    DataRow controlRow = this.detroitDataSet.Control.Rows[j];
                    this.myLog.LogAlert(AlertType.System, this.name, this.GetType().ToString(), "Init()",
                        "Adding control: id = " + Convert.ToInt32(controlRow["Id"]) + ", name = " + controlRow["Name"].ToString(), "system");
                    myStation.AddControl(Convert.ToInt32(
                        controlRow["Id"]),
                        controlRow["Name"].ToString(),
                        controlRow["OPC_variable"].ToString(),
                        controlRow["OPC_signal_variable"].ToString(),
                        controlRow["KeyString"].ToString(),
                        controlRow["IPAddr"].ToString(),
                        (int)controlRow["Channel"],
                        (int)controlRow["ModuleType"],
                        controlRow["Address"].ToString()
                    );
                }
            }

            this.OPCInit(this.lineStations, this.OPCProvider.OPCVariables);
            //this.SetOPCMode(false, Properties.Settings.Default.OPCServer);
            this.SetOPCMode(true, Properties.Settings.Default.OPCServer);
            this.isOPCAvailable = true;

            this.myLog.LogAlert(AlertType.System, this.Id.ToString(), this.GetType().ToString(), "Init()",
                    "SetOPCMode = true", "system");

            //x this.lineQueue = new LineQueue(this, this.myLog);
            //x this.productBuffer.OnRemoveProduct += new EventHandler(this.productBuffer_OnRemoveProduct);

            //x this.uncompletedProducts = new UncompletedProducts(this, this.detroitDataSet);
            //x this.batchStock = new BatchStock();

            // clean products with state "line" from cache table
            //this.lineSnapshotTableAdapter.DeleteByLineId(this.id);  // tbd - this must be removed than line will going to restore form LineSnapshot
            this.lineSnapshotTableAdapter.Fill(this.lineSnapshotTable, this.id);

            IEnumerable<IStation> lineIStatons = this.lineStations;
            this.dispatcher = new Dispatcher(this.id, lineIStatons, this.myLog);
            this.supplier = new Supplier(this.id, this.detroitDataSet, this.myLog, dispatcher);
            
            this.timeManager = new TimeManager(lineIStatons, this.myLog, this.detroitDataSet);

            this.restoreLine();

            this.dispatcher.OnProductMoved += new EventHandler<DispatcherMoveArgs>(dispatcher_OnProductMoved);
            this.dispatcher.OnProductFinishedLine += new EventHandler<DispatcherMoveArgs>(dispatcher_OnProductFinishedLine);
            this.timeManager.OnTick += new EventHandler(timeManager_OnTick);
            this.timeManager.SlowTask += new EventHandler(timeManager_SlowTask);
            this.timeManager.FastTask += new EventHandler(timeManager_FastTask);

            // link one time handler for all stations on the line
            for (int i = 0; i <= this.lineStations.Count - 1; i++)
            {
               // this.lineStations[i].OnFree += new EventHandler(this.onFreeStation);
            }

            this.timeManager.On();
         }



        //#region IAssembLine Members

        public override LineStationBase GetStation(int stationId)
        {
            try
            {
                return (LineStationBase)this.lineStations.Find(p => p.Id.Equals(stationId));
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.Id.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
                return null;
            }
        }
        public override List<LineStationBase> GetStations()
        {
            try
            {
                List<LineStationBase> resultList = new List<LineStationBase>();
                foreach (TimedLineStation station in this.lineStations)
                {
                    resultList.Add((LineStationBase)station);
                }
                return null;
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.Id.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
                return null;
            }
        }
        public LineStationBase[] GetStationsArray()
        {
            //LineStationBase[] resultList = new LineStationBase[this.lineStations.Count];
            //int i = 0;
            //foreach (TimedLineStation station in this.lineStations)
            //{
            //    resultList[i] = ((LineStationBase)station);
            //    i++;
            //}


            try
            {

                // Was passed ok 24.05.2012
                //
                LineStationBase[] resultList = new LineStationBase[this.lineStations.Count];

                int i = 0;
                foreach (TimedLineStation station in this.lineStations)
                {
                    resultList[i] = new LineStationBase();
                    resultList[i].Id = station.Id;
                    resultList[i].Name = station.Name;
                    resultList[i].CurrentProductName = station.CurrentProductName;
                    i++;
                }

                return resultList;
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.Id.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
                return null;
            }
        }
        public void RemoveStation(string LineStationRemoveId)
        {
            try
            {
                lineStations.Remove(lineStations.Find(p => p.Id.Equals(LineStationRemoveId)));
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.Id.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }
     
        public int GetCounter()
        {
            try
            {
                return this.timeManager.LineCounter;
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.Id.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
                return 0;
            }
        }

        // Set takt value ONLY for current takt and right now
        public void SetCounter(int value) 
        {
            try
            {
                this.timeManager.LineCounter = value;
                //this.SetCounterForever(value);
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.Id.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }

        // Set takt value from NEXT takt and permanently
        // Reset takt value to automatic calculation if 'value' is '0'
        public void SetCounterForever(int value)
        {
            try
            {
                if (value > 0)
                {
                    // set just now (disabled)
                    //this.timeManager.LineCounter = value * 60;
                    
                    // set from next takt
                    this.timeManager.FreezeTaktDuration(value);
                }
                else
                {
                    this.timeManager.UnFreezeTaktDuration();
                }
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.Id.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }


        public void Move()
        {
            this.isMoveInProgress = true;
            try
            {
                this.dispatcher.MoveProductsOnLine();
                this.supplier.StartNewProduct();
                this.moveCounter++;

            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.Name, this.GetType().ToString(), ex.TargetSite.Name, ex.Message + " " + ex.StackTrace, "system");
                this.myLog.LogAlert(AlertType.Error, this.Name, this.GetType().ToString(), ex.TargetSite.Name, ex.ToString(), "system");
                this.isMoveInProgress = false;
            }
            this.isMoveInProgress = false;
        }
        public void Execute()
        {
            // Go in running mode only if Ready, Stopped or Terminated
            if ((this.state == 0) || (this.state == 1) || (this.state == 3))
            {
                this.state = 2;
                this.myLog.LogAlert(AlertType.System, "Execute()", "Line state = " + this.state.ToString());
            }

            // Start once timer for "ideal"  takts
            this.timeManager.On();
            //this.resetButtons();
        }
        public void Terminate()
        {
            // Go to "Stopped" mode
            this.timeManager.Backup();
            this.timeManager.Off();
            foreach (TimedLineStation station in this.lineStations)
            {
                station.Timers.PauseAll();
            }

            this.state = 1;
        }
        public void ResetTimer() 
        {
            this.Terminate();
            this.resetButtons();
            this.timeManager.Reset();
        }
        public void RestoreLine() 
        {
            this.restoreLine();
        }
        public int GetState()
        {
            return this.state;
        }
        public void SetOPCRestarted() 
        {
            this.isOPCRestarted = true;
            this.myLog.LogAlert(AlertType.System, this.name, this.GetType().ToString(), "SetOPCRestarted()", "isOPCRestarted = true", "system");
        }

        public int PushStationButton(int StationIndex, string ControlKey)
        {
            try
            {
                string productName = "NA";
                Product currentProduct = this.lineStations[StationIndex - 1].CurrentProduct;
                if (currentProduct != null)
                {
                    productName = currentProduct.Name;
                }

                int buttonState = this.lineStations[StationIndex - 1].PushButton(ControlKey);
                if (buttonState == 1)
                {
                    this.myLog.LogAlert(AlertType.Info, this.id.ToString(), this.GetType().ToString(), "PushStationButton()",
                        this.lineStations[StationIndex - 1].Name + ", " + ControlKey + " button pressed." +
                        " Chassis N" + productName
                        , "system");
                }
                else if (buttonState == 0) 
                {
                    this.myLog.LogAlert(AlertType.Info, this.id.ToString(), this.GetType().ToString(), "PushStationButton()",
                            this.lineStations[StationIndex - 1].Name + ", " + ControlKey + " button de-pressed." +
                            " Chassis N" + productName
                            , "system");
                }


                // process only determinated PART buttons
                if (this.lineStations[StationIndex - 1].StationControls[ControlKey].PartsAddress != "")
                {
                    this.fillLogisticRequestTable(StationIndex, ControlKey, buttonState);
                }

                this.checkFailFunction(StationIndex, ControlKey, buttonState);
                //this.checkFinishButton(StationIndex, ControlKey, buttonState);

                return buttonState;
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.Id.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
                return 0;
            }
        }

        private void freezeButtonTemporary(int StationIndex, string ControlKey) 
        {
            try
            {
                this.lineStations[StationIndex - 1].FreezeButton(ControlKey);
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.Id.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }

        public string ReadStationButton(int StationIndex, string ControlKey)
        {
            try
            {
                return this.lineStations[StationIndex - 1].ReadButton(ControlKey);
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.Id.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
                return "";
            }
        }
        public string ReadStationName(int StationIndex)
        {
            try
            {
                return this.lineStations[StationIndex - 1].Name;
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.Id.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
                return "";
            }
        }

        public void SetOPCMode(bool mode, string OPCServerName) 
        {
            this.opcMode = mode;
            if (!mode)
            {
                // disconnect from OPC
                this.OPCProvider.OPCDisconnect();
            }
            else 
            { 
                // connect to OPC
                this.OPCProvider.OPCConnect(OPCServerName);
            }
        }
        public bool GetOPCMode() 
        { 
            return this.opcMode;        
        }

        public string ReadProduct(int stationIndex) 
        {
            try
            {
                string result = "NA";
                if (this.lineStations[stationIndex - 1].CurrentProduct != null)
                {
                    result = this.lineStations[stationIndex - 1].CurrentProduct.Name;
                }

                return result;
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.Id.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
                return "";
            }
        }
        public ProductBase[] GetProductStock() 
        {
            try {
                return this.supplier.GetProductStock();
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.Id.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
                return null;
            }

        }
        public ProductBase[] GetProductBuffer()
        {
            try
            {
                ProductBase[] result = this.supplier.GetProductBuffer();
                return result;
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.Id.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
                return null;
            }
        }
        public ProductBase[] GetStationBuffer(int stationId) 
        {
            try
            {
                //LineStation station = this.lineStations.Find(p => p.Id.Equals(stationId));
                //ProductBase[] buffer = station.Buffer.ToArray();
                //return buffer;
                ProductBase[] result = null;
                TimedLineStation station = this.lineStations.Find(p => p.Id.Equals(stationId));
                if (station != null || station.Buffer.Count > 0)
                {
                    result = new ProductBase[station.Buffer.Count];
                    for (int i = 0; i < station.Buffer.Count; i++)
                    {
                        result[i] = new ProductBase();
                        result[i].Name = station.Buffer.ToArray()[i].Name;
                        result[i].Id = station.Buffer.ToArray()[i].Id;
                    }
                }
                else
                {
                    result = null;
                }
                return result;
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.Id.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
                return null;
            }
        }
        public StationRealtimeData[] ReadRealTimeData(int stationIndex) 
        {
            try
            {
                TimedLineStation station = this.lineStations[stationIndex - 1];

                int dimention = station.Timers.KeyList().Count();
                dimention += 2;  // GAPS ...
                dimention += 6;  // BUTTONS ...
                dimention += 2;  // MOVE_COUNTER, EVENT_COUNTER

                dimention += 2;  // LINE_NAME, FRZ
                dimention += 5;  // T, S, P, F, SS
                dimention += 2;  // REGP.D, REGF.D
                dimention += 2;  // BitState, LIVE
                dimention += 2;  // REGP.M, REGF.M



                StationRealtimeData[] result = new StationRealtimeData[dimention];

                int i = 0;
                for (i = 0; i < station.Timers.KeyList().Count(); i++)
                {
                    result[i] = new StationRealtimeData();
                    string key = station.Timers.KeyList()[i];
                    result[i].Key = "TIMER_" + key;
                    result[i].Value = station.GetTimerValue(key).ToString();
                }

                result[i] = new StationRealtimeData();
                result[i].Key = "GAP_DAY";
                result[i].Value = this.timeManager.GetDayGap(station.Name).ToString(); //this.dayGap.ToString();

                i++;
                result[i] = new StationRealtimeData();
                result[i].Key = "GAP_MONTH";
                result[i].Value = this.timeManager.GetMonthGap(station.Name).ToString(); //this.monthGap.ToString();

                i++;
                result[i] = new StationRealtimeData();
                result[i].Key = "FINISH";
                result[i].Value = this.lineStations[stationIndex - 1].ReadButton(1);

                i++;
                result[i] = new StationRealtimeData();
                result[i].Key = "STOP";
                result[i].Value = this.lineStations[stationIndex - 1].ReadButton(2);

                i++;
                result[i] = new StationRealtimeData();
                result[i].Key = "HELP";
                result[i].Value = this.lineStations[stationIndex - 1].ReadButton(3);

                i++;
                result[i] = new StationRealtimeData();
                result[i].Key = "PART1";
                result[i].Value = this.lineStations[stationIndex - 1].ReadButton(4);

                i++;
                result[i] = new StationRealtimeData();
                result[i].Key = "PART2";
                result[i].Value = this.lineStations[stationIndex - 1].ReadButton(5);

                i++;
                result[i] = new StationRealtimeData();
                result[i].Key = "FAIL";
                result[i].Value = this.lineStations[stationIndex - 1].ReadButton(6);

                i++;
                result[i] = new StationRealtimeData();
                result[i].Key = "MOVE_COUNTER";
                result[i].Value = this.moveCounter.ToString();

                i++;
                result[i] = new StationRealtimeData();
                result[i].Key = "EVENT_COUNTER";
                result[i].Value = this.eventCounter.ToString();

                i++;
                result[i] = new StationRealtimeData();
                result[i].Key = "LINE_NAME";
                result[i].Value = this.name;

                i++;
                result[i] = new StationRealtimeData();
                result[i].Key = "FRZ";
                result[i].Value = this.timeManager.FreezedTaktDuration.ToString();

                i++;
                result[i] = new StationRealtimeData();
                result[i].Key = "T";
                result[i].Value = this.GetCounter().ToString();

                i++;
                result[i] = new StationRealtimeData();
                result[i].Key = "S";
                result[i].Value = this.ReadStationName(stationIndex);

                i++;
                result[i] = new StationRealtimeData();
                result[i].Key = "B";
                result[i].Value = this.ReadProduct(stationIndex);

                i++;
                result[i] = new StationRealtimeData();
                result[i].Key = "F";
                result[i].Value = this.ReadFrame().Name;

                i++;
                result[i] = new StationRealtimeData();
                result[i].Key = "SS";
                result[i].Value = this.GetSumStopTime().ToString();

                i++;
                result[i] = new StationRealtimeData();
                result[i].Key = "REGP.D";
                result[i].Value = this.timeManager.GetStationDayPlan(station.Name).ToString();

                i++;
                result[i] = new StationRealtimeData();
                result[i].Key = "REGF.D";
                result[i].Value = this.timeManager.GetStationDayFact(station.Name).ToString();


                i++;
                result[i] = new StationRealtimeData();
                result[i].Key = "BST";
                result[i].Value = this.lineStations[stationIndex - 1].BitState.ToString();

                i++;
                result[i] = new StationRealtimeData();
                result[i].Key = "LIVE";
                result[i].Value = this.lineStations[stationIndex - 1].Timers.Value(TimerKey.LiveTakt.ToString()).ToString();


                i++;
                result[i] = new StationRealtimeData();
                result[i].Key = "REGP.M";
                result[i].Value = this.timeManager.GetStationMonthPlan(station.Name).ToString();

                i++;
                result[i] = new StationRealtimeData();
                result[i].Key = "REGF.M";
                result[i].Value = this.timeManager.GetStationMonthFact(station.Name).ToString();

                return result;
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.Id.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
                return null;
            }
        }
        public StationRealtimeData[] ReadRealTimeDataForLine(int defStationIndex)
        {

            StationRealtimeData[] result = ReadRealTimeData(defStationIndex);
          
            try
            {
                int buttonCount = 13;
                int stationCount = this.lineStations.Count;
                int offset = result.Length;
                StationRealtimeData[] tmp = new StationRealtimeData[result.Length + stationCount * buttonCount];
                Array.Copy(result, 0, tmp, 0, result.Length);
                result = tmp;

                for (int stationIndex = 0; stationIndex < stationCount; stationIndex++)
                {
                    int itemIndex = offset + stationIndex * buttonCount;
                    result[itemIndex + 0] = new StationRealtimeData();
                    result[itemIndex + 0].Key = "FI" + (stationIndex + 1).ToString();
                    result[itemIndex + 0].Value = this.lineStations[stationIndex].ReadButton(1);

                    result[itemIndex + 1] = new StationRealtimeData();
                    result[itemIndex + 1].Key = "ST" + (stationIndex + 1).ToString();
                    result[itemIndex + 1].Value = this.lineStations[stationIndex].ReadButton(2);

                    result[itemIndex + 2] = new StationRealtimeData();
                    result[itemIndex + 2].Key = "HE" + (stationIndex + 1).ToString();
                    result[itemIndex + 2].Value = this.lineStations[stationIndex].ReadButton(3);

                    result[itemIndex + 3] = new StationRealtimeData();
                    result[itemIndex + 3].Key = "P1" + (stationIndex + 1).ToString();
                    result[itemIndex + 3].Value = this.lineStations[stationIndex].ReadButton(4);

                    result[itemIndex + 4] = new StationRealtimeData();
                    result[itemIndex + 4].Key = "P2" + (stationIndex + 1).ToString();
                    result[itemIndex + 4].Value = this.lineStations[stationIndex].ReadButton(5);

                    result[itemIndex + 5] = new StationRealtimeData();
                    result[itemIndex + 5].Key = "FA" + (stationIndex + 1).ToString();
                    result[itemIndex + 5].Value = this.lineStations[stationIndex].ReadButton(6);

                    result[itemIndex + 6] = new StationRealtimeData();
                    result[itemIndex + 6].Key = "BST" + (stationIndex + 1).ToString();
                    result[itemIndex + 6].Value = this.lineStations[stationIndex].BitState.ToString();

                    result[itemIndex + 7] = new StationRealtimeData();
                    result[itemIndex + 7].Key = "REGP.D" + (stationIndex + 1).ToString();
                    result[itemIndex + 7].Value = this.timeManager.GetStationDayPlan(this.lineStations[stationIndex].Name).ToString();

                    result[itemIndex + 8] = new StationRealtimeData();
                    result[itemIndex + 8].Key = "REGF.D" + (stationIndex + 1).ToString();
                    result[itemIndex + 8].Value = this.timeManager.GetStationDayFact(this.lineStations[stationIndex].Name).ToString();

                    result[itemIndex + 9] = new StationRealtimeData();
                    result[itemIndex + 9].Key = "LIVE" + (stationIndex + 1).ToString();
                    result[itemIndex + 9].Value = ((TimedLineStation)(this.lineStations[stationIndex])).Timers.Value(TimerKey.Operating.ToString()).ToString();

                    result[itemIndex + 10] = new StationRealtimeData();
                    result[itemIndex + 10].Key = "LIVTA" + (stationIndex + 1).ToString();
                    result[itemIndex + 10].Value = this.lineStations[stationIndex].Timers.Value(TimerKey.LiveTakt.ToString()).ToString();

                    result[itemIndex + 11] = new StationRealtimeData();
                    result[itemIndex + 11].Key = "REGP.M" + (stationIndex + 1).ToString();
                    result[itemIndex + 11].Value = this.timeManager.GetStationMonthPlan(this.lineStations[stationIndex].Name).ToString();

                    result[itemIndex + 12] = new StationRealtimeData();
                    result[itemIndex + 12].Key = "REGF.M" + (stationIndex + 1).ToString();
                    result[itemIndex + 12].Value = this.timeManager.GetStationMonthFact(this.lineStations[stationIndex].Name).ToString();

                }

            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, "ReadRealTimeDataForLine()", ex.Message);
            }

            return result;
        }
        public Frame ReadFrame() 
        {
            try
            {
                Frame result = this.timeManager.Frame;
                return result;
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.Id.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
                return null;
            }

        }
        public LogistTailElem[] GetLogisticTails()
        {
            // test code ...
            //// begin of temp proxy code : ======================================
            //LogistTailElem[] tails = new LogistTailElem[3];

            //tails[0] = new LogistTailElem();
            //tails[0].BatchName = "340001/8";
            //tails[0].BatchType = "8x6T";
            //tails[0].TailStationName = "FI";

            //tails[1] = new LogistTailElem();
            //tails[1].BatchName = "34000/5";
            //tails[1].BatchType = "8x6T";
            //tails[1].TailStationName = "FA2.1";

            //tails[2] = new LogistTailElem();
            //tails[2].BatchName = "22003";
            //tails[2].BatchType = "2x3C";
            //tails[2].TailStationName = "FA1";

            //int arraySize = (int)(DateTime.Now.Second / 20) + 1;
            //LogistTailElem[] newTails = new LogistTailElem[arraySize];
            //for (int x = 0; x < arraySize; x++)
            //{
            //    newTails[x] = tails[x];
            //}

            //return newTails; 

            //// end of temp proxy code : ======================================


            try
            {
                LogistTailElem[] result = new LogistTailElem[this.logisticTailTable.Count];
                this.logisticTailTable.Values.CopyTo(result, 0);
                return result;
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.Id.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
                return null;
            }
        }

        // ------------------------------------------------------------------
        // ILogistic 
        // ------------------------------------------------------------------

        public LogistRequestElem[] GetLogisticRequests()
        {
            try
            {
                LogistRequestElem[] result = new LogistRequestElem[this.logisticRequestTable.Count];
                this.logisticRequestTable.Values.CopyTo(result, 0);

                foreach (LogistRequestElem elem in result)
                {
                    elem.WaitingTime = this.lineStations[elem.StationIndex - 1].GetTimerValue(elem.Key);


                    DateTime baseTime = DateTime.Today.AddDays(-7);
                    DateTime requestWasMadeTime = DateTime.Now.AddSeconds(-elem.WaitingTime);
                    long requestOrderNum = requestWasMadeTime.Ticks - baseTime.Ticks;

                    elem.OrderNum = Convert.ToInt32(requestOrderNum / (10000 * 1000)); // result in seconds
                }

                return result;
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.Id.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
                return null;
            }
        }
        public LogistTailElem[] GetTails() 
        {
            return this.GetLogisticTails();
        }
        public LogistTailElem[] GetBatchesOnLine()
        {
            try
            {
                LogistTailElem[] result = new LogistTailElem[0];

                IEnumerable<Batch> batchList = this.supplier.GetBatchesOnLine();
                if (batchList != null)
                {
                    result = new LogistTailElem[batchList.Count()];
                    int i = 0;
                    foreach (Batch batch in batchList)
                    {
                        result[i] = new LogistTailElem();
                        result[i].BatchName = batch.Name;
                        result[i].BatchType = batch.Type;
                        result[i].TailStationName = "";
                        i++;
                    }

                }
                return result;
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.Id.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
                return null;
            }
        }
        public LogisticInfo GetLogisticInfo()
        {
            try
            {
                return this.supplier.GetNextBatchInfo();
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.Id.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
                return null;
            }
        }
        public StationRealtimeData[] ReadLogisticRealTimeData(int stationIndex) 
        {
            return this.ReadRealTimeData(stationIndex);
        }


        
        
        public int GetSumStopTime() 
        {
            //return this.sumStopTime;
            return 0;
        }
        public void SetStationDayPlan(string stationName, int amount) 
        {
            try
            {
                this.timeManager.SetStationDayPlan(stationName, amount);
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.Id.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }
        public void SetStationMonthPlan(string stationName, int amount)
        {
            try
            {
                this.timeManager.SetStationMonthPlan(stationName, amount);
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.Id.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }
        public void SetStationDayFact(string stationName, int amount)
        {
            try
            {
                this.timeManager.SetStationDayFact(stationName, amount);
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.Id.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }
        public void SetStationMonthFact(string stationName, int amount)
        {
            try
            {
                this.timeManager.SetStationMonthFact(stationName, amount);
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.Id.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }

        public void FinishStation(string stationName) 
        {
            try
            {
                TimedLineStation station = this.lineStations.FirstOrDefault(p => p.Name.Equals(stationName));
                if (station != null)
                {
                    station.Finish();
                }
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.Id.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }


        private void checkOPCServer()
        {
            try
            {
                if (this.isOPCRestarted)
                {
                    //this.OPCProvider = new LineOPCProvider(this.myLog);
                    //this.OPCInit(this.lineStations, this.OPCProvider.OPCVariables);
                    //this.OPCProvider.ImpDataChanged += new EventHandler(this.ButtonPressed);
                    this.SetOPCMode(false, Properties.Settings.Default.OPCServer);
                    this.SetOPCMode(true, Properties.Settings.Default.OPCServer);
                    this.isOPCRestarted = false;
                    //this.myLog.LogAlert(AlertType.System, "ckeckOPCServer()", "OPC reconnected. " + Properties.Settings.Default.OPCServer );
                }

                int opcState = this.OPCProvider.State;
                //this.myLog.LogAlert(AlertType.System, "ckeckOPCServer()", "opcState = " + opcState.ToString());
                this.isOPCAvailable = true;

            }
            catch (Exception ex) 
            {
                this.isOPCAvailable = false;
                this.myLog.LogAlert(AlertType.System, "ckeckOPCServer()", "OPC Server is not available.");
                this.myLog.LogAlert(AlertType.Error, this.Name, this.GetType().ToString(), "ckeckOPCServer()",
                    ex.ToString(), "system");            
            }
        }


        private int calcPlanGap(Product enProduct, PlanMode planMode)
        {
            //int gap = 0;
            //ProductTableAdapter productTableAdapter = new ProductTableAdapter();

            //string planKey = "line " + this.id.ToString();
            //if (planMode == PlanMode.Day)
            //{
            //    planKey += " day " + DateTime.Today.ToShortDateString().ToString();
            //}
            //else if (planMode == PlanMode.Month)
            //{
            //    planKey += " mon " + new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).ToShortDateString().ToString();
            //}
            //else 
            //{ 
            //    // exception unrecognized plan type :(((
            //}


            //int gapTakts = 0;  // fact delay in takts
            //object db_result = productTableAdapter.GetPlanGap(this.idealTaktCounter.ToString(), planKey, enProduct.Owner.Id, enProduct.Id);
            //if (db_result != null)
            //{
            //    gapTakts = (int)db_result;
            //}
            
            //gap = gapTakts;

            //return gap;

            return -2;
        }



        private void fillLogisticRequestTable(int stationIndex, string controlKey, int buttonState) 
        {
            try
            {


                if (buttonState == 1 && controlKey.Substring(0, 4) == "PART")
                {
                    LogistRequestElem request = new LogistRequestElem();
                    request.StationName = this.lineStations[stationIndex - 1].Name;
                    request.PartName = ((Button)(this.lineStations[stationIndex - 1].StationControls[controlKey])).KeyString;
                    //request.WaitingTime = this.lineStations[stationIndex-1].GetTimerValue(controlKey);
                    request.WaitingTime = 0;
                    request.StationIndex = stationIndex;
                    request.Key = controlKey;
                    request.Address = ((Button)(this.lineStations[stationIndex - 1].StationControls[controlKey])).PartsAddress;
                                        
                    this.logisticRequestTable.Add(request.StationName + "-" + request.PartName, request);
                }
                else
                {
                    this.logisticRequestTable.Remove(
                        this.lineStations[stationIndex - 1].Name
                        + "-"
                        + ((Button)(this.lineStations[stationIndex - 1].StationControls[controlKey])).KeyString
                        );
                }
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.Id.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }

        private void fillLogisticTail()
        {
            try
            {
                this.logisticTailTable.Clear();

                foreach (LineStation station in this.lineStations)
                {
                    Product product = station.CurrentProduct;
                    if (product != null)
                    {
                        if (product.Id == product.Owner.Capacity)
                        {
                            LogistTailElem tail = new LogistTailElem();
                            string key = (station.Name + "-" + product.Owner.Name + "/" + product.Id.ToString());

                            tail.BatchName = product.Owner.Name + "/" + product.Id.ToString();
                            tail.BatchType = product.Owner.Type; // tbd get type name
                            tail.TailStationName = station.Name;
                            tail.TailStationIndex = station.Index;
                            this.logisticTailTable.Add(key, tail);
                        }
                    }
                }
                this.eventCounter++;
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.Id.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }


        private void fillLogisticTailTable2(LineStation station, Product product, StationProductAction action) 
        {
            try
            {
                // is it "tale" of batch?
                if (product.Id == product.Owner.Capacity)
                {
                    LogistTailElem tail = new LogistTailElem();
                    string key = (station.Name + "-" + product.Owner.Name + "/" + product.Id.ToString());

                    if (action == StationProductAction.Add)
                    {
                        tail.BatchName = product.Owner.Name + "/" + product.Id.ToString();
                        tail.BatchType = product.Owner.Type; // tbd get type name
                        tail.TailStationName = station.Name;
                        tail.TailStationIndex = station.Index;
                        this.logisticTailTable.Add(key, tail);
                    }
                    else
                    {
                        this.logisticTailTable.Remove(key);
                    }
                }
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.Id.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }
        private void checkFailFunction(int stationIndex, string controlKey, int buttonState) 
        {
            try
            {
                if (buttonState == 1 && controlKey.Substring(0, 4) == "FAIL")
                {
                    LineStation enStation = this.lineStations[stationIndex - 1];
                    Product enProduct = enStation.CurrentProduct;

                    if (enProduct != null)  // !!!!
                    {
                        this.supplier.SetUncompletedProduct(enProduct);  // check it at the end of the AssemblyLine
                        enProduct.OperationFailed(enStation.Name);
                    }

                    //this.updateProductInLineSnapshot(enProduct);
                    //this.lineStations[stationIndex - 1].ReleaseProduct();
                    //enProduct.Owner.RemoveProduct(enProduct);
                    //this.removeProductFromQueue((Queue<Product>)this.productsOnLine, enProduct);
                }
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.Id.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }
 
        private void backupLine() 
        {
            // data to backup:
            // - products in buffer
            // - products on line (stations and buffers)
            // - batch on line
            // - takt timer
            // - buttons state
            // - stations timers

            try
            {

                this.timeManager.Backup();

                LineParams lineParams = new LineParams();
                lineParams.LineCounter = this.GetCounter();
                lineParams.MoveCounter = this.moveCounter;
                lineParams.LineState = this.state;
                lineParams.EventCounter = this.eventCounter;
                lineParams.DayTaktNo = this.timeManager.DayTaktNo;
                lineParams.MonthTaktNo = this.timeManager.MonthTaktNo;

                Serialization sAgent = new Serialization("serialization_line" + this.Id.ToString() + "_user.dat");
                sAgent.Backup(lineParams);

                foreach (TimedLineStation ts in this.lineStations)
                {
                    ts.Timers.Backup();
                    //ts.Backup();
                }
            }
            catch (Exception ex)
            {
                myLog.LogAlert(AlertType.Error, this.name, this.GetType().ToString(), "backupLine()",
                    ex.Message, "system");
            }

        }
        private void restoreLine() 
        {
            Serialization sAgent = new Serialization("serialization_line" + this.Id.ToString() + "_user.dat");
            LineParams restoredLine = new LineParams();
            try
            {
                restoredLine = sAgent.Restore(restoredLine);

                if (restoredLine.DayTaktNo == 0) restoredLine.DayTaktNo = 1;
                if (restoredLine.MonthTaktNo == 0) restoredLine.MonthTaktNo = 1;
                if (restoredLine.LineCounter <= 0) restoredLine.LineCounter = 3600; // TODO: Default line takt = 60 min ?!

                this.moveCounter = restoredLine.MoveCounter;
                this.timeManager.LineCounter = restoredLine.LineCounter;
                this.eventCounter = restoredLine.EventCounter;
                this.state = restoredLine.LineState;


                //this.timeManager.LineCounter = 30; 77

                this.timeManager.DayTaktNo = 1; // TODO: restoredLine.DayTaktNo;
                this.timeManager.MonthTaktNo = 1; // restoredLine.MonthTaktNo;

                //restore buttons state?!  not yet
                this.resetSignals();
                this.supplier.Restore();
                this.timeManager.Restore();

                foreach (TimedLineStation ts in this.lineStations)
                {
                    ts.Timers.Restore();
                    //ts.Restore();
                }

            }
            catch (Exception ex) 
            {
                myLog.LogAlert(AlertType.Error, this.name, this.GetType().ToString(), "restoreLine()",
                    ex.Message, "system");

                this.moveCounter = 0;
                this.eventCounter = 0;
                //this.timeManager.LineCounter = 1000;
                this.state = 2;
            }

            //if (this.state == 2) 
            //    this.timeManager.On();
        }

        private void handlerOPCProvider_ServerRestarted(object sender, EventArgs e) 
        {
            this.SetOPCRestarted();
        }
        private void batchesOnLine_OnListChange(object sender, EventArgs e) 
        { 
            // publish to Logistic service

            ////LogisticService.LogistBatch[] batchArray = new LogisticService.LogistBatch[this.batchesOnLine.Count];
            ////int i = 0;
            ////foreach (Batch batch in this.batchesOnLine)
            ////{
            ////    batchArray[i] = new LogisticService.LogistBatch()
            ////    {
            ////        BatchName = batch.Name,
            ////        BatchType = batch.Type,
            ////        LineId = this.Id.ToString()
            ////    };
            ////}
            ////try
            ////{
            ////    this.logisticCollector.RenewBatchesOnLine(batchArray);
            ////}
            ////catch (Exception ex)
            ////{
            ////    this.myLog.LogAlert(AlertType.Error, this.name, this.GetType().ToString(), "batchesonline_onlistchange()", ex.ToString(), "system");            
            ////}
        }
        private void productBuffer_OnRemoveProduct(object sender, EventArgs e) 
        {
            // get "Next Batch info for Logistic"
            ////LogisticService.LogisticInfo result = new LogisticService.LogisticInfo();
            ////result.NextBatchName = "NA";
            ////result.TaktsTillNextBatch = 0;
            ////result.LineId = this.id.ToString();
            ////Batch nextBatch;
            ////if (this.lineQueue.Count > 0)
            ////{
            ////    nextBatch = this.lineQueue.Peek();
            ////    result.NextBatchName = nextBatch.Name;
            ////}
            ////result.TaktsTillNextBatch = this.productBuffer.Count;

            ////try
            ////{
            ////    this.logisticCollector.RenewNextBatch(result);
            ////}
            ////catch (Exception ex)
            ////{
            ////    this.myLog.LogAlert(AlertType.Error, this.name, this.GetType().ToString(), "batchesonline_onlistchange()", ex.ToString(), "system");
            ////}
        }

        private void dispatcher_OnProductMoved(object sender, DispatcherMoveArgs e) 
        {
            try
            {
                this.eventCounter++;
                string msg = String.Format("Chassis {0} moved to station: {1}", e.Product.Name, string.Join(",", e.DestinationList));
                this.myLog.LogAlert(AlertType.Info, this.id.ToString(), this.GetType().ToString(), "dispatcher_OnProductMoved()", msg, "system");
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.Id.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }


        void dispatcher_OnProductFinishedLine(object sender, DispatcherMoveArgs e)
        {
            this.eventCounter++;
        }


        private void timeManager_OnTick(object sender, EventArgs e)
        {
            //this.lineCounter = this.timeManager.LineCounter();
        }
        private void timeManager_SlowTask(object sender, EventArgs e) 
        {
            try
            {
                this.fillLogisticTail();
                this.backupLine();
                this.makeSnapShot();
                this.checkOPCServer();
                this.supplier.StartNewProduct();
                this.supplier.RefreshLineQueue();
                //Console.WriteLine(DateTime.Now.ToString() + "  timeManager_SlowTask done.");
            }
            catch (Exception ex)
            {
                myLog.LogAlert(AlertType.Error, this.name, this.GetType().ToString(), "timeManager_SlowTask()",
                    ex.ToString(), "system");
            }
        }

        private void timeManager_FastTask(object sender, EventArgs e) 
        { 
            
        }

        private void makeSnapShot()
        {
            // put products from buffer in Detroit LineSnapshot table
            // put products from line in Detroit LineSnapshot table

            try
            {
                this.supplier.UpdateLineSnaphot();
                this.lineSnapshotTableAdapter.DeleteByLineId(this.id);
                this.lineSnapshotTableAdapter.Update(this.lineSnapshotTable);


                foreach (DataRow row in this.lineSnapshotTable.Rows)
                {
                    if (row.RowState != DataRowState.Deleted)
                    {
                        row["TimeStamp"] = DateTime.Now;
                        row["TaktSecondsLeft"] = this.timeManager.LineCounter;
                    }
                }

                this.lineSnapshotTableAdapter.Update(this.lineSnapshotTable);
            }
            catch (DBConcurrencyException ex) 
            {
                if (this.lineSnapshotTable.HasErrors)
                {
                    DataRow[] drs = this.lineSnapshotTable.GetErrors();
                    foreach (DataRow dr in drs)
                    {
                        string test = dr.RowError.Substring(0, 21);
                        if (test == "Concurrency violation")
                        {
                            this.lineSnapshotTable.RemoveLineSnapshotRow((DetroitDataSet.LineSnapshotRow)dr);
                        }
                    }
                    this.lineSnapshotTable.AcceptChanges();

                    if (this.lineSnapshotTable.HasErrors)
                    {
                        throw new DataException("An exception was raised while updating the ReportColumn data table: "
                            + this.lineSnapshotTable.GetErrors()[0].RowError);
                    }
                }

                myLog.LogAlert(AlertType.Error, this.name, this.GetType().ToString(), "makeSnapShot()",
                    ex.Message, "system");
            }
        }

        public int TaktDuration
        {
            get { return this.timeManager.TaktDuration; }
        }

        public List<LogMessage> GetMemLogData() 
        { 
            //LogMessage[] dataArray = new LogMessage[myLog.ErrorData.Count];

            //int i = 0;
            //foreach (DataSet1.MemLogRow dataRow in myLog.ErrorData)
            //{
            //    dataArray[i] = new LogMessage();

            //    dataArray[i].EventTime = dataRow.EventTime;
            //    dataArray[i].AlertType = dataRow.AlertType;
            //    dataArray[i].Line = dataRow.Line;
            //    dataArray[i].ObjectType = dataRow.ObjectType;
            //    dataArray[i].ObjName = dataRow.ObjectNum;
            //    dataArray[i].MessageString = dataRow.Message;
            //    i++;
            //}

            ////myLog.Data.ToArray().CopyTo(dataArray,0);
            //return dataArray;


            return myLog.ErrorList;
        }

        //----------------------------------------------------------------------------
        //
        //   MediaServer functions !
        //
        //

        protected int clientContentMode = 2;
        protected string clientContentUrl = "";

        public ClientInstruction GetClientInstruction() 
        {
            ClientInstruction taktMode = new ClientInstruction() { Mode = this.clientContentMode, ContentUrl = this.clientContentUrl };
            return taktMode;
        }

        public void SetClientInstruction(int Mode, string ContentUrl) 
        {
            this.clientContentMode = Mode;
            this.clientContentUrl = ContentUrl;
        }




    }
}
