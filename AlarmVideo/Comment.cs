using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
