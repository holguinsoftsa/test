app.controller('AlertController', function ($scope, $q, alerts) {


    //Success Panel
    $scope.successPanel = new Panel();
    $scope.successMessage = "";

    //Error Panel
    $scope.errorPanel = new Panel();
    $scope.errorMessage = "";

    //Confirm panel
    var resolveProm, rejectProm;
    $scope.confirmPanel = new Panel();
    $scope.confirmMessage = "";
    $scope.clickConfirm = function (result) {
        if (result) resolveProm();
        else rejectProm();

        $scope.confirmPanel.Close();
    };

    //Service
    alerts.Success = function (message) {
        $scope.successMessage = message;
        $scope.successPanel.Open();
    }
    alerts.Error = function (message) {
        $scope.errorMessage = message;
        $scope.errorPanel.Open();
    };
    alerts.Confirm = function (message) {
        $scope.confirmPanel.Open()
        $scope.confirmMessage = message;
        return $q(function (resolve, reject) {
            resolveProm = resolve;
            rejectProm = reject;
        });
    };

});