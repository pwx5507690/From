define(['jQuery', 'baseServices', 'app'], function (jQuery, baseServices, app) {
	var departmentServices = function () {

		"use strict";

		var _self = this;

		this.existByCode = function (code, id, sendCall) {
			var api = "department/existCode/{0}/{1}".format(code, id);
			this.send(api, this.method.get, null, this.dataType.text, sendCall, function (p, a, b, c) {
				_self.error("department exist code api error ", p);
			});
		};

		this.existByName = function (name, id, sendCall) {
			var api = "department/existName/{0}/{1}".format(name, id);
			this.send(api, this.method.get, null, this.dataType.text, sendCall, function (p, a, b, c) {
				_self.error("department exist name api error ", p);
			});
		};

		this.add = function (data, sendCall) {
			this.send("department/add", this.method.post, data, this.dataType.text, sendCall, function (p, a, b, c) {
				_self.error("department add api error ", p);
			});
		};

		this.update = function (data, sendCall) {
			this.send("department/update", this.method.put, data, this.dataType.text, sendCall, function (p, a, b, c) {
				_self.error("department update api error ", p);
			});
		};

		this.delete = function (deptId, sendCall) {
			var api = "department/delete/{0}".format(deptId);
			this.send(api, this.method.delete, null, this.dataType.text, sendCall, function (p, a, b, c) {
				_self.error("department delete api error ", p);
			});
		};

		this.queryPage = function (page, current, sendCall) {
			var api = "department/page/{0}/current/{1}".format(page, current);
			this.send(api, this.method.get, null, this.dataType.json, sendCall, function (p, a, b, c) {
				_self.error("department query api error ", p);
			});
		};

		this.query = function (sendCall) {
			this.send("department", this.method.get, null, this.dataType.json, sendCall, function (p, a, b, c) {
				_self.error("department query api error ", p);
			});
		};

		this.queryById = function (deptId) {
			this.send("department/{0}".format(deptId), this.method.get, null, this.dataType.json, sendCall, function (p, a, b, c) {
				_self.error("department query api error ", p);
			});
		};

		this.queryByName = function (page, current, name, sendCall) {
			var api = "department/page/{0}/current/{1}/name/{2}".format(page, current, name);
			this.send(api, this.method.get, null, this.dataType.json, sendCall, function (p, a, b, c) {
				_self.error("department query api error ", p);
			});
		};
	};
	return jQuery.extend(new departmentServices, baseServices);
});