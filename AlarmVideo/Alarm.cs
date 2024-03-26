using System;
using System.Collections.Generic;

namespace AlarmVideo
{
    public class Alarm
    {

        public int Id { get; set; }
        public DateTime EventTime { get; set; }
        public string Source { get; set; }
        public string Event { get; set; }

        public string Status { get; set; }
    }
}