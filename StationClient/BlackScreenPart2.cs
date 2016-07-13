using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;

namespace StationClient
{
    ///
    /// topics to subscribe:
    /// 
    /// * takt timer ticks/seconds
    ///     andon/line/1/station/1/live
    ///     andor/line/1/takt                   // T
    /// 
    /// * operation ticks/seconds
    ///      andon/line/1/station/1/livta
    /// 
    /// * any button pressed
    ///     andon/line/1/station/1/button/1     //FI
    ///     andon/line/1/station/1/button/2     //ST
    ///     andon/line/1/station/1/button/3     //HE
    ///     andon/line/1/station/1/button/4     //P1
    ///     andon/line/1/station/1/button/5     //P2
    ///     andon/line/1/station/1/button/6     //FA
    /// 
    /// * month, day plan/fact
    ///     andon/line/1/station/1/planfact     // { "REGP_D":"1", "REGF_D":"2", "REGP_M":"1", "REGF_M":"2" }
    /// * bit state
    ///     andon/line/1/station/1/bitstate     // 0x1111
    ///     
    /// * timers    
    ///     andon/line/1/station/1/timer        // { "TIMER_STOP"       : "1",
    ///                                              "TIMER_HELP"       : "1",
    ///                                              "TIMER_PART1"      : "1",
    ///                                              "TIMER_PART2"      : "1",
    ///                                              "TIMER_FAIL"       : "1",
    ///                                              "TIMER_STOPLAST"   : "1", 
    ///                                              "TIMER_Late"       : "1",
    ///                                              "TIMER_Operating"  : "1",
    ///                                              "TIMER_LiveTakt"   : "1",
    ///                                              "TIMER_SumLate"    : "1", }
    ///                                               
    /// - GAP_DAY, GAP_MONTH - duplicate "REGx.x"
    /// 
    /// - buttons statet FINISH, STOP ... - dublicate FI, ST ...
    /// 
    /// - not used "MOVE_COUNTER", "EVENT_COUNTER", "LINE_NAME", "FRZ", in LineManager only
    /// 
    /// * station attributes
    ///     andon/line/1/station/1/state // { "S" : "station name", "B" : "product", "F" : "current frame" }
    ///                         
    /// - SS doesn't used anywhere
    /// - REGF.D, REGP.D, REGP.M, REGF.M - duplicate
    /// - LIVE, BST - duplicate


    public partial class BlackScreen
    {
        private MqttClient client;
        private MqttClient clientTi;
        private string brokerHostName = "localhost";
        private MqttLogger mqttLogger;
        
        private string context_line;
        private string context_station;
        private string context_button;
        private ClientInstruction instruction;

        private Dictionary<string, int> timers = new Dictionary<string, int>();

        protected void imqttInit()
        {
            context_line = "andon/line/" + this.lineId + "/";
            context_station = context_line + "station/" + this.stationIndex + "/";
            context_button = context_station + "button/";

            brokerHostName = Properties.Settings.Default.mqttHost;
            string clientId = Guid.NewGuid().ToString();
            byte[] qoS = new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE };

            client = new MqttClient(brokerHostName); // connect
            client.Connect(clientId);

            if (client.IsConnected) {
                safeChangeControl(laMessage, "");
            }

            client.Subscribe(new string[] { context_station + "attr" }, qoS);//andon/line/1/station/1/attr
            client.Subscribe(new string[] { context_station + "planfact" }, qoS);
            client.Subscribe(new string[] { context_station + "bst" }, qoS);
            client.Subscribe(new string[] { context_station + "instruction" }, qoS);
            client.Subscribe(new string[] { context_button + "#" }, qoS); //andon/line1/station1/button#
            client.MqttMsgPublishReceived += new MqttClient.MqttMsgPublishEventHandler(client_MqttMsgPublishReceived);  // register to message received

            clientTi = new MqttClient(brokerHostName); // connect
            clientId = Guid.NewGuid().ToString();
            clientTi.Connect(clientId);
            clientTi.Subscribe(new string[] { context_station + "timers" }, qoS);
            clientTi.MqttMsgPublishReceived += new MqttClient.MqttMsgPublishEventHandler(clientTi_MqttMsgPublishReceived);

            mqttLogger = new MqttLogger(brokerHostName, "scu" + stationIndex);
        }

        private void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            mqttLogger.Log("<<< \"" + e.Topic + "\" : \"" + Encoding.UTF8.GetString(e.Message) + "\"");  

            if (e.Topic == context_station + "instruction") { 
                instruction = JsonConvert.DeserializeObject<ClientInstruction>(Encoding.UTF8.GetString(e.Message));
                //json block {"Mode": "0", "ContentUrl": ""}
                //json block {"Mode": "1", "ContentUrl": "http://localhost:8080/shark.flv"}
                //json block {"Mode": "1", "ContentUrl": "http://vid2.anyclip.com/sr7smhbbtn"}
                //json block {"Mode": "2", "ContentUrl": "http://localhost:8080/pic.jpg"}
            }
            if (e.Topic.StartsWith(context_button)) {
                switch (e.Topic.Replace(context_button, "")) {
                    case "FINISH":
                        safeChangeControl(this.laFinishBtnValue, Encoding.UTF8.GetString(e.Message));
                        break;
                    case "STOP":
                        safeChangeControl(this.laStopBtnValue, Encoding.UTF8.GetString(e.Message));
                        break;
                    case "HELP":
                        safeChangeControl(this.laHelpBtnValue, Encoding.UTF8.GetString(e.Message));
                        break;
                    case "4":
                        mqttLogger.Log("Empty button 4 handler");
                        break;
                    case "5":
                        mqttLogger.Log("Empty button 5 handler");
                        break;
                    case "6":
                        mqttLogger.Log("Empty button 6 handler");
                        break;
                    default:
                        mqttLogger.Log("Unexpected mqtt topic \"" + e.Topic + "\"" );
                        break;
                }
            }
            else if (e.Topic.StartsWith(context_station)) {
                switch (e.Topic.Replace(context_station, "")) {
                    case "attr":
                        dynamic result = (JArray.Parse(Encoding.UTF8.GetString(e.Message)))[0];
                        //json block [{"S": "station name", "B": "product", "F": "current frame"}]
                        safeChangeControl(laStation, "Station ID: " + result.S);
                        safeChangeControl(laProduct, "Batch: " + result.B);
                        safeChangeControl(laFrame, "" + result.F);
                        break;
                    case "planfact":
                        result = (JArray.Parse(Encoding.UTF8.GetString(e.Message)))[0];
                        //json block [{"REGP_D":"1", "REGF_D":"2", "REGP_M":"1", "REGF_M":"2"}]
                        safeChangeControl(laDayPlan, "DPF: " + result.REGP_D + "/" + result.REGF_D);
                        safeChangeControl(laMonthPlan, "MPF: " + result.REGP_M + "/" + result.REGF_M);
                        safeChangeControl(laMonthplanValue, (Convert.ToInt16(result.REGF_M) - Convert.ToInt16(result.REGP_M)).ToString());
                        safeChangeControl(laPlanValue, (Convert.ToInt16(result.REGF_D) - Convert.ToInt16(result.REGP_D)).ToString());
                        break;
                    case "bst":
                        int newBitWord = Convert.ToUInt16(Encoding.UTF8.GetString(e.Message));
                        newBitWord = newBitWord & (int)~BSFlag.Blocked; // useless bit "Blocked"
                        safeChangeControl(laBitState, newBitWord.ToString());
                        safeChangeControl(laBitState2, "BS: " + newBitWord.ToString("X4"));
                        break;
                    default:
                        mqttLogger.Log("Unexpected mqtt topic \"" + e.Topic + "\"");
                        break;             
                }
            }
            else {
                Console.WriteLine("Ooops! Unknown topic of message ...");
            }
        }

        private void clientTi_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            mqttLogger.Log("<<< \"" + e.Topic + "\" : \"" + Encoding.UTF8.GetString(e.Message) + "\"");
            if (e.Topic.StartsWith(context_station)) {
                switch (e.Topic.Replace(context_station, "")) {
                    case "timers": 
                        dynamic resultTi = (JArray.Parse(Encoding.UTF8.GetString(e.Message)))[0];
                        timers["TIMER_LIVE"]        = resultTi.LiveTakt;
                        timers["T"]                 = resultTi.T;
                        timers["TIMER_STOP"]        = resultTi.STOP;
                        timers["TIMER_HELP"]        = resultTi.HELP;
                        timers["TIMER_STOPLAST"]    = resultTi.STOPLAST;
                        timers["TIMER_SumLate"]     = resultTi.SumLate;

                        //json block [{"STOP": 0, "HELP": 0, "PART1": 0, "PART2": 0, "Fail": 0, "STOPLAST": 0, 
                        //             "Late": 224, "Operating": 0, "SumLate": 0, "LiveTakt": 3300, "T": 3600}]
                        safeChangeControl(laMem, formatCounter(this.calcCounter()));
                        safeChangeControl(laTimeLeft, formatCounter(timers["T"]));
                        safeChangeControl(laLive, formatCounter(timers["TIMER_LIVE"]));
                        safeChangeControl(laSumstopValue, formatCounter(timers["TIMER_STOP"]));
                        safeChangeControl(laHelpTimeValue, formatCounter(timers["TIMER_HELP"]));
                        break;
                    default:
                        Console.WriteLine("Ooops! Unknown topic of message ...");
                        break;
                }
            }
            else {
                Console.WriteLine("Ooops! Unknown topic of message ...");
            }
        }

        ///<summary>
        /// Because you can't just change controls text from different thread. Just helper
        ///</summary>
        private void safeChangeControl(Control gui_ctrl, string text)
        {
            Action action = delegate()
            {
                gui_ctrl.Text = text;
            };

            if (this.InvokeRequired) {
                this.Invoke(action);
            }
            else {
                action();
            }
        }
    }

    /// <summary>
    /// Stub interface class
    /// </summary>
    public class StationAttributes {
       
        [JsonProperty("S")]
        public string StationName { get; set; }

        [JsonProperty("B")]
        public string Product { get; set; }

        [JsonProperty("F")]
        public string Frame { get; set; }
    }

    /// <summary>
    /// Stub interface class
    /// </summary>
    public class ClientInstruction
    {
        [JsonProperty]
        public int Mode;
        [JsonProperty]
        public string ContentUrl;
    }
}
