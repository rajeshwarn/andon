﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using Newtonsoft.Json;

using AppLog;
using System.Drawing;

namespace LineService
{
    ///----------------------------------------------------------------------------
    ///
    ///   Service class w communication functions !
    ///   * establish mgtt connection
    ///   * publish line data via mqtt service
    ///   
    public class Publisher : Object
    {
        private AssembLine line;
        private MqttClient mqttClient;
        private MemLog myLog;
        private string mqttBroker;
        private string clientId;

        public bool IsConnected 
        {
            get { return mqttClient.IsConnected; }
        }

        internal Publisher(AssembLine owner_line)
        {
            line = owner_line;
            myLog = new MemLog(LogType.SQL, Properties.Settings.Default.DetroitConnectionString.ToString(), false);

            initMqtt();

            line.timeManager.PlanFactChangedOneStation += new EventHandler<PlanFactEventArgs>(timeManager_PlanFactChangedOneStation);
            line.timeManager.PlanFactChangedAllStations += new EventHandler(timeManager_PlanFactChangedAllStations);
            line.timeManager.SlowTask += new EventHandler(timeManager_SlowTask);
            line.timeManager.FastTask += new EventHandler(timeManager_FastTask);


            foreach (TimedLineStation station in line.lineStations) {
                station.OnBitStateChanged += new EventHandler(station_OnBitStateChanged);

                foreach (ControlItem control in station.StationControlsList) {
                    control.OnChanged += new EventHandler(stationControl_OnChanged);
                }

                station_OnBitStateChanged(station, new EventArgs());
            }
        }

        private void initMqtt()
        {
            try {
                mqttBroker = "mqtt_broker";
                mqttClient = new MqttClient(mqttBroker);
                clientId = DateTime.Now.Ticks.ToString() + "-line-" + line.Id.ToString();
                mqttClient.Connect(clientId);
            }
            catch (Exception ex) { 
                myLog.LogAlert(AlertType.Error, line.Id.ToString(), ex.TargetSite.ToString(),
                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }

        private void timeManager_PlanFactChangedOneStation(object sender, PlanFactEventArgs e)
        {
            LineStation station = ((LineStation)e.Station);
            /// publish GAPx.x data to mqttBroker
            string topic = "andon/line/" + line.Id.ToString() + "/station/" + station.Index + "/planfact";
            byte[] messageStr;

            try {
                string json = line.timeManager.JsonPlanFact(station.Name, true);
                messageStr = Encoding.UTF8.GetBytes(json);
                mqttClient.Publish(topic, messageStr, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
            }
            catch (Exception ex) {
                myLog.LogAlert(AlertType.Error, line.Id.ToString(), ex.TargetSite.ToString(),
                        ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }

        private void timeManager_PlanFactChangedAllStations(object sender, EventArgs e)
        {
            /// publish GAPx.x data to mqttBroker
            foreach (LineStation station in line.lineStations) {
                timeManager_PlanFactChangedOneStation(sender, new PlanFactEventArgs(station));
            }
        }

        private void publishStationEvents()
        {
            string topic = "andon/line/" + line.Id.ToString();
            byte[] messageStr;

            try {
                /// publish timers in fast task, every 1s
                /// json block [{"STOP": "1", "HELP": "2", "PART1": "3", "PART2": "4", "Fail": "5", 
                ///              "STOPLAST": "6", "Late": "2242", "Operating": "7", "SumLate": "8", "LiveTakt": "3300", "T": "3600"}]
                foreach (TimedLineStation station in line.lineStations) {
                    string json = station.Timers.JsonData(false); // get all timers from station "as-is"
                    json += "\"T\": " + "\"" + line.GetCounter() + "\"";             // add extra counter form line
                    json = "{" + json + "}";

                    topic = "andon/line/" + line.Id + "/station/" + station.Index + "/timers";
                    messageStr = Encoding.UTF8.GetBytes(json);
                    mqttClient.Publish(topic, messageStr, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
                }
            }
            catch (Exception ex) {
                myLog.LogAlert(AlertType.Error, line.Id.ToString(), ex.TargetSite.ToString(),
                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }

        private void publishStationAttributes()
        {
            try {  //json block [{"S": "station name", "B": "product", "F": "current frame"}]

                foreach (TimedLineStation station in line.lineStations) {

                    string json = station.JsonAttributes(false);
                    json += "\"" + "F" + "\": " + "\"" + line.ReadFrame().Name + "\"";
                    json += ", \"" + "FT" + "\": " + "\"" + line.ReadFrame().Type + "\"";
                    json = "{" + json + "}";

                    string topic = "andon/line/" + line.Id + "/station/" + station.Index + "/attr";
                    byte[] messageStr = Encoding.UTF8.GetBytes(json);
                    mqttClient.Publish(topic, messageStr, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true); // retain
                }
            }
            catch (Exception ex) {
                myLog.LogAlert(AlertType.Error, line.Id.ToString(), ex.TargetSite.ToString(),
                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }

        private void station_OnBitStateChanged(object station, EventArgs e)
        {

            try {
                if (station == null) {
                    myLog.LogAlert(AlertType.Error, line.Id.ToString(), "LineService", "station_OnBitStateChanged()",
                        "'station' object is null", "system");
                }
                int bst = ((TimedLineStation)station).BitState;

                // publish bitState, retained
                //
                string msg = bst.ToString();
                string topic = "andon/line/" + line.Id + "/station/" + ((TimedLineStation)station).Index + "/bst";
                byte[] messageStr = Encoding.UTF8.GetBytes(msg);
                mqttClient.Publish(topic, messageStr, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true); 

                // publish screen color, retained
                //
                Color screenColor = applyColorSchema(bst);
                topic = "andon/line/" + line.Id + "/station/" + ((TimedLineStation)station).Index + "/color";
                messageStr = Encoding.UTF8.GetBytes(screenColor.Name.ToString());
                mqttClient.Publish(topic, messageStr, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true); 
            }
            catch (Exception ex) {
                myLog.LogAlert(AlertType.Error, line.Id.ToString(), ex.Source.ToString(),
                    ex.TargetSite.ToString(), ex.Message.ToString(), "system");
            }
        }

        private void stationControl_OnChanged(object control, EventArgs e)
        {
            /// publich control state
            if (control is ControlItem) {

                LineStation station = (LineStation)(((ControlItem)control).Owner);
                try {
                    string msg = ((ControlItem)control).State;
                    string topic = "andon/line/" + line.Id + "/station/" + station.Index + "/button/" + ((ControlItem)control).Name;
                    byte[] messageStr = Encoding.UTF8.GetBytes(msg);
                    mqttClient.Publish(topic, messageStr, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true); // retain

                }
                catch (Exception ex) {
                    myLog.LogAlert(AlertType.Error, line.Id.ToString(), ex.TargetSite.ToString(),
                        ex.Source.ToString(), ex.Message.ToString(), "system");
                }

            }
        }

        private void timeManager_SlowTask(object sender, EventArgs e)
        {
            try {
                if (!mqttClient.IsConnected) {
                    mqttClient.Connect(clientId);
                }
                publishStationAttributes();
            }
            catch (Exception ex) {
                myLog.LogAlert(AlertType.Error, line.Id.ToString(), ex.TargetSite.ToString(),
                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }

        private void timeManager_FastTask(object sender, EventArgs e)
        {
            publishStationEvents();
            Console.WriteLine(line.formatCounter(line.GetCounter()));
        }

        private Color applyColorSchema(int codeWord)
        {
            Color result = Color.Black;
            BSFlag redState = (BSFlag.Late | BSFlag.LiveLate);
            BSFlag greenState = (BSFlag.Finished | BSFlag.Outgo);

            if (((BSFlag)codeWord & BSFlag.Free) == BSFlag.Free) {
                result = Color.DimGray; // free and do nothing
            }
            else if ((codeWord & 0x0008) == 0x0008) {
                result = Color.DarkRed; // stop
            }
            else if ((((BSFlag)codeWord & redState) == redState) |
                     ((((BSFlag)codeWord & (BSFlag.Finished | BSFlag.LiveLate)) == (BSFlag.Finished | BSFlag.LiveLate)) &
                      (((BSFlag)codeWord & ~BSFlag.Late & ~BSFlag.Outgo) == (BSFlag)codeWord))) {
                result = Color.Red;
            }
            else if (((((BSFlag)codeWord & ~BSFlag.Finished & ~BSFlag.Late) == (BSFlag)codeWord) &
                      (((BSFlag)codeWord & BSFlag.LiveLate) == BSFlag.LiveLate)) |
                     ((((BSFlag)codeWord & (BSFlag.Finished | BSFlag.Late)) == (BSFlag.Finished | BSFlag.Late)) &
                      (((BSFlag)codeWord & ~BSFlag.LiveLate) == (BSFlag)codeWord)) |
                     ((((BSFlag)codeWord & BSFlag.Finished) == BSFlag.Finished) &
                      (((BSFlag)codeWord & ~BSFlag.Late & ~BSFlag.Outgo & ~BSFlag.LiveLate) == (BSFlag)codeWord))) {
                result = Color.Orange;
            }
            else if ((codeWord & 4) == 4) {
                result = Color.Goldenrod; // help
            }
            else if ((codeWord & 2) == 2) {
                // this.paBlack.BackColor = Color.Black;                 // non-working frame
            }
            else if (((BSFlag)codeWord & greenState) == greenState) {
                result = Color.ForestGreen; // finish
            }
            else {
                result = Color.Black; // default
            }
            return result;
        }



    }
}
