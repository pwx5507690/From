using GS.Cache.Identity;
using GS.Common.Util;
using GS.SQLModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace GS.Common.Http
{
    public class ApiProxy
    {
        private static string Token
        {
            get
            {
                return BaseIdentity.GetClientToken();
            }
        }

        public static string ApiAddress
        {
            get
            {
                var apiAddress = WebConfigurationManager.AppSettings["ApiAddress"];
                if (string.IsNullOrEmpty(apiAddress))
                    throw new Exception("config ApiAddress is null");

                return apiAddress;
            }
        }

        public static string AuthorizationTarget
        {
            get
            {
                return WebConfigurationManager.AppSettings["AuthorizationTarget"] ?? "Bearer";
            }
        }

        private static HttpClient GetClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add(
                "Authorization",
                $"{AuthorizationTarget} {Token}");
            return client;
        }

        private static HttpParam GetParam(HttpMethod method, string url, HttpContent content, bool isUseApiAddress = true)
        {
            var apiAddress = isUseApiAddress ? $"{ApiAddress}/api/{url}" : url;
            return new HttpParam()
            {
                IsUseBase = false,
                Method = method,
                Content = content,
                Url = apiAddress
            };
        }

        public static string Send(
            HttpMethod method,
            string url,
            HttpContent content = null,
            HttpClient client = null,
            IHttpException iHttpException = null,
            bool isUseApiAddress = true
            )
        {
            using (client = client ?? GetClient())
            {
                var param = GetParam(method, url, content, isUseApiAddress);
                return HttpRequest.Send(param, httpClient: client, iHttpException: iHttpException);
            }
        }
    }
}
