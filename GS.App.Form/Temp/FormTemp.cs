using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GS.Common.Util;

namespace GS.App.Form.Temp
{
    public class FormTemp
    {
        private readonly static string _path = "~/Temp/FormTemp.xml";
        private readonly static string _tempName = "item";
        private readonly static string _targetAttributeKey = "id";
        public static string GetTempByKey(string key, params string[] param)
        {
            var util = new XmlUtil.Attribute() { Name = _targetAttributeKey, Value = key };
            var result = XmlUtil.GetValue(Common.Web.Constant.HttpContext.Server.MapPath(_path), _tempName, util)?[key] ?? string.Empty;
            return string.Format(result, param);
        }
    }
}