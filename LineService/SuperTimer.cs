using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace LineService
{
    public class SuperCounter : Counter
    {
        DateTime startPoint = new DateTime();
        int startValue = 0;


        public SuperCounter(int direction, int start_value, int step, TimerCounterType type) 
            : base(direction, start_value, step, type) 
        {
           this.startPoint = DateTime.Now;   
           this.myTimer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
        }

        public override void Start()
        {
            base.Start();
            this.startPoint = DateTime.Now;
            this.startValue = this.value;
        }

        protected override void setNextValue()
        {
                long elapsedTicks = DateTime.Now.Ticks - this.startPoint.Ticks;
                TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);

                this.value = startValue + (int)(elapsedSpan.TotalSeconds + 0.5) * mult;

            //Console.WriteLine("value = " + startValue.ToString() + " + " + elapsedSpan.TotalSeconds.ToString() + " x " + mult.ToString());
        }
    }
}
