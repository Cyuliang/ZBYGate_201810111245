using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBYGate_201810111245.Gate
{
    class DoorStateEventArgs : EventArgs
    {
        public DoorStateEventArgs(int state, Int32 SN)
        {
            this.State = state;
            this.SN = SN;
        }

        public int State { get; private set; }
        public Int32 SN  { get; private set; }
    }
}
