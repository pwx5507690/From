(function (jQuery, window) {
    var _editor;
    var _delete = function () {
        jQuery("[data-delete]").on("click", function () {
            var url = jQuery(this).attr("data-delete");
            var relatedTarget = {
                onConfirm: function (target) {
                    location.href = url;
                }
            };
            top.window.confirm("是否删除", relatedTarget);
        });
    };
    var _isValidIP = function (ip) {
        var reg = /^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$/
        return reg.test(ip);
    };
    var _seach = function () {
        jQuery("#search").on("click", function () {
            var searchText = jQuery("#searchText").val();
            location.href = "/Cms/Site?name=" + searchText;
        });
    };
    var _icon = function () {
        var icon = jQuery("#icon");
        icon.on("click", function () {
            top.window.app.uploaderFile("cms", true, "image/*", function (result) {
                jQuery("[name='Icon']").val(result[0]);
            });
        });
    };
    var _formInitMessage = function () {
        var message = jQuery("[data-message]").attr("data-message");
        if (message)
            top.window.alert(message);
    };
    var _selectInit = function () {
        jQuery("select").width(150).chosen();
        jQuery("#site").change(function () {
            jQuery("[name='SiteId']").val(jQuery(this).val());
        });
        jQuery("#tempType").change(function () {
            jQuery("[name='TempType']").val(jQuery(this).val());
        });
    };
    var _changeTheme = function (theme) {
        var newTheme = (theme === 1 ? 'vs-dark' : (theme === 0 ? 'vs' : 'hc-black'));
        if (_editor) {
            _editor.updateOptions({ 'theme': newTheme });
        }
        if (diffEditor) {
            diffEditor.updateOptions({ 'theme': newTheme });
        }
    };
    var _createMonaco = function (value, language) {
        language = language || "html";
        value = value || "";
        var container = jQuery("#container");
        if (container.length === 0)
            return top.window.loading(false);

        container.children().remove();
        _editor = monaco.editor.create(container[0], {
            value: value,
            wrappingColumn: 0,
            enableSplitViewResizing: false,
            language: language,
            theme: 'vs-black'
        });
        top.window.loading(false);
    };
    var _monacoInit = function (callback) {
        require.config({ paths: { 'vs': '/js/monaco-editor-0.10.1/package/min/vs' } });
        require(['vs/editor/editor.main'], function () {
            callback && callback();
        });
    };
    var _addresourceLocal = function (data) {
        var temp = function (path, id) {
            var html = '<div class="tpl-table-black-operation">';
            html += ' <a href="javascript:;" data-href="' + path + '" data-fancybox="' + id + '">';
            html += '<i class="am-icon-search-plus"></i>预览';
            html += '</a> <a target="_blank" href="' + path + '"><i class="am-icon-download"></i>下载 </a>';
            html += '&nbsp;<a href="javascript:;" data-resource-id="' + id + '" class="tpl-table-black-operation-del">';
            html += '<i class="am-icon-trash"></i>删除</a></div>';
            return html;
        };
        for (var i = 0; i < data.length; i++) {
            var item = data[i];
            var type;
            if (item.resourceType == "PNG" || item.resourceType == "JPG" || item.resourceType == "JPEG"
                || item.resourceType == "ICON" || item.resourceType == "GIF")
                type = "图片";
            else
                type = "文本";

            var t = '<tr>';
            t += ' <td>' + item.resourceName + '</td>';
            t += ' <td>' + type + '</td>';
            t += ' <td>' + item.updatetime + '</td>';
            t += ' <td>' + temp(item.path, item.id) + '</td>';
            t += '</tr>';
            jQuery("#rsesourceContent").prepend(t);
            _initFancybox(jQuery("a[data-fancybox='" + item.id + "']"));
        }
    };
    var _deleteResource = function () {
        var $this = jQuery(this);
        var relatedTarget = {
            onConfirm: function (target) {
                top.window.loading(true, "删除中...");

                jQuery.post("/cms/DeleteResource", {
                    siteId: jQuery("#site").val(),
                    id: $this.attr('data-resource-id')
                })
                    .success(function () {
                        top.window.loading(false, function () {
                            $this.parent("div").parent("td").parent("tr").remove();
                            top.app.optionMessage = "资源删除成功.";
                            top.app.showMessage();
                        });
                    })
                    .error(function (XMLHttpRequest, textStatus) {
                        top.window.onerror("服务器响应,资源删除出错-错误:" + textStatus, "cms.js", 84)
                    });
            }
        };
        top.window.confirm("是否删除", relatedTarget);

    };
    var _addresource = function (resource) {
        top.window.loading(true, "保存资源中");
        jQuery.post("/cms/AddResource", {
            siteId: jQuery("#site").val(),
            resourcePath: resource
        })
            .success(function (result) {
                if (result.exception) {
                    return top.window.onerror("远程存取出错." + result.exception, "cms.js", 84);
                }
                _addresourceLocal(result);
                top.window.loading(false);
            })
            .error(function (XMLHttpRequest, textStatus) {
                top.window.onerror("服务器响应,存取文件出错-错误:" + textStatus, "cms.js", 84)
            });
    };
    var _fileTempReadEvent = function () {
        var folder = "cms/site/" + jQuery("#site").val();
        jQuery("#fileTemp").click(function () {
            top.window.app.uploaderFile(folder, true, "txt/*", function (result) {
                _createMonaco(result);
            },
                {
                    isRead: true
                });
        });
        jQuery("#fileResource").click(function () {
            top.window.app.uploaderFile(folder, "multiple", "txt/*", function (result) {
                _addresource(result);
            });
        });
    };
    var _siteChange = function () {
        location.href = "/Cms/CmsTemp?siteId=" + $(this).val();
    };
    var _saveTemp = function () {
        var tempName = jQuery("#name").val();
        if (!tempName)
            return top.window.alert("请填写模板名称!!!", function () {
                jQuery("#name").focus();
            });

        var param = {
            tempName: tempName,
            tempType: jQuery("#tempType").val(),
            siteName: jQuery("#siteName").val(),
            siteId: jQuery("#siteId").val(),
            id: jQuery("#tempId").val()
        };
        _validateTempName(param, function () {
            top.window.loadingTip("上传保存中....");
            var formData = new FormData();
            formData.append('tempName', tempName);
            formData.append('tempContent', _editor.getValue());
            formData.append('tempType', param.tempType);
            formData.append('siteName', param.siteName);
            formData.append('siteId', param.siteId);
            formData.append('tempId', param.id);

            var xhr = new XMLHttpRequest();
            xhr.responseType = "text";
            xhr.open('POST', '/cms/ModifyCmsTempView', true);
            xhr.onerror = function (e) {
                var than = this;
                top.window.loading(false, function () {
                    throw Error("传输错误!!!" + than.status);
                });
            };
            xhr.onload = function (e) {
                if (this.status == 200 || this.status == 304) {
                    top.window.loading(false);
                    if (this.responseText === "success") {
                        jQuery("li[class='scouce-active']").find("span").html(jQuery("#name").val());
                        top.app.optionMessage = "模板" + tempName + "保存成功";
                        top.app.showMessage();
                    }
                    else
                        return top.window.onerror("服务器响应,读取文件出错-错误:" + this.responseText, "cms.js", 133);

                }
            };
            xhr.send(formData);
        }, function () {
            jQuery("#name").focus();
        });

    };
    var _dataScouceChange = function () {
        var $this = jQuery(this);
        if ($this.attr("class") === "scouce-active")
            return;
        top.window.loading(true);
        jQuery("li[data-scouce]").removeClass("scouce-active");
        $this.addClass("scouce-active");
        var scouce = $this.attr("data-scouce");

        jQuery("#tempId").val($this.attr("data-temp-id"));
        jQuery("#name").val($this.attr("data-name"));
        jQuery.get("/Cms/QueryTempContent", {
            param: scouce
        })
            .success(function (result) {
                if (typeof result === "object") {
                    return top.window.onerror("服务器响应,读取文件出错-错误:" + result.exception, "cms.js", 162);
                }
                var language = jQuery("#tempType").val();
                if (language == "aspx" || language == "control")
                    language = "html";
                else if (language === "js")
                    language = "javascript";
                _createMonaco(result, language);

            })
            .error(function () {
                top.window.loading(false);
            });
    };
    var _initFancybox = function (elm) {
        if (top.jQuery("a[data-fancybox='" + elm.attr("data-fancybox") + "']").length == 0) {
            var clone = elm.clone();
            clone.each(function () {
                var $this = jQuery(this);
                $this.attr("href", $this.attr("data-href"));
            });
            top.jQuery("body").append(clone.hide());
            top.jQuery("[data-href]").fancybox();
        }     
        elm.click(function () {
            var $this = jQuery(this);
            var href = $this.attr("data-href");
            top.jQuery("[data-href='" + href + "']").click();
        });
    };
    var _deleteTemp = function (event) {
        event.stopPropagation();
        event.preventDefault();
        var $this = jQuery(this);
        var relatedTarget = {
            onConfirm: function (target) {
                top.window.loading(true, "删除中....");
                var param = $this.attr("data-temp-delete").split("|");
                jQuery.post("/Cms/DeleteTemp", {
                    id: param[0],
                    siteId: param[1],
                    tempName: param[2],
                    tempType: param[3]
                })
                    .success(function () {
                        var deleteli = $this.parent("li");
                        if (deleteli.attr("class") === "scouce-active") {
                            var li = deleteli.next("li");
                            if (li.length === 0) {
                                deleteli.parent().html('<li data-name="empty">无对应模板</li>');
                                jQuery("#panelContent").hide();
                            }
                            else
                                li.click();
                        }
                        deleteli.remove();
                        top.window.loading(false, function () {
                            top.app.optionMessage = "删除模板成功...";
                            top.app.showMessage();
                        });
                    })
                    .error(function (XMLHttpRequest, textStatus) {
                        top.window.onerror("服务器响应,删除模板出错-错误:" + textStatus, "cms.js", 262)
                    });
            }
        };
        top.window.confirm("是否删除", relatedTarget);

    };
    var _dataTempChange = function () {
        var $this = jQuery(this);
        if ($this.hasClass("am-active"))
            return;
        jQuery("li[data-temp]").removeClass("am-active");
        var temp = $this.addClass("am-active").attr("data-temp");

        if (temp === "resource") {
            jQuery("#resourceConent").show();
            jQuery("#tempConent").hide();

            return;
        }
        jQuery("#resourceConent").hide();
        jQuery("#tempConent").show();

        jQuery("#empty").hide();
        jQuery("li[data-scouce]").removeClass("scouce-active");

        jQuery("#tempType").val(temp);
        jQuery("ul[data-temp]").hide();

        var that = jQuery("ul[data-temp='" + temp + "']").show();
        if (that.find("[data-name]").first().attr("data-name") === "empty") {
            jQuery("#panelContent").hide();
            jQuery("#empty").show();
            return;
        }
        jQuery("#panelContent").show();
        that.find("[data-scouce]").first().click();
    };
    var _tempViewEvent = function () {
        _fileTempReadEvent();
        jQuery("#site").change(_siteChange);
        jQuery("#saveTemp").click(_saveTemp);
        jQuery("li[data-temp]").click(_dataTempChange);
        jQuery(document).on("click", "a[data-resource-id]", _deleteResource)
            .on("click", "i[data-temp-delete]", _deleteTemp)
            .on("click", "li[data-scouce]", _dataScouceChange);

    };
    var _validateTempName = function (param, callback, error) {
        top.window.loading(true, "验证中....");
        jQuery.post("/cms/ValidateTempName", param)
            .success(function (result) {
                !param.loading && top.window.loading(false);
                if (result.stats === "success")
                    callback && callback(result);
                else {
                    top.window.alert(result.message, function () {
                        error && error();
                    });
                }
            })
            .error(function () {
                top.window.loading(false);
            });;
    };
    var _tempViewInit = function () {
        _selectInit();
        _tempViewEvent();
        _monacoInit(function () {
            jQuery("li[data-temp]").first().click();
        });
        var message = jQuery("[data-message]").attr("data-message");
        if (message) {
            top.app.optionMessage = message;
            top.app.showMessage();
        }
        _initFancybox(jQuery("a[data-fancybox]"));
    };
    var _formInit = function () {
        var message = jQuery("[data-message]").attr("data-message");
        if (message) {
            top.window.alert(message);
        }
        _icon();
    };
    var _formTempInit = function () {
        _selectInit();
        _monacoInit(function () {
            _createMonaco();
        });
        _fileTempReadEvent();
        window.valid = function () {
            jQuery("#content").val(_editor.getValue());
            _validateTempName({
                id: -1,
                siteId: jQuery("[name='SiteId']").val(),
                tempName: jQuery("#tempName").val(),
                tempType: jQuery("#tempType").val()
            }, function () {
                document.forms[0].submit();
            }, function () {
                jQuery("#tempName").focus();
            });
            return false;
        };
    };
    var _pageInit = function () {
        var initType = jQuery("[data-init]").attr("data-init");
        if (initType === "view") {
            _delete();
            _seach();
            top.window.loading(false);
            var message = jQuery("[data-message]").attr("data-message");
            if (message) {
                top.app.optionMessage = message;
                top.app.showMessage();
            }
        }
        else if (initType === "formTemp") {
            top.window.loading(true);
            _formTempInit();
        }
        else if (initType === "tempView") {
            top.window.loading(true);
            _tempViewInit();
        }
        else if (initType === "form") {
            _formInit();
            window.valid = function () {
                var ip = jQuery("#ipFilter").val()||"";
                var ipValue = ip.split(',');
                for (var i = 0; i < ipValue.length; i++) {
                    if (!_isValidIP(ipValue[i])) {
                        top.window.alert("无效的ip,请检查.", function () { jQuery("#ipFilter").focus(); });
                        return false;
                    }
                }
                return true;
            };
        }

    };
    jQuery(function () {
        top.app.closeMessage();
        _pageInit();
    });
})($ || jQuery, window)