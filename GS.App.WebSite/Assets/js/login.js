(function (window, jQuery) {
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
		_init();
	});
})(window, $ || jQuery);