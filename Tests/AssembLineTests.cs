using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using Newtonsoft.Json;

namespace Tests
{
    internal class mqttMessage 
    {
        public long id;
        public string topic;
        public string message;

        public mqttMessage() 
        {
            id = DateTime.Now.Ticks - new DateTime(2015,1,1).Ticks;
        }
    }    

    [TestFixture]
    public class AssembLineTests
    {
        LineService.AssembLine myLine;
        List<mqttMessage> messageList;

        [Test]
        public void InitTest()
        {
            int lineId = 1;

            myLine = new LineService.AssembLine();
            myLine.Init(lineId);

            Thread.Sleep(10000);

            // TODO: Add your test code here
            Assert.AreEqual(lineId, myLine.Id);
        }

        [Test]
        public void PublisherConnectTest()
        {
            messageList = new List<mqttMessage>();
            Assert.IsTrue(myLine.publisher.IsConnected);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void PublisherStationDataTest(int stationIndex)
        {
            MqttClient client = new MqttClient("mqtt_broker");
            byte[] qoS = new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE };
            
            client.Connect("PublisherStationDataTest_" + DateTime.Now.Ticks.ToString());
            string topic = "andon/line/" + myLine.Id + "/station/" + stationIndex + "/attr";
            client.Subscribe(new string[] { topic }, qoS);
            client.MqttMsgPublishReceived += new MqttClient.MqttMsgPublishEventHandler(client_MqttMsgPublishReceived);

            Thread.Sleep(1000);
            string expected = "";
           
            List<LineService.LineStationBase> list = myLine.GetStations();
            LineService.LineStationBase[] array = list.ToArray<LineService.LineStationBase>();
            LineService.LineStation station = (LineService.LineStation)(array[stationIndex-1]);
            
            //Assert.IsNotNull(station);
            if (station != null) {
                string stName = station.Name;
                string product = (station.CurrentProduct != null ? station.CurrentProduct.Name : "");
                string frame = myLine.ReadFrame().Name;
                expected = "[{\"S\": " + stName + ", \"B\": " + product + ", \"F\": " + frame + "}]";
            }

            mqttMessage msgItem = messageList.FirstOrDefault(p => p.topic.Equals(topic));
            string receivedMsg = msgItem.message;
            Assert.AreEqual(expected, receivedMsg);
            messageList.Remove(msgItem);
        }

        private void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            //if (e.Topic == "andon/line/1/station/1/attr")           
            string receivedMsg = Encoding.UTF8.GetString(e.Message);
            messageList.Add(new mqttMessage(){ topic = e.Topic, message = receivedMsg });
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void PublisherStationButtonsTest(int stationIndex) 
        {
            MqttClient client = new MqttClient("mqtt_broker");
            byte[] qoS = new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE };

            client.Connect("PublisherStationDataTest_" + DateTime.Now.Ticks.ToString());
            string topic = "andon/line/" + myLine.Id + "/station/" + stationIndex + "/button/";
            client.Subscribe(new string[] { topic + "FINISH" }, qoS);
            client.Subscribe(new string[] { topic + "STOP" }, qoS);
            client.Subscribe(new string[] { topic + "HELP" }, qoS);
            client.MqttMsgPublishReceived += new MqttClient.MqttMsgPublishEventHandler(client_MqttMsgPublishReceived);

            Assert.IsTrue(checkStationButton("HELP", stationIndex, topic));
            Assert.IsTrue(checkStationButton("HELP", stationIndex, topic));
            Assert.IsTrue(checkStationButton("HELP", stationIndex, topic));

            //Assert.IsTrue(checkStationButton("FINISH", stationIndex, topic));
            //Assert.IsTrue(checkStationButton("FINISH", stationIndex, topic));

            Assert.IsTrue(checkStationButton("STOP", stationIndex, topic));
            Assert.IsTrue(checkStationButton("STOP", stationIndex, topic));
            Assert.IsTrue(checkStationButton("HELP", stationIndex, topic));
        }

        private bool checkStationButton(string key, int stationIndex, string topic) 
        {
            myLine.PushStationButton(stationIndex, key);
            Thread.Sleep(1000);

            string btnValue = myLine.ReadStationButton(stationIndex, key);

            mqttMessage msgItem = messageList.LastOrDefault(p => p.topic.Equals(topic + key));
            string receivedMsg = msgItem.message;
            messageList.Remove(msgItem);
            return (btnValue == receivedMsg);
        }





    }
}
