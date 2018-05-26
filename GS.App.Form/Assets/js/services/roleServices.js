define(['jQuery', 'baseServices', 'app'], function (jQuery, baseServices, app) {
	var menuServices = function () {

		"use strict";

		var _self = this;

		this.add = function (data, sendCall) {
			this.send("role/add", this.method.post, data, this.dataType.text, sendCall, function (p, a, b, c) {
				_self.error("role add api error ", p)
			});
		};

		this.queryUserRoleByUserId = function (id,sendCall) {
			this.send("role/userRole/{0}".format(id), this.method.get, null, this.dataType.json, sendCall, function (p, a, b, c) {
				_self.error("userRole role api error ", p)
			});
		};

		this.queryRole = function (sendCall) {
			this.send("role", this.method.get, null, this.dataType.json, sendCall, function (p, a, b, c) {
				_self.error("query role api error ", p)
			});
		};

		this.update = function (data, sendCall) {
			this.send("role/update", this.method.put, data, this.dataType.text, sendCall, function (p, a, b, c) {
				_self.error("role update api error ", p)
			});
		};

		this.delete = function (id, sendCall) {
			var api = "role/delete/{0}".format(id);
			this.send(api, this.method.delete, null, this.dataType.text, sendCall, function (p, a, b, c) {
				_self.error("role delete api error ", p)
			});
		};
	
		this.queryRoleById = function (id, sendCall) {
			var api = "role/{0}".format(id);
			this.send(api, this.method.get, null, this.dataType.json, sendCall, function (p, a, b, c) {
				_self.error("role query api error ", p);
			});
		};

		this.queryVRolePageByName = function (page, current, name, sendCall) {
			var api = "role/vRole/page/{0}/current/{1}/name/{2}".format(page, current, name);
			this.send(api, this.method.get, null, this.dataType.json, sendCall, function (p, a, b, c) {
				_self.error("vRole query api error ", p);
			});
		};

		this.queryVRolePage = function (page, current, sendCall) {
			var api = "role/vRole/page/{0}/current/{1}".format(page, current);
			this.send(api, this.method.get, null, this.dataType.json, sendCall, function (p, a, b, c) {
				_self.error("vRole query api error ", p);
			});
		};
	};
	return jQuery.extend(new menuServices, baseServices);
});