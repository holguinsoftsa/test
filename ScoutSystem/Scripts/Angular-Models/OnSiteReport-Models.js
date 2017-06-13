
function OnSiteReport() {
    this.Location = "";
    this.Production = "";
    this.Unit = "";
    this.Date = new Date();
    this.Site = new ReportTime();
    this.Crew = new ReportTime();
    this.Lunch = new ReportTime();
    this.Dinner = new ReportTime();
    this.LastVehicle = new ReportTime();
    this.Cleaning = new ReportTime();
    this.Catering = new ReportTime();
    this.Police = [];
    this.Incidents = [];
}
function ReportTime() {
    this.Start = new Date(1970, 0, 1, 0, 0, 0);
    this.End = new Date(1970, 0, 1, 0, 0, 0);
}
function IncidentReport() {
    this.Category = null;
    this.Date = new Date();
    this.Notes = "";
    this.Images = [];
}
function Location() {
    this.LocationName = "";
    this.LocationAddress = "";
    this.ContactName = "";
    this.ContactPhone = "";
    this.LocationID = 0;
    this.ContactID = 0;
}