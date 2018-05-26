define(['jQuery', 'app', 'underscore'], function (jQuery, app, _) {

	'use strict';

	var eventMap = {};

	var control = function () {

		this.map = {};

		this.getTarget = function (name, type) {
			type = (type ? type : "").toUpperCase();
			return type === "CLASS" && "." + name || type === "ATTR" && "[" + name + "]" || "#" + name;
		};

		this.get = function (name, isNotModifyCache) {
			//isNotModifyCache = isNotModifyCache == null ? true : isNotModifyCache;
			if (typeof name === "object") {
				var map = new Object();
				for (var key in name) {
					((key in this.map) && isNotModifyCache) ? map[key] = this.map[key] : (typeof name[key] === "object") ? map[name[key]["name"]] = this.map[name[key]] = jQuery(this.getTarget(name[key]["name"], name[key]["type"])) : map[key] = this.map[key] = jQuery(this.getTarget(name[key]));
				}
				return map;
			}
			(!(name in this.map) || !isNotModifyCache) && (this.map[name] = jQuery(this.getTarget(name)));
			return this.map[name];
		};

		this.getByClass = function (className, isNotModifyCache) {
			return this.get({
				className: {
					name: className,
					type: "class"
				}
			}, isNotModifyCache)[className];
		};

		this.getTemp = function (name) {
			return this.get(name).html();
		};

		this.getTempView = function (name, data) {
			return _.template(this.getTemp(name))(data);
		};

		this.registerEvent = function () {
			var name = "setEvent";
			if (!(typeof this[name] === "function")) {
				return;
			}
			var event = this[name].call();
			if (event == null) {
				return;
			}
			for (var i = 0; i < event.length; i++) {
				this.event(event[i].type, event[i].selector, event[i].callback);
			}
		};

		this.getValue = function (name) {
			return this.get(name).val();
		};

		this.setValue = function (name, value) {
			this.get(name).val(value);
			return this;
		};

		this.clearMap = function () {
			this.map = new Object();
		};

		this.getJsElement = function (name) {
			return this.get(name)[0];
		};

		this.valid = function (checked) {
			app.valid = function () {
				return checked;
			};
		};

		this.event = function (type, selector, callback) {
			if (selector in eventMap) {
				jQuery(document).off(type,selector);
			} else {
				eventMap[selector] = selector;
			}
			jQuery(document).on(type, selector, callback);
		};

	};
	return function () {
		return new control();
	};
});