using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ScoutSystem.Controllers
{
    [Authorize()]
    public class OnSiteReportController : Controller
    {
        public ActionResult Index()
        {
            if(this.User.IsInRole(UserRole.OnSiteManager.ToString()))
                return View(UserRole.OnSiteManager.ToString());

            return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
        }
    }
}