define(["jQuery", 'app'], function (jQuery, app) {
	var baseServices = function () {

		"use strict";

		var _self = this;

		var _baseUrl = config.baseApiUrl;

		var _method = config.http.method;

		var _dataType = config.http.dataType;

		var _getheaders = function () {
			var token = app.getToken();
			return {
				"Authorization": "Bearer " + token
			};
		};

		var _setSendParam = function (api, type, data, dataType) {
			var headers = _getheaders();
			return {
				url: _baseUrl + api,
				dataType: dataType,
				data: data,
				headers: headers,
				type: type
			};
		};

		this.dataType = _dataType;

		this.method = _method;

		this.setApiUrl = function (api) {
			if (api.isNullorEmpty()) {
				_baseUrl = config.baseApiUrl;
				return _baseUrl;
			}
			_baseUrl = api;
			return this;
		};

		this.error = function (mesage, p) {
			var data = app.empty;
			if (p.data) {
				data = JSON.stringify(p.data);
			}
			window.exceptMessage = "{0} url:{1}，data:{2} ".format(mesage, p.url, data);
			throw Error(mesage);
		};

		this.send = function (api, type, data, dataType, success, error) {
			var param = _setSendParam(api, type, data, dataType);
			window.loading.call(jQuery, true)
				.ajax(param).done(function (result) {
					var stats = success && success(result, param);
					if (!stats) {
						window.loading(false);
					}
				})
				.fail(function (a, b, c) {		
					if (+a.status === 401) {
						app.gotoLogin();
						return;
					}
					window.loading(false);
					error && error(param, a, b, c);
				});
			return this;
		};
	};
	return new baseServices();
});