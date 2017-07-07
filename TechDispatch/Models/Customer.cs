using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace TechDispatch.Models
{
    public class Customer : IValidatableObject
    {
        public enum CustomerStatus
        {
            Active, Pending, Hold, Cancelled
        }
        //this is incredibly ugly. I will change this later.
        public enum Speed
        {
            Basic, Pro, Max, Titanium
        }

        public virtual int CustomerID { get; set; }

        [Display(Name = "Customer State")]
        public virtual CustomerStatus CustomerState { get; set; }

        [Required]
        public virtual string Name { get; set; }

        [Required]
        public virtual string Address { get; set; }

        public string _PhoneNumber;

        [Display(Name = "Phone Number")]
        [Required]
        [RegularExpression("[0-9]{3}.*[0-9]{3}.*[0-9]{4}.*[0-9]*")]

        public virtual string PhoneNumber
        {
            get
            {
                if (_PhoneNumber == null)
                {
                    return null;
                }
                if (Regex.IsMatch(_PhoneNumber, @"^(\d{3})(\d{3})(\d{4})$"))
                {
                    _PhoneNumber = Regex.Replace(_PhoneNumber, @"(\d{3})(\d{3})(\d{4})",
                        m => string.Format("{0}-{1}-{2}",
                            m.Groups[1].Value,
                            m.Groups[2].Value,
                            m.Groups[3].Value));
                }
                else
                {
                    _PhoneNumber = Regex.Replace(_PhoneNumber, @"(\d{3})(\d{3})(\d{4})(\d+)",
                    m => string.Format("{0}-{1}-{2} ext {3}",
                        m.Groups[1].Value,
                        m.Groups[2].Value,
                        m.Groups[3].Value,
                        m.Groups[4].Value));
                }

                return _PhoneNumber;
            }
            set
            {
                //strip everything that's not a number out.
                if (value != null) {
                    var TN = Regex.Replace(value, @"[^\d]", "");
                    _PhoneNumber = TN;
                }
            }
        }

        [Display(Name = "Username")]
        public virtual string PPPoEUser { get; set; }
        [Display(Name = "Password")]
        public virtual string PPPoEPassword { get; set; }

        public virtual int? TowerId { get; set; }
        public virtual Tower Tower { get; set; }

        public virtual int? IPID { get; set; }
        public virtual IP IP { get; set; }

        //change this later!!
        [Display(Name = "Speed")]
        public virtual Speed? CustomerSpeed { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if ((CustomerState == CustomerStatus.Active || CustomerState == CustomerStatus.Pending) && (IPID == null || TowerId == null || CustomerSpeed == null))
            {
                if (TowerId == null)
                    yield return new ValidationResult("Tower is required!");
                if (IPID == null)
                    yield return new ValidationResult("IP is required!");
                if (CustomerSpeed == null)
                    yield return new ValidationResult("Speed is required!");
            }
            else
            {
                // do some further checks here. Make sure that the IP exists in the tower.
                TechDispatchContext db = new TechDispatchContext();

                if (CustomerState == CustomerStatus.Active || CustomerState == CustomerStatus.Pending)
                {
                    var _tower = db.Towers.DefaultIfEmpty(null).FirstOrDefault(x => x.TowerID == TowerId);
                    if (_tower == null)
                    {
                        yield return new ValidationResult("Invalid tower selected.");
                    }
                    else
                    {
                        var _ip = _tower.IPs.DefaultIfEmpty(null).FirstOrDefault(x => x.IPId == IPID);
                        if (_ip == null)
                        {
                            yield return new ValidationResult("IP not found in tower.");
                        }
                    }
                }

                // now check to see if IP isn't taken elsewhere, if it's defined.
                if (IPID != null && db.Customers.DefaultIfEmpty(null).FirstOrDefault(x => x.IPID == IPID && x.CustomerID != CustomerID) != null)
                {
                    yield return new ValidationResult("IP already taken.");
                }
            }
        }

        public void Cancel(bool hold) {
            TechDispatchContext db = new TechDispatchContext();
            CustomerState = hold == true ? CustomerStatus.Hold : CustomerStatus.Cancelled;
            if (IPID != null)
            {
                db.IPs.First(x => x.IPId == IPID).CustomerID = null;
                IPID = null;
            }
            PPPoEPassword = "";
            PPPoEUser = "";
            TowerId = null;
            db.SaveChanges();
        }

        public string ReadIP()
        {
            if (IPID == null)
            {
                return "Not set";
            }
            else
            {
                return (IP.Subnet + "." + IP.IPAddress);
            }
        }
    }

    [NotMapped]
    public class CustomerListJson
    {
        public virtual int CustomerID { get; set; }
        public virtual string Name { get; set; }
        public virtual string Address { get; set; }

        public CustomerListJson(Customer cx)
        {
            CustomerID = cx.CustomerID;
            Name = cx.Name;
            Address = cx.Address;

        }
    }

}