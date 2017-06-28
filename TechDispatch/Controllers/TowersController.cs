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
    [RoutePrefix("api/Towers")]
    public class TowersController : ApiController
    {
        private TechDispatchContext db = new TechDispatchContext();

        // GET: api/FieldTechs
        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> GetTowers(bool includeIP = false)
        {
            var towers = await db.Towers.Include("APs").Include("IPs").Include("InstallZone").ToListAsync();

            List<TowerView> towersView = new List<TowerView>();
            List<Task> tasks = new List<Task>();
            //in case the list becomes huge, of course.
            using (var dbb = db)
            {
                dbb.Database.Connection.Open();
                foreach (var t in towers)
                {
                    tasks.Add(Task.Factory.StartNew(() => { towersView.Add(new TowerView(t, includeIP)); }));
                }
            }
            Task.WaitAll(tasks.ToArray());

            return Ok(towersView);
        }

        // GET: api/FieldTeches/5
        [Route("{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetTower(int id, bool includeIP = false)
        {
            var tower = await db.Towers.FirstOrDefaultAsync(x=> x.TowerID == id);

            if (tower == null)
            {
                return NotFound();
            }

            return Ok(new TowerView(tower, includeIP));
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