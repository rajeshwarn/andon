using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LineService;

namespace LineManagerApp
{
    public partial class TestForm : Form
    {
        private TaktTimer2 Counter;
        private System.Timers.Timer Counter2;
        private TaktTimer2 Sync;
        private int apples;

        public TestForm()
        {
            InitializeComponent();
        }

        private void TestForm_Load(object sender, EventArgs e)
        {
            this.Counter = new TaktTimer2(1, 0, 1000);

            this.Counter2 = new System.Timers.Timer(1000);
            this.Counter2.BeginInit();
            this.Counter2.Elapsed += new System.Timers.ElapsedEventHandler(Counter2_Elapsed);

                        
            this.Sync = new TaktTimer2(1, 0, 2000);
            this.Sync.Elapsed += new EventHandler(Sync_Elapsed);
            this.Sync.TimerStart();



        }

        private void Counter2_Elapsed(object sender, EventArgs e) 
        {
            this.apples++;
        }


        private void Sync_Elapsed(object sender, EventArgs e) 
        {
            //this.laValue.Text = this.Counter.GetIntValue().ToString();
            this.laValue.Text = this.apples.ToString();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            this.Counter.TimerStart();
            this.Counter2.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Counter.TimerStop();
            this.Counter2.Stop();
        }
    }
}
