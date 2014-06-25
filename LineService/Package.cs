using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LineService
{
    public class Package
    {
        private string pkg;
        private string delimiter;
        private int dimention;

        public Package()
        {
            this.pkg = "";
            this.dimention = 0;
            this.delimiter = "§";
        }

        public void AddMessage(string key, string data)
        {
            this.pkg += key + delimiter + data + delimiter;
            this.dimention += 1;
        }

        public string Print() 
        {
            string result = dimention.ToString() + delimiter + this.pkg;
            return result;
        }
    }
}
