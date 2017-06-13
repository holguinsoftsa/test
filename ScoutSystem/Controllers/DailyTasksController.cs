using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using ScoutSystem.Entities;
using ScoutSystem.Models;
using System.Globalization;
using Newtonsoft.Json;
using ScoutSystem.Areas.Api.Models;

namespace ScoutSystem.Controllers
{
    [Authorize(Roles = "Scout")]
    public class DailyTasksController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }

}