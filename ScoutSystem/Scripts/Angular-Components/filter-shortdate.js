app.filter('shortdate', function () {
    return function (input) {
        return moment(input).format('MM/DD/YY');
    };
});