using GS.App.Form.Models;
using GS.Common.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GS.SQLModel.Form;
using GS.Services;
using GS.App.Form.Result;
using GS.App.Form.Authentication;
using Webdiyer.WebControls.Mvc;
using GS.Common.Http;

namespace GS.App.Form.Controllers
{
    public class CustomFromController : BaseController
    {
        public readonly IDynamicDataservices _dynamicDataservices;
        public CustomFromController(IDynamicDataservices dynamicDataservices)
        {
            _dynamicDataservices = dynamicDataservices;
        }
        private string CreateDyncFieldInfoByType(string type, string fldsz)
        {
            if (type == "date" || type == "time" || type == "datetime") return " [datetime] NULL";
            else if (type == "number") return " [int] NULL";
            else if (type == "html") return " text NULL";
            else
            {
                if (fldsz == "text") return $" text NULL";
                else return $" [nvarchar]({fldsz}) NULL";
            }
        }
        private string GetSourceName(string lab)
        {
            var defaultName = $"_{StringUitl.NextRandom(100000000, 1)}";
            if (!lab.IsChinese())
                return defaultName;
            var translateName = TranslateBaiDuProxy.TranslateZhToEn(lab);
            return string.IsNullOrEmpty(translateName) ? defaultName : translateName;
        }
        private void SetDyncFieldInfo(DyncField item, string formCode, List<DyncField> dyncField)
        {
            item.Info = CreateDyncFieldInfoByType(item.Type, item.Fldsz);
            item.Name = GetSourceName(item.Lbl).UpperFirst().TrimSpace();
            while (dyncField.Any(t => t.Name == item.Name))
                item.Name = $"_{StringUitl.NextRandom(100000000, 1)}";

            item.DyncForm = formCode;
            item.Updatetime = DateTime.UtcNow;

            if (item.Itms.IsNotNull() && item.Source == 0)
                item.SourceValue = item.Itms.SerializeObjectFilterNullValue().Trim();

            if (item.Secdesc.IsNotNullOrEmpty())
                item.Instr = item.Secdesc;
        }
        private void ValidateFormValue(DyncForm formData, List<DyncField> parameterData, bool isUpdate = false)
        {
            if (formData.Name.IsNullOrEmpty())
                throw new FormException("请输入表单名称");

            if (StringUitl.IsNumberic(formData.Name.First().ToString()))
                throw new FormException("表单名称请不要以数字和特殊字符开头");

            if (isUpdate)
            {
                var isExist = _dynamicDataservices.GetDyncFormByName(formData.Name);
                if (isExist.Any(t => t.Code != formData.Code && t.Name == formData.Name))
                    throw new FormException("表单名称已经存在");
            }
            else
            {
                if (_dynamicDataservices.IsTableExist(formData.Name) > 0)
                    throw new FormException("表单名称已经存在");
            }
            if (!parameterData.Any())
                throw new FormException("表单字段不能为空");
        }
        public void Add(DyncForm formData, List<DyncField> parameterData)
        {
            ValidateFormValue(formData, parameterData);
            var code = Guid.NewGuid().ToString();
            formData.Code = code;
            parameterData.ForEach(item => SetDyncFieldInfo(item, code, parameterData.Clone<List<DyncField>>()));
            _dynamicDataservices.CreateTable(formData, parameterData);
            SetMessage($"{formData.Name}添加成功");
        }
        public void Update(DyncForm formData, List<DyncField> parameterData)
        {
            ValidateFormValue(formData, parameterData, true);
            parameterData.ForEach(item => SetDyncFieldInfo(item, formData.Code, parameterData.Clone<List<DyncField>>()));
            _dynamicDataservices.UpdateForm(formData, parameterData);
            SetMessage($"{formData.Name}修改成功");
        }
        public void SetDyncFieldItms(List<DyncField> parameterData)
        {
            parameterData.ForEach(item =>
            {
                if (item.SourceValue.IsNotNullOrEmpty() && item.Source == 0)
                    item.Itms = item.SourceValue.DeserializeObject<JArray>();

                if (item.Instr.IsNotNullOrEmpty())
                    item.Secdesc = item.Instr;
            });
        }
        [HttpPost]
        [AjaxAuthorize]
        public ActionResult Save()
        {
            var value = GetPostContentModal<JObject>();
            var formData = value["formData"].ToObject<DyncForm>();
            var parameterData = value["parameterData"].ToObject<List<DyncField>>();
            var option = value["option"].ToString();

            formData.Updatetime = DateTime.UtcNow;

            if (option == "Add")
                Add(formData, parameterData);
            else if (option == "Update")
                Update(formData, parameterData);

            return new JsonNetResult() { Data = new { M = formData, F = parameterData } };
        }
        [HttpGet]
        [ActionName("Delete")]
        public ActionResult Delete(string code, int page)
        {
            var name = _dynamicDataservices.RemoveForm(code);
            SetMessage($"{name}删除成功");
            return Redirect($"~/CustomFrom/DyncForm?currentPage={page}");
        }
        [HttpGet]
        [ActionName("DyncForm")]
        public ActionResult DyncForm(string name, int currentPage = 1)
        {
            var pageIndex = currentPage - 1;
            var result = name.IsNullOrEmpty() ? _dynamicDataservices.GetDyncForm(_pageSize, pageIndex) : _dynamicDataservices.GetDyncFormByName(_pageSize, pageIndex, name);

            var form = result.Result.ToPagedList(currentPage, _pageSize);
            form.TotalItemCount = result.Count;
            form.CurrentPageIndex = currentPage;
            GetMessage();
            ViewBag.page = currentPage;
            return View(form);
        }
        [HttpGet]
        [ActionName("GetDyncFieldByFormCode")]
        public ActionResult Filed(string code)
        {
            return new JsonNetResult() { Data = _dynamicDataservices.GetDyncFieldByFormCode(code) };
        }
        [HttpGet]
        public ActionResult Index(string code)
        {
            var tableCollect = _dynamicDataservices.GetDyncForm();
            ViewBag.TableCollect = tableCollect;
            if (code.IsNullOrEmpty())
                return View(new FormViewModel() { OptionType = 1 });

            var dyncForm = tableCollect.Where(t => t.Code == code).SingleOrDefault();
            var dyncField = _dynamicDataservices.GetDyncFieldByFormCode(code).ToList();

            SetDyncFieldItms(dyncField);
            return View(new FormViewModel()
            {
                OptionType = 2,
                DyncField = dyncField.SerializeObject(),
                FormData = dyncForm.SerializeObject()
            });
        }
    }
}