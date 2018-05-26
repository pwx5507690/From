define(["jQuery", 'app'], function (jQuery, app) {

	'use strict';

	var _runParam = {};

	var _elm = {
		optionMessage: "optionMessage"
	};

	app.showMessage = function () {
		var message = jQuery(document.getElementById(_elm.optionMessage));
		if (this.optionMessage) {
			message.show().find("p").html(this.optionMessage);
			this.optionMessage = this.empty;
		} else {
			message.hide();
		}
		return this;
	};

	app.closeMessage = function () {
		this.optionMessage = this.empty;
		jQuery(document.getElementById(_elm.optionMessage)).hide();
		return this;
	};

	app.setDefaultPageCall = function () {
		for (var name in config.page) {
			this[name] = this.emptyFunction;
		}
	};

	app.on = function (type, callback) {
		if (type in config.page) {
			this[type] = callback;
		}
	};

	app.run = function () {
		this.location = null;
		var func = jQuery("run").attr("data-load");
		if (func in this) {
			typeof this[func] == "function" && this[func].call(this, _runParam[func]);
		}
	};

	(function registeredPage(app) {

		for (var i = 0; i < config.runPageConfig.length; i++) {
			var item = config.runPageConfig[i];

			config.page[item["view"]] = item["view"];
			_runParam[item["view"]] = item;

			app.on(config.page[item["view"]], function (param) {
				this.location = param.location;

				var exec = function (page) {
					if (this[param.storageName]) {
						return this[param.storageName][param.method](page);
					}
					require([param.controller], function (obj) {
						app[param.storageName] = obj[param.method](page);
					});
				};
				var page = this.page || 1;
				if (param.type === "query") {
					delete this.page;
				}
				exec.call(this, page);
			});
		}
	})(app);
});
