using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using LineService.DetroitDataSetTableAdapters;
using AppLog;
using System.Data;
using System.Runtime.Serialization;

namespace LineService
{
    [Serializable]
    class PlanRegister : SerializableDictionary<string, PlanRegisterRecord> 
    {
        public PlanRegister() {}
        public PlanRegister(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    
    [Serializable]
    class FactRegister : SerializableDictionary<string, RegisterRecord> 
    {
        public FactRegister() {}
        public FactRegister(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    class TMSavedData 
    {
        public PlanRegister planRegister;
        public FactRegister factRegister;

        public PlanRegister monthPlanRegister;
        public FactRegister monthFactRegister;
    }

    [Serializable]
    class PlanRegisterRecord
    {
        public string StationName;
        public int TaktNo;
        public int Amount;

        public PlanRegisterRecord() {}
        public PlanRegisterRecord(SerializationInfo info, StreamingContext context) : base() 
        {
            StationName = (string)info.GetValue("StationName", typeof(string));
            TaktNo = (int)info.GetValue("TaktNo", typeof(int));
            Amount = (int)info.GetValue("Amount", typeof(int));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("StationName", StationName, typeof(string));
            info.AddValue("TaktNo", TaktNo, typeof(int));
            info.AddValue("Amount", Amount, typeof(int));
        }

    }
   
    [Serializable]
    class RegisterRecord
    {
        public string StationName;
        public int SubTotal;
        public int TaktPosition;
    }

    public enum TMState { Off = 0, On = 1, Paused = 2 }

    public enum TFrame { NonWork = 0, Work = 1 };

    public static class FrameKeys 
    {
        public const string Reset = "*rt#";
        public const string ResetMonth = "*rmreg#";
        public const string ResetDay = "*rdr#";
    }

    public class PlanFactEventArgs : EventArgs {
        public IStation Station;

        public PlanFactEventArgs(IStation Station)
        {
            this.Station = Station;
        }
    }

    /// <summary>
    /// Counter class with associated string key
    /// </summary>
    public class KeyCounter : Counter 
    {
        private string key;
        public string Key { get { return this.key; } }
        
        public KeyCounter(int direction, int start_value, int step, TimerCounterType type, string key) 
            : base (direction, start_value, step, type)
        {
            this.key = key;
        }
    }

    public interface ITimeInformer 
    {
        bool IsWorkingTime { get; }
    }

    /// <summary>
    /// Controls time frames and takt counters
    /// </summary>
    public class TimeManager : ITimeInformer
    {
        private int idealTaktCounter = 1;       // number of current "ideal" takt without any stops TODAY
        private int idealMonthTaktCounter = 1;  // number of current "ideal" takt without any stops THIS MONTH

        private SchedulerFramesCache schedulerFramesCache;
        private Frame currentFrame; // 0 - non working, 1 - working
        private TMState state = TMState.Off;

        private Counter Takt;
        private int taktTimerStep = 1000;        // millisec
        private int taktDirection = 1;          // forward = 0, backward = 1
        private int taktDuration = 0;           // takt time in seconds

        private TaktTimer2 slowTaskTimer;
        private int slowTaskStep = 10000;       // 10 sec
        private TaktTimer2 fastTaskTimer;
        private int fastTaskStep = 500;        // 1 sec
        private DateTime currentDate;

        private DetroitDataSet detroitDataSet;
        private SchedulerFrameTableAdapter schedulerFrameTableAdapter;
        private bool handlerRoomFree = true;
        private int freezedTaktDuration = 0;  // in minutes !!

        private IEnumerable<IStation> lineStations;
        private LogProvider myLog;
   
        private PlanRegister planRegister;
        private FactRegister factRegister;
        private PlanRegister monthPlan;
        private FactRegister monthFact;

        
        //CONSTRUCTORS__________________________________________________________________________________________ 

        /// <summary>
        /// Defaulf abstract constructor
        /// </summary>
        public TimeManager()
        {
            this.planRegister = new PlanRegister() {
                { "FA0.1", new PlanRegisterRecord { StationName = "FA0.1", TaktNo = 1, Amount = 1 }},
                { "FA0.2", new PlanRegisterRecord { StationName = "FA0.2", TaktNo = 1, Amount = 0 }},
                { "FA1.1", new PlanRegisterRecord { StationName = "FA1.1", TaktNo = 1, Amount = 0 }}
            };

            this.factRegister = new FactRegister() {
                { "FA0.1", new RegisterRecord { StationName = "FA0.1", SubTotal = 0 }},
                { "FA0.2", new RegisterRecord { StationName = "FA0.2", SubTotal = 0 }},
                { "FA1.1", new RegisterRecord { StationName = "FA1.1", SubTotal = 0 }},
                { "FA1.2", new RegisterRecord { StationName = "FA1.2", SubTotal = 0 }},
                { "PFA2", new RegisterRecord { StationName = "PFA2", SubTotal = 0 }},
                { "FA2.1", new RegisterRecord { StationName = "FA2.1", SubTotal = 0 }},
                { "FA2.2", new RegisterRecord { StationName = "FA2.2", SubTotal = 0 }},
            };

            this.monthPlan = new PlanRegister() {
                { "FA0.1", new PlanRegisterRecord { StationName = "FA0.1", TaktNo = 1, Amount = 1 }},
                { "FA0.2", new PlanRegisterRecord { StationName = "FA0.2", TaktNo = 1, Amount = 0 }},
                { "FA1.1", new PlanRegisterRecord { StationName = "FA1.1", TaktNo = 1, Amount = 0 }}
            };

            this.monthFact = new FactRegister() {
                { "FA0.1", new RegisterRecord { StationName = "FA0.1", SubTotal = 0 }},
                { "FA0.2", new RegisterRecord { StationName = "FA0.2", SubTotal = 0 }},
                { "FA1.1", new RegisterRecord { StationName = "FA1.1", SubTotal = 0 }},
                { "FA1.2", new RegisterRecord { StationName = "FA1.2", SubTotal = 0 }},
                { "PFA2", new RegisterRecord { StationName = "PFA2", SubTotal = 0 }},
                { "FA2.1", new RegisterRecord { StationName = "FA2.1", SubTotal = 0 }},
                { "FA2.2", new RegisterRecord { StationName = "FA2.2", SubTotal = 0 }},
            };

            List<IStation> lineStations = new List<IStation>();
            lineStations.Add(new TestStation() { Name = "FA0.1" });
            lineStations.Add(new TestStation() { Name = "FA0.2" });
            lineStations.Add(new TestStation() { Name = "FA1.1" });
            this.lineStations = lineStations;

            this.currentFrame = new Frame();
            this.currentFrame.Name = "NA";
            this.currentFrame.Type = 0;

            this.Takt = new Counter(taktDirection, taktDuration, taktTimerStep);
            this.Takt.Pause();
            //x this.Takt.Elapsed += new EventHandler(lineTimer_handler);
            this.Takt.Zero += new EventHandler(zeroTakt_handler);

            // SLOW TIMER. 
            // Start timer to refresh data from Detroit database
            this.slowTaskTimer = new TaktTimer2(0, 0, slowTaskStep);
            this.slowTaskTimer.EventHandlerEnabled = false;
            this.slowTaskTimer.Elapsed += new EventHandler(slowTask_handler);


            // FAST TIMER. 
            // Start timer to refresh realtime data
            this.fastTaskTimer = new TaktTimer2(0, 0, fastTaskStep);
            this.fastTaskTimer.EventHandlerEnabled = false;
            this.fastTaskTimer.Elapsed += new EventHandler(fastTask_handler);

            this.currentDate = DateTime.Today;
            //this.currentDate = this.currentDate.AddDays(-1);
            this.schedulerFramesCache = new SchedulerFramesCache();
        }

        public TimeManager(IEnumerable<IStation> stations, LogProvider log, DetroitDataSet detroit) : this()
        {
            try
            {
                this.lineStations = stations;
                this.myLog = log;
                this.detroitDataSet = detroit;
                this.schedulerFrameTableAdapter = new DetroitDataSetTableAdapters.SchedulerFrameTableAdapter();
                this.schedulerFrameTableAdapter.FillByLineId(this.detroitDataSet.SchedulerFrame, this.detroitDataSet.LineId);

                //this.liveTakt = new Dictionary<string, KeyCounter>();
                //this.pausedLiveTakts = new List<KeyCounter>();

                this.factRegister.Clear();
                this.planRegister.Clear();
                this.monthFact.Clear();
                this.monthPlan.Clear();

                this.taktDuration = this.getTaktDuration() *60;
                foreach (IStation station in this.lineStations)
                {
                    station.OnFinished += new EventHandler<DispatcherStationArgs>(station_OnFinished);
                    station.OnGetNewProduct += new EventHandler<DispatcherStationArgs>(station_OnGetNewProduct);
                    station.OnFree += new EventHandler<DispatcherStationArgs>(station_OnFree);

                    this.factRegister.Add(station.Name, new RegisterRecord { StationName = station.Name, SubTotal = 0 });
                    this.planRegister.Add(station.Name, new PlanRegisterRecord { StationName = station.Name, TaktNo = 1, Amount = 1 });

                    this.monthFact.Add(station.Name, new RegisterRecord { StationName = station.Name, SubTotal = 0 });
                    this.monthPlan.Add(station.Name, new PlanRegisterRecord { StationName = station.Name, TaktNo = 1, Amount = 1 });

                    (station as TimedLineStation).TimeInformer = this;
                      
                    this.myLog.LogAlert(AlertType.System, this.detroitDataSet.LineId.ToString(), this.GetType().ToString(), "TimeManager()",
                                                 station.Name + ", plan = 1", "system");
                }

                this.updateStationPosition();
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.DeclaringType.FullName.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }

        }

     
        //PROPERTIES_____________________________________________________________________________________________
        
        public int LineCounter { 
            get { return this.Takt.GetIntValue(); }
            set 
            { 
                this.Takt.SetValue(value); 
            }
        }
        
        public Frame Frame 
        { get { return this.currentFrame; } }
        
        public int DayTaktNo 
        { get { return this.idealTaktCounter; } set { this.idealTaktCounter = value; } }
        
        public int MonthTaktNo 
        { 
            get { return this.idealMonthTaktCounter; } 
            set { this.idealMonthTaktCounter = value; } 
        }
        
        public bool IsWorkingTime
        {
            get { return (this.currentFrame.Type == 1); }
        }

        public int FreezedTaktDuration
        {
            get { return this.freezedTaktDuration; }
        }

        public int TaktDuration
        {
            get { return this.taktDuration; }
        }

      
        //METHODS________________________________________________________________________________________________

        public void On() 
        {
            try
            {
                this.fastTaskTimer.EventHandlerEnabled = true;
                this.slowTaskTimer.EventHandlerEnabled = true;

                this.state = TMState.On;

                this.slowTaskTimer.TimerStart();
                this.fastTaskTimer.TimerStart();

                if (this.currentFrame.Type == 1)
                {
                    this.Takt.Start();
                    this.startButtonTimers();
                }
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }
       
        public void Off() 
        {
            this.state = TMState.Off;
            this.Takt.Stop();
            this.slowTaskTimer.TimerStop();
            this.fastTaskTimer.TimerStop();
        }
       
        public void Reset() 
        {
            try
            {
                this.taktDuration = this.getTaktDuration() * 60;
                this.Takt.Destroy();
                this.Takt = new Counter(this.taktDirection, this.taktDuration, this.taktTimerStep);
                this.Takt.Pause();
                this.Takt.Zero += new EventHandler(zeroTakt_handler);
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }

        public int GetDayGap(string stationName) 
        { 
            int result = 0;
            // string key = stationName + "---" + this.idealTaktCounter.ToString();
            string key = stationName;
            try
            {
                if (this.planRegister.ContainsKey(key))
                {
                    int plan = this.planRegister[key].Amount;
                    int fact = this.factRegister[stationName].SubTotal;
                    result = fact - plan;
                }
            }
            catch (Exception ex) 
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(), ex.Source, ex.ToString(), "system"); 
                //Console.WriteLine(ex);
                return result;
            }
            return result;
        }

        public int GetMonthGap(string stationName) 
        {
            //return this.GetDayGap(stationName); // TODO: GetMonthGap ...

            int result = 0;
            string key = stationName;
            try
            {
                if (this.monthPlan.ContainsKey(key))
                {
                    int plan = this.monthPlan[key].Amount;
                    int fact = this.monthFact[stationName].SubTotal;
                    result = fact - plan;
                }
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(), ex.Source, ex.ToString(), "system");
                //Console.WriteLine(ex);
                return result;
            }
            return result;

        }

        public int GetStationDayPlan(string stationName) 
        {
            int result = 0;
            string key = stationName;
            try
            {
                if (this.planRegister.ContainsKey(key))
                {
                    result = this.planRegister[key].Amount;
                }
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(), ex.Source, ex.ToString(), "system"); 
                //Console.WriteLine(ex);
                return result;
            }
            return result;
        }
        
        public int GetStationDayFact(string stationName)
        {
            int result = 0;
            string key = stationName;
            try
            {
                if (this.factRegister.ContainsKey(key))
                {
                    result = this.factRegister[key].SubTotal;
                }
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(), ex.Source, ex.ToString(), "system"); 
                //Console.WriteLine(ex);
                return result;
            }
            return result;
        }

        public int GetStationMonthPlan(string stationName)
        {
            int result = 0;
            string key = stationName;
            try
            {
                if (this.monthPlan.ContainsKey(key))
                {
                    result = this.monthPlan[key].Amount;
                }
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(), ex.Source, ex.ToString(), "system");
                return result;
            }
            return result;
        }
        
        public int GetStationMonthFact(string stationName)
        {
            int result = 0;
            string key = stationName;
            try
            {
                if (this.monthFact.ContainsKey(key))
                {
                    result = this.monthFact[key].SubTotal;
                }
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(), ex.Source, ex.ToString(), "system");
                return result;
            }
            return result;
        }

        public void SetStationDayPlan(string stationName, int amount) 
        {
            try
            {
                if (this.planRegister != null && this.planRegister.ContainsKey(stationName))
                {
                    this.planRegister[stationName].Amount = amount;
                    this.checkSumLateEvent(null, stationName, this.GetStationDayFact(stationName) - amount);

                    IStation station = this.lineStations.FirstOrDefault(p => p.Name.Equals(stationName));
                    if (station != null) {
                        (station as TimedLineStation).TaktPosition = this.GetStationDayFact(stationName) - amount;
                    }
                }
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(), ex.Source, ex.ToString(), "system");
            }
        }

        public void SetStationMonthPlan(string stationName, int amount)
        {
            try
            {
                if (this.monthPlan != null && this.monthPlan.ContainsKey(stationName))
                {
                    this.monthPlan[stationName].Amount = amount;
                }
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }

        public void SetStationDayFact(string stationName, int amount)
        {
            try
            {
                if (this.factRegister != null && this.factRegister.ContainsKey(stationName))
                {
                    this.factRegister[stationName].SubTotal = amount;
                    this.checkSumLateEvent(null, stationName, amount - this.GetStationDayPlan(stationName));

                    IStation station = this.lineStations.FirstOrDefault(p => p.Name.Equals(stationName));
                    if (station != null)
                    {
                        (station as TimedLineStation).TaktPosition = amount - this.GetStationDayPlan(stationName);
                        if (PlanFactChangedOneStation != null) {
                            PlanFactChangedOneStation(this, new PlanFactEventArgs(station));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(), ex.Source, ex.ToString(), "system");
            }
        }

        public void SetStationMonthFact(string stationName, int amount)
        {
            try
            {
                if (this.monthFact != null && this.monthFact.ContainsKey(stationName))
                {
                    this.monthFact[stationName].SubTotal = amount;
                }
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }

        public void Backup() 
        { 
            TMSavedData dataToSave = new TMSavedData();
            dataToSave.planRegister = this.planRegister;
            dataToSave.factRegister = this.factRegister;
            dataToSave.monthPlanRegister = this.monthPlan;
            dataToSave.monthFactRegister = this.monthFact;

            try
            {
                Serialization sAgent = new Serialization("serialization_tm" + this.detroitDataSet.LineId.ToString() + "_user.dat");
                sAgent.Backup(dataToSave);
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(), ex.Source, ex.ToString(), "system"); 
            }
        }

        public void Restore() 
        {
            Serialization sAgent = new Serialization("serialization_tm" + this.detroitDataSet.LineId.ToString() + "_user.dat");
            TMSavedData restoredData = new TMSavedData();
            try
            {
                restoredData = sAgent.Restore(restoredData);

                if (restoredData.factRegister != null && restoredData.planRegister != null)
                {
                    this.factRegister = restoredData.factRegister;
                    this.planRegister = restoredData.planRegister;
                }

                if (restoredData.monthFactRegister != null && restoredData.monthPlanRegister != null)
                {
                    this.monthFact = restoredData.monthFactRegister;
                    this.monthPlan = restoredData.monthPlanRegister;
                }

                foreach (TimedLineStation station in this.lineStations)
                {
                    string key = station.Name;
                    if(this.factRegister.ContainsKey(key))
                    {
                        station.TaktPosition = this.factRegister[key].TaktPosition;
                    }
                }

                this.taktDuration = this.getTaktDuration() * 60;

                if (PlanFactChangedAllStations != null) {
                    PlanFactChangedAllStations(this, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(), ex.Source, ex.ToString(), "system"); 
            }
        }
         
        public void FreezeTaktDuration(int duration) 
        { 
           this.freezedTaktDuration = duration; 
        }

        public void UnFreezeTaktDuration()
        {
            this.freezedTaktDuration = 0;
        }
        
        public bool IsStationLate(string stationName)
        {
            bool result = true;
            try
            {
                result = (this.GetStationDayFact(stationName) < this.GetStationDayPlan(stationName));
                return result;
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(),
                                    ex.Source.ToString(), ex.Message.ToString(), "system");
                return result;
            }
        }

        public string JsonPlanFact(string stationName, bool wrapped)
        {
            //json block [{"REGP_D":"1", "REGF_D":"2", "REGP_M":"1", "REGF_M":"2"}]
            string result = "";

            int REGP_D = GetStationDayPlan(stationName);
            int REGF_D = GetStationDayFact(stationName);
            int REGP_M = GetStationMonthPlan(stationName);
            int REGF_M = GetStationMonthFact(stationName);

            result = "\"REGP_D\": " + REGP_D + ", \"REGF_D\": " + REGF_D 
                + ", \"REGP_M\": " + REGP_M + ", \"REGF_M\": " + REGF_M;

            if (wrapped) {
                result = "[{" + result + "}]";
            }
            return result;
        }


        //EVENTS_________________________________________________________________________________________________
    
        public event EventHandler OnTick;
        public event EventHandler SlowTask;
        public event EventHandler FastTask;
        public event EventHandler<PlanFactEventArgs> PlanFactChangedOneStation;
        public event EventHandler PlanFactChangedAllStations;


        //PRIVATE_METHODS___________=_____________________________________________________________________________

        private void station_OnFree(object sender, DispatcherStationArgs e)
        {
            try
            {
                //string stationName = e.Station.Name;
                //string key = stationName;

                //Console.WriteLine("TimeManager.station_OnFree, BS=" + e.Station.BitState.ToString());

                //// check is station releases FINISHED product ?
                //if (((BSFlag)e.Station.BitState & BSFlag.Finished) == BSFlag.Finished)
                //{
                //    if (!this.factRegister.ContainsKey(key))
                //    {
                //        this.factRegister.Add(key, new RegisterRecord() { StationName = stationName, SubTotal = 1 });
                //    }
                //    else
                //    {
                //        this.factRegister[key].SubTotal++;
                //    }

                //    if (!this.monthFact.ContainsKey(key))
                //    {
                //        this.monthFact.Add(key, new RegisterRecord() { StationName = stationName, SubTotal = 1 });
                //    }
                //    else
                //    {
                //        this.monthFact[key].SubTotal++;
                //    }

                //    key = e.Station.Name;
                //    (e.Station as TimedLineStation).TaktPosition = this.GetStationDayFact(key) - this.GetStationDayPlan(key);
                //}
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }

        private void slowTask_handler(object sender, EventArgs e)
        {
            if (handlerRoomFree)
            {
                handlerRoomFree = false;
                try
                {
                    if (this.SlowTask != null)
                    {
                        this.SlowTask(this, new EventArgs());
                    }

                    this.schedulerFrameTableAdapter.FillByLineId(this.detroitDataSet.SchedulerFrame, this.detroitDataSet.LineId);
                    string select = "Finish >= '" + DateTime.Now.ToString() + "'";
                    string sortOrder = "Finish";
                    if (this.schedulerFramesCache != null)
                    {
                        this.schedulerFramesCache.Rows = this.detroitDataSet.SchedulerFrame.Select(select, sortOrder);
                    }

                    if(this.currentFrame.Type == 1) 
                        this.correctTaktToSystem();

                    this.logRegisters();
                }
                catch (Exception ex)
                {
                    handlerRoomFree = true;
                    
                    //Console.WriteLine("slowTask_handler: \n" + ex);
                    this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), this.GetType().ToString(), ex.TargetSite.Name,
                        ex.Message + " " + ex.StackTrace, "system");

                }
                handlerRoomFree = true;
            }
        }
  
        private void fastTask_handler(object sender, EventArgs e)
        {
            if (handlerRoomFree)
            {
                handlerRoomFree = false;
                try
                {
                    this.checkFrameType();
                    if (this.FastTask != null)
                        this.FastTask(this, new EventArgs());
                }
                catch (Exception ex)
                {
                    handlerRoomFree = true;
                    this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), this.GetType().ToString(), ex.TargetSite.Name,
                        ex.Message + " " + ex.StackTrace, "system");
                }
                handlerRoomFree = true;
            }
        }
  
        private void zeroTakt_handler(object sender, EventArgs e)
        {
            try
            {
                // calculate new Takt duration
                // restart Takt timer
                this.resetAndRunTaktTimer();
                //this.factTaktCounter++;

                this.idealTaktCounter++;
                this.idealMonthTaktCounter++;

                foreach (KeyValuePair<string, PlanRegisterRecord> record in this.planRegister)
                {
                    record.Value.Amount++;
                    this.myLog.LogAlert(AlertType.System, this.detroitDataSet.LineId.ToString(), this.GetType().ToString(), "zeroTakt_handler()",
                                            record.Value.StationName + ", day-plan = " + record.Value.Amount.ToString(), "system");
                }

                foreach (KeyValuePair<string, PlanRegisterRecord> record in this.monthPlan)
                {
                    record.Value.Amount++;
                    this.myLog.LogAlert(AlertType.System, this.detroitDataSet.LineId.ToString(), this.GetType().ToString(), "zeroTakt_handler()",
                                            record.Value.StationName + ",month-plan = " + record.Value.Amount.ToString(), "system");

                }

                this.updateStationPosition();
                if (PlanFactChangedAllStations != null) { 
                    PlanFactChangedAllStations(this, new EventArgs ());
                }


                // start LateTimers for stations that have not finished yet
                // ...
                this.startLateTimers();

            }
            catch (Exception ex) 
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), this.GetType().ToString(), ex.TargetSite.Name,
                                        ex.Message , "system");            
            }
        }

        private void correctTaktToSystem() 
        {
            try
            {
                ////systemSeconds = 60 - systemSeconds;  // because takt is backword

                int taktSeconds = this.Takt.GetIntValue();
                int seconds = taktSeconds;
                int minutes = Convert.ToInt32(seconds / 60);
                seconds = seconds - minutes * 60;

                int systemSeconds = DateTime.Now.Second;
                systemSeconds = 60 - systemSeconds;  // because takt is backward
                if (systemSeconds == 60) systemSeconds = 0;

                // 43 - 17
                //
                // 20 -> m+17    (3.1)
                // 15 -> m+17    (2)
                
                // 58 - 2
                // 
                // 57 -> m+57+2   (1)
                // 4  -> m+2     (3.2)

                if ((seconds - 50) > systemSeconds)
                {
                    taktSeconds = minutes * 60 + 60 + systemSeconds;
                }
                else if ((systemSeconds -  50) > seconds)
                {
                    taktSeconds = minutes * 60 - 60 + systemSeconds;
                    if(taktSeconds < 0) 
                        taktSeconds = 0;
                }
                else
                {
                    taktSeconds = minutes * 60 + systemSeconds;
                }

                if (taktSeconds < 0)
                {
                    taktSeconds = 0;
                    this.Takt.SetValue(taktSeconds);
                    this.zeroTakt_handler(this, new EventArgs());
                }
                else 
                {
                    this.Takt.SetValue(taktSeconds);
                }
                
            }
            catch (Exception ex) 
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), this.GetType().ToString(), ex.TargetSite.Name,
                    ex.Message, "system");                
            }
        }

        private void resetAndRunTaktTimer()
        {
            try
            {
                this.taktDuration = this.getTaktDuration() * 60;
                if (this.taktDuration > 0) // if (this.taktDuration > 0 && this.productsOnLine.Count > 0) // check if there is something on line
                {
                    this.Takt.Destroy();
                    this.Takt = new Counter(this.taktDirection, this.taktDuration, this.taktTimerStep);
                    this.Takt.Zero += new EventHandler(zeroTakt_handler);
                    this.Takt.Start();

                    //x this.makeSnapShot(); 
                    //myLog.LogAlert(AlertType.System, this.detroitDataSet.LineId.ToString(), this.GetType().ToString(), "Move()", "Line Moving finished.", "system");
                }
                else
                {
                    if (this.Takt.Enabled)
                    {
                        this.Takt.Stop();
                    }
                }
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }
    
        private int getTaktDuration() // in minutes
        {
            int result = 0;

            try
            {
                if (this.freezedTaktDuration == 0)
                {

                    foreach (IStation station in this.lineStations)
                    {
                        if (station.CurrentProduct != null)
                        {
                            (station.CurrentProduct.Router as DbProductRouter).RefreshDataFromDb();
                            int productTakt = station.CurrentProduct.Router.TaktDuration;
                            {
                                result = Math.Max(result, productTakt);  // in minutes
                            }
                        }
                    }
                }
                else
                {
                    result = this.freezedTaktDuration;
                }
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }

            if (result == 0) result = 3600 * 10;

            return result;
        }
    
        private void checkFrameType()
        {

            try
            {
                DataRow curFrameRow = null;
                if (this.schedulerFramesCache.Rows != null)
                {
                    DataRow[] sRows = this.schedulerFramesCache.Rows;
                    for (int i = 0; i < sRows.Count(); i++)
                    {
                        DateTime finish = (DateTime)sRows[i]["Finish"];
                        //this.myLog.LogAlert(AlertType.System, "checkFrameType()", "finish[" + i.ToString() + "] = " + finish.ToString());

                        if (finish.CompareTo(DateTime.Now) >= 0)
                        {
                            curFrameRow = sRows[i];
                            break;
                        }
                    }
                }


                if (curFrameRow != null)
                {
                    // check if FrameType changed ?!
                    // stop or start timers

                    int newFrameType = Convert.ToInt32(curFrameRow["Type"]);
                    string newFrameName = curFrameRow["Name"].ToString();
                    int newFrameId = Convert.ToInt32(curFrameRow["Id"]);
                    //this.myLog.LogAlert(AlertType.System, "checkFrameType()", "curFrameRow[\"Name\"] = " + curFrameRow["Name"].ToString());
                    //this.myLog.LogAlert(AlertType.System, "checkFrameType()", "this.currentFrame.Type = " + this.currentFrame.Type.ToString());
                    //this.myLog.LogAlert(AlertType.System, "checkFrameType()", "this.currentFrame.Name = " + this.currentFrame.Name.ToString());
                    //this.myLog.LogAlert(AlertType.System, "checkFrameType()", "newFrameType = " + newFrameType.ToString());
                    //this.myLog.LogAlert(AlertType.System, "checkFrameType()", "newFrameId = " + newFrameId.ToString());

                    if (this.currentFrame.Type != newFrameType || this.currentFrame.Name != newFrameName)
                    {
                        this.currentFrame.Type = newFrameType;
                        this.currentFrame.Name = curFrameRow["Name"].ToString();
                        this.scheduleFrameTypeChanged();
                    }

                    if (this.currentFrame.Type == 0 && this.Takt.Enabled) 
                    {
                        this.Takt.Pause();
                        this.stopButtonTimers();
                    }
                    if (this.currentFrame.Type == 0)
                    {
                        this.stopButtonTimers();
                    }
                }
                else
                {
                    if (this.Takt.Enabled)
                    {
                        this.Takt.Pause();
                        this.stopButtonTimers();
                    }                    
                }
            }
            catch (Exception ex)
            {
                myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), this.GetType().ToString(), "checkFrameType()",
                    ex.Message, "system");
            }
        }

        private void scheduleFrameTypeChanged()
        {
            // rise event FrameTypeChanged !
            try
            {
                this.myLog.LogAlert(AlertType.Info, this.detroitDataSet.LineId.ToString(), this.GetType().ToString(), "OnScheduleFrameTypeChanged()",
                    String.Format("Time frame changed to {0} (type {1})", currentFrame.Name, currentFrame.Type.ToString()), "system");

                if (this.currentFrame.Type == (int)TFrame.Work)
                {
                    // atcion for working frame
                    string curFrameName = this.currentFrame.Name ?? "";
                    if (curFrameName.Contains(FrameKeys.Reset))
                    {
                        // reset takt timer for all stations
                        this.Off();
                        this.Reset();
                        
                        foreach (TimedLineStation station in this.lineStations)
                            station.ResetTimers();
                        
                        this.updateStationPosition();
                        this.On();
                    }

                    // check if it time to reset month plan/fact counters
                    if (curFrameName.Contains(FrameKeys.ResetMonth)) 
                        this.resetRegisterForNextMonth();

                    // check if it time to reset day plan/fact counters
                    if (curFrameName.Contains(FrameKeys.ResetDay)) 
                        this.resetRegisterForNextDay();                    
 
                    this.Takt.Start();
                    this.startButtonTimers();

                    if (PlanFactChangedAllStations != null) {
                        PlanFactChangedAllStations(this, new EventArgs());
                    }
                } 
                else if (this.currentFrame.Type == (int)TFrame.NonWork)
                {
                    // for non-working frame just stop
                    this.Takt.Pause();
                    this.stopButtonTimers();
                }
                
                // if new day comes
                this.checkDateChanged();



            }
            catch (Exception ex) 
            {
                myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), this.GetType().ToString(), ex.TargetSite.ToString(),
                                    ex.Message, "system");

                myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), this.GetType().ToString(), ex.TargetSite.ToString(),
                    ex.StackTrace.ToString(), "system");       
            }

            //this.makeSnapShot();
        }
        
        private void checkDateChanged()
        {
            bool registersChanged = false;
            try
            {
                DateTime date = new DateTime(this.currentDate.Year, this.currentDate.Month, this.currentDate.Day);


                if ((date.Year * 12 + date.Month) < (DateTime.Today.Year * 12 + DateTime.Today.Month))
                {
                    // new month happens !!!  :))
                    this.myLog.LogAlert(AlertType.Info, this.detroitDataSet.LineId.ToString(), "LineService.TimeManager",
                                                    "checkDateChanged()", "New month happens !!!", "system");
                    this.idealMonthTaktCounter = 1;
                    this.resetRegisterForNextMonth();
                    registersChanged = true;
                }


                if (date.Day < DateTime.Today.Day)
                {
                    // new day happens !! :-)
                    this.idealTaktCounter = 1;
                    this.resetRegisterForNextDay();
                    registersChanged = true;

                //    this.currentDate = DateTime.Today;
                }

                this.currentDate = DateTime.Today;

                if (registersChanged && PlanFactChangedAllStations != null) {
                    PlanFactChangedAllStations(this, new EventArgs());
                }
            
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }
      
        private void startLateTimers()
        {
            try
            {
                foreach (IStation station in this.lineStations)
                {
                    bool stationIsLate = this.IsStationLate(station.Name);
                    if (stationIsLate)
                    {
                        ((TimedLineStation)station).TimeIsOver();
                        string productName = (station.CurrentProduct != null) ? station.CurrentProduct.Name : "NA";
                        this.myLog.LogAlert(AlertType.Info, this.detroitDataSet.LineId.ToString(), this.GetType().ToString(), "startLateTimers()",
                                                "Station " + station.Name + " is being late, CS:" + productName, "system");
                    }
                    //else if (((TimedLineStation)station).IsFinished)
                }
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }
     
        private void stopButtonTimers()
        {
            try
            {
                foreach (IStation station in this.lineStations)
                {
                    ((TimedLineStation)station).Timers.PauseAll();
                }

                //this.pausedLiveTakts.Clear();
                //foreach (KeyValuePair<string, KeyCounter> item in this.liveTakt) 
                //{
                //    item.Value.Pause();
                //    this.pausedLiveTakts.Add(item.Value);
                //}
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }

        }
     
        private void startButtonTimers()
        {
            try
            {
                foreach (IStation station in this.lineStations)
                {
                    ((TimedLineStation)station).Timers.ContinueAll();

                    if (station.CurrentProduct != null) 
                    {
                        ((TimedLineStation)station).Timers.Start(TimerKey.Operating.ToString());
                    }
                }

                //foreach (KeyCounter item in this.pausedLiveTakts)
                //{
                //    item.Start();
                //}
                //this.pausedLiveTakts.Clear();
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }

        private void station_OnFinished(object sender, DispatcherStationArgs e)
        {
            try
            {
                string stationName = e.Station.Name;
                string key = stationName;

                if (!this.factRegister.ContainsKey(key))
                {
                    this.factRegister.Add(key, new RegisterRecord() { StationName = stationName, SubTotal = 1 });
                }
                else
                {
                    this.factRegister[key].SubTotal++;
                }


                if (!this.monthFact.ContainsKey(key))
                {
                    this.monthFact.Add(key, new RegisterRecord() { StationName = stationName, SubTotal = 1 });
                }
                else
                {
                    this.monthFact[key].SubTotal++;
                }

                key = e.Station.Name;
                (e.Station as TimedLineStation).TaktPosition = this.GetStationDayFact(key) - this.GetStationDayPlan(key);

                if (PlanFactChangedOneStation != null) {
                    PlanFactChangedOneStation(this, new PlanFactEventArgs(e.Station));
                }

                //this.checkSumLateEvent(e.Station, "", (e.Station as TimedLineStation).TaktPosition);
                //this.resetLiveLateBit(e.Station);
            }
            catch  (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");                
            }
         
        }
     
        private void station_OnGetNewProduct(object sender, DispatcherStationArgs e)
        {
            try
            {
                if (e.Station != null)
                {
                    string key = e.Station.Name;
                    (e.Station as TimedLineStation).TaktPosition = this.GetStationDayFact(key) - this.GetStationDayPlan(key);
                    (e.Station as TimedLineStation).StopLateTimer();
                    this.checkSumLateEvent(e.Station, "", (e.Station as TimedLineStation).TaktPosition);
                }
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(),
                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }

        private void checkSumLateEvent(IStation station, string stationName, int stationTaktPosition) 
        {
            try
            {
                if (station == null)
                {
                    station = this.lineStations.FirstOrDefault(p => p.Name.Equals(stationName));
                }

                if (station != null)
                {
                    if (stationTaktPosition < 0)
                    {
                        if ((station.BitState & (int)BSFlag.LiveLate) == (int)BSFlag.LiveLate)
                        {
                            station.BitState = station.BitState | (int)BSFlag.SumLate;
                            (station as TimedLineStation).Timers.Start(TimerKey.STOP.ToString());
                            (station as TimedLineStation).Timers.Start(TimerKey.SumLate.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }


        }

        private void updateStationPosition() 
        {
            try
            {
                foreach (TimedLineStation station in this.lineStations)
                {
                    string key = station.Name;
                    station.TaktPosition = this.GetStationDayFact(key) - this.GetStationDayPlan(key);
                    this.checkSumLateEvent(station, "", station.TaktPosition);
                }
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }

        private void resetLiveLateBit(IStation station) 
        {
            try
            {
                if (station != null)
                {
                    int flag = (int)BSFlag.LiveLate;
                    int ret = (station.BitState & ~flag);
                    station.BitState = ret;
                }
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }       
        }
    
        private void resetRegisterForNextDay() 
        {
            try
            {
                this.myLog.LogAlert(AlertType.System, this.detroitDataSet.LineId.ToString(), this.GetType().ToString(), "resetRegisterForNextDay()",
                    "Call resetRegisterForNextDay()", "system"); 

                foreach (KeyValuePair<string, RegisterRecord> record in this.factRegister)
                {
                    record.Value.SubTotal = 0;
                }

                foreach (KeyValuePair<string, PlanRegisterRecord> record in this.planRegister)
                {
                    record.Value.Amount = 0;
                }
            }
            catch (Exception ex) 
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(), 
                    ex.Source.ToString(), ex.Message.ToString(), "system");             
            }
        }

        private void resetRegisterForNextMonth()
        {
            try
            {
                this.myLog.LogAlert(AlertType.System, this.detroitDataSet.LineId.ToString(), this.GetType().ToString(), 
                    "resetRegisterForNextMonth()", "Call resetRegisterForNextMonth()", "system");

                foreach (KeyValuePair<string, RegisterRecord> record in this.monthFact)
                    record.Value.SubTotal = 0;

                foreach (KeyValuePair<string, PlanRegisterRecord> record in this.monthPlan)
                    record.Value.Amount = 0;
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(),
                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }

        private void logRegisters() 
        {
            try
            {
                ////string logMessage = "";
                ////foreach (KeyValuePair<string, RegisterRecord> record in this.factRegister)
                ////{
                ////    logMessage = logMessage + record.Value.StationName + ": " + record.Value.TaktDelta.ToString() + "; " + "\n";
                ////}
                ////this.myLog.LogAlert(AlertType.System, this.detroitDataSet.LineId.ToString(), this.GetType().ToString(), "logRegisters()",
                ////    logMessage, "system");
            }
            catch (Exception ex) 
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");                
            }
        }
 
        private void resetStationsLiveTimer() 
        {
            try
            {
                foreach (IStation station in this.lineStations)
                {
                    (station as TimedLineStation).Timers.Reset(TimerKey.LiveTakt.ToString(), this.taktDuration);
                }
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }

     
    }
}
