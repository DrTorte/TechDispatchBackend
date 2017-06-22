using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TechDispatch.Models
{
    public class TechDispatchRole
    {
        public virtual int TechDispatchRoleId { get; set; }

        //indicates whether this role is seeded at the creation of the DB.
        [Display(Name="Included by Default")]
        public virtual bool IncludedRole { get; set; }

        [Display(Name="Name of Role")]
        public virtual string Name { get; set; }

        //List of claims this has access to.
        public virtual List<AccessClaims> AccessClaims { get; set; }
    }
}