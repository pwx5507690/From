define(['jQuery', 'app'], function (jQuery, app) {
    var time = setInterval(function () {
        jQuery.get("/Authority/IsAuthority").success(function (result) {
            if (result === "success")
                return;
            gotoLogin();
        }).error(gotoLogin);
    }, 2000);

    var gotoLogin = function () {
        window.clearInterval(time);
        app.gotoLogin();
    };
});