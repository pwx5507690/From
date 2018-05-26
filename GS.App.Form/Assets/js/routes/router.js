define(["jQuery", "app"], function (jQuery, app) {

	"use strict";

	var _routes = {};

	var _root;

	var _routesMap = {};

	var _routerKey = "router";

	var _setCurrentMenu;

	var _rootPath = function () {
		var url = window.location.href;
		if (url.indexOf("#") === -1) {
			_setCurrentMenu();
			_root();
		}
	};

	var _routerCallback = function (temp) {	
		jQuery.AMUI.progress.done();
		this.container.html(temp);
		this.run();
		_setCurrentMenu();
	};

	var _loadPage = function (path) {
		// loading start
		jQuery.AMUI.progress.start();
		jQuery.get(path, function (temp) {
			_routerCallback.call(app, temp);
		});
	};

	var _getPath = function (path) {
		return '/{0}/{1}.html'.format(config.baseTempUrl, path);
	}

	var _setRouterConfigByMenu = function (menuData) {
		for (var i = 0; i < menuData.length; i++) {
			var fileName = app.getFilename(menuData[i]["name"]);
			var roteName = "/:temp";
            
			// routes config
            _routes[roteName] = function (path) {      
				_loadPage(_getPath(path));
			};
			roteName = "{0}/:page".format(roteName);
			// routes config
			_routes[roteName] = function (path, page) {
                app.page = page;
				_loadPage(_getPath(path));
			};			
		}
		_setRouterStorage(_routes);
		
	};

	var _init = function (menuData) {
		_setRouterConfigByMenu(menuData);

		app.router = Router(_routes);
		app.router.configure({
			notfound: _rootPath,
			strict: false
		});

		app.router.init();
		return app;
	};

	var _addRouterStorage = function (routerName) {
		if (_routesMap[routerName] == null) {
			_routesMap[routerName] = routerName;
			localStorage.setItem(_routerKey, JSON.stringify(_routesMap));
		} else {
			return true;
		}

	};

	var _setRouterStorage = function () {
		var value = localStorage.getItem(_routerKey);
		
		if (!value) {
			return;
		}
		
		_routesMap = JSON.parse(value);
		for (var name in _routesMap) {
			_routes[name] = function () {
				app.param = Array.prototype.slice.apply(arguments);
				var path = '/{0}/{1}'.format(config.baseTempUrl, app.param[0]);
				_loadPage(path);
			};
		}

	};

	app.registerRouter = function (routerName) {
		if (_addRouterStorage(routerName)) {
			return;
		}
		this.router.on(routerName, function () {
			app.param = Array.prototype.slice.apply(arguments);
			jQuery.AMUI.progress.start();
			var path = '/{0}/{1}'.format(config.baseTempUrl,app.param[0]);
			jQuery.get(path, function (temp) {
				_routerCallback.call(app, temp);
			});
		});
	};

	return function (menuData, root, setCurrentMenu) {
		_setCurrentMenu = setCurrentMenu;
		_root = root;
		return _init(menuData);
	};
});