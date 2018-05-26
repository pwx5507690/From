using GS.Services;
using GS.Common.Util;
using GS.SQL.DataSource;
using System;
using System.Linq;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;
using System.Collections;
using System.IO;
using System.Globalization;
using GS.App.Form.Result;
using GS.Common.Web;

namespace GS.App.Form.Controllers
{
    public partial class DyncFormController : BaseController
    {
        public readonly IDynamicDataservices _dynamicDataservices;
        public DyncFormController(IDynamicDataservices dynamicDataservices)
        {
            _dynamicDataservices = dynamicDataservices;
        }
        private string GetFormNameByCode(string code)
        {
            var form = _dynamicDataservices.GetDyncFormByCode(code);
            if (form.IsNull())
                throw new FormatException("表单不存在");
            return form.Name;
        }
        [HttpPost]
        public ActionResult Save()
        {
            var code = Request.Form["code"];
            var value = Request.Form["value"];
            var dataId = Convert.ToInt32(Request.Form["dataId"]);
            var form = _dynamicDataservices.GetDyncFormByCode(code);
            var filed = _dynamicDataservices.GetDyncFieldByFormCode(code);
            var saveModel = value.DeserializeObject<SQLDynamicRow>();
            foreach (var item in saveModel.Row)
            {
                var filedItem = filed.Where(t => t.Name == item.Name).FirstOrDefault();
                item.Type = filedItem.Type;

                if (filedItem.IsNull())
                    throw new FormatException("未能找到对应的字段!!!");

                if (filedItem.Type == "html")
                    item.Value = Request.Form[item.Name];
                //if (filedItem.Fldsz) {
                //	item.Value.Length
                //}
                if (filedItem.Reqd == "1" && item.Value.IsNotNullOrEmpty())
                    throw new FormatException($"{filedItem.Lbl}不能为空!!!");

                if (filedItem.Uuiq == "1" && _dynamicDataservices.ValidateDyncForm(form.Name, filedItem.Name, item.Value, dataId) > 1)
                    throw new FormatException($"{filedItem.Lbl}值已经存在!!!");
            }
            if (dataId == -1)
                _dynamicDataservices.AddDyncTableData(saveModel, form.Name);
            else
                _dynamicDataservices.UpdateDyncTableData(saveModel, form.Name, dataId);

            if (form.Cfmurl.IsNotNullOrEmpty())
                Response.Redirect(form.Cfmurl, true);

            SetMessage(form.Cfmmsg);
            return Redirect($"~/DyncForm/FormTable?code={code}");

        }
        [HttpGet]
        public ActionResult Delete(int id, string code, int page)
        {
            var name = GetFormNameByCode(code);
            _dynamicDataservices.RemoveFormTableById(id, name);
            SetMessage("删除成功");
            return Redirect($"~/DyncForm/FormTable?code={code}&currentPage={page}");
        }
        [HttpGet]
        [ActionName("FormTable")]
        public ActionResult FormTable(string code, int currentPage = 1)
        {
            ViewBag.Title = GetFormNameByCode(code);

            var pageIndex = currentPage - 1;
            var dyncTable = _dynamicDataservices.GetDyncFormTableByCode(_pageSize, pageIndex, code);

            var form = dyncTable.Result.ToPagedList<SQLDynamicRow>(currentPage, _pageSize);
            form.TotalItemCount = dyncTable.Count;
            form.CurrentPageIndex = currentPage;
            GetMessage();
            ViewBag.code = code;
            ViewBag.page = currentPage;
            return View(form);
        }
        [HttpGet]
        public ActionResult Index(string code, int? id)
        {
            var dyncForm = _dynamicDataservices.GetDyncFormByCode(code);
            var dyncField = _dynamicDataservices.GetDyncFieldByFormCode(code).ToList();
            GetMessage();
            if (id != null)
            {
                ViewBag.Id = id;
                ViewBag.DynamicData = _dynamicDataservices.GetDyncDataById(id.Value, dyncForm.Name);
            }
            else
            {
                ViewBag.Id = -1;
                ViewBag.DynamicData = null;
            }
            ViewBag.Code = code;
            ViewBag.Title = dyncForm.Name;
            ViewBag.DyncField = dyncField;
            ViewBag.FormData = dyncForm;
            return View();
        }
    }
    public partial class DyncFormController
    {
        [HttpPost]
        public ActionResult Upload()
        {
            var savePath = "~/Attach/";
            var saveUrl = "/Attach/";

            var maxSize = 1000000;
            var imgFile = Client.Request.Files["imgFile"];
            var extTable = new Hashtable();

            extTable.Add("image", "gif,jpg,jpeg,png,bmp");
            extTable.Add("flash", "swf,flv");
            extTable.Add("media", "swf,flv,mp3,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb");
            extTable.Add("file", "doc,docx,xls,xlsx,ppt,htm,html,txt,zip,rar,gz,bz2");

            if (imgFile == null)
                return new UploadResult("请选择文件", UploadResult.UploadType.ERROR);

            var dirPath = Client.Server.MapPath(savePath);
            if (!Directory.Exists(dirPath))
                return new UploadResult("上传目录不存在", UploadResult.UploadType.ERROR);

            var dirName = Client.Request.QueryString["dir"];
            if (String.IsNullOrEmpty(dirName))
                dirName = "image";
            if (!extTable.ContainsKey(dirName))
                return new UploadResult("目录名不正确", UploadResult.UploadType.ERROR);

            var fileName = imgFile.FileName;
            var fileExt = Path.GetExtension(fileName).ToLower();

            if (imgFile.InputStream == null || imgFile.InputStream.Length > maxSize)
                return new UploadResult("上传文件大小超过限制", UploadResult.UploadType.ERROR);

            if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(((String)extTable[dirName]).Split(','), fileExt.Substring(1).ToLower()) == -1)
                return new UploadResult($"上传文件扩展名是不允许的扩展名。\n只允许{extTable[dirName].ToString()}格式", UploadResult.UploadType.ERROR);

            dirPath += $"{dirName}/";
            saveUrl += $"{dirName}/";
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            var ymd = DateTime.UtcNow.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo);
            dirPath += $"{ymd}/";
            saveUrl += $"{ymd}/";

            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            var newFileName = DateTime.UtcNow.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
            var filePath = dirPath + newFileName;

            imgFile.SaveAs(filePath);

            var fileUrl = saveUrl + newFileName;
            return new UploadResult(fileUrl);
        }
    }
}