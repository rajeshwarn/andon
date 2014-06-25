using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using AppLog;
using System.Collections;

using System.Security.Permissions;
using System.Timers;

namespace LineService
{
    public enum BSFlag { Finished = 0x0001, Late = 0x0100, Blocked = 0x0010, Free = 0x0200, LiveLate = 0x0400, SumLate = 0x0800, Outgo = 0x0020 }
    public enum ResetControlsType { All = 0, Release = 1}
    public enum ButtonState { Off = 0, On = 1 }
    public enum TimerCounterType { Laps = 0, Sum = 1 }
    
    public class TimerSnapshort 
    {
        public string Key;
        public int Counter;
        public bool Enabled;
    }
    
    public class StationBuffer : Queue<Product> 
    {
        private LineStation station;
        private int size;

        public StationBuffer() : this(null, 0) { }

        public StationBuffer(LineStation owner, int bufferSize) 
        {
            this.station = owner;
            this.size = bufferSize;
        }

        public new bool Enqueue(Product item) 
        {
            // check if the queue is full
            bool result = false;
            if (this.Count < this.size) 
            {
                base.Enqueue(item);
                result = true;
            }
            return result;
        } 
    }

    [Serializable]
    public class SCtrlSavedData
    {
        public SerializableDictionary<string, ButtonState> Controls;
    }

    public class LineStation : LineStationBase, IStation
    {
        private const int FINISH_TIMEOUT = 1000;
        //private const int FINISH_FREEZE_TIMEOUT = 1 * 5 * 1000 ;

        private const int FINISH_FREEZE_TIMEOUT = 15 * 60 * 1000 ;

        private int type; // 0 - assistant station, 1 - saldom station, 2 - main station with assistant
        private int bufferSize;
        private int index;
        

        public Dictionary<string, Button> StationControls;
        private Product currentProduct;
        private StationBuffer stationBuffer;
        protected LogProvider myLog;
        protected Line line;

        private bool isOnHold = false;
        private int holdingEvents = 0;
        private bool finishPublished = false;

        public List<object> StationControlsList = new List<object>();

        public int Type { get { return this.type; } }
        public Product CurrentProduct { get { return this.currentProduct; } }


        public int BufferSize { get { return this.bufferSize; } }
        public StationBuffer Buffer { get { return this.stationBuffer; } }
        public int Index { get { return this.index; } }
        public virtual int BitState { get; set; }

        // This is the serialization constructor.
        // Satisfies rule: ImplementSerializationConstructors.
        //protected LineStation(SerializationInfo info, StreamingContext context)
        //{
        //    ////    this.id = (int)info.GetValue("id", typeof(int));
        //    ////    this.type = (int)info.GetValue("type", typeof(int));
        //    ////    this.name = (string)info.GetValue("name", typeof(string));
        //    ////    this.bufferSize = (int)info.GetValue("bufferSize", typeof(int));
        //    ////    this.index = (int)info.GetValue("index", typeof(int));
        //}
        //// The following method serializes the instance.
        //[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        //void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        //{
        //    ////    info.AddValue("id", this.id);
        //    ////    info.AddValue("type", this.type);
        //    ////    info.AddValue("name", this.name);
        //    ////    info.AddValue("bufferSize", this.bufferSize);
        //    ////    info.AddValue("index", this.index);
        //}


        public LineStation(int id, int type, string name, int bufferSize, int stationIndex, Line line, LogProvider logProvider)
            : base()
        {
            this.id = id;
            this.type = type;
            this.name = name;
            this.bufferSize = bufferSize;
            this.index = stationIndex;

            this.StationControls = new Dictionary<string, Button>();
            this.stationBuffer = new StationBuffer(this, bufferSize);
            this.myLog = logProvider;
            this.line = line;

            this.onFinished +=new EventHandler<DispatcherStationArgs>(LineStation_onFinished);
        }
        

        public void SetId(int id)
        {
            this.id = id;
        }
        
        public int GetId(int id)
        {
            return this.id;
        }

        public int PushButton(int button_index)
        {
            throw new Exception("Old method version execution. Class: " + this.GetType().ToString() + ", method: PushButton(int button_index)");
            return 0;
        }

        public virtual int PushButton(string button_key)
        {
            int result = 0;
            Button aButton = (Button)this.StationControls[button_key];
            if (aButton != null)
            {
                if (aButton.State == "1" && isStickyButton(button_key)) 
                {
                    result = 1;    
                } 
                else 
                {
                    result = aButton.Push();
                    this.checkButtonsLogic(button_key, result);
                }

                //myLog.LogAlert(AlertType.Error, this.name, this.GetType().ToString(), "makeSnapShot()",
                //   ex.Message, "system");
            }
            return result;
        }

        private bool isStickyButton(string button_key) 
        {
            bool result = false;
            if (button_key == "FINISH") 
            {
                result = true;
            }
            return result;
        }

        public virtual string ReadButton(string button_key)
        {
            string result = "0";
            Button aButton = (Button)this.StationControls[button_key];
            if(aButton != null) 
            {
                result = aButton.State;
            }
            return result;
        }

        public virtual string ReadButton(int button_index)
        {
            string result = "0";
            if (button_index <= this.StationControlsList.Count) 
            {
                Button aButton = (Button)this.StationControlsList[button_index - 1];
                if (aButton != null)
                {
                    result = aButton.State;
                }
            }
            return result;
        }
        
        public void AddControl(int id, string name, string varName, string varSignalName, string keyString, string ipAddr, int channel, int moduleType, string partAddress) 
        {
            Button myButton = new Button(id, name, varName, varSignalName, keyString);
            myButton.IPAddr = ipAddr;
            myButton.Channel = channel;
            myButton.ModuleType = moduleType;
            myButton.PartsAddress = partAddress;
            this.StationControls.Add(name, myButton);
            this.StationControlsList.Add(myButton);
        }

        public bool IsOnHold
        {
            get 
            {
                if (this.holdingEvents > 0) 
                    this.isOnHold = true;
                else
                    this.isOnHold = false;
                return this.isOnHold; 
            }
        }

        public bool IsBusy 
        {
            get
            {
                bool result = true;
                if (this.currentProduct != null | (this.bufferSize > 0 && this.stationBuffer.Count > 0))
                {
                    result = false;        
                }
                return result;
            }
        }

        public bool IsStillWorking
        {
            get
            {
                bool result = false;
                if (this.currentProduct != null && !this.IsFinished)
                {
                    result = true;
                }
                return result;
            }
        }

        protected bool isFinished
        {
            get
            {
                bool result = false;
                Button finishButton = (Button)this.StationControls["FINISH"];

                if (finishButton != null && finishButton.TState == ButtonState.On)
                {
                    result = true;
                }
                return result;
            }
        }

        public bool IsFinished { get { return (this.isFinished & this.finishPublished); } }

        public void Finish() 
        {
            this.PushButton("FINISH");
        }

        private void checkButtonsLogic(string buttonKey, int buttonNewState) 
        { 
            if(buttonKey == "STOP")
            { 
                if (buttonNewState == 1) 
                {
                    this.holdingEvents++;
                    ((Button)this.StationControls["FINISH"]).IsBlocked = true;
                } 
                else 
                {
                    this.holdingEvents--;
                    if (this.holdingEvents == 0)
                    {
                        ((Button)this.StationControls["FINISH"]).IsBlocked = false;
                    }
                        if (holdingEvents < 0) { 
                        throw new Exception("Empty stack of HoldingEvents, class: " + this.GetType().ToString()); 
                    }
                }
            }
            else if (buttonKey == "FINISH") 
            {
                try
                {
                    if (this.isFinished)
                    {
                        if (this.CurrentProduct != null)
                        {
                            this.CurrentProduct.OperationFinished = true;
                        }
                        else
                        {
                            ((Button)this.StationControls[buttonKey]).Reset();
                        }
                    }

                    if (this.isFinished)
                    {
                        if (this.onFinished != null)
                        {
                            this.onFinished(this, new DispatcherStationArgs() { Product = this.CurrentProduct, Station = this });
                        }
                    }
                }
                catch (Exception ex) 
                {
                    this.myLog.LogAlert(AlertType.Error, this.line.Id.ToString(), this.GetType().ToString(), "checkButtonsLogic()",
                        "Station " + this.Name + ", " + ex.Message
                        , "system");               
                }
            }
        }

        public void FreezeButton(string button_key)
        {
            try
            {
                Button button = (Button)(this.StationControls[button_key]);
                if (button != null)
                {
                    if (button_key == "FINISH")
                    {
                        this.holdingEvents++;
                    }
                    button.IsBlocked = true;

                    Timer timer = new Timer();
                    timer.Interval = FINISH_FREEZE_TIMEOUT;
                    timer.Elapsed += new ElapsedEventHandler((_s, _e) =>
                    {
                        timer.Stop();
                        timer.Dispose();

                        if (button_key == "FINISH")
                        {
                            this.holdingEvents--;
                            if (this.holdingEvents == 0)
                            {
                                button.IsBlocked = false;
                            }
                        }
                        else
                        {
                            button.IsBlocked = false;
                        }
                    });

                    timer.Start();

                }
            }
            catch (Exception ex) 
            {
                this.myLog.LogAlert(AlertType.Error, this.line.Id.ToString(), this.GetType().ToString(), "FreezeButton()",
                                    "Station " + this.Name + ", " + ex.Message,
                                    "system");               
            }

        }


        protected event EventHandler<DispatcherStationArgs> onFinished;




        private void LineStation_onFinished(object sender, DispatcherStationArgs e) 
        {
            Timer timer = new Timer();
            timer.Interval = FINISH_TIMEOUT;
            timer.Elapsed += new ElapsedEventHandler((_s, _e) =>
            {
                timer.Stop();
                timer.Dispose();

                this.BitState = this.BitState | (int)BSFlag.Finished;

                this.finishPublished = true;
                if (this.OnFinished != null)
                    this.OnFinished(this, new DispatcherStationArgs() { Product = e.Product, Station = e.Station });

                //if (this.OnFinished != null)
                //    EventQueue<object>.Add(this.OnFinished, this, new DispatcherStationArgs() { Product = e.Product, Station = e.Station });


            });
            timer.Start();
        }

        public void ResetFinish() 
        {
            string buttonKey = "FINISH";
            ((Button)this.StationControls[buttonKey]).Reset();
        }

        public virtual void ResetControls(ResetControlsType resetType) 
        {
            this.finishPublished = false;
        }


        /// <summary>
        /// IStation interface realisation
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public virtual bool AddProduct(Product product)
        {
            // product is going to occupy the next station
            // check buffer etc.
            bool result = false;

            // check if station is free
            if (this.currentProduct == null)
            {
                // set product on station 
                product.OperationFinished = false;
                this.currentProduct = product;
                this.currentProductName = product.Name;
                this.BitState = this.BitState & (int)~BSFlag.Free;

                if (this.OnGetNewProduct != null) 
                    this.OnGetNewProduct(this, new DispatcherStationArgs() { Station = this, Product = this.currentProduct });
                
                result = true;
                //this.myLog.LogAlert(AlertType.Info, this.line.Id.ToString(), this.GetType().ToString(), "AddProduct()",
                //    String.Format("Chassis {0} is set directly on station {1}.", product.Name, this.Name)
                //    , "system"
                //);

                this.myLog.LogAlert(AlertType.Info, this.line.Id.ToString(), this.GetType().ToString(), "AddProduct()",
                    String.Format("Station {1} set up new CS:{0}.", product.Name, this.Name)
                    , "system"
                );
            }
            else if (this.currentProduct.Name == product.Name) 
            {
                result = true;
                this.myLog.LogAlert(AlertType.Info, this.line.Id.ToString(), this.GetType().ToString(), "AddProduct()",
                    String.Format("Chassis {0} is ALREADY set on station {1}.", product.Name, this.Name)
                    , "system"
                );                
            }
            // try to set product into station buffer
            else if (this.stationBuffer.Enqueue(product))
            {
                product.OperationFinished = false;
                result = true;
                this.myLog.LogAlert(AlertType.System, this.line.Name, this.GetType().ToString(), "AddProduct()",
                    String.Format("Chassis {0} is set in buffer on station {1}.", product.Name, this.Name) , "system");
            }
            else
            {
                // buffer is full !!!
                this.myLog.LogAlert(AlertType.Error, this.GetType().ToString(),
                    String.Format("Product {0} couldn't be set in buffer  on station {1}. Station buffer (size={3}) is full!",
                        product.Name, this.Name, this.bufferSize.ToString()
                    )
                );
            }

            return result;
        }
        public virtual void ReleaseProduct()
        {
            try
            {
                this.myLog.LogAlert(AlertType.Info, this.line.Id.ToString(), this.GetType().ToString(), "ReleaseProduct()",
                    String.Format("Product {0} is trying to release from station {1}", currentProduct.Name, this.Name)
                    , "system"
                );


                // release current station, check station buffer for waiting products
                this.currentProduct = null;
                this.currentProductName = "";
                if (this.bufferSize > 0 && this.stationBuffer.Count > 0)
                {
                    // get product from buffer and set it to the station
                    // station still occupied by product from station_buffer
                    this.currentProduct = this.stationBuffer.Dequeue();
                    this.currentProductName = this.currentProduct.Name;
                }

                // clear "Blocked" bit before OnFree event
                this.BitState = this.BitState & ~(int)BSFlag.Blocked;                
                
                this.ResetControls(ResetControlsType.Release);

                this.myLog.LogAlert(AlertType.System, this.line.Id.ToString(), this.GetType().ToString(), "ReleaseProduct()",
                    String.Format("Station {0} BitState = {1}", this.Name, this.BitState.ToString()), "system");
                
                // rise event "Station is free"
                if (this.OnFree != null)
                    this.OnFree(this, new DispatcherStationArgs() { Product = null, Station = this });

                if (this.currentProduct == null)
                {
                    this.BitState = (int)BSFlag.Free;
                    this.myLog.LogAlert(AlertType.Info, this.line.Id.ToString(), this.GetType().ToString(), "ReleaseProduct()",
                                    "Station " + this.Name + " is hungry!", "system");
                }

                this.BitState = this.BitState & (int)~BSFlag.Finished;
                this.BitState = this.BitState & (int)~BSFlag.LiveLate;
                this.BitState = this.BitState & (int)~BSFlag.SumLate;

                //if (this.OnFree != null)
                //    EventQueue<object>.Add(this.OnFree, this, new DispatcherStationArgs() { Product = null, Station = this });

            }
            catch (Exception ex) 
            {
                this.myLog.LogAlert(AlertType.Error, this.line.Id.ToString(), this.GetType().ToString(), "ReleaseProduct()",
                                    "Station " + this.Name + ", " + ex.Message,
                                    "system");             
            }

        
        }

        public void RollbackProductByName(string productName) 
        {
            this.myLog.LogAlert(AlertType.Info, this.line.Id.ToString(), this.GetType().ToString(), "ReleaseProduct()",
                String.Format("Product {0} is trying to rollback from station {1}", productName, this.Name)
                , "system"
            );            

            if(this.CurrentProduct != null) 
            {
                if (productName == this.CurrentProductName) 
                {
                    this.ReleaseProduct();  
                }
                if (this.bufferSize > 0 && this.stationBuffer.Count > 0)
                {
                    for(int i=0; i<this.stationBuffer.Count(); i++) 
                    {
                        Product tmp = this.stationBuffer.Dequeue();
                        if (tmp.Name != productName) 
                        {
                            this.stationBuffer.Enqueue(tmp);
                        }
                    }
                }

            }

        }




        public bool IsFull
        {
            get
            {
                bool result = false;

                if (this.bufferSize > 0 && this.stationBuffer.Count == this.bufferSize)
                    result = true;

                if (this.bufferSize == 0 && this.currentProduct != null)
                    result = true;

                return result;
            }
        }
        public ProductState ProductPosition(Product product) 
        {
            if (this.stationBuffer != null && this.stationBuffer.Contains(product))
            {
                return ProductState.InStationBuff;
            }

            if (this.currentProduct != null && this.currentProduct.Name == product.Name)
            {
                return ProductState.InStation;
            }
            else
            {
                this.myLog.LogAlert(AlertType.Error, this.line.Id.ToString(), this.GetType().ToString(), "ProductPosition()", 
                    String.Format("Position {0} on station {1} is undefined!", product.Name, this.Name), 
                    "system"); 
                return ProductState.Undefined;
            }
        }
        public event EventHandler<DispatcherStationArgs> OnFinished;
        public event EventHandler<DispatcherStationArgs> OnFree;
        public event EventHandler<DispatcherStationArgs> OnGetNewProduct;


        public void Backup() 
        {
            SCtrlSavedData dataToSave = new SCtrlSavedData();
            dataToSave.Controls = new SerializableDictionary<string, ButtonState>();

            try
            {
                foreach (KeyValuePair<string, Button> item in this.StationControls)
                {
                    dataToSave.Controls.Add(item.Key, (item.Value as Button).TState);
                }

                Serialization sAgent = new Serialization("serialization_sctrl" + this.id + ".dat");
                sAgent.Backup(dataToSave);
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.line.Id.ToString(), this.GetType().ToString(), "Backup()", ex.ToString(), "system");  
                Console.WriteLine(DateTime.Now + " " + ex.TargetSite.ToString(), ex.Source, ex.ToString());
            }            
        }

        public void Restore()
        {
            Serialization sAgent = new Serialization("serialization_sctrl" + this.id + ".dat");
            SCtrlSavedData restoredData = new SCtrlSavedData();

            try
            {
                restoredData = sAgent.Restore(restoredData);

                if (restoredData.Controls != null)
                {
                    foreach (KeyValuePair<string, ButtonState> restoredItem in restoredData.Controls)
                    {
                        if (this.StationControls.ContainsKey(restoredItem.Key))
                        {
                            if (restoredItem.Value == ButtonState.On)
                            {
                                (this.StationControls[restoredItem.Key] as Button).Push();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.line.Id.ToString(), this.GetType().ToString(), "Restore()", ex.ToString(), "system");
                Console.WriteLine(DateTime.Now + " " + ex.TargetSite.ToString(), ex.Source, ex.ToString());
            }        
        }

    }

 }
