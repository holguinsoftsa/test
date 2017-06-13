$(document).ready(function () {

    var onTick = function () {
        var old = $(".slide-img.active");

        var active = old.next(".slide-img");
        if (active.length == 0) active = $(".slide-img").first();
        active.addClass("active");

        setTimeout(function () { old.removeClass("active"); }, 500);
    };
    var intertvalTime = 5000;

    setInterval(onTick, intertvalTime);

});