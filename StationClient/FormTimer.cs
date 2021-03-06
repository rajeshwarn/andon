﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// using System.Timers;
using System.Windows.Forms;

namespace StationClient
{
    delegate void myEventHandler(); 

    class FormTimer
    {

        private Timer timer = null;
        public int Counter = 0;
        public event myEventHandler CounterTick;
                 
        public void TimerOnOff()
        {
            if (this.timer.Enabled)
            {
                this.timer.Stop();
            }
            else 
            {
                this.timer.Start();
            } 

        }

        public void TimerDestroy()
        {
            this.timer.Dispose();
        }
        
        public FormTimer()
        {
            this.timer = new Timer();
            this.timer.Interval = 500;
            this.timer.Tick += new EventHandler(timer_Tick);
            this.timer.Enabled = true;
        }
       
        private void timer_Tick(object sender, EventArgs e)
        {
            this.Counter = Counter + 1;
            this.CounterTick();
        }


    }
}




namespace TestStationApp
{
    class TaktTimer
    {

    }
}
