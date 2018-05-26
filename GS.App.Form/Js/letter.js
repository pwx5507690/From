(function (jQuery, window) {
    var message = jQuery("[data-message]").attr("data-message");
    if (message) {
        top.app.optionMessage = message;
        top.app.showMessage();
    }
    $(function () {
        
        jQuery("#acceptUser").chosen();
        var $replayStats = jQuery("#replayStats");
        if ($replayStats.length > 0) {
            var local = "replay";
            var cookie = top.window.cookie();
            var replayStats = cookie.get("replay");

            var down = function () {
                cookie.set(local, "up");
                jQuery("[data-replay]").show();
                $replayStats.html("收起全部回复<i class='am-icon-angle-up'></i>").attr("data-stats", "up");
            };
            var up = function () {
                cookie.set(local, "down");
                jQuery("[data-replay]").hide();
                $replayStats.html("展开全部回复<i class='am-icon-angle-down'></i>").attr("data-stats", "down");
            };

            if (replayStats == "down")
                up();
            else if (replayStats == "up")
                down(); 

            $replayStats.on("click", function () {   
                var stats = $replayStats.attr("data-stats");
                if (stats == "down")
                    down();
                else if (stats == "up")
                    up();
            });
        }
    });
    KindEditor.ready(function (K) {
        K.create("#content", {
            uploadJson: '/DyncForm/Upload'
        });
        prettyPrint();
    });
    window.valid = function () {
        var acceptUser = $("#acceptUser");
        if (acceptUser.length > 0)
            jQuery("#acceptUserId").val(acceptUser.val());
        return true;
    };
})($ || jQuery, window);