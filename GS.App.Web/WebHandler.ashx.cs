using System;
using System.Web;
using GS.Common.Util;
using GS.Common.Web;
using System.Web.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace GS.App.Web
{
    public class WebHandler : IHttpHandler
    {

        private static readonly string _authorizedAddress = WebConfigurationManager.AppSettings["AuthorizedAddress"];

        private readonly static string _path = "~/Temp/SiteTemp.xml";
        private readonly static string _tempName = "item";
        private readonly static string _targetAttributeKey = "id";
        private static string GetTempByKey(string key, params string[] param)
        {
            var util = new XmlUtil.Attribute()
            {
                Name = _targetAttributeKey,
                Value = key
            };
            var result = XmlUtil.GetValue(Constant.HttpContext.Server.MapPath(_path), _tempName, util)?[key] ?? string.Empty;
            return string.Format(result, param);
        }
        public void ProcessRequest(HttpContext context)
        {
            Authorize();
            try
            {
                var obj = this.CallMethod(Command);
                if (obj.IsNotNull())
                    context.Response.Write(obj.ToString());
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private void Authorize()
        {
            if (Constant.Request.HttpMethod.ToLower() != "post")
            {
                Constant.Response.StatusCode = 404;
                Constant.Response.End();
            }
            if (_authorizedAddress.IsNullOrEmpty())
            {
                Constant.Response.StatusCode = 403;
                Constant.Response.End();
            }
            if (Constant.Request.Headers["Authorization"] != _authorizedAddress.GetMd5Str())
            {
                Constant.Response.StatusCode = 401;
                Constant.Response.End();
            }
        }
        private string GetDeletePath()
        {
            if (OldTempName.IsNullOrEmpty())
                return null;
            var tuple = GetPath(SiteName, OldTempName, TempType);
            if (tuple.IsNull())
                return null;
            return tuple.Item1;
        }
        private Tuple<string, string> GetPath(string siteName, string tempName, string tempType)
        {
            var path = string.Empty;
            var head = string.Empty;
            if (tempType == "aspx")
            {
                path = $"~/{siteName}/{tempName}.aspx";
                head = GetTempByKey(TempType, $"http://{Constant.Request.Url.Host}", siteName, $"{tempName}.aspx", DateTime.Now.ToString("yyyy-MM-dd hh:ss"));
            }
            else if (tempType == "css")
            {
                path = $"~/{siteName}/css/{tempName}.css";
                head = GetTempByKey(TempType, siteName, $"{tempName}.css", DateTime.Now.ToString("yyyy-MM-dd hh:ss"));
            }
            else if (tempType == "js")
            {
                path = $"~/{siteName}/js/{tempName}.js";
                head = GetTempByKey(TempType, siteName, $"{tempName}.js", DateTime.Now.ToString("yyyy-MM-dd hh:ss"));
            }
            else if (tempType == "control")
            {
                path = $"~/{siteName}/control/{tempName}.ascx";
                head = GetTempByKey(TempType, siteName, $"{tempName}.ascx", DateTime.Now.ToString("yyyy-MM-dd hh:ss"));
            }
            return new Tuple<string, string>(Constant.Server.MapPath(path), head);
        }
        private Tuple<string, string> GetPath()
        {
            if (TempName.IsNullOrEmpty() || TempType.IsNullOrEmpty())
                return null;
            var tuple = GetPath(SiteName, TempName, TempType);
            if (tuple.IsNull())
                return null;
            return tuple;
        }
        public void DeleteSite()
        {
            var path = Constant.Server.MapPath($"~/{SiteName}");
            FileUtil.DeleteFolder(path);
        }
        public void DeleteTemp()
        {
            var path = GetDeletePath();
            if (path.IsNotNullOrEmpty())
                FileUtil.DeleteFile(GetDeletePath());
        }
        public void ReNameTemp()
        {

            var path = GetDeletePath();
            var tuple = GetPath();
            if (path.IsNotNullOrEmpty() && tuple.IsNotNull())
                FileUtil.ReNameFile(GetDeletePath(), tuple.Item1);
        }
        public void DeleteResource()
        {
            Resource.Split(',').Each(t =>
            {
                var name = Path.GetFileName(t.ToString());
                FileUtil.DeleteFile(Constant.Server.MapPath($"~/{SiteName}/resource/{name}"));
            });
        }
        public void SaveResource()
        {
            Resource.Split(',').Each(t =>
            {
                var name = Path.GetFileName(t.ToString());
                FileUtil.CreateFile(Constant.Server.MapPath($"~/{SiteName}/resource/{name}"),
                Client.GetData($"http://{_authorizedAddress}/{t.ToString()}"));
            });
        }
        public void SaveTemp()
        {
            DeleteTemp();
            var tuple = GetPath();
            if (tuple.IsNotNull())
            {
                var head = tuple.Item2;
                head = Content.Contains("include=true") ? string.Empty : head;
                FileUtil.CreateFile(tuple.Item1, $"{head}{Content}");
            }
        }
        public void CreateIcon()
        {
            if (Icon.IsNotNullOrEmpty())
                FileUtil.CreateFile(Constant.Server.MapPath($"~/{SiteName}/icon.png"), Client.GetData($"http://{_authorizedAddress}/{Icon}"));
        }
        public void QueryTemp()
        {
            var tuple = GetPath();
            if (tuple.IsNull())
                return;
            Constant.Response.ContentEncoding = Encoding.GetEncoding("gb2312");
            Constant.Response.Write(FileUtil.ReadFile(tuple.Item1));
            Constant.Response.End();
        }
        public void CreateSite()
        {
            var sitePath = Constant.Server.MapPath($"~/{SiteName}");

            FileUtil.CreateFolder(sitePath, false);
            FileUtil.CreateFolder($"{sitePath}/js", false);
            FileUtil.CreateFolder($"{sitePath}/css", false);
            FileUtil.CreateFolder($"{sitePath}/control", false);
            FileUtil.CreateFolder($"{sitePath}/resource", false);

            CreateIcon();
            if (Option == "save")
                SaveTemp();
            if (Option == "ReName")
                ReNameTemp();
        }
        private string Command
        {
            get
            {
                return Constant.Request["cmd"];
            }
        }
        private string Option
        {
            get
            {
                return Constant.Request["option"];
            }
        }
        private string Resource
        {
            get
            {
                return Constant.Request["resource"];
            }
        }
        private string SiteName
        {
            get
            {
                return Constant.Request["siteName"];
            }
        }
        private string Icon
        {
            get
            {
                return Constant.Request["icon"];
            }
        }
        private string TempName
        {
            get
            {
                return Constant.Request["tempName"];
            }
        }
        private string TempType
        {
            get
            {
                return Constant.Request["tempType"];
            }
        }
        private string Content
        {
            get
            {
                return Constant.Request["content"];
            }
        }
        private string OldTempName
        {
            get
            {
                return Constant.Request["oldTempName"];
            }
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}