using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TechDispatch.Models
{
    public class Tower
    {
        public virtual int TowerID { get; set; }

        [Required]
        [Display(Name="Tower Name")]
        public virtual string TowerName { get; set; }

        [Required]
        [RegularExpression(@"^10.\d{1,3}.\d{1,3}$")]
        public virtual string Subnet { get; set; }

        public virtual List<IP> IPs { get; set; }
        public virtual List<AccessPoint> APs { get; set; }

        public virtual bool Active { get; set; }

        [Required]
        public virtual int InstallZoneId { get; set; }
        public virtual InstallZone InstallZone { get; set; }

        public virtual int APsToCreate { get; set; } //this is just used for seeding purposes.
    }

    [NotMapped]
    public class TowerView : Tower
    {
        //not much difference yet.

        public virtual string InstallZoneName { get; set; }
        public virtual List<AccessPointView> APViews { get; set; }

        public TowerView(Tower tower, bool full = false)
        {
            TowerID = tower.TowerID;
            TowerName = tower.TowerName;
            Subnet = tower.Subnet;

            if (full)
            {
                IPs = tower.IPs; //only do this if relevant.
            }

            APViews = new List<AccessPointView>();
            foreach (var a in tower.APs)
            {
                APViews.Add(new AccessPointView(a));
            }

            InstallZoneId = tower.InstallZoneId;

            if (tower.InstallZone != null)
            {
                InstallZoneName = tower.InstallZone.Name;
            } else
            {
                InstallZoneName = "N/A";
            }
        }

    }

    public class TowerCreate
    {
        public virtual int TowerID { get; set; }

        [Required]
        [Display(Name = "Tower Name")]
        public virtual string TowerName { get; set; }

        [Required]
        [RegularExpression(@"^10.\d{1,3}.\d{1,3}$")]
        public virtual string Subnet { get; set; }

        [Required]
        public virtual int InstallZoneId { get; set; }
    }

    public class APCreate
    {
        public int APCreateID { get; set; }
        [Required]
        [Display(Name="Name")]
        public String APName { get; set; }
        [Required]
        [Display(Name = "State")]
        public AccessPoint.AccessPointState APState { get; set; }
        [Required]
        [Display(Name = "Comment")]
        public String APComment { get; set; }
    }

    public class APCreateList
    {
        public List<APCreate> APs { get; set; }
    }

    public class TowerEdit
    {
        public int TowerEditID { get; set; }

        [Required]
        [Display(Name = "Tower Name")]
        public virtual string TowerName { get; set; }

        [RegularExpression(@"^10.\d{1,3}.\d{1,3}$")]
        public virtual string Subnet { get; set; }

        [Required]
        public virtual int InstallZoneId { get; set; }

        public virtual List<AccessPoint> APs { get; set; }
    }
}