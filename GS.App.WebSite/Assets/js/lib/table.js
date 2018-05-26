define(['jQuery', 'app', 'control'], function (jQuery, app, control) {
	var table = function () {

		"use strict";

		var _self = this;

		var _con = control();

		var _column;

		var _table;

		var _calcWidth = function (column, item) {
			return item["width"] ? item["width"] + "px" : 100 / column.length + "%";
		};

		var _setHeadClick = function (item) {
			//column = [{ name: "", key: "", width: "" }]
			_con.event("click", "data-head", function () {
				var key = jQuery(this).attr("data-head");
				_self.headClick && _self.headClick(key);
			});
			return "data-head='{0}'".format(item["name"]);
		};

		var _dataNameInHead = function (name, column) {
			for (var i = 0; i < column.length; i++) {
				if (column[i]["name"] === name) {
					return true;
				}
			}
			return false;
		};

		var _createEmptyBody = function (column) {
			return "<tr><td colspan='{0}'>未查询到相符数据</td></tr>".format(column.length)
		};

		var _createBody = function (data, column) {
			if (data == null || data.length == 0) {
				return _createEmptyBody(column);
			}
			var temp = new Array();
			temp.push("<tbody>");
			for (var i = 0; i < data.length; i++) {
				var item = data[i];
				var cla = i % 2 == 0 ? "gradeX" : "even gradeC";
				temp.push('<tr class=', cla, ' data-row=', JSON.stringify(item), '>');
				for (var value in item) {
					if (!_dataNameInHead(value, column)) {
						continue;
					}
					var text = item[value];
					//if (value === "value") {
					//	temp.push('<td>', text, '</td>');
					//}
					temp.push('<td>', text, '</td>');
				}
				temp.push("</tr>");
			}
			temp.push("</tbody>");
			return temp.join('');
		};

		var _createHead = function (column) {
			var content = new Array();
			content.push('<thead><tr>');
			for (var i = 0; i < column.length; i++) {
				var item = column[i];
				if (!item["name"]) {
					continue;
				}
				var width = _calcWidth(column, item);
				var click = _setHeadClick(item);
				content.push('<th ', click, ' width=', width, '>', column[i]["title"], '');
			}
			content.push('</tr></thead>');
			return content.join('');
		};

		this.headClick = function (key) {
			return null;
		};

		this.create = function (param) {
			_column = param.column;
			var temp = new Array();
			temp.push(_createHead(param.column));
			temp.push(_createBody(param.data, param.column));
			var container = _con.get(param.container).empty();
			_table = jQuery("<table>".format(app.getGuid()))
				.addClass("am-table am-table-compact am-table-striped tpl-table-black").html(temp.join(''));
			_table.appendTo(container);
			return this;
		};

		this.freshen = function (data) {
			_table.find("tbody").remove();
			_table.append(_createBody(data, _column));
		};
	};

	return function () {
		return new table();
	};
});