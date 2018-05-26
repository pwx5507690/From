using GS.Common.Http;
using GS.Services;
using GS.SQLModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;
using GS.Common.Util;
using ResponseFileResult = GS.View.Model.VResponseFileResult;

namespace GS.Api.Controllers
{
	[RoutePrefix("api/file")]
	public class FileController : BaseController
	{
		private readonly IFileServices _fileServices;
		private const string _uploadName = "_uploadName";
		public FileController(IFileServices fileServices)
		{
			_fileServices = fileServices;
		}
		[HttpGet]
		[Route("group")]
		public IEnumerable<FileGroup> GetGroup()
		{
			return _fileServices.GetGroup();
		}
		[HttpPost]
		[Route("addFile")]
		public int AddFile(File item)
		{
			return _fileServices.AddFile(item);
		}
		[HttpDelete]
		[Route("deleteFolder/{id}")]
		public int DeleteFolder(int id)
		{

			var folder = _fileServices.GetFolderById(id);
			if (folder.IsNull())
			{
				return -1;
			}

			var content = new MultipartFormDataContent();
			content.Add(new StringContent(folder.Url), "url");

			var  param = new HttpParam();
			param.Content = content;
			param.Type = Common.Http.HttpMethod.DELETE.ToString();

			var result = HttpRequest.Send<ResponseFileResult>(param);

			if (result.IsSuccess)
			{
				return _fileServices.DeleteFolder(id);
			}
			return -1;
		}
		[HttpPost]
		[Route("addFolder")]
		public int AddFolder(Folder item)
		{
			
			//var folder = _fileServices.GetFolderByParent(item.Parent);
			//var parentUrl = string.Empty;

			//if (folder.IsNull())
			//{
			//	return -1;
			//}
			//parentUrl = folder.Url;

			//var param = new HttpParam();
			//var content = new MultipartFormDataContent();
			//content.Add(new StringContent(parentUrl), "url");
			//content.Add(new StringContent(item.Name), "folder");

			//param.Content = content;
			//param.Type = Common.Http.HttpMethod.POST.ToString();

			//var result = HttpRequest.Send<ResponseFileResult>(param).Result;
			//if (result.IsSuccess)
			//{
			//	return _fileServices.AddFolder(item);
			//}
			return -1;

		}
		[HttpGet]
		[Route("folder/group/{id}")]
		public IEnumerable<Folder> GetFolder(int id)
		{
			return _fileServices.GetFolderByGroup(id);
		}
		[HttpPost]
		[Route("upload")]
		public async Task<HttpResponseMessage> Upload()
		{
			if (!Request.Content.IsMimeMultipartContent("form-data"))
			{
				throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
			}
			HttpResponseMessage response = null;
			try
			{
				using (var fileStream = await Request.Content.ReadAsStreamAsync())
				{
					var content = new MultipartFormDataContent();
					var savePath = string.Empty;
					content.Add(new StringContent(savePath), nameof(savePath));
					content.Add(new StreamContent(fileStream), _uploadName, _uploadName);

					var param = new HttpParam();
					param.Content = content;
					param.Type = Common.Http.HttpMethod.POST.ToString();
					param.Url = WebConfigurationManager.AppSettings["FileStorage"];

					var result = HttpRequest.Send<ResponseFileResult>(param);
					if (!result.IsSuccess)
					{
						throw new HttpResponseException(HttpStatusCode.BadRequest);
					}
					response = Request.CreateResponse(HttpStatusCode.Accepted, result);
				}
			}
			catch
			{
				throw new HttpResponseException(HttpStatusCode.BadRequest);
			}
			return response;
		}
	}
}
