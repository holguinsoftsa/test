using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScoutSystem.Areas.Api.Models
{

    public class NewLocationReport
    {
        public int LocationReportID { get; set; }
        public Location Location { get; set; }
        public Production Production { get; set; }
        public ReportUnit Unit { get; set; }
        public string AspUserID { get; set; }
        public string Comments { get; set; }
        public DateTime Date { get; set; }
        public LocationReportTime Site { get; set; }
        public LocationReportTime Crew { get; set; }
        public LocationReportTime Catering { get; set; }
        public LocationReportTime Cleaning { get; set; }
        public LocationReportTime Lunch { get; set; }
        public LocationReportTime Dinner { get; set; }
        public LocationReportTime LastVehicle { get; set; }
        public LocationReportTime[] Police { get; set; }
        public IncidentReport[] Incidents { get; set; }

        /// <summary>
        /// Returns Entity without relations.
        /// </summary>
        /// <returns></returns>
        public Entities.LocationReport ToEntity()
        {
            var item = new Entities.LocationReport()
            {
                LocationReportID = this.LocationReportID,
                LocationID = this.Location.LocationID,
                ProductionID = this.Production.ID,
                UnitID = this.Unit.UnitID,
                AspUserID = this.AspUserID,
                Comments = this.Comments,
                Date = this.Date,
                SiteStart = this.Site.Start.TimeOfDay,
                SiteEnd = this.Site.End.TimeOfDay,
                ProductionStart = this.Crew.Start.TimeOfDay,
                ProductionEnd = this.Crew.End.TimeOfDay,
                CateringArrival = this.Catering.Start.TimeOfDay,
                CateringEnd = this.Catering.End.TimeOfDay,
                CleaningCrewArrival = this.Cleaning.Start.TimeOfDay,
                CleaningCrewEnd = this.Cleaning.End.TimeOfDay,
                ProductionLunch = this.Lunch.Start.TimeOfDay,
                ProductionLunchEnd = this.Lunch.End.TimeOfDay,
                DinnerBreak = this.Dinner.Start.TimeOfDay,
                DinnerBreakEnd = this.Dinner.End.TimeOfDay,
                LastVehicle = this.LastVehicle.Start.TimeOfDay
            };
            return item;
        }
        public ICollection<Entities.IncidentReport> IncidentsToEntities()
        {
            var list = new List<Entities.IncidentReport>();
            foreach (var inc in Incidents)
            {
                list.Add(inc.ToEntity());
            }
            return list;
        }
        public ICollection<Entities.LocationReportExternalService> ReportExternalServiceToEntities()
        {
            var list = new List<Entities.LocationReportExternalService>();
            foreach (var inc in Police)
            {
                list.Add(inc.ToEntity());
            }
            return list;
        }

        public NewLocationReport() { }
        public NewLocationReport(Entities.LocationReport Entity)
        {
            this.LocationReportID = Entity.LocationReportID;
            this.Location = new Location(Entity.Location);
            this.Production = new Production(Entity.Production);
            this.Unit = new ReportUnit(Entity.Unit);
            this.AspUserID = Entity.AspUserID;
            this.Comments = Entity.Comments;
            this.Date = Entity.Date;
            this.Site = new LocationReportTime(Entity.SiteStart, Entity.SiteEnd);
            this.Catering = new LocationReportTime(Entity.CateringArrival, Entity.CateringEnd);
            this.Cleaning = new LocationReportTime(Entity.CleaningCrewArrival, Entity.CleaningCrewEnd);
            this.Crew = new LocationReportTime(Entity.ProductionStart, Entity.ProductionEnd);
            this.Dinner = new LocationReportTime(Entity.DinnerBreak, Entity.DinnerBreakEnd);
            this.LastVehicle = new LocationReportTime(Entity.LastVehicle, Entity.CateringEnd);
            this.Lunch = new LocationReportTime(Entity.ProductionLunch, Entity.ProductionLunchEnd);

            var policeTimes = new List<LocationReportTime>();
            foreach (var item in Entity.LocationReportExternalService)
                policeTimes.Add(new LocationReportTime(item.StartTime, item.EndTime));

            this.Police = policeTimes.ToArray();

            var incidents = new List<IncidentReport>();
            foreach(var item in Entity.IncidentReport)
                incidents.Add(new IncidentReport(item));
            this.Incidents = incidents.ToArray();
        }
    }
    public class LocationReportTime
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        /// <summary>
        /// Returns a location report external service, with ID set to 0.
        /// </summary>
        /// <returns></returns>
        public Entities.LocationReportExternalService ToEntity()
        {
            var item = new Entities.LocationReportExternalService() {
                StartTime = this.Start.TimeOfDay,
                EndTime = this.End.TimeOfDay,
                ExternalServiceID = 1
            };

            return item;
        }
        public LocationReportTime() { }
        public LocationReportTime(TimeSpan Start, TimeSpan End)
        {
            this.Start = new DateTime().Add(Start);
            this.End = new DateTime().Add(End);
        }
    }
    public class IncidentReport
    {
        public IncidentCategory Category { get; set; }
        public DateTime Date { get; set; }
        public string[] Images { get; set; }
        public string Notes { get; set; }

        public IncidentReport() { }
        public IncidentReport(Entities.IncidentReport Entity)
        {
            this.Category = new IncidentCategory(Entity.Category);
            this.Date = Entity.Date;
            this.Images = Entity.Attachment.Select(m => m.File).ToArray();
            this.Notes = Entity.Notes;
        }
        public Entities.IncidentReport ToEntity()
        {
            var attachments = new List<Entities.Attachment>();
            foreach (var img in Images)
            {
                attachments.Add(new Entities.Attachment() { File = img });
            }
            var item = new Entities.IncidentReport() {
                CategoryID = this.Category.CategoryID,
                Category = new Entities.Category() {
                    CategoryID = this.Category.CategoryID,
                    Name = this.Category.Name},
                Date = this.Date,
                DateCreated = DateTime.Now,
                Notes = Notes,
                Attachment = attachments
            };

            return item;
        }

    }
    public class IncidentCategory
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }

        public IncidentCategory(Entities.Category Item)
        {
            this.CategoryID = Item.CategoryID;
            this.Name = Item.Name;
        }
        public IncidentCategory() { }

        public IncidentCategory(int ID, string Name)
        {
            this.CategoryID = ID;
            this.Name = Name;
        }

    }
    public class ReportUnit
    {
        public short UnitID { get; set; }
        public string Name { get; set; }

        public ReportUnit() { }
        public ReportUnit(Entities.Unit Unit)
        {
            this.UnitID = Unit.UnitID;
            this.Name = Unit.Name;
        }


    }
    public class Location
    {
        public int LocationID { get; set; }
        public string LocationName { get; set; }
        public string LocationAddress { get; set; }
        public int ContactID { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }

        /// <summary>
        /// Must include Contact Entity from Entity Framework.
        /// </summary>
        /// <param name="Ent"></param>
        public Location(Entities.Location Ent)
        {
            LocationID = Ent.LocationID;
            LocationName = Ent.Name;
            LocationAddress = Ent.Address;
            ContactID = Ent.ContactID;
            ContactName = Ent.Contact.Name;
            ContactPhone = Ent.Contact.Phone;
        }
        public Location() { }
    }
    public class ListItemLocationReport
    {
        public int LocationReportID { get; set; }
        public string Date { get; set; }
        public Location Location { get; set; }
        public Production Production { get; set; }
        public ReportUnit Unit { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="Report">The location report entity including dependencies: Location, Production and Unit.</param>
        public ListItemLocationReport(Entities.LocationReport Report)
        {
            this.LocationReportID = Report.LocationReportID;
            this.Date = Report.Date.ToString("ddd, MM/dd/yy");
            this.Location = new Location(Report.Location);
            this.Production = new Production(Report.Production);
            this.Unit = new ReportUnit(Report.Unit);
        }

    }
    public class LocationReportList
    {
        public List<ListItemLocationReport> Reports { get; set; }
        public Paginator Pages { get; set; }

        public LocationReportList()
        {
            Reports = new List<ListItemLocationReport>();
            Pages = new Paginator();
        }
    }
}