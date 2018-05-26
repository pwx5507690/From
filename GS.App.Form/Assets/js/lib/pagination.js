define(['jQuery', 'app', 'control'], function (jQuery, app, control) {

	var pagination = function () {

		'use strict';

		var _num = 12;

		var _calcPagination = function (pagesize, total) {
			var pagination = parseInt(total / pagesize);
			var remainder = total % pagesize;
			if (remainder == 0) {
				return pagination;
			}
			return pagination + 1;
		};

		var _getCurrentinGroup = function (current) {
			return _calcPagination(_num, current);
		};

		var _getGroupLoaction = function (current, pagination) {
			var group = pagination / _num;
			if (pagination % _num != 0) {
				group = group + 1;
			}
		};

		var _createFirst = function (temp, path, current) {
			if (current == 1) {
				temp.push('<li class="am-disabled"><a href="#">«</a></li>');
			} else {
				temp.push('<li><a href="', "{0}/{1}".format(path, (+current) - 1), '">«</a></li>');
			}
		};

		var _createLast = function (temp, path, current, pagination) {
			if (current == pagination) {
				temp.push('<li class="am-disabled"><a href="#">»</a></li>');
			} else {
				temp.push('<li><a href="', "{0}/{1}".format(path, (+current) + 1), '">»</a></li>');
			}
		};

		this.create = function (param) {
			var container = this.get(param.container).empty();
			if (!app.converToBool(param.total)) {
				return;
			}
			var path = param.path;
			var current = param.current;
			var pagination = _calcPagination(param.pagesize, param.total);
			var currentGroup = _getCurrentinGroup(param.current);
			var temp = new Array();
			var index = ((currentGroup - 1) * _num) + 1;	
			var last = (pagination > _num ? ((currentGroup) * _num) : pagination);

			_createFirst(temp, path, current);
			if (index > _num) {
				temp.push('<li><a href="', path, '/', (index - 1), '">...</a></li>');
			}

			for (var i = index; i < last + 1; i++) {

				var className = current == i ? "am-active" : "";
				if (i > pagination) {
					break;
				}
				temp.push('<li class=', className, '><a href="', path, '/', i, '">', i, '</a></li>');
			}

			if (pagination > last) {
				temp.push('<li><a href="', path, '/', (last + 1), '">...</a></li>');
			}
			_createLast(temp, path, current, pagination);

			jQuery("<ul>")
				.addClass("am-pagination tpl-pagination")
				.html(temp.join(''))
			    .appendTo(container);
			return this;
		};
	};

	return jQuery.extend(new pagination, control.call());
});