using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// using System.Timers;
using System.Windows.Forms;

namespace LineManagerApp
{
    delegate void myEventHandler(); 

    class FormTimer
    {


        private Timer timer = null;
        public int Counter = 0;
        public event EventHandler CounterTick;

        public void TimerDestroy()
        {
            this.timer.Dispose();
        }

        public FormTimer()
        {
            this.timer = new Timer();
            this.timer.Interval = 500;
            this.timer.Tick += new EventHandler(timer_Tick);
        }


        public void TimerOnOff()
        {
            this.timer.Enabled = !this.timer.Enabled;
        }

        public void TimerStart()
        {
            this.timer.Start();
        }

        public void TimerStop()
        {
            this.timer.Stop();
        }



        private void timer_Tick(object sender, EventArgs e)
        {
            this.Counter = Counter + 1;
            this.CounterTick(this, null);
        }
    }
}




namespace TestStationApp
{
    class TaktTimer
    {

    }
}
