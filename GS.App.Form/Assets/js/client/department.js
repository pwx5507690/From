define(['jQuery', 'departmentServices', 'control', 'app', 'table', 'pagination'],
   function (jQuery, departmentServices, control, app, table, pagination) {

   	app.registerRouter("/edit/tempDepartment/:tempDepartment/:id");

   	var department = function () {

   		"use strict";

   		var _tbl;

   		var _self = this;

   		var _currentId;

   		var _currentPage;

   		var _queryData;

   		var _elm = {
   			name: "name",
   			code: "code",
   			id: "id",
   			add: "add",
   			parent: "parent",
   			deptTable: "deptTable",
   			searchText: "searchText",
   			search: "search",
   			deptPagination: "deptPagination"
   		};

   		var _getQueryDeptHead = function () {
   			return [
				{ name: "code", title: "部门编号" },
				{ name: "name", title: "部门名称" },
				{ name: "parentName", title: "上级部门" },
				{ name: "time", title: "修改时间" },
				{ name: "option", title: "编辑" }
   			];
   		};

   		var _getOptionFormat = function (item) {
   			var data = {
   				editPath: "edit/tempDepartment/tempDepartment.html/{0}".format(item.id),
   				id: item.id
   			};

   			return _self.getTempView("optionTemp", data);
   		};

   		var _getParentFormat = function (item) {
   			var text = item.parent;
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

   		var _getQueryDeptData = function (result) {
   			var data = new Array();
   			for (var i = 0; i < result.length; i++) {
   				var item = result[i];
   				item.time = _getTimeFormat(item);
   				item.option = _getOptionFormat(item);
   				item.parent = _getParentFormat(item);
   				data.push(item);
   			}
   			return data;
   		};

   		var _query = function (isInit) {
   			_self.queryPage(config.pagination, _currentPage, function (result) {
   				_queryData = result.result;
   				var data = _getQueryDeptData(_queryData);
   				if (isInit) {
   					var param = {
   						column: _getQueryDeptHead(),
   						data: data,
   						container: _elm.deptTable
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
   				container: "deptPagination",
   				path: "#/tempDepartmentQuery"
   			};
   			pagination.create(paginationParam);
   		};

   		var _validDeptName = function (name, callback) {
   			_self.existByName(name, _currentId || -1, function (valid) {
   				if (!app.converToBool(valid)) {
   					return window.loading(false, function () {
   						alert("该部门名称已经存在", function () {
   							_self.getJsElement(_elm.name).focus();
   						});
   					});
   				}
   				callback && callback();
   				return true;
   			});
   		};

   		var _validDeptCode = function (code, callback) {
   			_self.existByCode(code, _currentId || -1, function (valid) {
   				if (!app.converToBool(valid)) {
   					return window.loading(false, function () {
   						alert("该部门编号已经存在", function () {
   							_self.getJsElement(_elm.code).focus();
   						});
   					});
   				}
   				callback && callback();
   				return true;
   			});
   		};

   		var _sendValidForm = function (data, callback) {
   			_validDeptName(data.name, function () {
   				_validDeptCode(data.code, function () {
   					callback && callback();
   				});
   			});
   		};

   		var _getSendData = function () {
   			return {
   				name: _self.getValue(_elm.name),
   				parent: _self.getValue(_elm.parent),
   				code: _self.getValue(_elm.code)
   			};
   		};

   		var _optionCall = function (optionMessage) {
   			app.optionMessage = optionMessage;
   			app.goBack();
   		};

   		var _update = function () {
   			var data = _getSendData();
   			data.id = _currentId;
   			_sendValidForm(data, function () {
   				_self.update(data, function (result, param) {
   					if (result.toInt() > 0) {
   						_optionCall(" {0} 修改成功".format(data.name));
   						return;
   					}
   					_self.error("部门修改失败", param);
   				});
   			});
   		};

   		var _add = function () {
   			var data = _getSendData();
   			_sendValidForm(data, function (r) {
   				_self.add(data, function (result, param) {
   					result.toInt() > 0 ?
					_optionCall("部门 {0} 添加成功".format(data.name)) :
					_self.error("部门添加失败", param);
   				});
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
   			_self.setValue(_elm.code, curentItem.code);
   			delete app.param;
   			return curentItem;
   		};

   		var _initOptionView = function () {
   			_self.query(function (result) {
   				var current = _fillOptionView(result);
   				var temp = ["<option value='1'>无</option>"];

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
   				var data = _getQueryDeptData(_queryData);
   				_tbl.freshen(data);
   				_initPagination(result.count);
   			});
   		};

   		var _initQueryView = function () {
   			_query(true);
   		};

   		var _registerRouterLink = function () {
   			_self.get(_elm.add).attr("href", "#/tempDepartment");
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
   	var dept = jQuery.extend(new department(), control.call());
   	return jQuery.extend(dept, departmentServices);
   });
