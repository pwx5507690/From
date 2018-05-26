define(['jQuery', 'userServices', 'roleServices', 'departmentServices', 'control', 'app', 'table', 'pagination'],
   function (jQuery, userServices, roleServices, departmentServices, control, app, table, pagination) {

   	app.registerRouter("/edit/tempUser/:tempUser/:id");

   	var menu = function () {

   		"use strict";

   		var _tbl;

   		var _self = this;

   		var _currentId;

   		var _defaultPassword = "123456";

   		var _currentPage;

   		var _queryData;

   		var _userSex = {
   			"1": "男",
   			"0": "女"
   		};

   		var _elm = {
   			name: "name",
   			mail: "mail",
   			phone: "phone",
   			department: "department",
   			dropdownCity: "dropdownCity",
   			address: "address",
   			city: "city",
   			dist: "dist",
   			prov: "prov",
   			sex: "sex",
   			age: "age",
   			add: "addUser",
   			rankCode: "rankCode",
   			role: "role",
   			parent: "parent",
   			userTable: "userTable",
   			searchText: "searchText",
   			search: "search",
   			userPagination: "userPagination"
   		};

   		var _getQueryUserHead = function () {
   			return [
				{ name: "departmentName", title: "所属部门" },
				{ name: "name", title: "姓名" },
				{ name: "sex", title: "性别" },
				{ name: "email", title: "邮箱" },
				{ name: "address", title: "地址" },
   			    { name: "phone", title: "手机" },
			    { name: "rankCode", title: "职位" },
			//	{ name: "professionalLevel", title: "职级" },
				{ name: "time", title: "修改时间" },
			    { name: "option", title: "编辑" }
   			];
   		};

   		var _getOptionFormat = function (item) {
   			var data = {
   				editPath: "edit/tempUser/tempUser.html/{0}".format(item.id),
   				id: item.id
   			};

   			return _self.getTempView("optionTemp", data);
   		};

   		var _getParentFormat = function (item) {
   			var text = item.departmentName;
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

   		var _validUserMail = function (email, callback) {
   			_self.existByEmail(email, _currentId || -1, function (valid) {
   				if (!app.converToBool(valid)) {
   					return window.loading(false, function () {
   						alert("该邮箱已经存在", function () {
   							_self.getJsElement(_elm.mail).focus();
   						});
   					});
   				}
   				callback && callback();
   				return true;
   			});
   		};

   		var _validUserPhone = function (phone, callback) {
   			_self.existByPhone(phone, _currentId || -1, function (valid) {
   				if (!app.converToBool(valid)) {
   					return window.loading(false, function () {
   						alert("该手机号已经存在", function () {
   							_self.getJsElement(_elm.phone).focus();
   						});
   					});
   				}
   				callback && callback();
   				return true;
   			});
   		};

   		var _sendValidForm = function (data, callback) {
   			_validUserMail(data.email, function () {
   				_validUserPhone(data.phone, function () {
   					callback && callback();
   				});
   			});
   		};

   		var _initDatepick = function () {
   			_self.get(_elm.age).datepicker()
   		};

   		var _getAddress = function (item) {
   			return "{0} {1} {2} {3}".format(item["province"] || "",
				item["city"] || app.empty, item["dist"] || app.empty, item["address"] || app.empty);
   		};

   		var _getQueryUserData = function (result) {
   			var data = new Array();
   			for (var i = 0; i < result.length; i++) {
   				var item = result[i];
   				var dataObject = new Object();

   				dataObject.departmentName = _getParentFormat(item);
   				dataObject.name = item.name;
   				dataObject.sex = item.sex == "1" ? "男" : "女";
   				dataObject.email = item.email;
   				dataObject.address = _getAddress(item);
   				dataObject.phone = item.phone;
   				dataObject.rankCode = item.rankCode;
   				//dataObject.professionalLevel = item.professionalLevel;
   				dataObject.time = _getTimeFormat(item);
   				dataObject.option = _getOptionFormat(item);
   				data.push(dataObject);
   			}
   			return data;
   		};

   		var _query = function (isInit) {
   			_self.queryPage(config.pagination, _currentPage, function (result) {
   				_queryData = result.result;
   				var data = _getQueryUserData(_queryData);
   				if (isInit) {
   					var param = {
   						column: _getQueryUserHead(),
   						data: data,
   						container: _elm.userTable
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
   				container: _elm.userPagination,
   				path: "#/tempUserQuery"
   			};
   			pagination.create(paginationParam);
   		};

   		var _getSendData = function () {
   			var city = _self.getValue(_elm.city) || app.empty;
   			var country = _self.getValue(_elm.dist) || app.empty;
   			var province = _self.getValue(_elm.prov) || app.empty;
   			var role = [];
   			jQuery("[data-user-role-id]").each(function (i, item) {
   				var $this = jQuery(item);
   				if ($this.prop("checked")) {
   					role.push($this.val());
   				}
   			});
   			return {
   				name: _self.getValue(_elm.name),
   				//password: _defaultPassword,
   				department: _self.getValue(_elm.department),
   				age: _self.getValue(_elm.age),
   				email: _self.getValue(_elm.mail),
   				phone: _self.getValue(_elm.phone),
   				sex: _self.getValue(_elm.sex),
   				province: province,
   				country: country,
   				city: city,
   				rankCode: _self.getValue(_elm.rankCode),
   				address: _self.getValue(_elm.address),
   				role: role
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
   						_optionCall("用户 {0} 修改成功".format(data.name));
   						return;
   					}
   					_self.error("用户修改失败", param);
   				});
   			});
   		};

   		var _add = function () {
   			_currentId = null;
   			var data = _getSendData();
   			_sendValidForm(data, function () {
   				_self.add(data, function (result, param) {
   					result.toInt() > 0 ?
					_optionCall("用户 {0} 添加成功".format(data.name)) :
					_self.error("用户 添加失败", param);
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

   		var _setUserRole = function (callback) {
   			roleServices.queryRole(function (r) {
   				var roleTemp = [];
   				for (var i = 0; i < r.length; i++) {
   					var item = r[i];
   					roleTemp.push(_self.getTempView("tempUserRole", {
   						name: item["name"],
   						value: item["id"]
   					}));
   				}
   				_self.get(_elm.role).html(roleTemp.join(''));
   				callback && callback();
   			});
   		};

   		var _fillOptionView = function (curentItem) {
   			console.error(curentItem);
   			_self.setValue(_elm.name, curentItem.name);
   			_self.setValue(_elm.department, curentItem.department);
   			_self.setValue(_elm.mail, curentItem.email);
   			_self.setValue(_elm.phone, curentItem.phone);
   			if (curentItem.age)
   				_self.setValue(_elm.age,
					curentItem.age.toString().dateFormat("yyyy-MM-dd"));
   			_self.setValue(_elm.rankCode, curentItem.rankCode);
   			return curentItem;
   		};

   		var _fillUserRole = function (r) {
   			for (var i = 0; i < r.length; i++) {
   				var item = r[i];
   				jQuery("[data-user-role-id='{0}']".format(r[i]["role"])).prop("checked", true);
   			}
   		};

   		var _createUesrSex = function (sex) {
   			var temp = [];
   			for (var value in _userSex) {
   				var selected = sex == value ? "selected" : "";
   				temp.push("<option ", selected, " value='", value, "'>", _userSex[value], "</option>");
   			}
   			_self.get(_elm.sex).html(temp.join('')).selected();
   		};

   		var _createInputControl = function (current) {
   			if (current) {
   				_fillOptionView(current);
   			}

   			_initDatepick();
   			_initDropdownCity(current);
   			_createUesrSex(current && current["sex"]);

   			departmentServices.query(function (result) {
   				var temp = ["<option value='-1'>无</option>"];

   				for (var i = 0; i < result.length; i++) {
   					var item = result[i];
   					var selected = app.empty;
   					if (current && current["department"] === item["id"]) {
   						selected = "selected";
   					}
   					temp.push("<option ", selected, " value='", item["id"], "'>", item["name"], "</option>");
   				}

   				_self.get(_elm.department).html(temp.join(''))
					.selected();
   			});
   		};

   		var _initDropdownCity = function (current) {
   			var dropdownCity = _self.get(_elm.dropdownCity);
   			if (current == null) {
   				dropdownCity.citySelect();
   			} else {
   				dropdownCity.citySelect({
   					prov: current["province"],
   					city: current["city"],
   					dist: current["dist"]
   				});
   			}
   		};

   		var _initOptionView = function () {
   			_setUserRole(function () {
   				if (app.param) {
   					_currentId = app.param[1];
   					delete app.param;
   					_self.queryById(_currentId, function (userResult) {
   						_createInputControl(userResult);

   						roleServices.queryUserRoleByUserId(_currentId, _fillUserRole);
   					});

   				} else {
   					_createInputControl();
   				}
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
   				var data = _getQueryUserData(_queryData);
   				_tbl.freshen(data);
   				_initPagination(result.count);
   			});
   		};

   		var _initQueryView = function () {
   			_query(true);
   		};

   		var _registerRouterLink = function () {
   			_self.get(_elm.add).attr("href", "#/tempUser");
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
   	return jQuery.extend(m, userServices);
   });
