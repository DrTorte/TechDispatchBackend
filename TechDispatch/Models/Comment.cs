using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechDispatch.Models
{
    public class Comment
    {
        public virtual int CommentId { get; set; }

        public virtual int AppointmentId { get; set; }
        public string Creator { get; set; }
        public string Value { get; set; }
        public DateTime CreationDate { get; set; }

        public Comment()
        {
            CreationDate = DateTime.Now;
        }
    }
}