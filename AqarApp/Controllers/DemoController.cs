using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


using AqarApp.Security;
using AqarApp.Models;

namespace AqarApp.Controllers
{
    public class DemoController : Controller
    {
        //
        // GET: /Demo/
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [CustomAuthorizeAttripute(Roles = "superadmin")]
        public ActionResult Work1()
        {
            return View("Work1");
        }

        [CustomAuthorizeAttripute(Roles = "admin")]
        public ActionResult Work2()
        {
            return View("Work2");
        }


        [CustomAuthorizeAttripute(Roles  = "employee")]
        public ActionResult Work3()
        {
            return View("Work3");
        }
	}
}