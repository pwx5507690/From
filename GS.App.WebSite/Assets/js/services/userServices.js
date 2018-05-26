define(['jQuery', 'baseServices', 'app'], function (jQuery, baseServices, app) {
	var menuServices = function () {

		"use strict";

		var _self = this;

		this.existByEmail = function (email, id, sendCall) {
			var api = "user/existEmail/{0}/{1}".format(email, id);
			this.send(api, this.method.get, null, this.dataType.text, sendCall, function (p, a, b, c) {
				_self.error("department exist code api error ", p);
			});
		};

		this.existByPhone = function (phone, id, sendCall) {
			var api = "user/existPhone/{0}/{1}".format(phone, id);
			this.send(api, this.method.get, null, this.dataType.text, sendCall, function (p, a, b, c) {
				_self.error("department exist name api error ", p);
			});
		};

		this.add = function (data, sendCall) {
			this.send("user/add", this.method.post, data, this.dataType.text, sendCall, function (p, a, b, c) {
				_self.error("user add api error ", p)
			});
		};

		this.update = function (data, sendCall) {
			this.send("user/update", this.method.put, data, this.dataType.text, sendCall, function (p, a, b, c) {
				_self.error("user update api error ", p)
			});
		};

		this.delete = function (deptId, sendCall) {
			var api = "user/delete/{0}".format(deptId);
			this.send(api, this.method.delete, null, this.dataType.text, sendCall, function (p, a, b, c) {
				_self.error("user delete api error ", p)
			});
		};

		this.queryById = function (id,sendCall) {
			this.send("user/{0}".format(id), this.method.get, null, this.dataType.json, sendCall, function (p, a, b, c) {
				_self.error("user query api error ", p);
			});
		};

		this.queryByName = function (page, current, name, sendCall) {
			var api = "user/page/{0}/current/{1}/name/{2}".format(page, current, name);
			this.send(api, this.method.get, null, this.dataType.json, sendCall, function (p, a, b, c) {
				_self.error("user query api error ", p);
			});
		};

		this.queryPage = function (page, current, sendCall) {
			var api = "user/page/{0}/current/{1}".format(page, current);
			this.send(api, this.method.get, null, this.dataType.json, sendCall, function (p, a, b, c) {
				_self.error("user query api error ", p);
			});
		};
	};
	return jQuery.extend(new menuServices, baseServices);
});