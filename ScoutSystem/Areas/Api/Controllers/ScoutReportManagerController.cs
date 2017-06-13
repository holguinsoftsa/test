using Microsoft.AspNet.Identity;
using ScoutSystem.Entities;
using ScoutSystem.Areas.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity.EntityFramework;
using ScoutSystem.Models;

namespace ScoutSystem.Areas.Api.Controllers
{
    public class ScoutReportManagerController : ApiController
    {
        private ScoutSystemDevEntities db = new ScoutSystemDevEntities();
        private ApplicationDbContext context = new ApplicationDbContext();

        [HttpGet]
        public JsonResponse GetListData(DateTime Start, DateTime End)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (Start == null)
                Start = new DateTime(DateTime.Now.Year, 1, 1);
            if (End == null)
                End = DateTime.Now;

            var userID = User.Identity.GetUserId();
            var userInfo = db.UserInfo.Find(userID);
            var data = db.ScoutDailyReport.Include(m => m.Production).Where(m =>
                   m.Date >= Start
                && m.Date <= End
                && m.IsDraft == false)
                .OrderByDescending(m => m.Date);
            var list = new List<EntryListItem>();

            foreach (var item in data)
            {
                string userEmail = UserManager.FindById(item.UserID).Email;
                list.Add(new EntryListItem(item, userEmail));
            }

            return new JsonResponse(true, list);
        }
        [HttpGet]
        public JsonResponse GetTask(int EntryID)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var entry = db.ScoutDailyReport.First(m => m.EntryID == EntryID);
            string email = UserManager.FindById(entry.UserID).Email;
            return new JsonResponse(true, new EntryData(entry, email));
        }
    }
}
