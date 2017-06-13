app.service('alerts', function () {
    this.Success = function () {
        alert("'alerts' service: This function should be overriden by alerts controller...");
    };
    this.Error = function () {
        alert("'alerts' service: This function should be overriden by alerts controller...");
    };
});
