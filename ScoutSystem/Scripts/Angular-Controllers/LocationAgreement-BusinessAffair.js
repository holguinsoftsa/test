app.controller('LocationAgreement-BusinessAffairController', function ($scope, $http, $q, loader, alerts, signature) {

    // CKEditor options.
    $scope.options = {
        language: 'en',
        allowedContent: true,
        entities: false,
        height: '400px'
    };
    $http.get("/Files/LocationAgreementDefault.html", {}).then(
           function (response) { //Success
               $scope.ckContent = response.data;
           }, function (response) { //Failed

               alerts.Error(ERROR_MESSAGE);
           }
       ).finally(function () { //finally

       });
    $scope.CKReady = function () { };

    $scope.agreementList = [
        { ID: 0, FirstName: "John", LastName: "Smith", LocationName: "Mansion Garcia", DateCreated: "2/10/17", Status: "Pending" },
        { ID: 1, FirstName: "Penny", LastName: "Smith", LocationName: "Pink Palace", DateCreated: "2/10/17", Status: "Pending" },
        { ID: 1, FirstName: "Penny", LastName: "Smith", LocationName: "Pink Palace", DateCreated: "2/10/17", Status: "Pending" },
        { ID: 1, FirstName: "Penny", LastName: "Smith", LocationName: "Pink Palace", DateCreated: "2/10/17", Status: "Pending" },
    ];

    $scope.docTempValues = [
        { Name: "Name", User: "Scout", Selector: "[NAME]", Type: "Text" },
        { Name: "Production", User: "Scout", Selector: "[PRODUCTION]", Type: "Text" },
        { Name: "Location", User: "Scout", Selector: "[LOCATION]", Type: "Text" },
        { Name: "Owner Signature", User: "Owner", Selector: "[OWNERSIG]", Type: "Signature" }
    ];


    //Default Agreement
    $scope.defaultAgreementPanel = new Panel();



    //View Agreement
    var agrr = new ViewAgreement();
    agrr.OwnerFirstName = "John";
    agrr.OwnerLastName = "Doe";
    agrr.OwnerEmail = "John.Doe@nbcuni.com";
    agrr.LocationName = "Mansion Garcia";
    agrr.LocationAddress = "275 W 32 ST";
    agrr.LocationCity = "Miami";
    agrr.LocationState = "FL";
    agrr.LocationZip = 33010;
    agrr.DocumentValues = [
        { ID: 0, Name: "Name", Value: "John Smith" },
        { ID: 1, Name: "Production", Value: "Jenni" },
        { ID: 2, Name: "Location", Value: "Miami Springs Mansion" },
        { ID: 3, Name: "Address", Value: "123 W 59 ST" }];
    $scope.viewAgreement = agrr;

    $scope.docPanel = new Panel();


    //View Agreement
    $scope.viewAgreementPanel = new Panel();
    $scope.PreviewDoc = function ($event, n) {
        $event.stopPropagation();
    };

});