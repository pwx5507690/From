define(['jQuery', 'menuServices', 'control', 'app', 'table', 'pagination'],
   function (jQuery, menuServices, control, app, table, pagination) {

   	app.registerRouter("/edit/tempMenu/:tempMenu/:id");

   	var menu = function () {

   		"use strict";

   		var _tbl;

   		var _self = this;

   		var _currentId;

   		var _currentPage;

   		var _queryData;

   		var _menuType = {
   			Group: "Group",
   			Temp: "Temp",
   			Link: "Link"
   		};

   		var _elm = {
   			name: "name",
   			url: "url",
   			id: "id",
   			icon: "icon",
   			sort: "sort",
   			type: "type",
   			add: "addMenu",
   			parent: "parent",
   			menuTable: "menuTable",
   			searchText: "searchText",
   			search: "search",
   			menuPagination: "menuPagination"
   		};

   		var _getQueryMenuHead = function () {
   			return [
				{ name: "parent", title: "上级菜单" },
				{ name: "name", title: "菜单名称" },
				{ name: "url", title: "路径" },
				{ name: "icon", title: "图标" },
   			    { name: "sort", title: "排序" },
			    { name: "type", title: "类型" },
				{ name: "time", title: "修改时间" },
			    { name: "option", title: "编辑" }
   			];
   		};

   		var _getOptionFormat = function (item) {
   			var data = {
   				editPath: "edit/tempMenu/tempMenu.html/{0}".format(item.id),
   				id: item.id
   			};

   			return _self.getTempView("optionTemp", data);
   		};

   		var _getParentFormat = function (item) {
   			var text = item.parentName;
   			if (!app.converToBool(text)) {
   				return "无";
   			}
   			return text;
   		};

   		var _getTimeFormat = function (item) {
   			if (!item.updatetime) {
   				return app.empty;
   			}

   			return new Date(item.updatetime).formatToyyyyMMdd();
   		};

   		var _getQueryMenuData = function (result) {
   			var data = new Array();
   			for (var i = 0; i < result.length; i++) {
   				var item = result[i];
   				var dataObject = new Object();

   				dataObject.parent = _getParentFormat(item);
   				dataObject.name = item.name;
   				dataObject.url = item.url;
   				dataObject.icon = item.icon;
   				dataObject.sort = item.sort;
   				dataObject.type = item.type;
   				dataObject.time = _getTimeFormat(item);
   				dataObject.option = _getOptionFormat(item);
   				data.push(dataObject);
   			}
   			return data;
   		};

   		var _query = function (isInit) {
   			_self.queryPage(config.pagination, _currentPage, function (result) {
   				_queryData = result.result;
   				var data = _getQueryMenuData(_queryData);
   				if (isInit) {
   					var param = {
   						column: _getQueryMenuHead(),
   						data: data,
   						container: _elm.menuTable
   					};
   					_tbl.create(param);
   				} else {
   					_tbl.freshen(data);
   				}
   				_initPagination(result.count);
   			});
   		};

   		var _initPagination = function (total) {
   			var paginationParam = {
   				pagesize: config.pagination,
   				current: _currentPage,
   				total: total,
   				container: _elm.menuPagination,
   				path: "#/tempMenuQuery"
   			};
   			pagination.create(paginationParam);
   		};

   		var _getSendData = function () {
   			return {
   				name: _self.getValue(_elm.name),
   				parent: _self.getValue(_elm.parent),
   				url: _self.getValue(_elm.url) || "default",
   				icon: _self.getValue(_elm.icon),
   				sort: _self.getValue(_elm.sort),
   				type: _self.getValue(_elm.type)
   			};
   		};

   		var _optionCall = function (optionMessage) {
   			app.optionMessage = optionMessage;
   			app.goBack();
   		};

   		var _update = function () {
   			var data = _getSendData();
   			data.id = _currentId;
   			_self.update(data, function (result, param) {
   				if (result.toInt() > 0) {
   					_optionCall("菜单 {0} 修改成功".format(data.name));
   					return;
   				}
   				_self.error("菜单修改失败", param);
   			});
   		};

   		var _add = function () {
   			var data = _getSendData();
   			_self.add(data, function (result, param) {
   				result.toInt() > 0 ?
				_optionCall("菜单{0}添加成功".format(data.name)) :
				_self.error("菜单添加失败", param);
   			});
   		};

   		var _deleteCall = function ($this) {
   			var id = $this.attr("data-delete");
   			_self.delete(id, function (r, param) {
   				if (r.toInt() > 0) {
   					_query();
   					app.optionMessage = "删除成功";
   					app.showMessage();
   				} else {
   					_self.error("删除失败", param);
   				}
   			});
   		};

   		var _delete = function () {
   			var $this = jQuery(this);
   			var relatedTarget = {
   				elm: $this,
   				onConfirm: function (target) {
   					_deleteCall(target.elm);
   				}
   			};
   			window.confirm("是否删除", relatedTarget);
   		};

   		var _fillOptionView = function (result) {
   			_currentId = null;
   			if (!app.param) {
   				return;
   			}
   			var curentItem;

   			for (var i = 0; i < result.length; i++) {
   				var item = result[i];
   				if (+item["id"] === +app.param[1]) {
   					curentItem = item;
   					break;
   				}
   			}
   			if (!curentItem) {
   				return;
   			}
   			_currentId = curentItem["id"];

   			_self.setValue(_elm.name, curentItem.name);
   			_self.setValue(_elm.url, curentItem.url);
   			_self.setValue(_elm.icon, curentItem.icon);
   			_self.setValue(_elm.type, curentItem.type);
   			_self.setValue(_elm.sort, curentItem.sort);
   			delete app.param;
   			return curentItem;
   		};

   		var _createMenuType = function (currentType) {
   			var temp = [];
   			for (var value in _menuType) {
   				var selected = currentType == _menuType[value] ? "selected" : "";
   				temp.push("<option ", selected, " value='", _menuType[value], "'>", value, "</option>");
   			}
                    _self.get(_elm.type).html(temp.join('')).chosen();
   		};

   		var _initOptionView = function () {

   			_self.queryAll(function (result) {
   				var current = _fillOptionView(result);
   				_createMenuType(current && current["type"]);
   				var temp = ["<option value='-1'>无</option>"];

   				for (var i = 0; i < result.length; i++) {
   					var item = result[i];
   					var selected = app.empty;
   					if (current && current["parent"] === item["id"]) {
   						selected = "selected";
   					}
   					if (current && current["id"] === item["id"]) {
   						continue;
   					}
   					temp.push("<option ", selected, " value='", item["id"], "'>", item["name"], "</option>");
   				}

   				_self.get(_elm.parent).html(temp.join(''))
                            .chosen();
   			});
   		};

   		var _search = function () {
   			var value = _self.getValue(_elm.searchText);
   			_currentPage = 1;
   			if (!value) {
   				_query();
   				return;
   			}
   			_self.queryByName(config.pagination, _currentPage, value, function (result) {
   				_queryData = result.result;
   				var data = _getQueryMenuData(_queryData);
   				_tbl.freshen(data);
   				_initPagination(result.count);
   			});
   		};

   		var _initQueryView = function () {
   			_query(true);
   		};

   		var _registerRouterLink = function () {
   			_self.get(_elm.add).attr("href", "#/tempMenu");
   		};

   		this.setEvent = function () {
   			return [{
   				type: "click",
   				selector: "[data-delete]",
   				callback: _delete
   			}, {
   				type: "click",
   				selector: "#{0}".format(_elm.search),
   				callback: _search
   			}];
   		};

   		this.initOptionView = function () {
   			app.closeMessage().valid = function () {
   				if (_currentId == null) {
   					_add();
   				}
   				else {
   					_update();
   				}
   				return false;
   			};
   			_initOptionView();

   			return this;
   		};

   		this.initQueryView = function (currentPage) {
   			_tbl = table();
   			app.showMessage();
   			_currentPage = currentPage;
   			_registerRouterLink();
   			_initQueryView();
   			this.registerEvent();
   			return this;
   		};
   	};
   	var m = jQuery.extend(new menu(), control.call());
   	return jQuery.extend(m, menuServices);
   });
