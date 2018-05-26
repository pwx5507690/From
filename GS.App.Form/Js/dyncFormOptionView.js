var app = new Object();
(function (jQuery, window) {
    var _key = "v6HcVV24BW8BSbpaHwq7wwtE5ELK1eGm";
    var _map = null;
    var _current = null;
    var _elm = {
        map: "map",
        prov: "prov",
        fileModel: "fileModel",
        city: "city",
        dist: "dist",
        mapContent: "mapContent",
        addressTemp: "addressTemp",
        dropdownCity: "dropdownCity"
    };
    var _initMap = function (input) {
        _loadMap();
        var top = input.offset().top;
        var height = input.height();
        var left = input.offset().left;

        jQuery(document.getElementById(_elm.mapContent)).css({
            "top": (top + height) + "px",
            "left": left + "px",
        }).show();
    };
    var _loadMap = function () {
        var url = "http://api.map.baidu.com/api?v=2.0&ak=" + _key;
        loadScript(url, function () {
            var _map = new BMap.Map(_elm.map);
            var point = new BMap.Point(116.331398, 39.897445);
            _map.centerAndZoom(point, 12);

            function fun(result) {
                var cityName = result.name;
                _map.setCenter(cityName);
            };
            _map.enableScrollWheelZoom();
            var city = new BMap.LocalCity();
            city.get(fun);
        });
    };
    var _cityChangeValue = function (input) {
        var prov = jQuery(document.getElementById(_elm.prov)).trigger("chosen:updated").val();
        var city = jQuery(document.getElementById(_elm.city)).trigger("chosen:updated").val();
        var dist = jQuery(document.getElementById(_elm.dist)).trigger("chosen:updated").val();
        var value = "";
        if (prov) {
            value = value + prov + "-";
        }
        if (city) {
            value = value + city + "-";
        }
        if (dist) {
            value = value + dist + "  ";
        } else {
            value = value.substring(0, value.length - 1);
        }
        value = value;
        input.attr("data-value", value);
    };
    var _initDropdownCity = function (input) {
        input.before(jQuery(document.getElementById(_elm.addressTemp)).html());
        _loadDropdownCity(input);
        window.provCall = window.cityCall = window.distCall = function () {
            _cityChangeValue.call(this, input)
        };
    };
    var _initDropdown2 = function (input) {
        var html = [];
        var selectTarget = input.attr("data-select");
        var temp = '<div class="am-u-sm-{$}"><select data-dropdown2="' + selectTarget + '" data-count="{count}"></select></div>';
        var count = +input.attr("data-pn") || "2";
        var width = +count;

        if (+count == 2) {
            width = 4;
        }
        temp = temp.replace("{$}", width);

        for (var i = 0; i < count; i++) {
            html.push(temp.replace("{count}", i));
        }
        input.before(html.join(''));
        jQuery(document).on("change", "[data-dropdown='" + selectTarget + "']", function () {
            var value = [];
            jQuery("[data-dropdown='" + selectTarget + "']").each(function () {
                var $this = jQuery(this);
                if (!$this.val()) {
                    value[$this.attr("data-count")] = $this.val();
                }
            });
            input.val(value.join(','));
        });
    };
    var _initUpload = function () {
        jQuery(document).on("click", "[data-file]", function () {
            var $this = $(this);
            jQuery(document.getElementById(_elm.fileModel)).modal({
                width: 950
            });
        });
    };
    var _loadDropdownCity = function (input) {
        loadScript("/js/jquery.cityselect.js", function () {
            var dropdownCity = jQuery(document.getElementById(_elm.dropdownCity));
            var param = {
                loadCall: function () {
                    try {
                        var vl = input.val();
                        if (vl.indexOf("__") != -1) {
                            vl = vl.split("__")[0]
                        }

                        var value = vl.split("-");
                        jQuery("#" + _elm.prov).val(value[0]).change();

                        jQuery("#" + _elm.city).val(value[1]).change();
                        jQuery("#" + _elm.dist).val(value[2]);
                    } catch (e) {
                        console.log(e.message);
                    }
                    jQuery("#" + _elm.prov + ",#" + _elm.city + ",#" + _elm.dist).chosen();
                }
            };
            if (_current == null) {
                dropdownCity.citySelect(param);
            } else {
                dropdownCity.citySelect(jQuery.extend({
                    prov: current["province"],
                    city: current["city"],
                    dist: current["dist"]
                }, param));
            }
        });
    };
    var _initKindEditor = function (htmlType) {
        KindEditor.ready(function (K) {
            //fileManagerJson: '../asp.net/file_manager_json.ashx',
            //allowFileManager: true
            for (var i = 0; i < htmlType.length; i++) {
                K.create("#" + htmlType[i], {
                    uploadJson: '/DyncForm/Upload'
                });
            }
            prettyPrint();
        });
    };
    var _initForm = function () {
        jQuery("select").chosen();
        var elm = jQuery("[data-type]");
        var htmlType = [];
        for (var i = 0; i < elm.length; i++) {
            var $item = jQuery(elm[i]);
            var type = $item.attr("data-type");
            if (type == "map") {
                _initMap();
            }
            if (type == "dropdown2") {
                _initDropdown2($item);
            }
            if (type == "address") {
                _initDropdownCity($item);
            }
            if (type == "html") {
                htmlType.push($item.attr("id"));
            }
        }
        if (htmlType.length > 0)
            _initKindEditor(htmlType);
        _initUpload();

    };
    var _pageInit = function () {
        _initForm();
    };
    app.valid = function () {
        try {
            var row = [];
            var input = jQuery("[data-type]");

            for (var i = 0; i < input.length; i++) {
                var name = input[i].tagName.toLocaleLowerCase();
                var $item = jQuery(input[i]);
                var reqd = $item.attr("data-reqd");
                var type = $item.attr("data-type");
                var val;
                if (name === "input" || name == "select" || name == "textarea") {
                    if (type == "name") {
                        var nameGroup = jQuery("[data-name='" + $item.attr("data-name") + "']");
                        if (nameGroup.length > 1) {
                            val = nameGroup.eq(0).val() + "." + nameGroup.eq(1).val();
                        } else {
                            val = $item.val();
                        }
                    } else if (type == "address") {
                        val = $item.attr("data-value");
                        if ($item.val()) {
                            if ($item.val().indexOf("__") != -1) {
                                val = val + "__" + $item.val().split("__")[1];
                            } else {
                                val = val + "__" + $item.val();
                            }
                        }
                    }
                    else {
                        val = $item.val();
                    }
                }
                else if (name == "div" && (type == "checkbox" || type == "radio")) {
                    var v = [];
                    $item.find("input").each(function () {
                        if (this.checked && this.value) {
                            v.push(this.value);
                        }
                    });
                    if (v.length > 0) {
                        val = v.join(',');
                    }
                } else {
                    val = $item.html();
                }
                if (type == "number") {
                    if (!validateNum(val)) {
                        $item.focus();
                        return false;
                    }
                }
                if (type == "phone") {
                    if (!validatemobile(val)) {
                        return false;
                    }
                }
                if (type == "email") {
                    if (!isEmail(val)) {
                        $item.focus();
                        return false;
                    }
                }
                if (+reqd == 1 && !val) {
                    message($item.attr("data-lbl") + "不能为空");
                    $item.focus();
                    return false;
                }

                row.push({
                    Value: val,
                    Name: $item.attr("data-name")
                });
            }

            var sendData = {
                Code: document.getElementById("code").value,
                DataId: document.getElementById("dataId").value,
                Row: row
            };
            document.getElementById("value").value = JSON.stringify(sendData);
        } catch (e) {
            console.log(e);
            return false;
        }
    };
    jQuery(function () {
        _pageInit();
    });
})($ || jQuery, window)