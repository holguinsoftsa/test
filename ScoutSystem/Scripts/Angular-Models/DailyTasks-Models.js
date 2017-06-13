/*All Dates have this: 
*   this.Date.setMinutes(this.Date.getMinutes() + this.Date.getTimezoneOffset() + 60*20);
*
* because they are 1 day behind (The date in memory is one day behind the desired date)
* and do not get formatted correctly. Seems to be an issue with angular.
* Might get fixed in the future.
*/
function NewTask() {
    //Parse max date
    this.Date = new Date();
    this.Date.setMinutes(this.Date.getMinutes() + this.Date.getTimezoneOffset() + 60 * 20);
    this.Date.setDate(this.Date.getDate());

    this.StartTime = new Date(1970, 0, 1, 05, 0, 0);
    this.EndTime = new Date(1970, 0, 1, 17, 0, 0);
    this.CarNumber = 0;
    this.DailyTask = "";
    this.Visited = 1;
    this.Options = 1;
    this.IsDraft = false;

    //For Local Use
    var dateMax = new Date();
    dateMax.setMinutes(dateMax.getMinutes() + dateMax.getTimezoneOffset() + 60 * 20);
    dateMax.setDate(dateMax.getDate());
    this.DateMax = DateToInputValue(dateMax);
    this.IsVisited = false;
    this.IsOptions = false;
}
