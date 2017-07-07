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
    [RoutePrefix("api/Customers")]
    public class CustomersController : ApiController
    {
        private TechDispatchContext db = new TechDispatchContext();

        // GET: api/Customers
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetCustomers(string search = "", bool active = true, bool inactive = false)
        {
            search = search.ToLower();

            List<Customer> Customers = db.Customers.Where(x=>x.Name.ToLower().Contains(search) || x.PhoneNumber.ToLower().Contains(search) || x.Address.ToLower().Contains(search))
                .Where(x=>(inactive && active) || (active && x.CustomerState == Customer.CustomerStatus.Active) || (inactive && x.CustomerState != Customer.CustomerStatus.Active))
                .ToList();

            List<Task> tasks = new List<Task>();
            List<CustomerListJson> cxList = new List<CustomerListJson>();

            using (var dbb = db){
                await dbb.Database.Connection.OpenAsync();
                foreach (var c in Customers)
                {
                    tasks.Add(Task.Factory.StartNew(() => { cxList.Add(new CustomerListJson(c)); }));
                }
            }
            Task.WaitAll(tasks.ToArray());
            return Ok(cxList);
        }

        // GET: api/Customers/5
        [Route("{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetCustomer(int id)
        {
            Customer customer = await db.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CustomerExists(int id)
        {
            return db.Customers.Count(e => e.CustomerID == id) > 0;
        }
    }
}