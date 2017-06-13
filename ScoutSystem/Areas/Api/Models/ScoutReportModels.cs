using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScoutSystem.Areas.Api.Models
{

    public class EntryListItem
    {
        public int EntryID { get; set; }
        public string Date { get; set; }
        public string DateDayOfWeek { get; set; }
        public string Production { get; set; }
        public string SenderEmail { get; set; }
        public int LocationsVisited { get; set; }
        public int LocationOptions { get; set; }

        public EntryListItem(Entities.ScoutDailyReport Entry)
        {
            this.EntryID = Entry.EntryID;
            this.Date = Entry.Date.ToString("d");
            this.DateDayOfWeek = Entry.Date.ToString("dddd");
            this.Production = Entry.Production.Name;
            this.LocationsVisited = Entry.LocationsVisited;
            this.LocationOptions = Entry.LocationOptions;
        }

        public EntryListItem(Entities.ScoutDailyReport Entry, string SenderEmail)
        {
            this.EntryID = Entry.EntryID;
            this.Date = Entry.Date.ToString("d");
            this.DateDayOfWeek = Entry.Date.ToString("dddd");
            this.Production = Entry.Production.Name;
            this.LocationsVisited = Entry.LocationsVisited;
            this.LocationOptions = Entry.LocationOptions;
            this.SenderEmail = SenderEmail;
        }
    }
    public class EntryData
    {
        const string DATEFORMAT = "MM/dd/yyyy";

        public int EntryID { get; set; }

        public string Date { get; set; }
        public string DateCreated { get; set; }
        public string DailyTask { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int CarNumber { get; set; }
        public int Visited { get; set; }
        public int Options { get; set; }
        public bool IsDraft { get; set; }
        public bool IsVisited { get; set; }
        public bool IsOptions { get; set; }
        public string SenderEmail { get; set; }

        public EntryData(Entities.ScoutDailyReport Entry)
        {
            this.EntryID = Entry.EntryID;
            this.Date = Entry.Date.ToString(DATEFORMAT);
            this.DateCreated = Entry.DateCreated.ToString(DATEFORMAT);
            this.DailyTask = Entry.Task;
            this.StartTime = Entry.StartTime.ToString("t");
            this.EndTime = Entry.EndTime.ToString("t");
            this.IsDraft = Entry.IsDraft;
            this.CarNumber = Entry.CarNumber;
            this.Visited = Entry.LocationsVisited;
            this.Options = Entry.LocationOptions;
        }
        public EntryData(Entities.ScoutDailyReport Entry, string SenderEmail)
        {
            this.EntryID = Entry.EntryID;
            this.Date = Entry.Date.ToString(DATEFORMAT);
            this.DateCreated = Entry.DateCreated.ToString(DATEFORMAT);
            this.DailyTask = Entry.Task;
            this.StartTime = Entry.StartTime.ToString("t");
            this.EndTime = Entry.EndTime.ToString("t");
            this.IsDraft = Entry.IsDraft;
            this.CarNumber = Entry.CarNumber;
            this.Visited = Entry.LocationsVisited;
            this.Options = Entry.LocationOptions;
            this.SenderEmail = SenderEmail;
        }


    }

    public class DraftListEntry
    {
        const int SUBJECTLENGTH = 50;
        const string DATEFORMAT = "dddd, MM/dd/yy";

        public string Date { get; set; }
        public string Task { get; set; }

        public DraftListEntry(Entities.ScoutDailyReport Entry)
        {
            this.Date = Entry.Date.ToString(DATEFORMAT);
            this.Task = Entry.Task.Substring(0, SUBJECTLENGTH);
        }
    }
    public class NewEntry
    {
        public DateTime Date { get; set; }
        public string DailyTask { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int CarNumber { get; set; }
        public int Visited { get; set; }
        public int Options { get; set; }
        public bool IsDraft { get; set; }
        public bool IsVisited { get; set; }
        public bool IsOptions { get; set; }

        public NewEntry() { }

        public NewEntry(Entities.ScoutDailyReport Entry)
        {
            this.Date = Entry.Date;
            this.DailyTask = Entry.Task;
            this.StartTime = Entry.StartTime;
            this.EndTime = Entry.EndTime;
            this.CarNumber = Entry.CarNumber;
            this.Visited = Entry.LocationsVisited;
            this.Options = Entry.LocationOptions;
            this.IsDraft = Entry.IsDraft;
            this.IsVisited = (Entry.LocationsVisited > 0);
            this.IsOptions = (Entry.LocationOptions > 0);
        }
        public Entities.ScoutDailyReport ToEntity()
        {
            var entry = new Entities.ScoutDailyReport();
            entry.Date = new DateTime(this.Date.Year, this.Date.Month, this.Date.Day);
            entry.Task = this.DailyTask;
            entry.StartTime = this.StartTime;
            entry.EndTime = this.EndTime;
            entry.IsDraft = this.IsDraft;
            entry.CarNumber = this.CarNumber;

            if (this.IsVisited)
                entry.LocationsVisited = this.Visited;
            if (this.IsOptions)
                entry.LocationOptions = this.Options;

            return entry;
        }
    }


    public class ScoutReportEmail
    {
        public string Sender { get; set; }
        public Entities.ScoutDailyReport Entry { get; set; }

        public ScoutReportEmail(string Sender, Entities.ScoutDailyReport Entry)
        {
            this.Sender = Sender;
            this.Entry = Entry;
        }
    }

}