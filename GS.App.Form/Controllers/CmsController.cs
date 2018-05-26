using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;
using System.Web.Configuration;
using System.Net.Http;
using System.IO;
using GS.Common.Http;
using GS.Services;
using GS.Common.Util;
using GS.SQLModel.Cms;
using GS.App.Form.Models;
using GS.App.Form.Result;

namespace GS.App.Form.Controllers
{
    public partial class CmsController : BaseController
    {
        public readonly ICmsServices _iCmsServices;
        public CmsController(ICmsServices iCmsServices)
        {
            _iCmsServices = iCmsServices;
        }
        private IEnumerable<KeyValuePair<string, string>> GetSendContent(WebCmsSendModel webCmsSendModel)
        {
            var values = new[]
                {
                new KeyValuePair<string, string>("oldTempName", webCmsSendModel.OldTempName??string.Empty),
                new KeyValuePair<string, string>("tempName", webCmsSendModel.TempName??string.Empty),
                new KeyValuePair<string, string>("tempType", webCmsSendModel.TempType??string.Empty),
                new KeyValuePair<string, string>("siteName", webCmsSendModel.SiteName??string.Empty),
                new KeyValuePair<string, string>("cmd", webCmsSendModel.Cmd??string.Empty),
                new KeyValuePair<string, string>("content", webCmsSendModel.Content??string.Empty),
                new KeyValuePair<string, string>("option", webCmsSendModel.Option??string.Empty),
                new KeyValuePair<string, string>("resource", webCmsSendModel.Resource??string.Empty),
                new KeyValuePair<string, string>("icon", webCmsSendModel.Icon??string.Empty)
            };
            return values;
        }
        private string SendWebCms(WebCmsSendModel webCmsSendModel)
        {
            var webCmsAddress = WebConfigurationManager.AppSettings["WebCmsAddress"];
            var values = GetSendContent(webCmsSendModel);
            var httpParam = new HttpParam()
            {
                IsUseBase = false,
                Method = Common.Http.HttpMethod.POST,
                Content = new StringContent(values.Select(value => $"{value.Key}={value.Value}").JoinString("&"), System.Text.Encoding.GetEncoding("GB2312")),
                Url = webCmsAddress,
                Type = ContentType.FORM_DATA
            };
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", Request.Url.Authority.GetMd5Str());
            return HttpRequest.Send(httpParam, httpClient: httpClient);
        }
        [HttpPost]
        public ActionResult UpdateSite(Site model)
        {
            var exist = _iCmsServices.GetSiteByName(model.Name).FirstOrDefault();
            if (exist.IsNotNull() && exist.Id != model.Id)
            {
                SetMessage($"站点{model.Name}已经存在");
                return Redirect("~/Cms/ModifySite");
            }
            var old = _iCmsServices.GetSiteById(model.Id);
            var oldPageName = old.PageName;
            var icon = old.Icon == model.Icon ? null : model.Icon;

            SendWebCms(new WebCmsSendModel
            {
                Cmd = "CreateSite",
                Option = "reName",
                SiteName = model.Name,
                TempName = model.PageName,
                OldTempName = oldPageName,
                Icon = icon
            });
            var oldTemp = _iCmsServices.GetSiteTempByName(oldPageName).FirstOrDefault();
            _iCmsServices.Update(model);
            _iCmsServices.AddTemp(new SiteTemp()
            {
                Id = oldTemp.Id,
                TempName = model.PageName
            });
            SetMessage($"站点名称{model.Name}修改成功");
            return Redirect("~/Cms/Site");
        }
        [HttpPost]
        public ActionResult AddSite(Site model)
        {
            if (_iCmsServices.GetSiteByName(model.Name).Any())
            {
                SetMessage($"站点{model.Name}已经存在");
                return Redirect("~/Cms/ModifySite");
            }
            SendWebCms(new WebCmsSendModel
            {
                Cmd = "CreateSite",
                Option = "save",
                TempType = "aspx",
                SiteName = model.Name,
                TempName = model.PageName,
                Icon = model.Icon
            });

            var id = _iCmsServices.AddSite(model);
            _iCmsServices.AddTemp(new SiteTemp()
            {
                SiteId = id,
                TempType = "aspx",
                TempName = model.PageName
            });
            SetMessage($"{model.Name}添加成功");
            return Redirect("~/Cms/Site");
        }
        [HttpGet]
        public ActionResult GenerateCmsTempView()
        {
            var site = _iCmsServices.GetSite();
            var id = site.FirstOrDefault()?.Id;
            SetSiteControl("site", site, id.Value);
            ViewData["Default"] = id;
            return View();
        }
        [HttpGet]
        public ActionResult ModifySite(int? code)
        {
            GetMessage();
            if (code != null)
            {
                var siteModel = _iCmsServices.GetSiteById(code.Value);
                ViewBag.TempName = _iCmsServices.GetSiteTempBySiteId(siteModel.Id).Where(t => t.TempType == "aspx").Select(t => t.TempName);
                ViewBag.Action = "/Cms/UpdateSite";
                return View(siteModel);
            }
            ViewBag.Action = "/Cms/AddSite";
            return View(new Site());
        }
        [HttpGet]
        public ActionResult Delete(int code, int page)
        {
            var name = _iCmsServices.GetSiteById(code).Name;
            SendWebCms(new WebCmsSendModel()
            {
                Cmd = "DeleteSite",
                SiteName = name
            });
            var result = _iCmsServices.Remove(new Site()
            {
                Id = code
            });
            SetMessage($"{name}删除成功");
            return Redirect($"~/Cms/Site?currentPage={page}");
        }
        [HttpGet]
        public ActionResult Site(string name, int currentPage = 1)
        {
            var pageIndex = currentPage - 1;
            var result = name.IsNullOrEmpty() ? _iCmsServices.GetSite(_pageSize, pageIndex) : _iCmsServices.GetSiteByName(name, _pageSize, pageIndex);

            var form = result.Result.ToPagedList<Site>(currentPage, _pageSize);
            form.TotalItemCount = result.Count;
            form.CurrentPageIndex = currentPage;
            GetMessage();
            ViewBag.page = currentPage;
            return View(form);
        }
    }
    public partial class CmsController
    {
        private void SetSiteControl(string clientId, IEnumerable<Site> site, int selected)
        {
            ViewData["SiteDropDown"] = new DropdownControlModel()
            {
                Item = site.Select(t => new SelectListItem()
                {
                    Selected = t.Id.Equals(selected),
                    Text = t.Name,
                    Value = t.Id.ToString()
                }),
                HtmlAttributes = new Dictionary<string, string>() { { "id", clientId } }
            };
        }
        [HttpGet]
        public ActionResult QueryTempContent(string param)
        {
            var p = param.Split('|');
            return new ContentResult()
            {
                Content = SendWebCms(new WebCmsSendModel()
                {
                    Cmd = "QueryTemp",
                    SiteName = p[0],
                    TempName = p[1],
                    TempType = p[2]
                })
            };
        }
        [HttpGet]
        public ActionResult CmsTemp(int? siteId)
        {
            var site = _iCmsServices.GetSite();
            var tempModel = new CmsTempModel();

            if (!site.Any())
                return View(tempModel);

            tempModel.Site = site;

            var defaultSite = site.FirstOrDefault();
            if (siteId == null)
                siteId = defaultSite.Id;
            else
                defaultSite = site.Where(t => t.Id == siteId.Value).SingleOrDefault();

            tempModel.SiteId = siteId.Value;
            tempModel.Current = defaultSite;

            var temp = _iCmsServices.GetSiteTempBySiteId(siteId.Value);
            if (temp.Any())
            {
                tempModel.CssTemp = temp.Where(t => t.TempType == "css");
                tempModel.JsTemp = temp.Where(t => t.TempType == "js");
                tempModel.ControlTemp = temp.Where(t => t.TempType == "control");
                tempModel.PageTemp = temp.Where(t => t.TempType == "aspx");
            }
            tempModel.SiteResource = _iCmsServices.GetSiteSiteResourceBySiteId(siteId.Value);
            SetSiteControl("site", tempModel.Site, tempModel.SiteId);
            GetMessage();
            return View(tempModel);
        }
        [HttpPost]
        public ActionResult ValidateTempName(SiteTemp model)
        {
            var temp = _iCmsServices.GetSiteTempBySiteId(model.SiteId);
            Func<string, JsonNetResult> func = r =>
            {
                var result = new JsonNetResult();
                if (r == "error") result.Data = new
                {
                    stats = "error",
                    message = $"已经存在{model.TempName}名称的模板"
                };
                else if (r == "success") result.Data = new { stats = "success" };
                return result;
            };
            if (model.Id == -1)
            {
                if (temp.Any(t => t.TempType.Equals(model.TempType) && t.TempName.Equals(model.TempName)))
                    return func("error");

            }
            else
            {
                if (temp.Any(t => t.TempType.Equals(model.TempType) && t.TempName.Equals(model.TempName)
                && !t.Id.Equals(model.Id)))
                    return func("error");
            }
            return func("success");
        }
        [HttpPost]
        public void DeleteTemp(SiteTemp model)
        {
            var site = _iCmsServices.GetSiteById(model.SiteId);
            var webCmsSendModel = new WebCmsSendModel()
            {
                Cmd = "DeleteTemp",
                OldTempName = model.TempName,
                TempType = model.TempType,
                SiteName = site.Name
            };
            SendWebCms(webCmsSendModel);
            _iCmsServices.Remove(model);
        }
        [HttpPost]
        public void DeleteResource(SiteResource siteResource)
        {
            var site = _iCmsServices.GetSiteById(siteResource.SiteId);
            var webCmsSendModel = new WebCmsSendModel()
            {
                Resource = siteResource.Path,
                Cmd = "DeleteResource",
                SiteName = site.Name
            };
            SendWebCms(webCmsSendModel);
            _iCmsServices.Remove(siteResource);
        }
        [HttpPost]
        public ActionResult AddResource(CmsSiteResource cmsSiteResource)
        {
            var site = _iCmsServices.GetSiteById(cmsSiteResource.SiteId);
            var webCmsSendModel = new WebCmsSendModel()
            {
                Resource = cmsSiteResource.ResourcePath.JoinString(","),
                Cmd = "SaveResource",
                SiteName = site.Name
            };
            SendWebCms(webCmsSendModel);
            var resource = cmsSiteResource.ResourcePath.Select(item =>
            {
                var extension = Path.GetExtension(item).Replace(".", "").ToUpper();
                var siteResources = new SiteResource()
                {
                    SiteId = site.Id,
                    Path = item,
                    ResourceName = Path.GetFileName(item),
                    ResourceType = extension
                };
                siteResources.Id = _iCmsServices.AddResource(siteResources);
                return siteResources;
            });
            return new JsonNetResult() { Data = resource };
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddCmsTempView(SiteTemp model)
        {
            var siteName = _iCmsServices.GetSiteById(model.SiteId).Name;
            SendWebCms(new WebCmsSendModel
            {
                Cmd = "SaveTemp",
                SiteName = siteName,
                TempName = model.TempName,
                TempType = model.TempType,
                Content = Request["Content"]
            });
            SetMessage($"模板{model.TempName}添加成功");
            _iCmsServices.AddTemp(model);
            return Redirect($"~/Cms/CmsTemp?siteId={model.SiteId}");
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ModifyCmsTempView()
        {
            var tempName = Request["tempName"];
            if (tempName.IsNullOrEmpty())
                new ContentResult() { Content = "请填写模板名称!!!" };

            var tempType = Request["tempType"];

            var webCmsSendModel = new WebCmsSendModel()
            {
                Cmd = "SaveTemp",
                Content = Request["tempContent"],
                TempType = tempType,
                SiteName = Request["siteName"],
                TempName = tempName
            };
            var tempId = Convert.ToInt32(Request["tempId"]);
            var temp = _iCmsServices.GetSiteTempById(tempId);

            if (temp.IsNotNull() && tempName != temp.TempName)
                webCmsSendModel.OldTempName = temp.TempName;

            SendWebCms(webCmsSendModel);
            var siteId = Convert.ToInt32(Request["siteId"]);

            _iCmsServices.Update(new SiteTemp()
            {
                Id = tempId,
                SiteId = siteId,
                TempName = tempName,
                TempType = tempType
            });
            return new ContentResult() { Content = "success" };
        }
    }
}