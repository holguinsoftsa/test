app.directive('paginator', function () {
    return {
        restrict: 'AE',
        templateUrl: '/Files/paginator.html',
        transclude: false,
        require: 'ngModel',
        scope: {
            ngModel: '=',
            onPage: '=',
        },
        link: function ($scope, $elem, $attr) {


            if ($scope.ngModel == null)
                $scope.ngModel = new Paginator();

            //Set Attribute page size
            $scope.ngModel.Size = $scope.pageSize;

            $scope.$watch('ngModel', function () {

                var pages = $scope.ngModel;
                $scope.list = [];
                for (var i = 0; i < pages.Count; i++) {

                    if (i < 3)
                        $scope.list.push(i);
                    else if(i < pages.Number + 2 && i > pages.Number - 2)
                        $scope.list.push(i);
                    else if (i > pages.Count - 4)
                        $scope.list.push(i);

                }
            });

            $scope.onClick = function (n) {
                $scope.ngModel.Number = n;
                $scope.onPage();
            };
            $scope.clickPrev = function () {
                if ($scope.ngModel.Number != 0) {
                    $scope.ngModel.Number--;
                    $scope.onPage();
                }
            };
            $scope.clickNext = function () {
                if ($scope.ngModel.Number != $scope.ngModel.Count - 1) {
                    $scope.ngModel.Number++;
                    $scope.onPage();
                }
            };
        }
    }
});

//Angular Paginator.
function Paginator() {
    this.Count = 0;
    this.Size = 12; //Default 12
    this.Number = 0;
}