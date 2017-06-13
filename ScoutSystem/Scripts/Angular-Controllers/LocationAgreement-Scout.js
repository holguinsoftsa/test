app.controller('LocationAgreement-ScoutController', function ($scope, $http, $q, loader, alerts, signature) {
    //Production

    GetUserProduction($q, $http, loader).then(function (result) {
        $scope.userProd = result;
    });

    $scope.agreementList = [
        { ID: 0, FirstName: "John",  LastName: "Smith", LocationName: "Mansion Garcia", DateCreated: "2/10/17", Status: "Pending" },
        { ID: 1, FirstName: "Penny", LastName: "Smith", LocationName: "Pink Palace", DateCreated: "2/10/17", Status: "Pending" },
        { ID: 1, FirstName: "Penny", LastName: "Smith", LocationName: "Pink Palace", DateCreated: "2/10/17", Status: "Pending" },
        { ID: 1, FirstName: "Penny", LastName: "Smith", LocationName: "Pink Palace", DateCreated: "2/10/17", Status: "Pending" },
    ];

    //New Agreement
    $scope.newAgreement = new NewAgreement();
    $scope.newAgreementPanel = new Panel();
    $scope.SubmitNewAgreement = function () {
        console.log($scope.newAgreement);
    };

    //View Agreement
    $scope.viewAgreementPanel = new Panel();
    $scope.PreviewDoc = function ($event, n) {
        $event.stopPropagation();
    };

});
