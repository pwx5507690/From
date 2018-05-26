define(['jQuery', 'roleServices', 'menuServices', 'control', 'app', 'table', 'pagination'],
   function (jQuery, roleServices, menuServices, control, app, table, pagination) {

   	app.registerRouter("/edit/tempRole/:tempRole/:id");

   	var menu = function () {

   		"use strict";

   		var _tbl;

   		var _self = this;

   		var _defaultRoleName = "管理员";

   		var _currentId;

   		var _currentPage;

   		var _queryData;

   		var _elm = {
   			name: "name",
   			add: "addRole",
   			menuModel: "menuModel",
   			roleTable: "roleTable",
   			searchText: "searchText",
   			search: "search",
   			rolePagination: "rolePagination"
   		};

   		var _getQueryRoleHead = function () {
   			return [
				{ name: "name", title: "角色名称" },
				{ name: "menuName", title: "菜单模块" },
				{ name: "time", title: "修改时间" },
			    { name: "option", title: "编辑" }
   			];
   		};

   		var _getOptionFormat = function (item) {
   			var data = {
   				editPath: "edit/tempRole/tempRole.html/{0}".format(item.id),
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

   		var _getQueryRoleData = function (result) {

   			var data = new Array();
   			for (var i = 0; i < result.length; i++) {
   				var item = result[i];
   				var dataObject = new Object();

   				dataObject.name = item.name;
   				dataObject.menuName = item.menuName;
   				dataObject.time = _getTimeFormat(item);
   				dataObject.option = _getOptionFormat(item);
   				//if (dataObject.name === "管理员") {

   				//} else {
   				//	dataObject.option = app.empty;
   				//}		
   				data.push(dataObject);
   			}
   			return data;
   		};

   		var _getMenu = function (id, menuRole) {
   			var result = [];
   			for (var i = 0; i < menuRole.length; i++) {
   				if (+menuRole[i]["role"] == +id) {
   					result.push(menuRole[i]["menuName"]);
   				}
   			}
   			return result.join(' , ');
   		};

   		var _fillRoleData = function (result) {
   			var menuRole = result.menuRole;
   			var role = result.role.result;
   			for (var i = 0; i < role.length; i++) {
   				var item = role[i];
   				item["menuName"] = _getMenu(item["id"], menuRole);
   			}
   		};

   		var _query = function (isInit) {
   			_self.queryVRolePage(config.pagination, _currentPage, function (result) {
   				_fillRoleData(result);
   				_queryData = result.role.result;
   				var data = _getQueryRoleData(_queryData);
   				if (isInit) {
   					var param = {
   						column: _getQueryRoleHead(),
   						data: data,
   						container: _elm.roleTable
   					};
   					_tbl.create(param);
   				} else {
   					_tbl.freshen(data);
   				}
   				_initPagination(result.role.count);
   			});
   		};

   		var _initPagination = function (total) {
   			var paginationParam = {
   				pagesize: config.pagination,
   				current: _currentPage,
   				total: total,
   				container: _elm.rolePagination,
   				path: "#/tempRoleQuery"
   			};
   			pagination.create(paginationParam);
   		};

   		var _getSendData = function () {
   			var menu = [];
   			jQuery("[data-menu-id]").each(function (i, item) {
   				var $this = jQuery(item);
   				if ($this.prop("checked")) {
   					menu.push($this.val());
   				}
   			});

   			return {
   				menuId: menu,
   				name: _self.getValue(_elm.name),
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
   					_optionCall("角色 {0} 修改成功".format(data.name));
   					return;
   				}
   				_self.error("角色修改失败", param);
   			});
   		};

   		var _add = function () {
   			var data = _getSendData();
   			_self.add(data, function (result, param) {
   				result.toInt() > 0 ?
				_optionCall("角色 {0} 添加成功".format(data.name)) :
				_self.error("角色添加失败", param);
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

   		var _fillOptionView = function (curentItem) {
   			if (!curentItem) {
   				return;
   			}
   			_self.setValue(_elm.name, curentItem.name);

   			return curentItem;
   		};

   		var _setMenuModel = function (models) {
   			for (var i = 0; i < models.length; i++) {
   				jQuery("[data-menu-id='{0}']".format(models[i]["menu"])).prop("checked", true);
   			}
   		};

   		var _createMenucheckBox = function (menuResult) {
   			var menuCheckTemp = [];
   			for (var i = 0; i < menuResult.length; i++) {
   				var item = menuResult[i];
   				var menu = item.menu;
   				menuCheckTemp.push(_self.getTempView("tempMenuModel", {
   					value: -1,
   					name: item["name"]
   				}));

   				menuCheckTemp.push("<br><div style='height:10px'/>");
   				for (var n = 0; n < menu.length; n++) {
   					menuCheckTemp.push(_self.getTempView("tempMenuModel", {
   						value: menu[n]["id"],
   						name: menu[n]["name"]
   					}));

   				}
   				menuCheckTemp.push("<br><div style='height:10px'/>");
   			}
   			_self.get(_elm.menuModel).html(menuCheckTemp.join(''));
   		};

   		var _initOptionView = function () {

   			menuServices.query(function (menuResult) {
   				_createMenucheckBox(menuResult);

   				if (!app.param) {
   					_currentId = null;
   					return;
   				}
   				_currentId = +app.param[1];
   				delete app.param;

   				_self.queryRoleById(_currentId, function (result) {
   					_fillOptionView(result.role.result.getFirstOrDefault());
   					_setMenuModel(result.menuRole);
   				});
   			});
   		};

   		var _search = function () {
   			var value = _self.getValue(_elm.searchText);
   			_currentPage = 1;
   			if (!value) {
   				_query();
   				return;
   			}
   			_self.queryVRolePageByName(config.pagination, _currentPage, value, function (result) {
   				_fillRoleData(result);
   				_queryData = result.role.result;
   				var data = _getQueryRoleData(_queryData);
   				_tbl.freshen(data);
   				_initPagination(result.role.count);
   			});
   		};

   		var _initQueryView = function () {
   			_query(true);
   		};

   		var _registerRouterLink = function () {
   			_self.get(_elm.add).attr("href", "#/tempRole");
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
   	return jQuery.extend(m, roleServices);
   });
