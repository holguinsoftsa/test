using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ScoutSystem.Controllers
{
    public class ScoutReportController : Controller
    {
        // GET: ScoutReport
        public ActionResult Index()
        {
            if (User.IsInRole(UserRole.Scout.ToString()))
                return View(UserRole.Scout.ToString());
            else if (User.IsInRole(UserRole.Manager.ToString()) || User.IsInRole(UserRole.Admin.ToString()))
                return View(UserRole.Manager.ToString());

            return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
        }
    }
}