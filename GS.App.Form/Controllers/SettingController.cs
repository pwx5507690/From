using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Configuration;
using System.Threading.Tasks;
using GS.Common.Util;
using GS.Services;
using GS.Api.Model.Config;
using GS.Common.Http;
using GS.SQLModel;
using Content = System.Net.Http.StringContent;
using HttpException = GS.Common.Http.HttpException;

namespace GS.App.Form.Controllers
{
    public class ApiException : IHttpException
    {
        public int Code { get; set; }
        public string StackTrace { get; set; }
        public ApiException()
        {
            Code = 200;
        }
        public void SetHttpException(HttpException httpException)
        {
            StackTrace = httpException.Content;
            Code = httpException.Code.GetHashCode();
        }
    }
    public class SettingController : BaseController
    {
        public readonly IAuthenticationServices _iAuthenticationServices;
        public readonly IConfigParamServices _iConfigParamServices;
        public SettingController(IConfigParamServices iConfigParamServices)
        {
            _iConfigParamServices = iConfigParamServices;
            _iAuthenticationServices = ((ConfigParamServices)_iConfigParamServices)._iAuthenticationServices;
        }
        private Task<Tuple<int, string, string>> SendAsync(string url, HttpMethod httpMethod, Content content = null, bool isUseApiAddress = false)
        {
            return Task.Factory.StartNew(() => Send(url, httpMethod, content, isUseApiAddress));
        }
        private Tuple<int, string, string> Send(string url, HttpMethod httpMethod, Content content = null, bool isUseApiAddress = false)
        {
            var authenticationSite = new AuthenticationSite() { AuthenticationType = "Auth", Uuid = Guid.NewGuid().ToString() };
            _iAuthenticationServices.Add(authenticationSite);

            var client = new System.Net.Http.HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", authenticationSite.Uuid);

            var apiException = new ApiException();

            var result = ApiProxy.Send(httpMethod, url, content, client, apiException, isUseApiAddress);

            var error = string.Empty;
            if (apiException.Code != 200)
            {
                if (apiException.Code == 404)
                    error = $"参数保存失败,{url}地址不存在,请检查输入的域名地址{apiException.Code}";
                else
                    error = $"参数保存失败,{url}服务器错误 Code={apiException.Code}";
            }
            return new Tuple<int, string, string>(apiException.Code, error, result);
        }
        public async Task<ActionResult> Param(string error)
        {
            var apiAddress = WebConfigurationManager.AppSettings["ApiAddress"] ?? string.Empty;
            var loginPath = WebConfigurationManager.AppSettings["LoginPath"] ?? string.Empty;

            ViewBag.ApiAddress = apiAddress;
            ViewBag.LoginPath = loginPath;
            ViewBag.WebCmsAddress = WebConfigurationManager.AppSettings["WebCmsAddress"] ?? string.Empty;
            ViewBag.UploadExtension = WebConfigurationManager.AppSettings["UploadExtension"] ?? string.Empty;
            ViewBag.CacheStorage = WebConfigurationManager.AppSettings["MemoryCache"] ?? string.Empty;
            var appsetting = _iConfigParamServices.Param();

            var result = await SendAsync($"{apiAddress}/api/setting/param", HttpMethod.GET);
            if (result.Item1 != 200)
                error = error + result.Item2;

            var apiSetting = result.Item3.DeserializeObject<AppSettings>();
            appsetting.CorsDomains = apiSetting.CorsDomains;
            appsetting.IsAuthentication = apiSetting.IsAuthentication;
            ViewBag.AppSettings = appsetting;
            ViewBag.Error = error ?? string.Empty;
            GetMessage();
            return View();
        }
        public async Task<ActionResult> ModifiedParam()
        {
            var api = Request["ApiAddress"];
            var loginPath = Request["LoginPath"];
            var requestType = Request["Type"];
            var appSettings = new AppSettings();

            appSettings.CacheStorage = Request["CacheStorage"];
            appSettings.RedisReaderPath = Request["RedisReaderPath"];
            appSettings.RedisWriterPath = Request["RedisWriterPath"];
            appSettings.IsAuthentication = Convert.ToBoolean(Request["IsAuthentication"]);

            var sendTask = new List<Task<Tuple<int, string, string>>>() {
                SendAsync($"{api}/api/setting/param", HttpMethod.POST, new Content(appSettings.SerializeObject())),
                SendAsync($"{loginPath}/Login/param", HttpMethod.POST, new Content(appSettings.SerializeObject()))
            };
            var result = await Task.WhenAll(sendTask);
            foreach (var item in result)
            {
                if (item.Item1 != 200)
                    return Redirect($"~/Setting/Param?error={item.Item2}&type={requestType}");
            }

            var webCmsAddress = Request["WebCmsAddress"];

            _iConfigParamServices.Param(appSettings);
            AppSettingsUtil.Modified("WebCmsAddress", webCmsAddress);
            AppSettingsUtil.Modified("ApiAddress", api);

            AppSettingsUtil.Modified("LoginPath", loginPath);
            AppSettingsUtil.Modified("UploadExtension", Request["UploadExtension"] ?? string.Empty);
            if (requestType.IsNotNullOrEmpty())
            {
                return Redirect("~/Index");
            }
            else
            {
                SetMessage("设置保存成功");
                return Redirect("~/Setting/Param");
            }
        }
    }
}