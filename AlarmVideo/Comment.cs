using System;

namespace AlarmVideo
{
    public class Comment
    {
        public int Id { get; set; }
        public string comment { get; set; }
        public int AlarmId { get; set; }

        public DateTime CommentTime { get; set; }
    }
}
