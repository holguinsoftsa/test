using ScoutSystem.Entities;
using ScoutSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ScoutSystem.Areas.Api.Models;

namespace ScoutSystem.Controllers
{
    public class ProductionController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}