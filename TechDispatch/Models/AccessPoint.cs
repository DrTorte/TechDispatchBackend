﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace TechDispatch.Models
{
    public class AccessPoint
    {
        public enum AccessPointState { Open, Limited, Full}

        public virtual int AccessPointID { get; set; }

        public virtual Tower Tower { get; set; }
        public virtual int TowerId { get; set; }

        [Required]
        public virtual string Name { get; set; }

        [Required]
        [Display(Name="Current Status")]
        public virtual AccessPointState CurrentState { get; set; }

        [Display(Name = "Comment")]
        public virtual string Comment { get; set; }
    }
}
