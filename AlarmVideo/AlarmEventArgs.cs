using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlarmVideo
{
    public class AlarmEventArgs : EventArgs
    {
        public List<Alarm> NewAlarms { get; }

        public AlarmEventArgs(List<Alarm> newAlarms)
        {
            NewAlarms = newAlarms;
        }
    }
}
