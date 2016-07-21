using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LineService
{
    public abstract class ControlItem : Object 
    {
        protected object owner;
        protected int id;
        protected string name;
        protected string state;
        protected string strStateProp 
        {
            get { return state; }
            set {

                if (state != value && OnChanged != null) {
                    state = value;
                    OnChanged(this, new EventArgs());
                }
                else {
                    state = value;
                }
            }
        }

        public event EventHandler OnChanged;
      
        public int Id
        {
            get { return this.id; }
        }
        public string State
        {
            get { return this.strStateProp; }
        }
        public string Name
        {
            get { return this.name; }
        }
        public object Owner
        {
            get { return owner; }
        }   
    }

    public class Button : ControlItem
    {
        private string varName = "NA";
        private string varSignalName = "NA";

        private string keyString;
        private bool isBlocked = false;
        private string partsAddress = "";

        public ButtonState TState 
        { 
            get { return (ButtonState)(Convert.ToInt16(this.strStateProp)); } 
        }

        public string VarName 
        { 
            get { return this.varName; } 
        }
        public string VarSignalName 
        { 
            get { return this.varSignalName; } 
        }
        public string KeyString 
        { 
            get { return this.keyString; } 
        }
        public string PartsAddress 
        { 
            get { return this.partsAddress; } 
            set { this.partsAddress = value; } 
        }

        public Button(object owner) : this(owner, 0, "NA", "NA", "NA_s", "NA") { }

        public Button(object owner, int id, string name, string varName, string varSignalName, string keyString)
        {
            this.owner = owner;
            this.id = id;
            this.name = name;
            this.varName = varName;
            this.varSignalName = varSignalName;
            this.keyString = keyString;
            this.state = "0"; //"Off";
        }
        public int Push()
        {
            int result = 0;
            if (!this.isBlocked && this.strStateProp == "0") //"Off")
            {
                this.strStateProp = "1"; // "On";
                result = 1;
            }
            else
            {
                this.strStateProp = "0"; // "Off";
                result = 0;
            }
            return result;
        }
        public int Reset()
        {
            this.strStateProp = "0"; //"Off";
            return 0;
        }

        public bool IsBlocked 
        { 
            get { return this.isBlocked; } 
            set { this.isBlocked = value; } 
        }
        public string IPAddr 
        { 
            get; set; 
        }
        public int Channel 
        { 
            get; set; 
        }
        public int ModuleType 
        { 
            get; set; 
        }

    }



}
