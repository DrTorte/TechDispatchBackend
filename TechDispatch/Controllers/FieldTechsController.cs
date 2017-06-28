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
    [RoutePrefix("api/FieldTechs")]
    public class FieldTechsController : ApiController
    {
        private TechDispatchContext db = new TechDispatchContext();

        // GET: api/FieldTechs
        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> GetFieldTechs()
        {
            var fieldTechs = db.FieldTechs.Include("User").ToList();

            List<FieldTechView> fieldTechsView = new List<FieldTechView>();
            List<Task> tasks = new List<Task>();
            //in case the list becomes huge, of course.
            foreach (var f in fieldTechs)
            {
                var user = db.Users.FirstOrDefault(x => x.Id == f.UserID);
                tasks.Add(Task.Factory.StartNew(() => { fieldTechsView.Add(new FieldTechView(f)); }));
            }

            Task.WaitAll(tasks.ToArray());

            return Ok(fieldTechsView);
        }

        // GET: api/FieldTeches/5
        [Route("{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetFieldTech(int id)
        {
            FieldTech fieldTech = db.FieldTechs.Include("User").FirstOrDefault(x=>x.FieldTechID == id);
            if (fieldTech == null)
            {
                return NotFound();
            }
            return Ok(new FieldTechView(fieldTech));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FieldTechExists(int id)
        {
            return db.FieldTechs.Count(e => e.FieldTechID == id) > 0;
        }
    }
}