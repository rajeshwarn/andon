using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LineService
{
    public class Button
    {
        private int id = 0;
        private string name = "NA";
        private string varName = "NA";
        private string varSignalName = "NA";
        private string strState = "0"; //"Off";
        private string keyString;
        private bool isBlocked = false;
        private string partsAddress = "";

        public int Id { get { return this.id; } }
        public string State { get { return this.strState; } }
        public ButtonState TState { get { return (ButtonState)(Convert.ToInt16(this.strState)); } }
        public string Name { get { return this.name; } }
        public string VarName { get { return this.varName; } }
        public string VarSignalName { get { return this.varSignalName; } }
        public string KeyString { get { return this.keyString; } }
        public string PartsAddress { get { return this.partsAddress; } set { this.partsAddress = value; } } 

        public Button() : this(0, "NA", "NA", "NA_s", "NA") { }

        public Button(int id, string name, string varName, string varSignalName, string keyString)
        {
            this.id = id;
            this.name = name;
            this.varName = varName;
            this.varSignalName = varSignalName;
            this.keyString = keyString;
        }
        public int Push()
        {
            int result = 0;
            if (!this.isBlocked && this.strState == "0") //"Off")
            {
                this.strState = "1"; // "On";
                result = 1;
            }
            else
            {
                this.strState = "0"; // "Off";
                result = 0;
            }
            return result;
        }
        public int Reset()
        {
            this.strState = "0"; //"Off";
            return 0;
        }

        public bool IsBlocked { get { return this.isBlocked; } set { this.isBlocked = value; } }
        public string IPAddr { get; set; }
        public int Channel { get; set; }
        public int ModuleType { get; set; }
    }



}
