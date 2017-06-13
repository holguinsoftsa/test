app.controller('ScoutReport-ScoutController', function ($scope, $http, $q, $timeout, loader, alerts) {
    //User prod
    GetUserProduction($q, $http, loader).then(function (result) {
        $scope.userProd = result;
    });

    //Get List Data
    var updateList = function () {

        loader.Start();
        $http.get("/Api/ScoutReport/GetListData", {
            params: {
                Start: $scope.dateSearch.StartDate,
                End: $scope.dateSearch.EndDate
            }
        }).then(
            function (response) { //Success
                $scope.list = response.data.Data;

            }, function (response) { //Failed
                alerts.Error(ERROR_MESSAGE);

            }
        ).finally(function () { //finally
            loader.Stop();
        });
    };
    $timeout(updateList, 500); //Done on delay because datepicker not init yet

    //Date Search
    $scope.dateSearch = new DateFilter();
    $scope.OnClickFilter = function () {
        updateList();
    };

    //Task List
    $scope.selectedRow = null;
    $scope.selectedTask = null;

    //Task Data Panel
    function onCloseDataPanel()
    {
        $scope.selectedRow = null;
        return true;
    }
    $scope.dataPanel = new Panel(null, onCloseDataPanel);
    $scope.ClickListItem = function (obj, index) {
        loader.Start();
        //Get Task Info
        $http.get("/Api/ScoutReport/GetTask", { params: { EntryID: $scope.list[index].EntryID } }).then(function (response) { //Post success
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

        }).finally(function () {
            loader.Stop();
        });
    };
    $scope.ResendReport = function () {
        loader.Start();

        //make request
        $http.get("/Api/ScoutReport/ResendReport", { params: { Id: $scope.selectedTask.EntryID } }).then(
            function (response) { //Post success
                if (response.data.Success) {
                    $scope.dataPanel.Close();
                    alerts.Success("Report successfully resent.");
                } else {
                    alerts.Success(response.data.Message);
                }
            }, function (response) {//post failed
                alerts.Error(ERROR_MESSAGE);
            }).finally(function () { //Finally
                loader.Stop();
            });;
    };

    //Draft Panel
    $scope.draftList = null;
    var getDrafts = function () {
        loader.Start();

        $http.get("/Api/ScoutReport/GetDraftList", {
            params: {}
        }).then(
            function (response) { //Success
                $scope.draftList = response.data.Data;

            }, function (response) { //Failed
                alerts.Error(ERROR_MESSAGE);
            }
        ).finally(function () { //Finally
            loader.Stop();
        });
    };
    $scope.draftPanel = new Panel(null, null);
    getDrafts();
    $scope.ClickDraftItem = function (obj, index) {
        loader.Start();
        //Get Task Info
        $http.get("/Api/ScoutReport/GetDraft", { params: { EntryID: $scope.draftList[index].EntryID } }).then(function (response) { //Post success
            if (response.data.Success) {

                //Set task
                $scope.newTask = response.data.Data;
                $scope.newTask.IsDraft = false;
                $scope.newTaskPanel.Open();
                getDrafts();
            } else {
                alerts.Error(ERROR_MESSAGE);
            }
        }, function (response) {//post failed
            alerts.Error(ERROR_MESSAGE);
        }).finally(function () { //finally
            loader.Stop();
        });
    };


    //New Task
    function onCloseTaskPanel() {
        var close = confirm("Are you sure you want to cancel your report? All fields will be deleted.");
        if (close) {
            $scope.ClearNewTask();
        }

        return close;
    };
    $scope.newTaskPanel = new Panel(null, onCloseTaskPanel);
    $scope.newTask = new NewTask();
    $scope.ClearNewTask = function () {
        $scope.newTask = new NewTask();
    };
    $scope.NewTaskSubmit = function (isDraft) //Handles posting submit to server
    {
        var confirmation = false;

        //Make prompt
        if (!isDraft)
            confirmation = confirm("Are you sure you want to submit?")
        else 
            confirmation = confirm("Are you sure you want to save this draft?")

        //If Task is empty, prompt user
        if ($scope.newTask.DailyTask.replace(/\s/g, '') == ""){
            alert("The task field can not be empty.");
        }
        else if (confirmation) {
            loader.Start();
            
            //set date if blank or if date = today for somereason is comes as undefined (WORK AROUND T_T)
            if ($scope.newTask.Date == undefined)
                $scope.newTask.Date = new Date();

            //make request
            $http.post("/Api/ScoutReport/NewEntry", $scope.newTask, []).then(
                function (response) { //Post success
                    if (response.data.Success) {
                        updateList();
                        getDrafts();
                        $scope.newTaskPanel.Toggle(); //Bypassing Onclose event
                        $scope.ClearNewTask();

                        if (isDraft)
                            alerts.Success("Report has been saved as a draft.");
                        else
                            alerts.Success("Report successfully submitted.");
                    } else {
                        alerts.Error(response.data.Message);
                    }
            }, function (response) {//post failed
                   alerts(ERROR_MESSAGE);
            }).finally(function () { //finally
                   loader.Stop();
            });
        }//end else if
    };
    $scope.DraftSubmit = function () {
        $scope.newTask.IsDraft = true;
        $scope.NewTaskSubmit(true);
    };

});