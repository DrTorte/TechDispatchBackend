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
    [RoutePrefix("api/Zones")]
    public class InstallZonesController : ApiController
    {
        private TechDispatchContext db = new TechDispatchContext();

        // GET: api/InstallZones
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetInstallZones()
        {
            return Ok(db.InstallZones);
        }

        // GET: api/InstallZones/5
        [ResponseType(typeof(InstallZone))]
        public async Task<IHttpActionResult> GetInstallZone(int id)
        {
            InstallZone installZone = await db.InstallZones.FindAsync(id);
            if (installZone == null)
            {
                return NotFound();
            }

            return Ok(installZone);
        }

        // PUT: api/InstallZones/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutInstallZone(int id, InstallZone installZone)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != installZone.InstallZoneId)
            {
                return BadRequest();
            }

            db.Entry(installZone).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InstallZoneExists(id))
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

        // POST: api/InstallZones
        [ResponseType(typeof(InstallZone))]
        public async Task<IHttpActionResult> PostInstallZone(InstallZone installZone)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.InstallZones.Add(installZone);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = installZone.InstallZoneId }, installZone);
        }

        // DELETE: api/InstallZones/5
        [ResponseType(typeof(InstallZone))]
        public async Task<IHttpActionResult> DeleteInstallZone(int id)
        {
            InstallZone installZone = await db.InstallZones.FindAsync(id);
            if (installZone == null)
            {
                return NotFound();
            }

            db.InstallZones.Remove(installZone);
            await db.SaveChangesAsync();

            return Ok(installZone);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool InstallZoneExists(int id)
        {
            return db.InstallZones.Count(e => e.InstallZoneId == id) > 0;
        }
    }
}