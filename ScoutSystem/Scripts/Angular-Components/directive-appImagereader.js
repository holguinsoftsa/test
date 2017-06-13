//Image Upload directive
app.directive('appImagereader', function ($q) {
    var slice = Array.prototype.slice;

    return {
        restrict: 'A',
        require: '?ngModel',
        link: function (scope, element, attrs, ngModel) {
            if (!ngModel) return;

            ngModel.$render = function () { };

            element.bind('change', function (e) {
                var element = e.target;

                $q.all(slice.call(element.files, 0).map(readFile))
                    .then(function (values) {
                        if (element.multiple) ngModel.$setViewValue(values);
                        else ngModel.$setViewValue(values.length ? values[0] : null);
                    });

                function readFile(file) {
                    var deferred = $q.defer();

                    var reader = new FileReader();
                    reader.onload = function (e) {
                        //Resize img
                        var maxWidth = 800;
                        var maxHeight = 800;

                        var image = new Image();
                        var dataURL = e.target.result;
                        image.src = dataURL;
                        image.onload = function () {
                            var width = image.width;
                            var height = image.height;
                            var shouldResize = (width > maxWidth) || (height > maxHeight);

                            if (shouldResize) {
                                var newWidth;
                                var newHeight;

                                if (width > height) {
                                    newHeight = height * (maxWidth / width);
                                    newWidth = maxWidth;
                                } else {
                                    newWidth = width * (maxHeight / height);
                                    newHeight = maxHeight;
                                }

                                var canvas = document.createElement('canvas');
                                canvas.width = newWidth;
                                canvas.height = newHeight;
                                var context = canvas.getContext('2d');
                                context.drawImage(this, 0, 0, newWidth, newHeight);

                                dataURL = canvas.toDataURL(file.type);
                            }
                            deferred.resolve(dataURL);
                        };
                    };
                    reader.onerror = function (e) {
                        deferred.reject(e);
                    };
                    reader.readAsDataURL(file);

                    return deferred.promise;
                }

            }); //change

        } //link
    }; //return
});
