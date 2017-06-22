using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace TechDispatch.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public int TechDispatchRoleId { get; set; }
        public int AuthId { get; set; }
        public string Name { get; set; }
        public List<AccessClaims> MyClaims { get;set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            TechDispatchContext db = new TechDispatchContext();
            userIdentity.AddClaim(new Claim("UserRole", db.TechDispatchRoles.FirstAsync(x => x.TechDispatchRoleId == TechDispatchRoleId).Result.Name));

            //to-do: Custom authorization. For now though, we simply add the authorization codes based on role.
            MyClaims = db.TechDispatchRoles.FirstAsync(x => x.TechDispatchRoleId >= TechDispatchRoleId).Result.AccessClaims;

            MyClaims.ForEach(d =>
                userIdentity.AddClaim(new Claim(d.ClaimName, d.ClaimValue))
            );

            userIdentity.AddClaim(new Claim("Name", Name == null ? Email : Name));
            userIdentity.AddClaim(new Claim("Email", Email));
            userIdentity.AddClaim(new Claim("AuthId", AuthId.ToString()));

            return userIdentity;
        }
    }

    //client-side user info.

    public class ClientUser
    {
        public ClientUser(ApplicationUser user)
        {

        }
    }
}