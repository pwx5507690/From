using System;
using System.Net;
using System.Web.Http;
using GS.Common.Util;
using GS.Api.Model.Config;
using GS.Api.Constants;
using GS.Services;
using System.Web.Configuration;

namespace GS.Api.Controllers
{
    [RoutePrefix("api/setting")]
    public class SettingController : ApiController
    {
        public readonly IConfigParamServices _iConfigParamServices;
        public SettingController(IConfigParamServices iConfigParamServices)
        {

            _iConfigParamServices = iConfigParamServices;
        }
        [HttpGet]
        [Route("param")]
        public AppSettings SetParam()
        {
            var result = _iConfigParamServices.Param(Request.Headers.Authorization.ToString());
            if (result.Item1 != 200)
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            return result.Item2;
        }
        [HttpPost]
        [Route("param")]
        public int SetParam(AppSettings appSettings)
        {
            var code = _iConfigParamServices.Param(Request.Headers.Authorization.ToString(), appSettings);
            if (code != 200)
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            return code;
        }
    }
}
