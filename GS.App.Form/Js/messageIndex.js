(function (jQuery) {
    top.window.loading(false);
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
        var type = jQuery("[data-type]").attr("data-type") || 2;
        jQuery("#date").on("change", function () {
            location.href = "/Message/Index?time=" + jQuery(this).val() + "&type=" + type;
        });
        jQuery("#search").on("click", function () {
            var searchText = jQuery("#searchText").val();
            location.href = "/Message/Index?title=" + searchText + "&type=" + type;
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
})($ || jQuery, window);