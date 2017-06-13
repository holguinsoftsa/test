app.controller('ProductionController', function ($scope, $http, loader, alerts) {

    //Data
    $scope.productions = null;
    var getProductions = function () {
        $http.get("/Api/Production/GetProductions", {
            params: {}
        }).then(
           function (response) { //Success
               $scope.productions = response.data.Data;
           }, function (response) { //Failed
               alerts.Error(ERROR_MESSAGE);
               loader.Stop();
           }
       );
    }
    getProductions();

    $scope.RemoveUsers = function (prod) {
        $http.post("/Api/Production/RemoveUsers/" + prod.ID, null, []).then(
        function (response) { //Success
            if (response.data.Success) { //Success
                alerts.Success(response.data.Data + " users removed.");
            }
            else { //Error Occured
                alerts.Error(response.data.Message);
            }
            loader.Stop();
        }, function (response) { //Failed
            alerts.Error(ERROR_MESSAGE);
            loader.Stop();
        }
    );
    };

    //New Production
    $scope.newProdName = "";
    $scope.showNewProductionWindow = false;
    $scope.ClickNewProduction = function () {
        $scope.showNewProductionWindow = true;
    };
    $scope.CloseNewProduction = function () {
        $scope.showNewProductionWindow = false;
    };
    $scope.SubmitNewProduction = function () {

        var prod = {
            Name: $scope.newProdName
        };
        prod.Name = prod.Name.toUpperCase();
        loader.Start();

        $http.post("/Api/Production/CreateProduction", prod, []).then(
        function (response) { //Success
            if (response.data.Success) { //Success
                alerts.Success("Production '" + prod.Name.toUpperCase() + "' has been created.");

                $scope.productions.push(prod);
                $scope.CloseNewProduction();
                $scope.newProdName = "";
            }
            else { //Error Occured
                alerts.Error(response.data.Message);
            }
            loader.Stop();
        }, function (response) { //Failed
            alerts.Error(ERROR_MESSAGE);
            loader.Stop();
        }
    );
    };

});