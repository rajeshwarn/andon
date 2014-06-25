using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LineService
{
    public interface IOPCStation
    {
        LineOPCProvider OPCProvider { set; }
        void ResetControls(ResetControlsType resetType);
        void SetControls(Dictionary<string, ButtonState> controls);
    }
}
