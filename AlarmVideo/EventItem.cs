using System;

namespace AlarmVideo
{
    public class EventItem
    {
        public string Comment { get; set; }
        public DateTime CommentTime { get; set; }
        public string Source { get; set; }
        public string EventType { get; set; }
        public int Id { get; set; }
        public string CameraName { get; set; }
    }
}
