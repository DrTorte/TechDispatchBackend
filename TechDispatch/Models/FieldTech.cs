using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TechDispatch.Models
{
    public class FieldTech
    {
        public virtual int FieldTechID { get; set; }
        public virtual string UserID { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual int? InstallZoneID { get; set; }
        public virtual InstallZone InstallZone { get; set; }

        public virtual string Notes { get; set; }
        public virtual bool Active { get; set; }

        public FieldTech() { }
    }

    [NotMapped]
    public class FieldTechView : FieldTech
    {
        public virtual string UserName { get; set; }
        public virtual string Name { get; set; }

        public FieldTechView(FieldTech tech)
        {
            FieldTechID = tech.FieldTechID;
            UserID = tech.UserID;

            InstallZoneID = tech.InstallZoneID;
            Notes = tech.Notes;
            Active = tech.Active;

            if (tech.User != null)
            {
                UserName = tech.User.UserName;
                Name = tech.User.Name;
            }
            else
            {
                UserName = "Undefined";
                Name = "Undefined";
            }
        }
    }
}