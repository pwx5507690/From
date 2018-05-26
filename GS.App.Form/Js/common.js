var message = function (mesg) {
    top.window.alert(mesg);
};

var loadScript = function (url, callback) {
	var script = document.createElement("script");
	script.type = "text/javascript";
	if (typeof (callback) != "undefined") {
		if (script.readyState) {
			script.onreadystatechange = function () {
				if (script.readyState == "loaded" || script.readyState == "complete") {
					script.onreadystatechange = null;
					callback();
				}
			};
		} else {
			script.onload = function () {
				callback();
			};
		}
	}
	script.src = url;
	document.body.appendChild(script);
};

var isEmail = function (str) {
	var strRegex = /^(\w)+(\.\w+)*@(\w)+((\.\w+)+)$/;
	var is = strRegex.test(str);
	if (!isEmail) {
		message('请输入有效的邮箱地址！');
		return false;
	}
	return true;
};
function validateNum(value) {
	if (isNaN(value)) {
		message('请输入有效的数字！');
		return false;
	}
	return true;
}
var validatemobile = function (mobile) {
	if (mobile.length != 11) {
		message('请输入有效的手机号码！');
		return false;
	}

	var strRegex = /^(((13[0-9]{1})|(15[0-9]{1})|(18[0-9]{1}))+\d{8})$/;
	if (!strRegex.test(mobile)) {
		message('请输入有效的手机号码！');
		return false;
	}
	return true;
};

var isUrl = function (url) {
	var strRegex = '^((https|http|ftp|rtsp|mms)?://)'
	+ '?(([0-9a-z_!~*\'().&=+$%-]+: )?[0-9a-z_!~*\'().&=+$%-]+@)?' //ftp的user@ 
	+ '(([0-9]{1,3}.){3}[0-9]{1,3}' // IP形式的URL- 199.194.52.184 
	+ '|' // 允许IP和DOMAIN（域名） 
	+ '([0-9a-z_!~*\'()-]+.)*' // 域名- www. 
	+ '([0-9a-z][0-9a-z-]{0,61})?[0-9a-z].' // 二级域名 
	+ '[a-z]{2,6})' // first level domain- .com or .museum 
	+ '(:[0-9]{1,4})?' // 端口- :80 
	+ '((/?)|' // a slash isn't required if there is no file name 
	+ '(/[0-9a-z_!~*\'().;?:@&=+$,%#-]+)+/?)$';
	var re = new RegExp(strRegex);
	//re.test() 
	return re.test(url);
};