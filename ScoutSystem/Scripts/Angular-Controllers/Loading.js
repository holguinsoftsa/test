app.controller('LoadingController', function ($scope, loader) {

    $scope.showLoading = false;

    var queue = 0;
    var check = function () {
        if (queue > 0)
            $scope.showLoading = true;
        else if (queue <= 0) {
            queue = 0;
            $scope.showLoading = false;
        }
    };

    loader.Start = function () {
        queue++;
        check();
    };
    loader.Stop = function () {
        queue--;
        check();
    };
});