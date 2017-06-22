using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TechDispatch.Models
{
    public class TechDispatchContext : IdentityDbContext<ApplicationUser>
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        public TechDispatchContext()
            : base("name=TechDispatchContext")
        {
        }

        public static TechDispatchContext Create()
        {
            return new TechDispatchContext();
        }
        
        public System.Data.Entity.DbSet<TechDispatch.Models.AccessPoint> AccessPoints { get; set; }

        public System.Data.Entity.DbSet<TechDispatch.Models.IP> IPs { get; set; }

        public System.Data.Entity.DbSet<TechDispatch.Models.Tower> Towers { get; set; }

        public System.Data.Entity.DbSet<TechDispatch.Models.TimeSlot> TimeSlots { get; set; }

        public System.Data.Entity.DbSet<TechDispatch.Models.InstallZone> InstallZones { get; set; }

        public System.Data.Entity.DbSet<TechDispatch.Models.Appointment> Appointments { get; set; }

        public System.Data.Entity.DbSet<TechDispatch.Models.Openings> Openings { get; set; }

        public System.Data.Entity.DbSet<TechDispatch.Models.Schedule> Schedules { get; set; }

        public System.Data.Entity.DbSet<TechDispatch.Models.Customer> Customers { get; set; }

        public System.Data.Entity.DbSet<TechDispatch.Models.AccessClaims> AccessClaims { get; set; }

        public System.Data.Entity.DbSet<TechDispatch.Models.TechDispatchRole> TechDispatchRoles { get; set; }
        
        public System.Data.Entity.DbSet<TechDispatch.Models.APCreate> APCreates { get; set; }

        public System.Data.Entity.DbSet<TechDispatch.Models.Comment> Comments { get; set; }

        public System.Data.Entity.DbSet<TechDispatch.Models.FieldTech> FieldTechs { get; set; }

        public System.Data.Entity.DbSet<TechDispatch.Models.AppointmentSubReason> AppointmentSubReasons { get; set; }

        public System.Data.Entity.DbSet<TechDispatch.Models.AppointmentResolutionReason> AppointmentResolutionReasons { get; set; }
    }
}
