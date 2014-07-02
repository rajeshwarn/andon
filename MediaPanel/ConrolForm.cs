using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MediaPanel
{
    public partial class ConrolForm : Form
    {
        protected ServiceReference.AssembLineClient myLineClient;
        protected string lineId;
        protected ContentAdressList contentList;

        private const string defVideo = "http://localhost:8080/shark.flv";
        private const string defPicture = "http://localhost:8080/pic.jpg";

        int activeContentType = 0;
        string activeContentUrl = "";

        public ConrolForm()
        {
            InitializeComponent();

            contentList = new ContentAdressList();

            rbModeAndon.Enabled = false;
            rbModePic.Enabled = false;
            rbModeVideo.Enabled = false;

            contentAdressListBindingSource.DataSource = contentList;
            dataGridView1.AllowUserToAddRows = true;
            dataGridView1.AllowUserToDeleteRows = true;
            this.showConnectionStatus(false); 
        }

        protected bool Connect(string LineId)
        {
            bool result = false;
            try
            {
                int basePort = Properties.Settings.Default.BasePort;
                string servicePort = ":" + (basePort + LineId).ToString();
                string serviceName = "/LineService" + LineId.ToString();

                string endpoint_address = Properties.Settings.Default.EndpointAddress + servicePort + serviceName;
                string endpoint_confname = Properties.Settings.Default.EndpointName;

                this.myLineClient = new ServiceReference.AssembLineClient(endpoint_confname, endpoint_address);
                string data = myLineClient.GetCounter().ToString();
                result = (data != "");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Source + " --- " + ex.Message);
            }
            return result;
        }

        private void btDisconnect_Click(object sender, EventArgs e)
        {
            tbLineId.Enabled = true;
            rbModeAndon.Enabled = false;
            rbModePic.Enabled = false;
            rbModeVideo.Enabled = false;
            btPlay.Enabled = false;
            btDisconnect.Enabled = false;
        }

        private void btConnect_Click(object sender, EventArgs e)
        {
            this.contentAdressListBindingSource.ResetBindings(false); 
            string id = tbLineId.Text;
            if (this.Connect(id)) 
            {
                string data = myLineClient.GetCounter().ToString();
                this.showConnectionStatus(true);

                tbLineId.Enabled = false;
                rbModeAndon.Enabled = true;
                rbModePic.Enabled = true;
                rbModeVideo.Enabled = true;
                btPlay.Enabled = true;
                btDisconnect.Enabled = true;
            }

        }

        private void rbModeVideo_CheckedChanged(object sender, EventArgs e)
        {
            string url = tbURL.Text + tbFile.Text;
            this.activeContentUrl = (url == "") ? defVideo : url;
            this.activeContentType = 1;
             
            //myLineClient.SetClientInstruction(1, url);
        }

        private void rbModePic_CheckedChanged(object sender, EventArgs e)
        {
            string url = tbURL.Text + tbFile.Text;
            this.activeContentUrl = (url == "") ? defPicture : url;
            this.activeContentType = 2;
            //myLineClient.SetClientInstruction(2, url);
        }

        private void rbModeAndon_CheckedChanged(object sender, EventArgs e)
        {
            this.activeContentType = 0;
            this.activeContentUrl = "";
            //myLineClient.SetClientInstruction(0, "");
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            ContentAdress currentItem = (ContentAdress)dataGridView1.CurrentRow.DataBoundItem;
            if (currentItem != null) 
            {
                this.tbURL.Text = currentItem.ServerUrl;
                this.tbFile.Text = currentItem.FileName;
                this.activeContentUrl = tbURL.Text + tbFile.Text;
            }
        }

        private void btPlay_Click(object sender, EventArgs e)
        {
            if (this.myLineClient != null && this.myLineClient.State == System.ServiceModel.CommunicationState.Opened)
            {
                myLineClient.SetClientInstruction(this.activeContentType, this.activeContentUrl);
            }
        }

        private void ConrolForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.contentList.SaveSettings();
        }

        private void showConnectionStatus(bool status) 
        {
            if (status == true)
            {
                //MessageBox.Show("Connected succefully! Getting data: " + data);
                laConnectionState.Text = "Connected";
                laConnectionState.ForeColor = Color.Green;
            }
            else if (status == false)
            {
                laConnectionState.Text = "No Connection";
                laConnectionState.ForeColor = Color.LightGray;
            }
        }



    }

    public class ContentAdress
    {
        public string FileName { get; set; }
        public string ServerUrl { get; set; }

        public ContentAdress() 
        { }

        public ContentAdress(string fileName, string serverUrl) 
        {
            this.FileName = fileName;
            this.ServerUrl = serverUrl;
        }
    }

    public class ContentAdressList : List<ContentAdress> 
    {
        private const string defMediaServer = "http://localhost:8080/";
        private const string settingsFileName = "content_links"; 
        private const string settingsFileExt = ".json";

        public ContentAdressList()
        {
            try
            {
                this.ReadSettings();
            }
            catch (Exception ex) 
            {
                Console.WriteLine("Can't open settings file ...");
                Console.WriteLine(ex.Message);
            }
        }

        public ContentAdressList(bool def) 
        {
            this.Add(new ContentAdress() { FileName = "video.avi", ServerUrl = defMediaServer });
            this.Add(new ContentAdress() { FileName = "shark.flv", ServerUrl = defMediaServer });
            this.Add(new ContentAdress() { FileName = "pic.jpg", ServerUrl = defMediaServer });
        }

        public void SaveSettings()
        {
            using (StreamWriter file = File.CreateText(settingsFileName + settingsFileExt))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, this);
            }
        }

        public void ReadSettings()
        {
            this.Clear();
            List<ContentAdress> list = JsonConvert.DeserializeObject<List<ContentAdress>>(
                File.ReadAllText(settingsFileName + settingsFileExt));
            this.AddRange(list);
        }
    }
}
