app.controller('OnSiteReport-OnSiteManagerController', function ($scope, $http, $window, loader, alerts) {

    //Get reports
    $scope.pages = new Paginator();
    $scope.reports = [];
    $scope.getReports = function () {

        loader.Start();
        $http.post("/Api/OnSiteReport/GetReportList", $scope.pages, []).then(
            function (response) { //Success
                $scope.reports = response.data.Data.Reports;
                $scope.pages = response.data.Data.Pages;
            }, function (response) { //Failed
                alerts.Error(ERROR_MESSAGE);
            })
        .finally(function () {
            loader.Stop();
        });
    };
    $scope.getReports();

    //New Report Panel
    $scope.newReport = new OnSiteReport();
    $scope.newReportPanel = new Panel(null, onClose);
    function onClose() {
        var conf = confirm("Are you sure you want to cancel? This will delete all information.");

        if (conf)
            $scope.newReport = new OnSiteReport();

        return conf;
    };
    $scope.SubmitReport = function () {

        loader.Start();
        $http.post("/Api/OnSiteReport/NewReport", $scope.newReport, []).then(
              function (response) { //Post success
                  if (response.data.Success) {

                      if ($scope.newReport.IsDraft)
                          deleteDraft($scope.newReport);

                      alerts.Success("Report successfully submitted.");
                      $scope.newReport = new OnSiteReport();
                      $scope.getReports();
                      $scope.newReportPanel.Toggle();

                  } else {
                      alerts.Error(response.data.Message);
                  }
              },
              function (response) {//post failed
                  alerts.Error(ERROR_MESSAGE);
              }
        ).finally(function () {
            loader.Stop();
        });
    };


    //Drafts
    $scope.reportDrafts = [];
    var saveDrafts = function () {
        $window.localStorage.ReportDrafts = JSON.stringify($scope.reportDrafts);
    };
    var initDrafts = function () {
        if ($window.localStorage.ReportDrafts == null)
            $window.localStorage.ReportDrafts = JSON.stringify([]);

        $scope.reportDrafts = JSON.parse($window.localStorage.ReportDrafts, JSON.dateParser);
    };
    var deleteDraft = function (draft) {
        var index = $scope.reportDrafts.indexOf(draft);
        $scope.reportDrafts.splice(index, 1);
        saveDrafts();
    };
    $scope.DeleteDraft = function (draft) {

        if (confirm("Are you sure you want to delete this draft?")) {
            deleteDraft(draft);
            $scope.newReport = new OnSiteReport();
            $scope.newReportPanel.Toggle();
        }
    };
    $scope.SubmitDraft = function () {
        loader.Start();

        //This means its a new draft
        if (!$scope.newReport.IsDraft) {
            //Adds it as new draft if its not one already
            $scope.newReport.IsDraft = true;
            $scope.reportDrafts.push($scope.newReport);
        }

        saveDrafts();
        $scope.newReportPanel.Toggle();
        $scope.newReport = new OnSiteReport();
        alerts.Success("Report has been saved as a draft.");

        loader.Stop();
    };
    $scope.ClickDraft = function (draft, index) {
        $scope.newReport = draft;
        $scope.newReportPanel.Open();
    };
    initDrafts();


    //Toggle new report sub panels
    $scope.showInformation = true;
    $scope.ToggleInformation = function () {
        $scope.showInformation = !$scope.showInformation;
    };
    $scope.showTimes = false;
    $scope.ToggleTimes = function () {
        $scope.showTimes = !$scope.showTimes;
    };
    $scope.showIncident = false;
    $scope.ToggleIncident = function () {
        $scope.showIncident = !$scope.showIncident;
    };

    $scope.AddPolice = function () {
        $scope.newReport.Police.push(new ReportTime());
    };
    $scope.RemovePolice = function (index) {
        $scope.newReport.Police.splice(index, 1);
    };

    //Locations
    $scope.locations = [];
    var GetLocations = function () {
        loader.Start();
        $http.get("/Api/OnSiteReport/GetLocations", {
            params: {}
        }).then(
          function (response) { //Success
              $scope.locations = response.data.Data;
              loader.Stop();
          }, function (response) { //Failed
              alerts.Error(ERROR_MESSAGE);
              loader.Stop();
          }
      );
    };
    GetLocations();

    //Unit
    $scope.units = [];
    var GetUnits = function () {
        loader.Start();
        $http.get("/Api/OnSiteReport/GetUnits", {
            params: {}
        }).then(
          function (response) { //Success
              $scope.units = response.data.Data;
              loader.Stop();
          }, function (response) { //Failed
              alerts.Error(ERROR_MESSAGE);
              loader.Stop();
          }
      );
    };
    GetUnits();

    //Productions
    $scope.productions = [];
    var GetProductions = function () {
        loader.Start();
        $http.get("/Api/Production/GetProductions", {
            params: {}
        }).then(
          function (response) { //Success
              $scope.productions = response.data.Data;
          }, function (response) { //Failed
              alerts.Error(ERROR_MESSAGE);
          }
        ).finally(function () {
            loader.Stop();
        });
    };
    GetProductions();
    $scope.selectedProduction = { value: $scope.productions[0] };

    //Incident Report
    $scope.incidentCategories = [];
    var GetCategories = function () {

        loader.Start();
        $http.get("/Api/OnSiteReport/GetIncidentCategories", {
            params: {}
        }).then(
            function (response) { //Success
                $scope.incidentCategories = response.data.Data;

            }, function (response) { //Failed
                alerts.Error(ERROR_MESSAGE);
            })
        .finally(function () {
            loader.Stop();
        });
    };
    GetCategories();
    $scope.AddIncidentReport = function () {
        $scope.newReport.Incidents.push(new IncidentReport());
    };
    $scope.RemoveIncidentReport = function (index) {
        $scope.newReport.Incidents.splice(index, 1);
    };
    $scope.fileSize = function (n) {
        var size = 0;
        for (var i = 0; i < n.Images.length; i++) {
            size += n.Images[i].length;
        }
        return Math.round(100 * size / 1048576) / 100;
    };

    //New Location
    $scope.newLocation = new Location();
    $scope.newLocationPanel = new Panel();
    $scope.OnPhoneValidate = function () {

    };
    $scope.SubmitNewLocation = function () {
        if ($scope.newLocation.LocationName != '' && $scope.newLocation.LocationAddress != '') {

            loader.Start();
            $http.post("/Api/OnSiteReport/NewLocation", $scope.newLocation, []).then(
                  function (response) { //Post success
                      if (response.data.Success) {
                          alerts.Success("Location added.");
                          $scope.newLocationPanel.Close();
                          $scope.newLocation = new Location();
                          GetLocations();
                      } else {
                          alerts.Error(response.data.Message);
                      }
                      loader.Stop();
                  },
                  function (response) {//post failed
                      alerts.Error(ERROR_MESSAGE);
                      loader.Stop();
                  }
            );
        }
    };

    //View Report
    $scope.viewReportPanel = new Panel();
    $scope.selectedReport = new OnSiteReport();
    $scope.ClickListItem = function (id) {

        loader.Start();
        $http.get("/Api/OnSiteReport/GetReport/" + id, {
            params: {}
        }).then(
          function (response) { //Success
              $scope.selectedReport = response.data.Data;
              $scope.viewReportPanel.Open();

          }, function (response) { //Failed
              alerts.Error(ERROR_MESSAGE);

          }).finally(function () {
              loader.Stop();
          });
    };
    $scope.ResendReport = function (report) {

        loader.Start();
        $http.get("/Api/OnSiteReport/ResendReport/" + report.LocationReportID, {
            params: {}
        }).then(
          function (response) { //Success
              alerts.Success("Report has been resent.");

          }, function (response) { //Failed
              alerts.Error(ERROR_MESSAGE);

          }).finally(function () {
              loader.Stop();
          });
    };

    //View Image
    $scope.imagePanel = new Panel();
    $scope.incidentImages = [];
    $scope.imageIndex = 0;
    $scope.viewImages = function (images) {
        $scope.imagePanel.Open();
        $scope.incidentImages = images;
    };
    $scope.imgNext = function (right) {
        if (!right)
            $scope.imageIndex--;
        else if (right)
            $scope.imageIndex++;

        if ($scope.imageIndex <= 0)
            $scope.imageIndex = $scope.incidentImages.length - 1;
        else if ($scope.imageIndex >= $scope.incidentImages.length)
            $scope.imageIndex = 0;
    };
});
