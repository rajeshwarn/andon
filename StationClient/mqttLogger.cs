using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using Newtonsoft.Json;

namespace StationClient
{
    public class MqttLogger : IDisposable
    {
        private string hostName = "localhost";
        private MqttClient client;
        private string clientId;
        private string senderId;

        public MqttLogger(string hostName, string senderId) 
        {
            try {
                client = new MqttClient(hostName);
                clientId = Guid.NewGuid().ToString();
                if (senderId == "")
                    senderId = Guid.NewGuid().ToString();
                client.Connect(clientId);
                this.senderId = senderId;
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

        public void Log(string msg) {
            byte[] messageComp = Encoding.UTF8.GetBytes(msg);
            string topic = "andon/mqttlog/" + senderId;
            client.Publish(topic, messageComp, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }

        public void Dispose() {
            if (client != null && client.IsConnected)
                client.Disconnect();   
        }
    }
}
