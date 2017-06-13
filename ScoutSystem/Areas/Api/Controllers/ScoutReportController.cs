using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Data.Entity;
using ScoutSystem.Areas.Api.Models;
using ScoutSystem.Entities;
using Microsoft.AspNet.Identity;

namespace ScoutSystem.Areas.Api.Controllers
{
    [Authorize(Roles = "Scout")]
    public class ScoutReportController : ApiController
    {
        private ScoutSystemDevEntities db = new ScoutSystemDevEntities();

        [HttpPost]
        public JsonResponse NewEntry(NewEntry Entry)
        {
            //This is a work around because Angular does not post the date when date = today T_T SMH WHY!
            if (Entry.Date == null)
                Entry.Date = DateTime.Now;

            //Workaround because angular posts times differently then displayed in browser. WHY?? TimeZone issue maybe
            Entry.StartTime = Entry.StartTime.AddHours(AppConfig.TimeZoneOffset);
            Entry.EndTime = Entry.EndTime.AddHours(AppConfig.TimeZoneOffset);

            //Parse Entry to Entity
            var user = User.Identity;
            var userInfo = db.UserInfo.Find(user.GetUserId());
            var entry = Entry.ToEntity();
            entry.ProductionID = userInfo.ProductionID;
            entry.UserID = user.GetUserId();

            //If entry already exists for this day and it's not a Draft
            if (db.ScoutDailyReport.Where(m => m.Date == entry.Date && m.ProductionID == userInfo.ProductionID && m.UserID == entry.UserID).Count() > 0 && !entry.IsDraft)
                return new JsonResponse(false, "Report already exists for " + Entry.Date.ToString("d") + ".");
           
            //Save to DB
            db.ScoutDailyReport.Add(entry);
            db.SaveChanges();

            //Get entry from DB (DB Validation)
            if (!entry.IsDraft)
            {
                var emailEntry = db.ScoutDailyReport.Include("Production").Where(m => m.Date == entry.Date && m.UserID == entry.UserID).FirstOrDefault();
                sendReportEmail(new ScoutReportEmail(user.GetUserName(), emailEntry), emailEntry.Production.Name + " - Daily Scout Report");
            }

            return new JsonResponse(true);
        }

        [HttpGet]
        public JsonResponse GetListData()
        {
            var Start = new DateTime(DateTime.Now.Year, 1, 1);
            var End = DateTime.Now;

            return this.GetListData(Start, End);
        }

        [HttpGet]
        public JsonResponse GetListData(DateTime Start, DateTime End)
        {
            var userID = User.Identity.GetUserId();
            var userInfo = db.UserInfo.Find(userID);
            var data = db.ScoutDailyReport.Include(m => m.Production).Where(m => m.Date >= Start && m.Date <= End
                && m.IsDraft == false
                && m.UserID == userID
                && m.ProductionID == userInfo.ProductionID)
                .OrderByDescending(m => m.Date);
            var list = new List<EntryListItem>();

            foreach (var item in data)
            {
                list.Add(new EntryListItem(item));
            }

            return new JsonResponse(true, list);
        }

        [HttpGet]
        public JsonResponse GetDraft(int EntryID)
        {
            var userID = User.Identity.GetUserId();
            var item = db.ScoutDailyReport.First(m => m.EntryID == EntryID && m.UserID == userID && m.IsDraft == true);
            db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return new JsonResponse(true, new NewEntry(item));

        }

        [HttpGet]
        public JsonResponse GetTask(int EntryID)
        {
            var userID = User.Identity.GetUserId();
            var item = db.ScoutDailyReport.First(m => m.EntryID == EntryID && m.UserID == userID);
            return new JsonResponse(true, new EntryData(item));
        }

        [HttpGet]
        public JsonResponse GetDraftList()
        {
            var userID = User.Identity.GetUserId();
            var data = db.ScoutDailyReport.Where(m => m.IsDraft == true && m.UserID == userID).OrderByDescending(m => m.Date);
            var list = new List<EntryListItem>();

            foreach (var item in data)
            {
                list.Add(new EntryListItem(item));
            }

            return new JsonResponse(true, list.ToArray());
        }
        [HttpGet] [HttpPost]
        public JsonResponse ResendReport(int Id)
        {
            try
            {
                var userID = User.Identity.GetUserId();
                var emailEntry = db.ScoutDailyReport.Include("Production").Where(m => m.EntryID == Id && m.UserID == userID).FirstOrDefault();
                sendReportEmail(new ScoutReportEmail(User.Identity.GetUserName(), emailEntry), emailEntry.Production.Name + " - Daily Scout Report (RESEND)");
                return new JsonResponse(true);
            }
            catch (Exception ex)
            {
                return new JsonResponse(true, "Failed to resend email. " + ex.Message + "\n " + ex.StackTrace);
            }
        }



        void sendReportEmail(ScoutReportEmail model, string subject)
        {
            string body = Email.RenderApiHTML("ScoutReport", "~/Areas/Api/Views/ScoutReport/ReportEmail.cshtml", model);
            Email.Send(Properties.Settings.Default.ScoutReport_DistributionList, model.Sender, subject, body);
        }
    }
}
