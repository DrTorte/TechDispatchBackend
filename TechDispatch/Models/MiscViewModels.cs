using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TechDispatch.Models
{
    public class NavigationBars
    {
        public String Name { get; set; }
        public String Controller { get; set; }
        public String Action { get; set; }
        public bool? Admin { get; set; }
    }

    public class AppointmentSubReason
    {
        public int AppointmentSubReasonID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool Active { get; set; }
    }

    public class AppointmentStateReason
    {
        public int AppointmentStateReasonID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool Active { get; set; }
    }

}