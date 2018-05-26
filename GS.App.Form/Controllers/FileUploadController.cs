using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GS.Common.Util;
using System.IO;
using GS.App.Form.Result;
using System.Web.Configuration;

namespace GS.App.Form.Controllers
{
    public class FileUploadController : BaseController
    {
        private const string _root = "Attach";
        public ActionResult Index()
        {
            var folder = Request["folder"];
            var isRead = Request["isRead"];
            if (isRead.IsNullOrEmpty())
                if (folder.IsNullOrEmpty())
                    return new ExceptionResult("请给定文件夹参数", isWriteLog: false);

            var multiple = string.IsNullOrEmpty(Request["multiple"]) ? "" : "multiple";
            var accept = string.IsNullOrEmpty(Request["accept"]) ? "" : $"accept={Request["accept"]}";
            ViewBag.Multiple = multiple;
            ViewBag.Accept = accept;
            //ViewBag.Folder = folder;
            return View();
        }
        [HttpPost]
        public ActionResult Uploader()
        {
            var file = Request.Files["file"];
            var isRead = Request["isRead"];
            if (isRead.IsNotNullOrEmpty())
            {
                using (var sr = new StreamReader(file.InputStream))
                {
                    return new ContentResult() { Content = sr.ReadToEnd() };
                }
            }
            var folder = Request["folder"];
            if (folder.IsNullOrEmpty())
                return new UploadResult("请给定文件夹参数", UploadResult.UploadType.ERROR);

            var extension = WebConfigurationManager.AppSettings["UploadExtension"].Split(',');
            var suffix = Path.GetExtension(file.FileName).Replace(".", "").ToLower();

            if (!extension.Any(t => t.Equals(suffix)))
                return new UploadResult("不能识别的文件类型", UploadResult.UploadType.ERROR);

            folder = $"/{_root}/{folder}";
            var folderPath = Server.MapPath($"~{folder}");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            file.SaveAs($"{folderPath}/{file.FileName}");

            return new UploadResult($"{folder}/{file.FileName}");
        }
    }
}