(function (pageBefore) {
    'use strict';

    var extendFunc = pageBefore();

    if (extendFunc == null) 
    	return;

    require(['jQuery', 'app', 'control', 'modal', 'extendObject', 'baseServices', 'menuServices', 'userStorage', 'director',
        'router',
        'main',
        'run'],
        function (jQuery, app, control, modal, extendObject, baseServices, menuServices, userStorage, director,
            router,
            main,
            run
        ) {
            app = jQuery.extend(app, extendFunc);
            window.app = app;

            if (app.menu) {
                app.container = main.init(app.menu).getContainer();
                return;
            }
            
            menuServices.queryByCurrentUser(function (result) {          
                app.menu = result;
                main.init(app.menu);
                var root = main.GetFirstView();
                app.setSelectedMenu = main.setSelectedMenu;
                router(app.menu, root, main.setSelectedMenu).container = main.getContainer();
            });
        });
})(function () {
    'use strict';

    var _checkUserloading = jQuery(document.getElementById("checkUserloading")).modal('open');
    var _loginUrl = "http://" + window.location.host + "/Auth";

    var setCookie = function (name, value, expiredays) {
        var exdate = null;

        if (expiredays != null) {
            exdate = new Date();
            exdate.setDate(exdate.getDate() + expiredays);
        }
        document.cookie = name + "=" + escape(value) + ((exdate == null) ? "" : ";expires=" + exdate.toGMTString()) + "; path=/";
    };

    var getCookie = function (objName) {
        var arrStr = document.cookie.split("; ");
        for (var i = 0; i < arrStr.length; i++) {
            var temp = arrStr[i].split("=");
            if (temp[0] === objName) {
                return unescape(temp[1]);
            }
        }
    };

    var removeCookie = function (name) {
        var exp = new Date();
        exp.setTime(exp.getTime() + (-1 * 24 * 60 * 60 * 1000));
        var cval = this.getCookie(name);
        document.cookie = name + "=" + cval + "; expires=" + exp.toGMTString() + "; path=/";
    };

    var post = function (url, params) {
        var temp = document.createElement("form");
        temp.action = url;
        temp.method = "post";
        temp.style.display = "none";
        for (var x in params) {
            var opt = document.createElement("textarea");
            opt.name = x;
            opt.value = params[x];
            temp.appendChild(opt);
        }
        document.body.appendChild(temp);
        temp.submit();
    };

    var uploaderFile = function (folder, multiple, accept, uploadSuccessCall, param) {
        if (!folder)
            throw Error("请给定调用参数folder");

        var path = "/FileUpload/Index?folder=" + folder;

        accept = accept ? "&accept=" + accept : "";
        multiple = multiple ? "&multiple=multiple" : "";
        window.loading(true);

        jQuery.get(path + multiple + accept).success(function (html) {
            window.loading(false, function () {
                window.modal({ title: "文件上传", content: html, height: 450 });
                window.uploadSuccessCall = function (result) {
                    window.modal('close');
                    uploadSuccessCall(result);
                };

                window.uploader.setServerParam(jQuery.extend({
                    folder: folder
                }, param));
            });
        });
    };

    var gotoLogin = function () {
        location.href = _loginUrl;
    };

    var setLoginUrl = function (url) {
        if (!url) {
            _loginUrl = config.loginUrl;
            return _loginUrl;
        }
        _loginUrl = url;
        return this;
    };

    var getToken = function () {
        return getCookie(config.userTokenName);
    };

    var token = getToken();
    if (!token || token === "undefined") {
        gotoLogin();
        return;
    }
    _checkUserloading.modal("close");
    jQuery(document.getElementById("body")).show(500);

    //var saveSelectColor = {
    //    'Name': 'SelcetColor',
    //    'Color': 'theme-black'
    //};

    //var storageLoad = function (objectName) {
    //    if (localStorage.getItem(objectName)) {
    //        return JSON.parse(localStorage.getItem(objectName))
    //    }
    //    return false
    //};

    var storageSave = function (objectData) {
        localStorage.setItem(objectData.Name, JSON.stringify(objectData));
    };

    //if (storageLoad('SelcetColor')) {
    //    jQuery('body').attr('class', storageLoad('SelcetColor').Color)
    //} else {
    //    storageSave(saveSelectColor);
    //    jQuery('body').attr('class', saveSelectColor.Color)
    //}

    jQuery(document.getElementById("loading")).attr("data-show", true).modal('open');

    require.config({
        paths: config.require.paths,
        shim: config.require.shim
    });

    return {
        post: post,
        getToken: getToken,
        uploaderFile: uploaderFile,
        setCookie: setCookie,
        getCookie: getCookie,
        removeCookie: removeCookie,
        gotoLogin: gotoLogin,
        setLoginUrl: setLoginUrl,
        //saveSelectColor: saveSelectColor
        storageSave: storageSave
    };
});



