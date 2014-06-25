using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LineService
{
    public class DispatcherStationArgs : EventArgs
    {
        public Product Product { get; set; }
        public IStation Station { get; set; }
        public string Message { get; set; }
    }
}
