(function (window, jQuery) {
    var progress = function () {
        var _timers;
        var _container;
        var _dic;
        var _getProgress = function (name, percentage) {
            var temp = "<div style='width: 343px; margin: 0 auto 25px auto;padding-top:20px;'>";
            temp += "<div><label data-name>" + name + "</label></div>";
            temp += "<div class='am-progress am-progress-xs'>";
            temp += "<div class='am-progress-bar' style='width: " + percentage + "%'></div>";
            temp += "</div>";
            temp += "</div>";
            return jQuery(temp);
        };
        var _setProgress = function (key, percentage) {
            if (percentage > 100) {
                return;
            }
            _dic[key].find(".am-progress-bar").css("width", percentage + "%");
            if (percentage == 100)
                clearInterval(_timers[key]);
        };
        var _run = function (key, name, percentage) {
            if (key in _dic) {
                _setProgress(key, percentage);
            } else {
                _dic[key] = _getProgress(name, percentage).appendTo(_container);
                _timers[key] = setInterval(function () {
                    percentage++;
                    _setProgress(key, percentage);
                }, 100);
            }
        };
        this.setError = function (key, title) {
            var elm = _dic[key];
            elm.find("label").css("color", "red");
            _dic[key].find(".am-progress-bar").addClass("am-progress-bar-danger");
            if (title)
                this.setTitle(key, title);
        };
        this.setTitle = function (key, title) {
            _dic[key].find("label").html(title);
            return this;
        };
        this.stop = function (key) {
            clearInterval(_timers[key]);
            return this;
        };
        this.init = function (param) {
            _container = typeof param.container === "string"
                ? jQuery(document.getElementById(param.container)) : param.container;
            _dic = new Object();
            _timers = new Object();
            return this;
        };
        this.run = function (key, name, percentage) {
            _run(key, name, percentage);
            return this;
        };
    };

    var uploader = function () {
        var _server = "/FileUpload/Uploader";
        var _name = "file";
        var _html5UploaderConent = ["#dropbox", "#multiple"];
        var _keys = [];
        var _result = [];
        var _progress;
        var _progressContent;
        var _drogDrop;
        var _onClientLoad = function (e, file) {
            _progressContent.show();
            _drogDrop.hide();
            file.key = _keys.length + "f";
            _keys.push(file.key);
            _progress.run(file.key, file.name, 0);
        };
        var isJSON = function (text) {
            return /^[\],:{}\s]*$/.test(text.replace(/\\["\\\/bfnrtu]/g, '@').replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g, ']').replace(/(?:^|:|,)(?:\s*\[)+/g, ''));
        };
        var _onSuccess = function (e, file, responseText) {
            if (!isJSON(responseText)) {
                _progress.run(file.key, file.name, 100);
                _keys.splice($.inArray(file.key, _keys), 1);
                window.uploadSuccessCall && window.uploadSuccessCall(responseText);
                return;
            }
            var result = JSON.parse(responseText);
            if (result["type"] === "1") {
                return _progress.stop(file.key)
                    .run(file.key, file.name, 80)
                    .setError(file.key, file.name + "上传失败" + "-" + result["message"]);
            }
            _progress.run(file.key, file.name, 100);
            _keys.splice($.inArray(file.key, _keys), 1);
            _result.push(result["url"]);
            if (_keys.length === 0) {
                window.uploadSuccessCall && window.uploadSuccessCall(_result);
            }
        };
        var _html5UploaderInit = function () {
            jQuery(_html5UploaderConent.join(',')).html5Uploader({
                name: _name,
                postUrl: _server,
                onClientLoad: _onClientLoad,
                onSuccess: _onSuccess
            });
        };
        this.setServerParam = function (requestParam) {
            var value;
            for (var name in requestParam) {
                if (!value)
                    value = "?" + name + "=" + requestParam[name];
                else
                    value += "&" + name + "=" + requestParam[name];
            }
            $.setHtml5UploaderPostUrl(_server + value);
            return this;
        };
        this.init = function () {
            _progressContent = jQuery("#progressContent");
            _drogDrop = jQuery(".drogDrop");
            _progress = new progress().init({
                container: _progressContent
            });
            _html5UploaderInit();
            return this;
        };

    };

    jQuery(function () {
        window.uploader = new uploader().init();
    });
})(window, $ || jQuery)