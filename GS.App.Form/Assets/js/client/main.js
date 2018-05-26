define(['jQuery', 'control', 'app'],
    function (jQuery, control, app) {

        'use strict';

        var main = function () {
            var _self = this;

            var _menuLoadView = {};

            var _elm = {
                container: "container",
                menuContainer: "menuContainer"
            };

            var _temp = {
                menuItem: "tempMenu"
            };

            var _getUrl = function (url, type) {
                console.log(type);
                if (type === "Temp") {
                    url = "#/{0}".format(app.delExtension(url));
                } else if (type === "Link") {
                    url = "#/tempLink?href={0}".format(url);
                }
                return url;
            };

            var _getFirstDefaultView = function () {
                for (var name in _menuLoadView) {
                    if (_menuLoadView[name]["def"]) {
                        return _menuLoadView[name];
                    }
                }
            };

            var _getMenuGroupTemp = function (name, eName, url, type) {
                if (type !== "Group") {
                    return _getMenuItemTemp(name, eName, url, type);
                }

                return _self.getTempView(_temp.menuItem, {
                    liClass: "sidebar-nav-heading",
                    aClass: "sidebar-nav-heading-info",
                    url: "javascript:;",
                    title: name,
                    id: app.getGuid(),
                    eTitle: eName,
                    icon: app.empty
                });
            };

            var _getMenuItemTemp = function (name, icon, url, type) {
                var loadPath = _getUrl(url, type);
                var id = app.getGuid();

                var isEmpty = app.isEmptyObject(_menuLoadView);

                if (_menuLoadView[id] == null) {
                    _menuLoadView[id] = {
                        url: url,
                        def: isEmpty
                    };
                }

                return _self.getTempView(_temp.menuItem, {
                    liClass: "sidebar-nav-link",
                    aClass: isEmpty ? "active" : "",
                    id: id,
                    url: loadPath,
                    title: name,
                    eTitle: app.empty,
                    icon: icon
                });
            };

            var _createMenu = function (menuData) {
                console.log(menuData);
                var temp = new Array();
                for (var i = 0; i < menuData.length; i++) {
                    var group = menuData[i];
                    temp.push(_getMenuGroupTemp(group.name, group.eName, group.url, group.type));
                    var menu = group["menu"];
                    for (var n = 0; n < menu.length; n++) {
                        var item = menu[n];
                        temp.push(_getMenuItemTemp(item.name, item.icon, item.url, item.type));
                    }
                }
                return _self.get(_elm.menuContainer).html(temp.join(''));
            };

            var _createFirstView = function () {
                //var defaultView = _getFirstDefaultView();
                //var path = '/{0}/{1}'.format(config.baseTempUrl, defaultView.url);
                //jQuery.get(path, function (temp) {
                //	app.container.html(temp);
                //	//app.run();
                //	_self.setSelectedMenu();
                //});
            };

            var _sidebarTitle = function () {
                jQuery(this).siblings('.sidebar-nav-sub').slideToggle(80).end()
                    .find('.sidebar-nav-sub-ico').toggleClass('sidebar-nav-sub-ico-rotate');
            };

            var _contentBar = function () {
                var color = jQuery(this).attr('data-color');
                jQuery('body').attr('class', color)
                app.saveSelectColor.Color = color;
                app.storageSave(app.saveSelectColor);
            };

            var _toggle = function () {
                jQuery('.tpl-skiner').toggleClass('active');
            };

            var _setMenuSelected = function () {
                jQuery(".sidebar-nav-link a").removeClass("active");
                jQuery(this).addClass("active");
            };

            this.setSelectedMenu = function () {
                if (!app.location)
                    return;
                var location = app.location;
                var $a = jQuery(".sidebar-nav-link a").removeClass("active");
                for (var i = 0; i < $a.length; i++) {
                    var $item = jQuery($a[i]);
                    if ($item.attr("href").replace("#/", "") == location) {
                        $item.addClass("active");
                        return;
                    }
                }
                $a.first().addClass("active");
            };

            this.setEvent = function () {
                return [{
                    type: "click",
                    selector: ".tpl-skiner-toggle",
                    callback: _toggle
                }, {
                    type: "click",
                    selector: "[data-color]",
                    callback: _contentBar
                }, {
                    type: "click",
                    selector: ".sidebar-nav-sub-title",
                    callback: _sidebarTitle
                }, {
                    type: "click",
                    selector: ".sidebar-nav-link a",
                    callback: _setMenuSelected
                }];
            };

            this.getMenuView = function () {
                return _menuLoadView;
            };

            this.GetFirstView = function () {
                return _createFirstView;
            };

            this.getContainer = function () {
                return this.get(_elm.container);
            };

            this.init = function (menuData) {
                _createMenu(menuData);
                _createFirstView();

                this.registerEvent();
                return this;
            };
        };
        return jQuery.extend(new main(), control.call());
    });
