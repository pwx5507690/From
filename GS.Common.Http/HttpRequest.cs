using GS.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GS.Common.Http
{
    public class HttpRequest
    {
        private static HttpRequestMessage GetHttpRequestMessage(HttpParam param)
        {
            var request = new HttpRequestMessage();
            if (param.Content != null)
            {
                request.Content = param.Content;
                if (!string.IsNullOrEmpty(param.Type))
                    request.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue(param.Type);
            }
            request.RequestUri = param.Uri;
            request.Method = new System.Net.Http.HttpMethod(param.Method.ToString());
            return request;
        }
        public static string Send(HttpParam param, IHttpException iHttpException = null, HttpClient httpClient = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var client = httpClient ?? new HttpClient())
            {
                HttpRequestMessage request = null;
                HttpResponseMessage response = null;
                request = GetHttpRequestMessage(param);
                response = client.SendAsync(request, cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    var message = $"send api {param.Uri.ToString()} error";
                    var exception = new HttpException(message, param.Uri.ToString(), param.Method, response);
                    if (iHttpException == null)
                        throw exception;
                    iHttpException.SetHttpException(exception);
                    return null;
                }
            }
        }
        public static T Send<T>(HttpParam param, IHttpException iHttpException = null, HttpClient httpClient = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var value = Send(param, iHttpException, httpClient, cancellationToken);
            return value.DeserializeObject<T>();
        }
    }
}
