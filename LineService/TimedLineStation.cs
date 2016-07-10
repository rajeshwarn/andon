using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppLog;
using System.Runtime.Serialization;

namespace LineService
{
    public enum TimerKey { 
        STOP = 1, 
        HELP = 2, 
        PART1 = 3, 
        PART2 = 4, 
        Fail = 5, 
        STOPLAST = 6, 
        Late = 7, 
        Operating = 8, 
        LiveTakt = 9, 
        SumLate = 10 
    }

    public class TimedLineStation : LineStation, IOPCStation
    {
        private int bitState = (int)BSFlag.Free;
        private TimersController timers;
        private int positionOffset = 0;

        public TimersController Timers { get { return this.timers;} }
        public ITimeInformer TimeInformer;



        public TimedLineStation(int id, int type, string name, int bufferSize, int stationIndex, Line line, LogProvider logProvider, ITimeInformer aTimeInformer)
            : base(id, type, name, bufferSize, stationIndex, line, logProvider)
        {
            this.TimeInformer = aTimeInformer;

            this.timers = new TimersController(this.name);
            this.timers.Add(TimerKey.STOP, 0, 0, 1000, TimerCounterType.Sum);
            this.timers.Add(TimerKey.HELP, 0, 0, 1000, TimerCounterType.Sum);
            this.timers.Add(TimerKey.PART1, 0, 0, 1000, TimerCounterType.Laps);
            this.timers.Add(TimerKey.PART2, 0, 0, 1000, TimerCounterType.Laps);
            this.timers.Add(TimerKey.Fail, 0, 0, 1000, TimerCounterType.Laps);
            this.timers.Add(TimerKey.STOPLAST, 0, 0, 1000, TimerCounterType.Laps);
            
            
            Counter lateTaktCounter = this.timers.Add(TimerKey.Late, 0, 0, 1000, TimerCounterType.Laps);
            lateTaktCounter.Elapsed += new EventHandler(lateTaktCounter_Elapsed);


            this.timers.Add(TimerKey.Operating, 0, 0, 1000, TimerCounterType.Laps);
            this.timers.Add(TimerKey.SumLate, 0, 0, 1000, TimerCounterType.Sum);
            
            Counter liveTaktCounter = this.timers.Add(TimerKey.LiveTakt, 600, 1, 1000, TimerCounterType.Laps);
            liveTaktCounter.Zero += new EventHandler(liveTaktCounter_Zero);

            this.OnBeginToLateMore += new EventHandler(TimedLineStation_OnBeginToLateMore);
        }

        public override int PushButton(string button_key)
        {
            try
            {
                int result = base.PushButton(button_key);

                // check if there is a timer for the button with the same name/key (!)
                // start or stop timer depending on the buttons state
                string timer_key = button_key;
                if (this.timers.ContainsKey(timer_key))
                {
                    if (result == 0)
                    {
                        // result = 0 - button is switched off
                        // så ställa timern 
                        this.timers.Stop(timer_key);

                        if (button_key == "STOP")
                        {
                            this.timers.Stop(TimerKey.STOPLAST.ToString());
                        }
                    }
                    else
                    {
                        // result = 1 - button is switched on
                        // så köra timern 

                        if (button_key == "STOP")
                        {
                            this.timers.Start(TimerKey.STOPLAST.ToString());
                        }
                        else
                        {
                            this.timers.Start(timer_key);
                        }
                    }


                }

                if (button_key == "FINISH" & ((Button)(this.StationControls["STOP"])).TState != ButtonState.On)
                {
                    // this call moved to ReleaseProduct() method
                    //this.timers.Stop(TimerKey.Late.ToString());
                }

                return result;
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.line.Id.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
                return 0;
            }

        }
        public override string ReadButtonValue(string button_key)
        {
            try
            {
                string result = base.ReadButtonValue(button_key);
                return result;
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.line.Id.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
                return "";
            }
        }
        public override string ReadButtonValue(int button_index)
        {
            try
            {
                string result = base.ReadButtonValue(button_index);
                return result;
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.line.Id.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
                return "";
            }
        }
        public int GetTimerValue(string timer_key) 
        {
            if (this.timers != null)
            {
                return this.timers.Value(timer_key);
            }
            return 0;
        }
        public int TaktPosition 
        { 
            get { 
                return this.positionOffset; 
            } 
            set { 
                this.positionOffset = value;

                if (this.positionOffset > 0)
                {
                    this.bitState = this.bitState & (int)~BSFlag.Late | (int)BSFlag.Outgo;
                }
                else if (this.positionOffset == 0)
                {
                    this.bitState = this.bitState & (int)~BSFlag.Late & (int)~BSFlag.Outgo;
                }
                else if (this.positionOffset < 0)
                {
                    this.bitState = this.bitState & (int)~BSFlag.Outgo | (int)BSFlag.Late;
                }
            } 
        }

        private LineOPCProvider opcProvider;
        public LineOPCProvider OPCProvider { set { this.opcProvider = value; } }
        public override void ResetControls(ResetControlsType resetType) 
        {
            try
            {
                base.ResetControls(resetType);

                Button aButton;
                TimedLineStation station = this;

                if (resetType == ResetControlsType.All)
                    station.Timers.StopAll();
                else if (resetType == ResetControlsType.Release)
                    station.Timers.StopForRelease();


                aButton = (Button)station.StationControls["FINISH"];
                if (aButton != null && aButton.State == "1")
                {
                    aButton.Reset();
                    this.myLog.LogAlert(AlertType.System, this.line.Id.ToString(), this.GetType().ToString(), "ResetControls()",
                                           "station.Index=" + station.Index.ToString() + ", aButton.Name=" + aButton.Name.ToString(), "system");
                    this.opcProvider.ResetButtonSignal(station.Index, aButton.Name);
                }

                aButton = (Button)station.StationControls["STOP"];
                if (aButton != null && aButton.State == "1")
                {
                    aButton.Reset();
                    this.opcProvider.ResetButtonSignal(station.Index, aButton.Name);
                }

                aButton = (Button)station.StationControls["FAIL"];
                if (aButton != null && aButton.State == "1")
                {
                    aButton.Reset();
                    this.opcProvider.ResetButtonSignal(station.Index, aButton.Name);
                }


                if (resetType == ResetControlsType.All)
                {
                    aButton = (Button)station.StationControls["HELP"];
                    if (aButton != null && aButton.State == "1")
                    {
                        aButton.Reset();
                        this.opcProvider.ResetButtonSignal(station.Index, aButton.Name);
                    }


                    aButton = (Button)station.StationControls["PART1"];
                    if (aButton != null && aButton.State == "1")
                    {
                        aButton.Reset();
                        this.opcProvider.ResetButtonSignal(station.Index, aButton.Name);
                    }

                    aButton = (Button)station.StationControls["PART2"];
                    if (aButton != null && aButton.State == "1")
                    {
                        aButton.Reset();
                        this.opcProvider.ResetButtonSignal(station.Index, aButton.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, "resetButtons()", ex.Message);
            }           
        }
        public void StopLateTimer() 
        {
            try
            {
                this.timers.Stop(TimerKey.Late.ToString());
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.line.Id.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }
        public void TimeIsOver() 
        {
            try
            {
                this.myLog.LogAlert(AlertType.System, this.line.Id.ToString(), this.GetType().ToString(), "TimeIsOver()",
                                    this.Name + " start late timer", "system");
                this.Timers.Start(TimerKey.Late.ToString());
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.line.Id.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }
        public bool IsBeingLate 
        {
            get 
            { 
                string key = TimerKey.Late.ToString();
                bool result = this.timers.Value(key) > 0;
                return result;
            }
        }

        public override int BitState
        {
            get { return this.bitState; } 

            set { this.bitState = value; }
        }
        public override bool AddProduct(Product product) 
        {
            bool result = false;
            try
            {
                result = base.AddProduct(product);
                if (result)
                {
                    // stop counting SUM-STOP if station get new product!

                    this.timers.Stop(TimerKey.STOP.ToString());

                    int taktDuration = (this.line as AssembLine).TaktDuration;

                    //Console.WriteLine(DateTime.Now.ToString() + " " + "taktDuration = " + taktDuration.ToString());


                    int stationTakt = product.Router.TaktDuration * 60;
                    this.timers.Reset(TimerKey.LiveTakt.ToString(), stationTakt);
                    //TODO: should be changed to batchType takt !!!
                                        
                    
                    this.timers.Reset(TimerKey.Operating.ToString(), 0);
                    this.timers.Reset(TimerKey.SumLate.ToString());

                    //Console.WriteLine("AddProduct Event. Station " + this.Name + ". IsWorkingFrame = " + this.TimeInformer.IsWorkingTime().ToString()); 

                    if (this.TimeInformer != null && this.TimeInformer.IsWorkingTime)
                    {
                        this.timers.Start(TimerKey.Operating.ToString());
                        this.timers.Start(TimerKey.LiveTakt.ToString());
                    }
                    this.TaktPosition = this.positionOffset;
                }
                return result;
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.line.Id.ToString(), this.GetType().ToString(), "AddProduct()",
                                    "Station " + this.Name + ", " + ex.Message,
                                    "system");
                return result;
            }       
        }
        public override void ReleaseProduct()
        {
            try
            {
                this.timers.Stop(TimerKey.Operating.ToString());
                this.timers.Stop(TimerKey.LiveTakt.ToString());
                this.timers.Stop(TimerKey.STOP.ToString());
                this.timers.Stop(TimerKey.SumLate.ToString());


                // count SUM-STOP while station is empty!
                this.timers.Start(TimerKey.STOP.ToString());
                
                base.ReleaseProduct();

                //this.myLog.LogAlert(AlertType.Info, this.line.Id.ToString(), this.GetType().ToString(), "ReleaseProduct()",
                //                    "Station " + this.Name + ", operation time(sec) = " 
                //                    + this.timers.Value(TimerKey.Operating.ToString()).ToString(),
                //                    "system");
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.line.Id.ToString(), this.GetType().ToString(), "ReleaseProduct()",
                                    "Station " + this.Name + ", " + ex.Message,
                                    "system");  
            }
        }

        private void liveTaktCounter_Zero(object sender, EventArgs e)
        {
            try
            {
                //Console.WriteLine(DateTime.Now.ToString() + " " + this.Name + ", TaktPosition = " + this.positionOffset.ToString());
                
                this.BitState = this.BitState | (int)BSFlag.LiveLate; // delay more than whole takt (live time = whole takt)

                // is it next takt or current (in current takt TaktPosition == 0)
                if ((this.TaktPosition < 0) | (this.TaktPosition == 0 & (((BSFlag)this.BitState & BSFlag.Finished) == BSFlag.Finished))) 
                {
                    // station is in the next takt
                    
                    this.BitState = this.BitState | (int)BSFlag.SumLate;  // accumulating delay begins
                    this.timers.Start(TimerKey.STOP.ToString());
                    this.timers.Start(TimerKey.SumLate.ToString());

                    this.myLog.LogAlert(AlertType.Info, this.line.Id.ToString(), "TimedLineStation()",
                        "liveCounter_Zero()", "Station " + this.Name + " has exceeded the operating time! CS:" +
                        ((this.CurrentProduct != null) ? this.CurrentProduct.Name : "null")
                        , "system");
                }
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.line.Id.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }
        void lateTaktCounter_Elapsed(object sender, EventArgs e)
        {
            ////int value = (sender as Counter).GetIntValue();
            ////int sumLate = this.timers.Value(TimerKey.SumLate.ToString());

            ////if (value > sumLate) 
            ////{
            ////    this.timers.Reset(TimerKey.SumLate.ToString(), value+1);
            ////    this.OnBeginToLateMore(sender, new EventArgs());
            ////}
        }

        public void ResetTimers() 
        {
            try
            {
                this.Timers.Reset(TimerKey.STOP.ToString());
                this.Timers.Reset(TimerKey.STOPLAST.ToString());
                this.Timers.Reset(TimerKey.SumLate.ToString());
                this.Timers.Reset(TimerKey.Late.ToString());
                this.timers.Reset(TimerKey.Operating.ToString(), 0);

                this.timers.Reset(TimerKey.HELP.ToString());

                int taktDuration = (this.line as AssembLine).TaktDuration;
                this.timers.Reset(TimerKey.LiveTakt.ToString(), taktDuration);

                // count SUM-STOP while station is empty!
                if (this.CurrentProduct == null)
                {
                    this.timers.Start(TimerKey.STOP.ToString());
                }
                else
                {
                    this.timers.Start(TimerKey.Operating.ToString());
                    this.timers.Start(TimerKey.LiveTakt.ToString());
                }
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.line.Id.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }

        public event EventHandler OnBeginToLateMore;

        void TimedLineStation_OnBeginToLateMore(object sender, EventArgs e)
        {
            try
            {
                this.timers.Start(TimerKey.SumLate.ToString());

                int flag = (int)BSFlag.SumLate;
                int ret = (this.BitState | flag);
                this.BitState = ret;

                this.timers.Start(TimerKey.STOP.ToString());

                this.myLog.LogAlert(AlertType.Info, this.line.Id.ToString(), "TimedLineStation()",
                    "OnBeginToLateMore()", "Station " + this.Name + " has begun to be late more and more! CS:" +
                    ((this.CurrentProduct != null) ? this.CurrentProduct.Name : "null")
                    , "system");
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.line.Id.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }

        }

        public void Restore() 
        {
            base.Restore();

            try
            {
                Dictionary<string, ButtonState> controls = new Dictionary<string, ButtonState>();
                foreach (KeyValuePair<string, Button> control in this.StationControls)
                {
                    controls.Add(control.Value.Name, control.Value.TState);
                }
                this.SetControls(controls);
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.line.Id.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }

        }


        public void SetControls(Dictionary<string, ButtonState> controls) 
        {
            try
            {
                //this.myLog.LogAlert(AlertType.System, this.line.Id.ToString(), this.GetType().ToString(), "SetControls()", "Method started ...", "system");

                Button aButton;
                TimedLineStation station = this;

                //this.myLog.LogAlert(AlertType.System, this.line.Id.ToString(), this.GetType().ToString(), "SetControls()", "Method bookmark #2 ...", "system");

                foreach (KeyValuePair<string, ButtonState> item in controls)
                {
                    aButton = (Button)station.StationControls[item.Key];
                    if (aButton != null)
                    {
                        //this.opcProvider.SetButtonSignal(station.Index, aButton.Name, Convert.ToBoolean(item.Value));
                    }
                }
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, "SetControls()", ex.Message);
            }           
        }



    }






}
