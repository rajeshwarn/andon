using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Runtime.Serialization;
using System.Security.Permissions;

using Newtonsoft.Json;

namespace LineService
{
    [Serializable]
    class TimerSerializable : Timer, ISerializable
    {
        public TimerSerializable(int step) :base(step) 
        { 
        }

        // This is the serialization constructor.
        // Satisfies rule: ImplementSerializationConstructors.
        protected TimerSerializable(SerializationInfo info, StreamingContext context)
        {
            base.AutoReset = (bool)info.GetValue("AutoReset", typeof(bool));
            base.Interval = (int)info.GetValue("Interval", typeof(int));
            base.Enabled = (bool)info.GetValue("Enabled", typeof(bool));
        }

        // The following method serializes the instance.
        [SecurityPermission(SecurityAction.LinkDemand, Flags=SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("AutoReset", base.AutoReset);
            info.AddValue("Interval", base.Interval);
            info.AddValue("Enabled", base.Enabled);
        }
    };


    public class Counter
    {
        protected Timer myTimer = null;
        protected int value = 0;
        protected int offset = 0;
        protected int mult = 0;
        protected TimerCounterType type;

        public event EventHandler Elapsed; // (object sender, EventArgs e) 
        public event EventHandler Zero;

        public Counter(int direction, int start_value, int step) 
            : this(direction, start_value, step, TimerCounterType.Sum)
        {
        }

        public Counter(int direction, int start_value, int step, TimerCounterType type) 
        {
            //this.myTimer = new TimerSerializable(step);
            this.myTimer = new Timer(step);
            this.myTimer.AutoReset = true;
            this.myTimer.Enabled = false;
            this.myTimer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            this.offset = start_value;
            this.type = type;
            if (direction == 0)
            {
                this.mult = 1;
            }
            else
            {
                this.mult = -1;
            }
            this.Reset();
        }

        // This is the serialization constructor.
        // Satisfies rule: ImplementSerializationConstructors.
        protected Counter(
           SerializationInfo info, 
           StreamingContext context)
        {
            this.myTimer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
        }


        public void Destroy()
        {
            this.myTimer.Dispose();
        }

        public virtual void Reset()
        {
            this.value = 0 + offset;
        }

        public void Reset(int offset) 
        {
            this.offset = offset;
            this.Reset();
        }

        public virtual void Start()
        {
            //this.myTimer.Start();
            this.myTimer.Enabled = true;
        }

        public int Stop()
        {
            this.myTimer.Stop();
            if (this.type == TimerCounterType.Laps) { this.Reset(); }
            return this.value;
        }

        public int Pause() 
        {
            this.myTimer.Stop();
            return this.value;
        }

        protected virtual void timer_Elapsed(object sender, EventArgs e)
        {
            setNextValue();

            if(this.Elapsed != null)
                this.Elapsed(this, e);
            if (this.value == 0) 
            {
                if( this.Zero != null)
                    this.Zero(this, new EventArgs());
            }
        }

        protected virtual void setNextValue() 
        {
            this.value = this.value + mult * 1;        
        }

        public int GetIntValue()
        {
            return this.value;
        }

        public TimerCounterType Type { get { return this.type;} }

        public bool Enabled { get { return this.myTimer.Enabled; } }

        public void SetValue(int newValue) 
        {
            this.value = newValue;
        }

        [JsonProperty]
        public int Value { get { return value;} }
    }
}
