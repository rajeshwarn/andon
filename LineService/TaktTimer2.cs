using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace LineService
{
    [Serializable]
    public class TaktTimer2
    {
        private TimerSerializable myTimer = null;
        private DateTime StartTime;
        private int counter = 0;
        private int offset = 0;
        private int mult = 0;
        private bool eventHandlerEnabled = true;

        public event EventHandler Elapsed; // (object sender, EventArgs e) 

        public TaktTimer2(int direction, int start_value, int step)
        {
            this.StartTime = DateTime.Now;
            this.myTimer = new TimerSerializable(step);
            this.myTimer.AutoReset = true;
            this.myTimer.Enabled = true;
            this.myTimer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            this.offset = start_value;
            if (direction == 0)
            {
                this.mult = 1;
            }
            else
            {
                this.mult = -1;
            }
        }

        
        // This is the serialization constructor.
        // Satisfies rule: ImplementSerializationConstructors.
        protected TaktTimer2(SerializationInfo info, StreamingContext context)
        {
            this.myTimer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
        }


        public void TimerDestroy()
        {
            this.myTimer.Dispose();
        }

        public void TimerReset() 
        {
            this.StartTime = new DateTime(0);
        }

        public void TimerStart()
        {
            DateTime tmpDate = new DateTime(0);
            if (this.StartTime == tmpDate) 
            {
                this.StartTime = DateTime.Now;
            }
            this.myTimer.Start();
            this.myTimer.Enabled = true;
            this.myTimer.AutoReset = true;
        }

        public void TimerStop()
        {
            this.myTimer.Enabled = false;
        }

        //public void TimerOnOff()
        //{
        //    this.myTimer.Enabled = !this.myTimer.Enabled;
        //}
              
        private void timer_Elapsed(object sender, EventArgs e)
        {
            long Interval = DateTime.Now.Ticks - StartTime.Ticks;
            this.counter = offset + mult * (int)(Interval * 1E-7);

            if (eventHandlerEnabled)
            {
                this.Elapsed(this, e);
            }
        }

        public int GetIntValue()
        {
            return counter;
        }

        public bool EventHandlerEnabled 
        {
            get { return this.eventHandlerEnabled; }
            set { this.eventHandlerEnabled = value; }
        }

    }



}
