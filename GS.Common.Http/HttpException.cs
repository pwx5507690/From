using GS.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GS.Common.Http
{
    public interface IHttpException
    {
        void SetHttpException(HttpException httpException);
    }
    public enum HttpMethod
    {
        POST,
        DELETE,
        PUT,
        GET,
        PATCH
    }
    public class HttpException : Exception
    {
        public HttpStatusCode Code { get; }
        public HttpMethod Method { get; }
        public string Content { get; }
        public HttpException(string message) : base(message)
        {
            LogUtil.ErrorFormat("request api error  message={0}", message);
        }
        public HttpException(string message, string url, HttpMethod method, HttpResponseMessage response) : base(message)
        {
            Method = method;
            Content = response.Content.ReadAsStringAsync().Result;
            Code = response.StatusCode;
            LogUtil.ErrorFormat("request api error api={0}|StatusCode={1}, message={2}", url, response.StatusCode,response.Content);
        }
    }
}
