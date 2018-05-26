define(['jQuery', 'app'], function (jQuery, app) {
	var userStorage = function () {

		"use strict";

		var _self = this;

		var _key;

		var _getKey = function () {
			_key = _key || app.getGuid();
			return _key;
		};

		this.get = function (emptyCall) {
			var user = localStorage.getItem(_getKey());
			if (!user) {
				var stats = emptyCall && emptyCall();
				if (stats) {
					app.gotoLogin();
				}
				return;
			}
			return JSON.parse(user);
		};

		this.set = function (user) {
			if (typeof user === "object") {
				user = JSON.stringify(user);
			}
			localStorage.setItem(_getKey(), user);
		};

		this.remove = function () {
			localStorage.removeItem(_getKey());
		};
	};
	return new userStorage();
});