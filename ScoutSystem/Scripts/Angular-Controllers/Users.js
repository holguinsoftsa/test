app.controller('UserController', function ($scope, $http, loader, alerts) {

    //User List
    $scope.userList = null;
    var getUserList = function () {
        loader.Start();
        $http.get("/Api/Users/GetUserList", {
            params: {

            }
        }).then(
            function (response) { //Success
                $scope.userList = response.data.Data;
                loader.Stop();

            }, function (response) { //Failed
                alerts.Error(ERROR_MESSAGE);
                loader.Stop();
            }
        );
    }
    getUserList();
    $scope.ChangePassword = function (user) {
        var password = prompt("Enter new password for " + user.Email, "Pink7713pink!");

        if (password == null)
            return; //If clicked cancel

        loader.Start();
        $http.post("/Api/Users/ChangePassword", {
            Email: user.Email,
            Password: password
        }, []).then(
          function (response) { //Success
              if (response.data.Success) { //Success
                  alerts.Success("Password has been changed for user " + user.Email);
                  getUserList();
              }
              else { //Error Occured
                  alerts.Error(response.data.Message);
              }
              loader.Stop();

          }, function (response) { //Failed

              alerts.Error(ERROR_MESSAGE)
              loader.Stop();
          }
      );
    };
    $scope.ToggleActive = function (user) {

        loader.Start();
        $http.post("/Api/Users/ToggleActive", { Email: user.Email }, []).then(
          function (response) { //Success
              if (response.data.Success) { //Success
                  user.Active = !user.Active;
                  if (user.Active)
                      alerts.Success(user.Username + " has been activated.");
                  else
                      alerts.Success(user.Username + " has been deactivated.");
              }
              else { //Error Occured
                  alerts.Error(response.data.Message);
              }
              loader.Stop();

          }, function (response) { //Failed

              alerts.Error(ERROR_MESSAGE)
              loader.Stop();
          }
      );

    };

    //New User Panel
    $scope.newUserPanel = new Panel(null, closeNewUserPanel);
    var closeNewUserPanel = function () {
        if (confirm("Are you sure you want to cancel?")) {
            return true;
        }
    };
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
    var getRoles = function () {
        $http.get("/Api/Users/GetAllRoles", {
            params: {}
        }).then(
           function (response) { //Success
               $scope.roles = response.data.Data;

           }, function (response) { //Failed
               alerts.Error(ERROR_MESSAGE);
               loader.Stop();
           }
       );
    };
    getRoles();
    $scope.newUser = new NewUser();
    $scope.newUserErrorMessage = "";
    $scope.NewUserSubmit = function () {
        loader.Start();

        //Validate
        var user = $scope.newUser;

        //Check if blank
        if (user.Email == "" || user.Password == "" || user.ConfirmPassword == "" || user.Role == "")
            $scope.newUserErrorMessage = "Fields can not be blank.";
        else if (user.Password != user.ConfirmPassword)
            $scope.newUserErrorMessage = "Passwords do not match.";

        //Make request 
        $http.post("/Api/Users/CreateUser", $scope.newUser, []).then(
           function (response) { //Success

               if (response.data.Success) { //Success
                   alerts.Success("User successfully created.");
                   $scope.newUserPanel.Close();
                   $scope.newUser = new NewUser();
                   getUserList();
               }
               else { //Error Occured
                   $scope.newUserErrorMessage = response.data.Message;
               }
               loader.Stop();

           }, function (response) { //Failed

               $scope.newUserErrorMessage = response.data.Message;
               loader.Stop();
           }
       );

    };

    //Change Production Panel
    $scope.prodUser = null;
    $scope.showProductionPanel = false;
    $scope.ChangeProduction = function (user) {
        $scope.prodUser = user;
        $scope.showProductionPanel = true;
    };
    $scope.CloseProductionPanel = function () {
        $scope.showProductionPanel = false;
    };
    $scope.SubmitChangeProduction = function () {
        loader.Start();
        $http.post("/Api/Users/ChangeProduction", {
            Email: $scope.prodUser.Email,
            ProductionID: $scope.prodUser.ProductionID
        }, []).then(
          function (response) { //Success
              if (response.data.Success) { //Success
                  alerts.Success("Production changed for user " + $scope.prodUser.Email + ".");
                  getUserList();
                  $scope.CloseProductionPanel();
              }
              else { //Error Occured
                  alerts.Error(response.data.Message);
              }
              loader.Stop();

          }, function (response) { //Failed
              alerts.Error(ERROR_MESSAGE)
              loader.Stop();
          }
      );
    };
});
