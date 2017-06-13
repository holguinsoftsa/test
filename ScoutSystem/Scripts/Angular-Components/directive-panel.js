app.directive('panel', function () {
    return {
        restrict: 'AE',
        templateUrl: '/Files/panel.html',
        transclude: true,
        require: 'ngModel',
        scope: {
            ngModel: '=',
            title: '@',
            fullheight: '@',
            headerColor: "@"
        },
        link: function ($scope, $elem, $attr) {

            if ($scope.ngModel == null)
                $scope.ngModel = new Panel();

        }
    }
});

//Angular Panel. Callbacks should return a bool, true to proceed with event, false to cancel.
function Panel(OpenCallback, CloseCallback, ToggleCallBack) {
    this.Show = false;
    this.Open = function () {
        this.Show = true;

        if (OpenCallback != null) {
            var result = OpenCallback();
            if (result || result == null)
                this.Show = true;
        }
        else this.Show = true;
    };

    this.Close = function () {
        if (CloseCallback != null) {
            var result = CloseCallback();
            if (result || result == null)
                this.Show = false;
        }
        else this.Show = false;
    };

    this.Toggle = function () {

        if (ToggleCallBack != null) {
            var result = ToggleCallBack();
            if (result || result == null)
                this.Show = !this.Show;
        }
        else
            this.Show = !this.Show;
    };
}