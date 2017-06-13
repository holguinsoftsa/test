app.controller('ScoutReport-ManagerController', function ($scope, $http, $timeout, loader, alerts) {

    //Date Search
    $scope.dateSearch = new DateFilter();
    $scope.OnClickFilter = function () {
        updateList();
    };

    //Get List Data
    $scope.list = [];
    var updateList = function () {

        loader.Start();
        $http.get("/Api/ScoutReportManager/GetListData", {
            params: {
                Start: $scope.dateSearch.StartDate,
                End: $scope.dateSearch.EndDate
            }
        }).then(
            function (response) { //Success
                $scope.list = response.data.Data;
                loader.Stop();

            }, function (response) { //Failed
                alerts.Error(ERROR_MESSAGE);
                loader.Stop();
            }
        );
    };
    $timeout(updateList, 500); //Done on delay because datepicker not init yet

    $scope.LocationVisSum = function () {
        var sum = 0;
        $scope.listResults.forEach(function (item) {
            sum += item.LocationsVisited;
        });
        return sum;
    };

    //Task Data Panel
    $scope.dataPanel = new Panel(null, onCloseDataPanel);
    function onCloseDataPanel () {
        $scope.selectedRow = null;
        return true;
    };
    $scope.ClickListItem = function (obj, index) {
        //Get Task Info
        $http.get("/Api/ScoutReportManager/GetTask", { params: { EntryID: $scope.list[index].EntryID } }).then(
            function (response) { //Post success
                if (response.data.Success) {
                    //Highlights selected row
                    $scope.selectedRow = index;

                    //Set task
                    $scope.selectedTask = response.data.Data;

                    //opens data panel
                    $scope.dataPanel.Open();
                } else {
                    alerts.Error(response.data.Message);
                }
        }, function (response) {//post failed
            alerts.Error(ERROR_MESSAGE);
        });
    };
});