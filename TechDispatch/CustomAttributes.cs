using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using TechDispatch.Models;

namespace TechDispatch.Attributes
{
    public class AccessAuthentication : ActionFilterAttribute
    {
        string Type;
        string Name; // an array of all approved accesses.

        public AccessAuthentication(string type, string name)
        {
            Type = type;
            Name = name;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            //get the user.
            var user = HttpContext.Current.User.Identity as ClaimsIdentity;

            if (user != null) //only bother checking if we've got a user.
            {
                if (user.HasClaim(Type, Name))
                {
                    return;
                }
            }

            //log failed access code here.
            actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
        }
    }

    public class TokenAuthorize : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);
            //do not continue if there's already a response.
            if (actionContext.Response == null)
            {
                //add our DB here.
                TechDispatchContext db = new TechDispatchContext();
                ClaimsIdentity user = HttpContext.Current.User.Identity as ClaimsIdentity;
                //find a user that matches both ID and auth ID. Otherwise, fail.
                //grab the string that holds the current auth ID so we can compare it.
                Claim _authId = user.Claims.DefaultIfEmpty(null).FirstOrDefault(x => x.Type == "AuthId");
                int AuthId;

                if (_authId == null || !Int32.TryParse(_authId.Value, out AuthId))
                {
                    //missing a value here, refuse access.
                    actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
                    return;
                }
                string Email = user.Claims.FirstOrDefault(x => x.Type == "Email").Value;

                ApplicationUser target = db.Users.DefaultIfEmpty(null).FirstOrDefault(x => x.Email == Email && x.AuthId == AuthId);
                if (target == null) 
                {
                    //refuse access.
                    actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
                }

            }

        }
    }
}