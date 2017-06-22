using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechDispatch.Models
{
    public class AppointmentResolutionReason
    {
        public virtual int AppointmentResolutionReasonID { get; set; }

        public virtual string Name { get; set; }
        public virtual bool Active { get; set; }

        public virtual bool Cancel { get; set; }
        public virtual bool Fail { get; set; }
        public virtual bool Complete { get; set; }
        public virtual bool Reschedule { get; set; }
        public virtual bool RequireComment { get; set; }
    }
}