/*All Dates have this: 
*   this.Date.setMinutes(this.Date.getMinutes() + this.Date.getTimezoneOffset() + 60*20);
*
* because they are 1 day behind (The date in memory is one day behind the desired date)
* and do not get formatted correctly. Seems to be an issue with angular.
* Might get fixed in the future.
*/
function DateFilter() {
    this.StartDate = GetMonday(new Date());
    this.EndDate = new Date();
    this.StartDate.setMinutes(this.StartDate.getMinutes() + 60 * 24);
    this.EndDate.setMinutes(this.EndDate.getMinutes() + 60 * 24);
}