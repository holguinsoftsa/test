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
using System.Reflection;
using System.IO;
using Microsoft.AspNet.Identity.EntityFramework;
using ScoutSystem.Models;

namespace ScoutSystem.Areas.Api.Controllers
{
    [Authorize(Roles = "OnSite Manager")]
    public class OnSiteReportController : ApiController
    {

        private ScoutSystemDevEntities db = new ScoutSystemDevEntities();

        [HttpPost]
        public JsonResponse NewReport(NewLocationReport model)
        {
            newReport_FixTimeZones(ref model);

            //Error checks
            if (model.Location == null)
                return new JsonResponse(false, "Please select a location.");
            else if (model.Production == null)
                return new JsonResponse(false, "Please select a production.");
            else if (model.Unit == null)
                return new JsonResponse(false, "Please select a unit.");
            else if (model.Comments == null)
                model.Comments = "None";

            //Create Police External Service cause wtf why is it made this way -.-
            if(db.ExternalService.Where(m => m.ExternalServiceID == 1).Count() == 0)
            {
                db.ExternalService.Add(new ExternalService() { Name = "Police", ExternalServiceID = 1 });
                db.SaveChanges();
            }
            
            //Save Entity
            model.AspUserID = User.Identity.GetUserId();
            var report = model.ToEntity(); //Convert to entity
            report.IncidentReport = model.IncidentsToEntities(); //Convert incident to entity 
            report.LocationReportExternalService = model.ReportExternalServiceToEntities(); //Converts Police to external service

            //To Fix duplicating Incident Categories
            foreach(var incident in report.IncidentReport)
                incident.Category = null;

            db.LocationReport.Add(report);
            db.SaveChanges();

            //Send Email
            report.UserInfo = db.UserInfo.First(m => m.AspUserID == model.AspUserID);
            report.Production = db.Production.First(m => m.ProductionID == report.ProductionID);
            report.Unit = db.Unit.First(m => m.UnitID == report.UnitID);
            report.IncidentReport = db.IncidentReport.Include(m => m.Attachment).Include(m => m.Category).Where(m => m.LocationReportID == report.LocationReportID).ToList();
            sendReportEmail(report, false);

            return new JsonResponse(true);
        }
        [HttpPost]
        public JsonResponse NewLocation(Models.Location model)
        {
            //Save New Contact
            var contact = new Entities.Contact(){
                Name = model.ContactName.ToUpper(),
                Phone = model.ContactPhone.ToUpper()
            };
            db.Contact.Add(contact);
            db.SaveChanges();
            db.Entry(contact).GetDatabaseValues();

            //Save new Location
            var location = new Entities.Location() {
                Name = model.LocationName.ToUpper(),
                Address = model.LocationAddress.ToUpper(),
                ContactID = contact.ContactID
            };
            db.Location.Add(location);
            db.SaveChanges();
            db.Entry(location).GetDatabaseValues();

            return new JsonResponse(true, location.LocationID);
        }
        [HttpGet]
        public JsonResponse GetLocations()
        {
            var list = new List<Models.Location>();

            foreach(var loc in db.Location.ToArray())
                list.Add(new Models.Location(loc));

            return new JsonResponse(true, list);
        }
        [HttpGet]
        public JsonResponse GetUnits()
        {
            var units = new List<ReportUnit>();

            foreach (var item in db.Unit)
                units.Add(new ReportUnit(item));

            return new JsonResponse(true, units.ToArray());
        }
        [HttpGet]
        public JsonResponse GetIncidentCategories()
        {
            var list = new List<IncidentCategory>();
            foreach(var item in db.Category.ToList())
            {
                list.Add(new IncidentCategory(item));
            }
            return new JsonResponse(true, list);
        }
        [HttpGet][HttpPost]
        public JsonResponse GetReportList(Paginator Pages)
        {
            //Get User Info
            var userID = User.Identity.GetUserId();
            var userInfo = db.UserInfo.First(m => m.AspUserID == userID);

            //Get Reports
            var reportSet = db.LocationReport.Include(m => m.Location).Include(m => m.Production).Include(m => m.Unit)
                .Where(m => m.AspUserID == userID)
                .OrderByDescending(m => m.Date);

            var result = new LocationReportList();
            result.Pages = Pages;

            //Sort the list
            var reportList = Pages.GetResult<LocationReport>(reportSet.ToList());
            foreach (var report in reportList)
                result.Reports.Add(new ListItemLocationReport(report));

            return new JsonResponse(true, result);
        }
        [HttpGet]
        public JsonResponse GetReport(int id)
        {
            var item = db.LocationReport.Include(m => m.Location).Include(m => m.Production).Include(m => m.Unit).Include(m => m.IncidentReport.Select(x => x.Attachment))/*.Include("IncidentReport.Attachment")*/
                .First(m => m.LocationReportID == id);
            var item2 = new Models.NewLocationReport(item);

            return new JsonResponse(true, item2);
        }
        [HttpGet]
        public JsonResponse ResendReport(int id)
        {
            var report = db.LocationReport.Include(m => m.UserInfo).Include(m => m.Production).Include(m => m.Unit).Include(m => m.LocationReportExternalService).Include(m => m.IncidentReport.Select(x => x.Attachment))
                .First(m => m.LocationReportID == id);

            sendReportEmail(report, true);

            return new JsonResponse(true, null);
        }


        //Helper Methods
        /// <summary>
        /// Fixes the time zone difference 5hrs. Idk why this has to be an issue
        /// </summary>
        /// <param name="model"></param>
        private void newReport_FixTimeZones(ref NewLocationReport model)
        {
            model.Date = model.Date.AddHours(AppConfig.TimeZoneOffset);

            PropertyInfo[] props = model.GetType().GetProperties();
            var times = props.Where(m => m.PropertyType == typeof(LocationReportTime));
            foreach (var item in times)
            {
                LocationReportTime time = (LocationReportTime)item.GetValue(model);
                time.Start = time.Start.AddHours(AppConfig.TimeZoneOffset);
                time.End = time.End.AddHours(AppConfig.TimeZoneOffset);
                item.SetValue(model, time);
            }

        }
        private void sendReportEmail(LocationReport report, bool isResent)
        {
            var list = new List<System.Net.Mail.Attachment>();

            //Parse images
            foreach(var inc in report.IncidentReport)
            {
                foreach(var img in inc.Attachment)
                {
                    var fileType = img.File.Split('/')[1].Split(';')[0];
                    byte[] imageBytes = Convert.FromBase64String(img.File.Substring(19 + fileType.Length));
                    var atch = new System.Net.Mail.Attachment(new MemoryStream(imageBytes, 0, imageBytes.Length), img.AttachmentID + "." + fileType);
                    list.Add(atch);
                }
            }
            var UserManager = new UserManager<ScoutSystem.Models.ApplicationUser>(new UserStore<ScoutSystem.Models.ApplicationUser>(new ApplicationDbContext()));

            string sender = UserManager.GetEmail(report.AspUserID);
            string body = Email.RenderApiHTML("OnSiteReport", "~/Areas/Api/Views/OnSiteReport/ReportEmail.cshtml", report);
            string subject = Properties.Settings.Default.OnSiteReport_EmailSubject + (isResent ? " (Resent)" : "");

            Email.Send(AppConfig.OnSiteManager_Email, sender, string.Format(subject, report.Production.Name), body, list.ToArray());
        }
    }
}
