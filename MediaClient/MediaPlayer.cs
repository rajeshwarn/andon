using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace MediaClient
{
    public enum MPState { Inactive = 0, PlayingVideo = 1, ShowingPicture = 2 }




    public class MediaPlayer
    {



        private AxAXVLC.AxVLCPlugin2 axVLCP;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Form parentForm;

        private string contentUrl;
        private MPState state = MPState.Inactive;


        public MediaPlayer(Form parentForm)
        {
            this.parentForm = parentForm;
            this.axVLCP = (AxAXVLC.AxVLCPlugin2)(parentForm.Controls["axVLCPlugin"]);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.axVLCP.Parent = (Control)this.parentForm;
            this.pictureBox1.Parent = (Control)this.parentForm;
            ((System.ComponentModel.ISupportInitialize)(this.axVLCP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();

            // 
            // axVLCP
            // 
            this.axVLCP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axVLCP.Enabled = true;
            this.axVLCP.Location = new System.Drawing.Point(0, 27);
            this.axVLCP.Name = "axVLCP";

            this.parentForm.Controls.Add(this.axVLCP);
            this.parentForm.Controls.Add(this.pictureBox1);

            this.axVLCP.Size = new System.Drawing.Size(1192, 618);
            this.axVLCP.TabIndex = 9991;
            this.axVLCP.Visible = false;

            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.WindowText;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(71, 114);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1121, 531);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 9992;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;



            this.pictureBox1.Dock = DockStyle.Fill;
            this.axVLCP.Dock = DockStyle.Fill;

        }

        public void ResetView()
        {
            if (this.state != MPState.Inactive)
            {
                pictureBox1.Image = null;
                pictureBox1.Visible = false;

                axVLCP.playlist.stop();
                axVLCP.Visible = false;


                this.contentUrl = "";
                this.state = MPState.Inactive;
            }

        }

        public void PlayVideo(string url)
        {
            if (this.contentUrl != url)
            {

                string[] options = new string[2] { ":aspect-ratio=4:3", "--rtsp-tcp" };
                axVLCP.playlist.items.clear();
                axVLCP.playlist.add(url, options);
                axVLCP.playlist.playItem(0);

                this.ResetView();

                axVLCP.BringToFront();
                axVLCP.playlist.play();
                axVLCP.Visible = true;
                this.contentUrl = url;
                this.state = MPState.PlayingVideo;
            }
        }

        public void ShowPicture(string url)
        {
            if (this.contentUrl != url)
            {
                this.ResetView();

                pictureBox1.BringToFront();
                pictureBox1.Dock = DockStyle.Fill;
                pictureBox1.Load(url);
                pictureBox1.Show();
                pictureBox1.Visible = true;
                this.contentUrl = url;
                this.state = MPState.ShowingPicture;
            }
        }
    }
}
