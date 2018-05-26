;define(['jQuery'], function (jQuery) {

	'use strict';

	var app = function () {

		var _fileNameExp = /(.*\/)*([^.]+).*/ig;

		var _extensionExp = /.+\./;

		var _unescapeExp = /&(?:nbsp|#160|lt|#60|gt|62|amp|#38|quot|#34|cent|#162|pound|#163|yen|#165|euro|#8364|sect|#167|copy|#169|reg|#174|trade|#8482|times|#215|divide|#247);/g;

		var _unescapeEntity = {
			'&nbsp;': ' ',
			'&#160;': ' ',
			'&lt;': '<',
			'&#60;': '<',
			'&gt;': '>',
			'&62;': '>',
			'&amp;': '&',
			'&#38;': '&',
			'&quot;': '"',
			'&#34;': '"',
			'&cent;': '￠',
			'&#162;': '￠',
			'&pound;': '£',
			'&#163;': '£',
			'&yen;': '¥',
			'&#165;': '¥',
			'&euro;': '€',
			'&#8364;': '€',
			'&sect;': '§',
			'&#167;': '§',
			'&copy;': '©',
			'&#169;': '©',
			'&reg;': '®',
			'&#174;': '®',
			'&trade;': '™',
			'&#8482;': '™',
			'&times;': '×',
			'&#215;': '×',
			'&divide;': '÷',
			'&#247;': '÷'
		};

		var _s4 = function () {
			return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
		};

		this.emptyFunction = new Function();

		this.empty = "";

		this.isEmptyObject = function (obj) {
			for (var key in obj) {
				return false
			};
			return true
		};

		this.converTolocalTime = function (value) {
			var date = new Date();
			var timezoneOffset = date.getTimezoneOffset();
			var localTime = new Date(value).valueOf() + (timezoneOffset * 60000);
			return new Date(localTime);
		};

		this.getGuid = function () {
			return (_s4() + _s4() + "-" + _s4() + "-" + _s4() + "-" + _s4() + "-" + _s4() + _s4() + _s4());
		};

		this.getFilename = function (value) {
			return value.replace(_fileNameExp, "$2");
		};

		this.delExtension = function (value) {
			if (!value) {
				return this.empty;
			}
			return value.substring(0, value.indexOf("."));
		};

		this.getExtension = function (value) {
			return value.replace(_extension, "");
		};

		this.valid = function () {
			return false;
		};

		this.converToBool = function (value) {
			return !(value == 0 || value == "" || value == "0" || value == "false" || value == null);
		};

		this.goBack = function () {
			history.back();
		};

		this.unescapeEntity = function (str) {
			if (!str) {
				return this.empty;
			}
			str = str.toString();
			return str.indexOf(';') < 0 ? str : str.replace(_unescapeExp, function (chars) {
				return _unescapeEntity[chars];
			});
		}
	};

	return new app();
});