(function (jQuery, window) {
	var _delete = function () {
        jQuery("[data-delete]").on("click", function () {
            var url = jQuery(this).attr("data-delete");
            var relatedTarget = {
                onConfirm: function (target) {
                    location.href = url;
                }
            };
            top.window.confirm("是否删除", relatedTarget);
		});
	};

	var _seach = function () {
		jQuery("#search").on("click", function () {
			var searchText = jQuery("#searchText").val();
			location.href = "/CustomFrom/DyncForm?name=" + searchText;
		});
	};

    var _pageInit = function () {
        var message = jQuery("[data-message]").attr("data-message");
        if (message) {
            top.app.optionMessage = message;
            top.app.showMessage();
        }
		_delete();
		_seach();
	};

    jQuery(function () {
		_pageInit();
	});
})($ || jQuery, window)