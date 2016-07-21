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
    public class ClientInstruction
    {
        [JsonProperty]
        public int Mode;
        [JsonProperty]
        public string ContentUrl;
    }
}
