using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TechDispatch.Models
{
    public class IP
    {
        public virtual int IPId { get; set; }
        [Required]
        public virtual string Subnet { get; set; }
        [Required]
        public virtual byte IPAddress { get; set; }

        public virtual int TowerId { get; set; }

        public virtual int? CustomerID { get; set; }

        [Required]
        public virtual bool CustomerUseable { get; set; }

        public virtual string Comment { get; set; }

        public string ReadIP {get {
            return (Subnet + "." + IPAddress.ToString());
        } set {} }
    }

    public class IPSelectViewModel
    {

    }
}