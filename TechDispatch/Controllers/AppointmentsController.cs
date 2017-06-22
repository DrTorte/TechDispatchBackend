using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TechDispatch.Attributes;
using TechDispatch.Models;

namespace TechDispatch.Controllers
{
    [TokenAuthorize]
    [RoutePrefix("api/Appointments")]
    public class AppointmentsController : ApiController
    {
        private TechDispatchContext db = new TechDispatchContext();

        // GET: api/Appointments
        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAppointments(DateTime? FromDate = null, 
            DateTime? ToDate = null, int? Skip = null, int? Count = null, int? CustomerId = null, int? TowerId = null, int? ZoneId = null)
        {
            List<Appointment> Appointments = db.Appointments.Include("Customer").Include("TimeSlot").Include("Customer.Tower").Include("Customer.Tower.InstallZone").Include("FieldTech")
                .Where(x=> CustomerId == null || x.CustomerID == CustomerId)
                .Where(x=> TowerId == null || x.Customer.Tower.TowerID == TowerId)
                .Where(x=> ZoneId == null || x.Customer.Tower.InstallZoneId == ZoneId)
                .Where(x=> FromDate == null || x.Date >= FromDate)
                .Where(x=> ToDate == null || x.Date <= ToDate)
                .ToList();
            
            if (Skip != null)
            {
                Appointments = Appointments.Skip((int)Skip).ToList();
            }
            if (Count != null)
            {
                Appointments = Appointments.Take((int)Count).ToList();
            }
            List <AppointmentJsonView> apps = new List<AppointmentJsonView>();
            List<Task> tasks = new List<Task>();
            using (var dbb = db)
            {
                dbb.Database.Connection.Open();
                foreach (var x in Appointments)
                {
                    tasks.Add(Task.Factory.StartNew(() => { apps.Add(new AppointmentJsonView(x)); }));
                }
            }

            Task.WaitAll(tasks.ToArray());
            apps = apps.OrderBy(x => x.Date).ThenBy(x => x.TimeSlotName).ThenBy(x => x.CustomerName).ToList();
            return Ok(apps);
        }

        // GET: api/Appointments/5
        [HttpGet]
        [ResponseType(typeof(Appointment))]
        [Route("{id}")]
        public async Task<IHttpActionResult> GetAppointment(int id)
        {
            Appointment appointment = await db.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }



            return Ok(new AppointmentDetailJson(appointment,db));
        }

        // PUT: api/Appointments/5
        //in this case we are working with certain limitatiosn which means put and similar are not available, leaving us with just Get and Post.
        [HttpPost]
        [Route("Update")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAppointment(Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(appointment).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(appointment.AppointmentID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Appointments
        [Route("Create")]
        [ResponseType(typeof(Appointment))]
        public async Task<IHttpActionResult> PostAppointment(Appointment appointment)
        {
            Validate<Appointment>(appointment);
            if (appointment == null)
            {
                ModelState.AddModelError("","Request is null.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //now we want to validate that there are only open slots. Admins can override this: But only admins.

            db.Appointments.Add(appointment);
            await db.SaveChangesAsync();

            //now that this is done, we also want to go ahead and create the comments.
            db.Comments.Add(new Comment { AppointmentId = appointment.AppointmentID, CreationDate = DateTime.Now, Creator = User.Identity.Name, Value ="GFASDFSADF" });

            //and once more, save changes.
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = appointment.AppointmentID }, appointment);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AppointmentExists(int id)
        {
            return db.Appointments.Count(e => e.AppointmentID == id) > 0;
        }
    }
}