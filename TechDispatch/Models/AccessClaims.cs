using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TechDispatch.Models
{
    public class AccessClaims
    {
        public virtual int AccessClaimsId { get; set; }

        //The "key" value.
        [Display(Name="Claim Name")]
        public virtual string ClaimName { get; set; }
        //The actual value.
        [Display(Name = "Claim Value")]
        public virtual string ClaimValue { get; set; }

        //indicates whether this access claim is seeded at the creation of the DB.
        [Display(Name = "Included by Default")]
        public virtual bool IncludedClaim { get; set; }
    }
}