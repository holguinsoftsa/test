using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;

namespace ScoutSystem.Controllers
{
    [Authorize]
    public class LocationAgreementController : Controller
    {
        public ActionResult Index()
        {
            if (User.IsInRole(UserRole.Scout.ToString()))
                return View(UserRole.Scout.ToString());

            else if (User.IsInRole(UserRole.Admin.ToString()))
                return View(UserRole.Admin.ToString());

            //else if (User.IsInRole(UserRole.Manager.ToString()))
            //    return View(UserRole.Manager.ToString());

            else if (User.IsInRole(UserRole.BusinessAffair.ToString()))
                return View(UserRole.BusinessAffair.ToString());

            else if (User.IsInRole(UserRole.Executive.ToString()))
                return View(UserRole.Executive.ToString());

            return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
        }

        [AllowAnonymous]
        public ActionResult Owner()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult LocationManager()
        {
            return View("Location Manager");
        }



    }
}