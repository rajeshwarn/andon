using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using Newtonsoft.Json;

namespace metra
{
    class Program
    {
        static void Main(string[] args)
        {
            try {
                Console.WriteLine("metra - mqtt cli tracer (c)lenvo.se");
                Console.WriteLine("------------------------------------------");
                Console.WriteLine("Usage: metra <mqtt_host_name> <topic>");
                Console.WriteLine(">");

                string hostName = args[0];
                MqttClient client = new MqttClient(hostName);
                string clientId = Guid.NewGuid().ToString();
                client.Connect(clientId);
                client.Subscribe(new string[] { args[1] }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
                client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
                Console.ReadLine();
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

        static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            Console.WriteLine(Encoding.UTF8.GetString(e.Message));
        }
    }
}
