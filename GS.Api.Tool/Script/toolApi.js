(function (window) {
	var map = null;

	var setMap = function () {
		map = new Object();

		map["url"] = document.getElementById("url");
		map["send"] = document.getElementById("send");
		map["messageContent"] = document.getElementById("messageContent");
		map["urlLabel"] = document.getElementById("urlLabel");
		map["type"] = document.getElementById("type");
		map["paramTypeContent"] = document.getElementById("paramTypeContent");
		map["paramValueContent"] = document.getElementById("paramValueContent");

		for (var i = 0; i < map["messageContent"].childNodes.length; i++) {
			if (map["messageContent"].childNodes[i].nodeName === "A") {
				map["message"] = map["messageContent"].childNodes[i];
				break;
			}
		}
	};

	var showParamValueContent = function () {
		if (map["type"].value === "PUT" || map["type"].value === "POST") {
			map["paramTypeContent"].style.display = "";
			map["paramValueContent"].style.display = "";
		} else {
			map["paramTypeContent"].style.display = "none";
			map["paramValueContent"].style.display = "none";
		}
	};

	var event = function () {
		map["send"].onclick = function () {
			if (!checkUrl()) {
				return false;
			}
		};

		map["type"].onchange = showParamValueContent
	};

	var checkUrl = function () {
		if (map["url"].value) {
			return true;
		}
		map["messageContent"].style.display = "";
		map["message"].innerHTML = "请求路径不能为空！！！";
		map["url"].style.borderColor = "#b94a48";
		map["urlLabel"].style.color = "#b94a48";
	};

	var init = function () {
		setMap();
		event();
		showParamValueContent();
	};

	window.onload = function () {
		init();
	};
})(window);