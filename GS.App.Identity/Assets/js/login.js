(function (window, jQuery) {
	var _post = function (url, params) {
		var temp = document.createElement("form");
		temp.action = url;
		temp.method = "post";
		temp.style.display = "none";
		for (var x in params) {
			var opt = document.createElement("textarea");
			opt.name = x;
			opt.value = params[x];
			temp.appendChild(opt);
		}
		document.body.appendChild(temp);
		temp.submit();
	};

	var _validator = function () {
		jQuery('#loginForm').validator({
			onValid: function (validity) {
				jQuery(validity.field).closest('.am-form-group').find('.am-alert').hide();
			},
			onInValid: function (validity) {
				var $field = jQuery(validity.field);
				var $group = $field.closest('.am-form-group');
				var $alert = $group.find('.am-alert');
				var msg = $field.data('validationMessage') || this.getValidationMessage(validity);
				if (!$alert.length) {
					$alert = jQuery('<div class="am-alert am-alert-danger"></div>').hide().
					  appendTo($group);
				}
				$alert.html(msg).show();
			}
		});
	};
	var _init = function () {
		_validator();
	};
	jQuery(function () {
		var model = window.model;
		if (model && model.isLogin && model.isLogin.toUpperCase() == "TRUE") {
			jQuery(document.getElementById("loading")).modal("open");
			var params = {};
			params["location"] = model.location;
			params["userCache"] = model.sessionId;
			_post(model.loginUrl, params);
		} else if (model && model.message) {
			jQuery(document.getElementById("alert")).modal("open");
		}
		else {
			_init();
		}
	});
})(window, $ || jQuery);