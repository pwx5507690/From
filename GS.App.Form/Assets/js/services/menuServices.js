define(['jQuery', 'baseServices', 'app'], function (jQuery, baseServices, app) {
	var menuServices = function () {

		"use strict";

		var _self = this;
		
		this.add = function (data, sendCall) {
			this.send("menu/add", this.method.post, data, this.dataType.text, sendCall, function (p, a, b, c) {
				_self.error("menu add api error ", p)
			});
		};

		this.update = function (data, sendCall) {
			this.send("menu/update", this.method.put, data, this.dataType.text, sendCall, function (p, a, b, c) {
				_self.error("menu update api error ", p)
			});
		};

		this.delete = function (id, sendCall) {
			var api = "menu/delete/{0}".format(id);
			this.send(api, this.method.delete, null, this.dataType.text, sendCall, function (p, a, b, c) {
				_self.error("menu delete api error ", p)
			});
		};

		this.queryAll = function (sendCall) {
			this.send("menu/query", this.method.get, null, this.dataType.json, sendCall, function (p, a, b, c) {
				_self.error("menu query api error ", p);
			});
		};

		this.queryByName = function (page, current, name, sendCall) {
			var api = "menu/page/{0}/current/{1}/name/{2}".format(page, current, name);
			this.send(api, this.method.get, null, this.dataType.json, sendCall, function (p, a, b, c) {
				_self.error("menu query api error ", p);
			});
		};

		this.queryPage = function (page, current, sendCall) {
			var api = "menu/page/{0}/current/{1}".format(page, current);
			this.send(api, this.method.get, null, this.dataType.json, sendCall, function (p, a, b, c) {
				_self.error("menu query api error ", p);
			});
		};
        this.queryByCurrentUser = function (sendCall) {
            this.send("menu/currentUser", this.method.get, null, this.dataType.json, sendCall, function (p, a, b, c) {
                _self.error("department query api error ", p);
            });
        };
		this.query = function (sendCall) {
			this.send("menu", this.method.get, null, this.dataType.json, sendCall, function (p, a, b, c) {
				_self.error("department query api error ", p);
			});
		};

	};
	return jQuery.extend(new menuServices, baseServices);
});