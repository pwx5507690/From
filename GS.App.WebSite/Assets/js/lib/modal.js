define(['jQuery', 'app', 'control'], function (jQuery, app, control) {
	var modal = function () {

		'use strict';

		var _self = this;

		var _alertCloseCall;

		var _loadCloseCall;

		var _element = {
			load: "loading",
			alert: "alert",
			message: "message",
			confirm: "confirm",
			conMessage: "conMessage",
			modal: "modal",
			modalTitle: "modalTitle",
			modalContent: "modalContent"
		};

		this.showLoading = function () {
			var load = _self.get(_element.load);
			if (!app.converToBool(load.attr("data-show"))) {
				load.attr("data-show", true).modal('open');
			}
			return this;
		};

		this.closeLoading = function (callback) {
			this.setLoadCloseCall(callback);
			var load = _self.get(_element.load);
			load.attr("data-show", false).modal('close');
			return this;
		};

		this.confirm = function (message, relatedTarget) {
			_self.get(_element.conMessage).html(message);
			_self.get(_element.confirm).modal({
				relatedTarget: relatedTarget,
				onConfirm: function (options) {
					this.relatedTarget.onConfirm && this.relatedTarget.onConfirm(this.relatedTarget);
				},
				onCancel: function () {
					this.relatedTarget.onCancel && this.relatedTarget.onCancel();
				}
			});
		};

		this.setLoadCloseCall = function (loadCloseCall) {
			_loadCloseCall = loadCloseCall
		};

		this.setAlertCloseCall = function (alertCloseCall) {
			_alertCloseCall = alertCloseCall;
		};

		this.alert = function (message) {
			_self.get(_element.message).html(message);
			_self.get(_element.alert).modal('toggle');
			return this;
		};

		this.show = function (param) {
			if (typeof param === "string") {
				_self.get(_element.modal).modal(param);
				return;
			}
			param = jQuery.extend({
				width: 400,
				height: 200,
				closeViaDimmer: false,
				dimmer: true,
				closeOnConfirm: true
			}, param);

			if (param.title) {
				_self.get(_element.modalTitle).html(param.title);
			}
			if (param.content) {
				_self.get(_element.modalContent).html(param.content);
			}
			_self.get(_element.modal).modal(param);
		};

		this.init = function () {
			_self.get(_element.load).on("closed.modal.amui", function () {
				if (_loadCloseCall) {
					_loadCloseCall();
					_loadCloseCall = null;
				}
			});
			_self.get(_element.alert).on("closed.modal.amui", function () {
				if (_alertCloseCall) {
					_alertCloseCall();
					_alertCloseCall = null;
				}
			});
			return this;
		};
	};

	return jQuery.extend(new modal, control.call()).init();
});