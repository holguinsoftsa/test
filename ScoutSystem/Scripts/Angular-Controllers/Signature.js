/// <reference path="../signature_pad.js" />

app.controller('SignatureController', function ($scope, $q, signature) {
    var SIG_FORMAT = "image/svg+xml";

    //Canvas
    var canvas = angular.element("#sigCanvas")[0];
    var signaturePad = new SignaturePad(canvas);
    var ctx = canvas.getContext("2d");

    function resetCanvas() {
        signaturePad.clear();
        ctx.beginPath();
        ctx.lineWidth = 2;
        ctx.strokeStyle = "#aaaaaa";
        ctx.moveTo(0, 110);
        ctx.lineTo(canvas.width, 110);
        ctx.stroke();
    };
    
    function resizeCanvas() {//Forces canvas with device ppi
        var ratio = Math.max(window.devicePixelRatio || 1, 1);
        canvas.width = canvas.offsetWidth * ratio;
        canvas.height = canvas.offsetHeight * ratio;
        canvas.getContext("2d").scale(ratio, ratio);
        signaturePad.clear(); // otherwise isEmpty() might return incorrect value
        resetCanvas();
    }
    window.addEventListener("resize", resizeCanvas);
    resizeCanvas();

    //Signature promise
    var promise = null;
    var resolveProm, rejectProm;

    //Panel
    function onCloseSigPanel() {
        resetCanvas();
        rejectProm("Closed");
        return true;
    };
    $scope.sigPanel = new Panel(null, onCloseSigPanel);
    $scope.ClearSignature = function () {
        resetCanvas();
    };
    $scope.SubmitSig = function () {

        var img = signaturePad.toDataURL(SIG_FORMAT);
        resolveProm(img);
        resetCanvas();
        $scope.sigPanel.Close(); 
    };

    //Service
    signature.GetSignature = function () {
        $scope.sigPanel.Open();

        promise = $q(function (resolve, reject) {
            resolveProm = resolve;
            rejectProm = reject;
        });
        
        return promise;
    };

});
