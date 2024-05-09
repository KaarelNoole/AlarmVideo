using System;
using System.Collections.Generic;

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
