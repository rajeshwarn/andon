using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LineService
{
    public class DispatcherMoveArgs : EventArgs
    {
        public Product Product { get; set; }
        public IEnumerable<string> DestinationList { get; set; }
    }
}
