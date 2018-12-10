using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


using AqarApp.Models;


namespace AqarApp.Security
{
    public class CustomAuthorizeAttripute:AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext) {
            if (string.IsNullOrEmpty(SessionPersister.Username))
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new {  controller="Admin" ,action ="Login"}));
            else
            {
                AccountModel am = new AccountModel();
                CustomPrinciple CustomPrinciple = new CustomPrinciple(am.find(SessionPersister.Username));
                if (!CustomPrinciple.IsInRole(Roles))
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "AccessDenied", action = "Index" }));
                }
            }
        }
    }
}