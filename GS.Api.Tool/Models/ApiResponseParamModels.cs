using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace GS.Api.Tool.Models
{
	public class ApiResponseParamModels
	{
		private ApiResponseParamModels()
		{

		}
		public string Url { get; set; }
		public string ResponseMessage { get; set; }
		public string Content { get; set; }
		public string StatusCode { get; set; }
		public bool IsSuccessStatusCode { get; set; }

		public static ApiResponseParamModels Get(HttpResponseMessage responseMessage)
		{
			var apiResponseParamModels = new ApiResponseParamModels();
			apiResponseParamModels.Content = responseMessage.Content.ReadAsStringAsync().Result;
			apiResponseParamModels.StatusCode = responseMessage.StatusCode.GetHashCode().ToString();
			apiResponseParamModels.Url = responseMessage.RequestMessage.RequestUri.ToString();
			apiResponseParamModels.IsSuccessStatusCode = responseMessage.IsSuccessStatusCode;

			if (apiResponseParamModels.IsSuccessStatusCode)
				apiResponseParamModels.ResponseMessage = "目标API发送响应成功";
			else
				apiResponseParamModels.ResponseMessage = string.Format("{0}:{1}", "目标API发送响应失败",
					apiResponseParamModels.Url);
			return apiResponseParamModels;
		}
	}
}