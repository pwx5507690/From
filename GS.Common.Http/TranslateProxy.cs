using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using GS.Common.Util;

namespace GS.Common.Http
{
    public class TranslateBaiDuProxyResult
    {
        public string Src { get; set; }
        public string Dst { get; set; }
    }
    public class TranslateBaiDuProxyParam
    {
        public string Text { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public override string ToString()
        {
            return $"q={System.Web.HttpUtility.UrlEncode(Text)}&from={From}&to={To}";
        }
    }
    public class TranslateBaiDuProxy
    {
        private static readonly string _appid = WebConfigurationManager.AppSettings["Appid"];
        private static readonly string _salt = WebConfigurationManager.AppSettings["Salt"];
     //   private static readonly string _sign = WebConfigurationManager.AppSettings["Sign"];
        private static readonly string _url = WebConfigurationManager.AppSettings["TranslateProxyUrl"];
        private static string GetProxyUrl(TranslateBaiDuProxyParam param)
        {
            if (string.IsNullOrEmpty(_appid) || string.IsNullOrEmpty(_salt)
             || string.IsNullOrEmpty(_url))
                throw new Exception("请在webconfig中 设置百度翻译参数代理 Appid，Salt，Sign，TranslateProxyUrl");
            var sign  = $"{_appid}{param.Text}{_salt}12345678".GetMd5Str32();
            return $"{_url}?{param.ToString()}&appid={_appid}&salt={_salt}&sign={sign}";
        }
        public static IEnumerable<TranslateBaiDuProxyResult> Translate(TranslateBaiDuProxyParam param)
        {
            using (var client = new HttpClient())
            {
                var httpParam = new HttpParam()
                {
                    IsUseBase = false,
                    Method = HttpMethod.GET,
                    Content = null,
                    Url = GetProxyUrl(param)
                };
                var result = HttpRequest.Send(httpParam, httpClient: client).DeserializeObject<JObject>();
                return result["trans_result"]?.ToObject<IEnumerable<TranslateBaiDuProxyResult>>();
            }
        }
        public static string TranslateEnToZh(string text)
        {
            var result = Translate(new TranslateBaiDuProxyParam
            {
                From = "en",
                To = "zh",
                Text = text
            })?.FirstOrDefault()?.Dst;
            return !string.IsNullOrEmpty(result) ? System.Web.HttpUtility.UrlDecode(result) : result;
        }
        public static string TranslateZhToEn(string text)
        {
            return Translate(new TranslateBaiDuProxyParam
            {
                From = "zh",
                To = "en",
                Text = text
            })?.FirstOrDefault()?.Dst;
        }
    }
}
