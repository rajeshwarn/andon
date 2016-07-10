using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WMPLauncher;

namespace MediaClient
{




    public partial class Form1 : Form
    {
        MediaPlayer mplayer;

        public Form1()
        {
            InitializeComponent();
            this.mplayer = new MediaPlayer(this);

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.mplayer.PlayVideo("http://localhost:8080/video.avi");
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            this.mplayer.PlayVideo("http://localhost:8080/shark.flv");
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            this.mplayer.ResetView();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            this.mplayer.ShowPicture("http://localhost:8080/pic.jpg");

        }





    }
}
