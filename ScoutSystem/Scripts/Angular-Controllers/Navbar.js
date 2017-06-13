app.controller('NavbarController', function ($scope, $location, $http) {

    $scope.isActive = function (viewLocation) {
        return viewLocation === $location.path();
    };

    $scope.User = null;
    function GetUserName() {
        $http.get("/Api/Users/GetUserInfoMe", {}).then(
            function (response) { //Success
                $scope.User = response.data.Data;

            }, function (response) { //Failed

                alerts.Error(ERROR_MESSAGE);
            }
       );
    };
    GetUserName();
});