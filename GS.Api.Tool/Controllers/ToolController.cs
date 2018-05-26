using GS.Api.Tool.Models;
using GS.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace GS.Api.Tool.Controllers
{
	public class ToolController : Controller
	{
		public ActionResult Api()
		{
			ViewBag.apiRequestParamModels = new ApiRequestParamModels();
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Send(ApiRequestParamModels apiRequestParamModels)
		{
			var client = new HttpClient();
			HttpResponseMessage response = null;
			ApiResponseParamModels apiResponseParamModels = null;

			if (apiRequestParamModels.HeardParam.IsNotNullOrEmpty()
				&& apiRequestParamModels.HeardParamValue.IsNotNullOrEmpty())
			{
				client.DefaultRequestHeaders.Add(apiRequestParamModels.HeardParam, apiRequestParamModels.HeardParamValue);
			}
			if (apiRequestParamModels.Type.Equals("GET"))
			{
				response = client.GetAsync(apiRequestParamModels.Url).Result;
			}
			if (apiRequestParamModels.Type.Equals("DELETE"))
			{
				response = client.DeleteAsync(apiRequestParamModels.Url).Result;
			}
			if (apiRequestParamModels.Type.Equals("PUT"))
			{
				var contentPost = Content(client, apiRequestParamModels);
				response = client.PutAsync(apiRequestParamModels.Url, contentPost).Result;
			}
			if (apiRequestParamModels.Type.Equals("POST"))
			{
				var contentPost = Content(client, apiRequestParamModels);
				response = client.PostAsync(apiRequestParamModels.Url, contentPost).Result;
			}

			ViewBag.apiRequestParamModels = apiRequestParamModels;
			apiResponseParamModels = ApiResponseParamModels.Get(response);
			return View("Api", apiResponseParamModels);
		}

		private StringContent Content(HttpClient client, ApiRequestParamModels apiRequestParamModels)
		{
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(apiRequestParamModels.ParamType));
			StringContent contentPost = null;

			if (apiRequestParamModels.ParamValue.IsNotNullOrEmpty())
			{
				contentPost = new StringContent(apiRequestParamModels.ParamValue,
				Encoding.UTF8, apiRequestParamModels.ParamType);
			}

			return contentPost;
		}

	}
}