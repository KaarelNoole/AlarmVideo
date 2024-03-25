using System;
using System.Collections.Generic;

namespace AlarmVideo
{
    public class Alarm
    {

        public DateTime EventTime { get; set; }
        public string Source { get; set; }
        public string Event { get; set; }

        public string Comment { get; set; }

        public string CommentTime { get; set; }
        public List<string> Comments { get; set; } = new List<string>();

        
    }
}