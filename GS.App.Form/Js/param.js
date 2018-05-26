(function (jQuery,window) {
    if (error.trim())
        top.window.alert(error);

    if (message) {
        top.app.optionMessage = message;
        top.app.showMessage();
    }
    top.window.loading(false);
    jQuery(function () {
        jQuery("#uploadtype").chosen();
        jQuery("#cacheStorage").change(function () {
            if (jQuery(this).val() == "Redis")
                jQuery("#redisDb").show().find("input").attr("required", "");
            else
                jQuery("#redisDb").hide().find("input").removeAttr("required");
        }).change();
    });
    function valid() {
        top.window.loading(true, "设置保存中...");
        jQuery("input[name='UploadExtension']").val(jQuery("#uploadtype").val());
        jQuery("input[name='CacheStorage']").val(jQuery("#cacheStorage").val());
        jQuery("input[name='IsAuthentication']").val(jQuery("#isAuthentication").val());
        return true;
    };
})($ || jQuery, window);