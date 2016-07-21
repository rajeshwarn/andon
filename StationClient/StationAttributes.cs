using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace StationClient
{
    /// <summary>
    /// Stub interface class
    /// </summary>
    public class StationAttributes
    {

        [JsonProperty("S")]
        public string StationName { get; set; }

        [JsonProperty("B")]
        public string Product { get; set; }

        [JsonProperty("F")]
        public string Frame { get; set; }
    }
}
