using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TechDispatch.Models
{
    public class FieldTech
    {
        public virtual int FieldTechID { get; set; }
        public virtual string UserID { get; set; }

        public virtual int? InstallZoneID { get; set; }
        public virtual InstallZone InstallZone { get; set; }

        public virtual string Notes { get; set; }
        public virtual bool Active { get; set; }
    }

    public class FieldTechIndexView
    {
        public virtual int FieldTechID { get; set; }
        public virtual string UserID { get; set; }
        [Display(Name="User")]
        public virtual string UserName { get; set; }
        [Display(Name = "Rank")]
        public virtual string UserRank { get; set; }
        [Display(Name = "Field Tech?")]
        public virtual bool FieldTech { get; set; }
        public virtual int InstallZoneID { get; set; }
        public IEnumerable<SelectListItem> InstallZone { get; set; }
    }
}