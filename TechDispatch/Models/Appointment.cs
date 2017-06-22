using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TechDispatch.Models
{
    public class Appointment
    {
        public enum AppointmentState { Open, PendingCompletion, PendingAssignment, PendingDispatch, Dispatched, Failed, Completed, NeedsReschedule, Cancelled }
        public enum AppointmentReason { Install, Repair, Misc }

        public virtual int AppointmentID { get; set; }

        public virtual int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }

        [Required]
        public virtual AppointmentReason AppointmentType { get; set; }

        public virtual int? AppointmentSubReasonID { get; set; }
        public virtual AppointmentSubReason AppointmentSubReason { get; set; }

        public virtual int? AppointmentResolutionReasonID { get; set; }
        public virtual AppointmentResolutionReason AppointmentResolutionReason { get; set; }


        [DataType(DataType.Date)]
        public virtual DateTime Date { get; set; }

        [DataType(DataType.Date)]
        [Display(Name="Creation Date")]
        public virtual DateTime CreationDate { get; set; }

        [Display(Name="Time Slot")]
        public virtual int? TimeSlotID { get; set; }
        public virtual TimeSlot TimeSlot { get; set; }

        public virtual string Creator { get; set; }

        public virtual int? FieldTechID { get; set; }
        public virtual FieldTech FieldTech { get; set; }

        public virtual AppointmentState CurrentState { get; set; }

        public virtual List<Comment> Comments { get; set; }

        public Appointment()
        {
            CreationDate = DateTime.Now;
        }
    }

    public class AppointmentJsonView
    {
        public virtual int AppointmentID { get; set; }

        public virtual int CustomerID { get; set; }
        public virtual string CustomerName {get;set;}
        public virtual string CustomerAddress { get; set; }
        public virtual string CustomerPhone { get; set; }

        [Required]
        public virtual string AppointmentType { get; set; }

        [DataType(DataType.Date)]
        public virtual DateTime Date { get; set; }

        [Display(Name = "Time Slot")]
        public virtual string TimeSlotName{get;set;}

        public virtual string ZoneName { get; set; }

        public virtual int? FieldTechID { get; set; }
        public virtual string FieldTechUserID { get; set; }
        public virtual string CurrentState { get; set; }

        public string Classes { get; set; }

        public AppointmentJsonView(Appointment app)
        {
            AppointmentID = app.AppointmentID;
            CustomerName = app.Customer.Name;
            CustomerAddress = app.Customer.Address;
            CustomerPhone = app.Customer.PhoneNumber;
            if (app.Customer.Tower != null)
            {
                ZoneName = app.Customer.Tower.InstallZone.Name;
            }
            AppointmentType = app.AppointmentType.ToString();
            Date = app.Date;
            TimeSlotName = app.TimeSlot.Name;
            if (app.FieldTech != null)
            {
                FieldTechID = app.FieldTechID;
                FieldTechUserID = app.FieldTech.UserID;
            }
            else
            {
                FieldTechID = null;
                FieldTechUserID = null;
            }
            CurrentState = app.CurrentState.ToString();
        }
    }

    public class AppointmentDetailJson : AppointmentJsonView
    {

        public virtual int? IPID { get; set; }
        public virtual string IP { get; set; }
        public virtual int? TowerID { get; set; }
        public virtual string TowerName { get; set; }

        public virtual Comment[] Comments { get; set; }

        public AppointmentDetailJson(Appointment app, TechDispatchContext db) : base(app)
        {
            AppointmentID = app.AppointmentID;
            CustomerName = app.Customer.Name;
            CustomerAddress = app.Customer.Address;
            CustomerPhone = app.Customer.PhoneNumber;
            if (app.Customer.Tower != null)
            {
                ZoneName = app.Customer.Tower.InstallZone.Name;
            }
            AppointmentType = app.AppointmentType.ToString();
            Date = app.Date;
            TimeSlotName = app.TimeSlot.Name;
            if (app.FieldTech != null)
            {
                FieldTechID = app.FieldTechID;
                FieldTechUserID = app.FieldTech.UserID;
            }
            else
            {
                FieldTechID = null;
                FieldTechUserID = null;
            }
            CurrentState = app.CurrentState.ToString();

            IPID = app.Customer.IPID;
            if (IPID != null)
            {
                IP = app.Customer.ReadIP();
            } else
            {
                IP = "None";
            }

            TowerID = app.Customer.TowerId;
            if (TowerID != null)
            {
                TowerName = app.Customer.Tower.TowerName;
            } else
            {
                TowerName = "None";
            }

            Comments = db.Comments.Where(x => x.AppointmentId == AppointmentID).ToArray();

        }
    }
}