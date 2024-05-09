using System;

namespace AlarmVideo
{
    public class Event
    {
        public int EventID { get; set; }
        public DateTime Time { get; set; }
        public string Comment { get; set; }
        public DateTime CommentTime { get; set; }
        public DateTime AlarmEnd { get; set; }    
        public DateTime EventEnd { get; set; }    
        public DateTime Event_Recovery_time { get; set; }
    }
}
