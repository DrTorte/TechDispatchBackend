using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Core.Objects.DataClasses;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Runtime.Serialization;

namespace TechDispatch.Models
{

    public class Schedule
    {
        [Required]
        public virtual int ScheduleId { get; set; }

        //set whether this is the default schedule or not.
        [Required]
        [Display(Name = "Default Weekly Schedule")]
        public virtual bool DefaultOption { get; set; }

        //otherwise, use a specific date.
        [Required]
        [DataType(DataType.Date)]
        public virtual DateTime Date { get; set; }

        //and the specific openings available.
        public virtual List<Openings> Openings { get; set; }

        //this is only really used for default dates so we can disable old ones.
        public virtual bool Active { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DefaultOption == false && Date == null)
            {
                yield return new ValidationResult("Date is required!");
            }
        }

        //filter to specific openings.
        public Schedule ScheduleWithFilter(DateTime targetDate, TimeSlot timeSlot = null, InstallZone installZone = null, bool install = false)
        {
            Openings.Clear();
            Openings = AvailableOpenings(targetDate, timeSlot, installZone, install);

            return (this);
        }

        //filter out openings.
        private List<Openings> FilterOpenings(DateTime targetDate, TimeSlot timeSlot, InstallZone installZone)
        {
            TechDispatchContext db = new TechDispatchContext();
            IEnumerable<Appointment> Apps = db.Appointments.Where(x => x.Date == targetDate);
            List<Openings> SelectedOpenings = new List<Openings>();

            //we want to only pick days that are relevant.
            int day;
            if (DefaultOption)
            {
                day = (int)targetDate.DayOfWeek;
            }
            else
            {
                day = -1;
            }

            SelectedOpenings = db.Openings.Where(x => x.ScheduleID == ScheduleId && x.Day == day).OrderBy(x => x.InstallZone.Name).ThenBy(x => x.TimeSlot.Name).ToList();

            if (timeSlot != null)
            {
                SelectedOpenings = SelectedOpenings.Where(x => x.TimeSlotId == timeSlot.TimeSlotID).ToList();
                Apps = Apps.Where(x => x.TimeSlot == timeSlot);
            }
            if (installZone != null)
            {
                SelectedOpenings = SelectedOpenings.Where(x => x.InstallZoneId == installZone.InstallZoneId).ToList();
                Apps = Apps.Where(x => x.Customer.Tower.InstallZone == installZone);
            }

            return SelectedOpenings;
        }

        //filter available openings only.
        private List<Openings> AvailableOpenings(DateTime targetDate, TimeSlot timeSlot, InstallZone installZone, bool install = false) 
        {
            TechDispatchContext db = new TechDispatchContext();
            IEnumerable<Appointment> Apps = db.Appointments.Where(x => x.Date == targetDate && x.CurrentState != Appointment.AppointmentState.Cancelled 
                && x.CurrentState != Appointment.AppointmentState.Failed && x.CurrentState != Appointment.AppointmentState.NeedsReschedule);
            List<Openings> SelectedOpenings = FilterOpenings(targetDate,timeSlot,installZone);

            //if applicable, a list of IDs that have been modified to account for minus one installs.

            foreach (var x in SelectedOpenings)
            {
                x.AvailableAmount = x.Amount;
            }

            foreach (var x in Apps)
            {
                try
                {
                    SelectedOpenings.DefaultIfEmpty(null).FirstOrDefault(y => y.InstallZoneId == 
                        x.Customer.Tower.InstallZoneId && y.TimeSlotId == x.TimeSlotID).AvailableAmount--;
                }
                catch (NullReferenceException e)
                {
                    var y = 123123;
                }
            }

            if (install)
            {
                foreach (var x in SelectedOpenings)
                {
                    if (x.AvailableAmount > 0 && Apps.Where(y => x.InstallZoneId == y.Customer.Tower.InstallZoneId 
                        && x.TimeSlotId == y.TimeSlotID && y.AppointmentType != Appointment.AppointmentReason.Install).Count() == 0)
                    {
                        x.AvailableAmount--;
                    }
                }
            }

            return SelectedOpenings;
        }

        public Schedule Clone(Schedule schedule)
        {
            //lazy cloning, for the time being. Definitely need to improve this later.

            schedule.DefaultOption = this.DefaultOption;
            schedule.Date = this.Date;
            schedule.Active = this.Active;
            schedule.Openings = new List<Openings>();

            var OpeningsForMe = Openings.Where(x => x.ScheduleID == ScheduleId).ToList();
            for (int i = 0; i < OpeningsForMe.Count; i++)
            {
                Openings myOpening = new Openings();

                myOpening = OpeningsForMe[i].Clone(myOpening);

                schedule.Openings.Add(myOpening);
            }

            return schedule;
        }

    }

    public class Openings
    {
        public virtual int OpeningsID { get; set; }

        //assign to a schedule.
        [Required]
        public virtual int ScheduleID { get; set; }
        public virtual Schedule Schedule { get; set; }

        [Required]
        public virtual int InstallZoneId { get; set; }
        public virtual InstallZone InstallZone { get; set; }

        [Required]
        public virtual int TimeSlotId { get; set; }
        public virtual TimeSlot TimeSlot { get; set; }

        [Required]
        [Range(0,100)]
        public virtual int Amount { get; set; }
        public virtual int AvailableAmount { get; set; }

        [Required]
        [Range(-1,7)]
        public virtual int Day { get; set; }

        public Openings Clone(Openings Opening)
        {
            //Openings Opening = new Openings();
            Opening.InstallZoneId = InstallZoneId;
            Opening.InstallZone = InstallZone;
            Opening.TimeSlotId = TimeSlotId;
            Opening.TimeSlot = TimeSlot;
            Opening.Amount = Amount;
            Opening.AvailableAmount = AvailableAmount;
            Opening.Day = Day;

            return (Opening);
        }
    }
}