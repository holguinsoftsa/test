
//Returns a promise for the Production of the Currently Logged in User.
function GetUserProduction($q, $http) {

    return new $q(function (resolve, reject) {

        $http.get("/Api/Users/GetProduction", {}).then(
            function (response) { //Success
                resolve(response.data.Data);

            }, function (response) { //Failed

                alerts.Error(ERROR_MESSAGE);
                reject("");
            }
        );
    });

};

function DateToInputValue(d) {
    return (d.getYear() + 1900) + "-" + (d.getMonth() + 1) + "-" + ("0" + d.getDate()).slice(-2);
}
//Returns the previous monday nearest to the input date.
function GetMonday(d) {
    d = new Date(d);
    var day = d.getDay();
    var diff = d.getDate() - day + (day == 0 ? -6 : 1); // adjust when day is sunday
    return new Date(d.setDate(diff));
}


