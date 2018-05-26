define(['jQuery', 'modal', 'app'], function (jQuery, modal, app) {
	Date.prototype.format = function (fmt) { //author: meizz 
		var o = {
			"M+": this.getMonth() + 1, //月份 
			"d+": this.getDate(), //日 
			"h+": this.getHours(), //小时 
			"m+": this.getMinutes(), //分 
			"s+": this.getSeconds(), //秒 
			"q+": Math.floor((this.getMonth() + 3) / 3), //季度 
			"S": this.getMilliseconds() //毫秒 
		};
		if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
		for (var k in o)
			if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
		return fmt;
	};

	Date.prototype.formatToyyyyMMdd = function () {
		return app.converTolocalTime(this).format("yyyy年MM月dd日");
	};

	Array.prototype.getFirstOrDefault = function () {
		if (this.length > 0) {
			return this[0];
		}
		return null;
	};

	String.prototype.dateFormat = function (format) {
		return app.converTolocalTime(new Date(this)).format(format);
	};

	String.prototype.dateFormatToyyyyMMdd = function () {
		return new Date(this).formatToyyyyMMdd();
	};

	String.prototype.format = function (args) {
		var argsArray;
		var reg;
		var value = this.toString();
		if (arguments.length == 1 && typeof (args) == "object" && !(args instanceof Array)) {
			argsArray = args;
			reg = function (key) {
				return new RegExp("({" + key + "})", "g");
			};
		}
		else {
			argsArray = args instanceof Array ? args : Array.prototype.slice.call(arguments);
			reg = function (i) {
				return new RegExp("({)" + i + "(})", "g");
			};
		}
		for (var key in argsArray) {
			if (argsArray[key] != undefined) {
				value = value.replace(reg(key), argsArray[key]);
			}
		}
		return value;
	};

	String.prototype.isNullorEmpty = function () {
		return (this == "" || this == null);
	};

	String.prototype.isNotNullorEmpty = function () {
		return !this.isNullorEmpty();
	};

	String.prototype.toInt = function () {
		return parseInt(this);
	};

	String.prototype.toFloat = function () {
		return parseFloat(this);
	};

	window.log = function () {
		if (config.state !== "debug") {
			return;
		}
		var mes = arguments[0];
		if (arguments.length > 1) {
			var argsArray = Array.prototype.slice.call(arguments);
			argsArray.shift();
			mes = mes.format(argsArray);
		}
		console.log(mes);
	};

	window.alert = function (message, callback) {
		if (callback)
			modal.setAlertCloseCall(callback);
		modal.alert(message);
		return this;
	};

	window.loading = function (isShow, closeCall) {
		if (isShow)
			modal.showLoading();
		else
			modal.closeLoading(closeCall);
		return this;
	};

	window.confirm = function (message, relatedTarget) {
		modal.confirm(message, relatedTarget);
		return this;
	};

	window.modal = function (param) {
		modal.show(param);
		return this;
	};

	window.onerror = function (msg, url, l) {
		jQuery.AMUI.progress.done();
		this.loading(false);
		var message = this.exceptMessage;
		message = !message ? "未知错误,请联系管理员!" : message;
		this.alert(message).log("错误 \n\n 消息:{0}\n 路径:{1}\n 所在行:{2}\n\n", msg, url, l);
		this.exceptMessage = app.empty;
		return false;
	};
});