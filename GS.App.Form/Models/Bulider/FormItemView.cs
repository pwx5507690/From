using GS.App.Form.Models;
using GS.SQLModel.Form;
using GS.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using GS.SQL.DataSource;
using GS.App.Form.Temp;
using GS.Common.Http;
using GS.Api.Model;
using System.Net.Http;
using System.Text;

namespace GS.App.Form.Models.Bulider
{
    public class FormItemView
    {
        private const string _checked = "$.{Checked}";
        private const string _value = "$.{value}";
        private const string _isChecked = "$.{isChecked}";
        public static string BuliderFormItemView(DyncField dyncField, string value)
        {
            SetSourceValue(dyncField);
            var builder = CreateTagBuilder(dyncField.Type);

            var htmlAttributes = new Dictionary<string, string>() {
                {"data-name", dyncField.Name},
                {"name", dyncField.Name},
                {"id", dyncField.Name},
                {"data-lbl", dyncField.Lbl},
                {"data-reqd", dyncField.Reqd},
                {"data-uuid", dyncField.Uuiq??"0"}
        };

            var defalutValue = value ?? dyncField.DefaultValue ?? string.Empty;
            if (dyncField.Type == "date" && defalutValue.IsNullOrEmpty())
            {
                defalutValue = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (dyncField.Type == "datetime" && defalutValue.IsNullOrEmpty())
            {
                defalutValue = DateTime.Now.ToString("yyyy-MM-dd HH:ss");
            }
            if (dyncField.Type == "time" && defalutValue.IsNullOrEmpty())
            {
                defalutValue = DateTime.Now.ToString("HH:ss");
            }
            if (dyncField.Type == "number"
                || dyncField.Type == "text"
                || dyncField.Type == "url"
                || dyncField.Type == "email"
                || dyncField.Type == "map"
                || dyncField.Type == "phone"
                || dyncField.Type == "name"
                || dyncField.Type == "address"
                || dyncField.Type == "time"
                || dyncField.Type == "date"
                || dyncField.Type == "datetime")
            {
                htmlAttributes.Add("value", defalutValue);
                htmlAttributes.Add("placeholder", "请入输" + dyncField.Lbl);
            }

            if (dyncField.Type == "file")
                htmlAttributes.Add("value", defalutValue);

            if (dyncField.Type == "name")
                htmlAttributes.Add("data-fmt", dyncField.Fmt);

            if (dyncField.Type == "textarea")
            {
                builder.SetInnerText(defalutValue);
                htmlAttributes.Add("placeholder", "请入输" + dyncField.Lbl);
            }

            if (dyncField.Type == "dropdown2")
            {
                htmlAttributes.Add("data-pn", dyncField.PN ?? "2");
                htmlAttributes.Add("data-select", dyncField.Name);
            }

            if (dyncField.Type == "dropdown")
            {
                builder.InnerHtml = GetDropdownElement(dyncField, "<option value='" + _checked + "'>" + _value + "</option>", string.Empty);
                htmlAttributes.Add("value", defalutValue);

            }

            if (dyncField.Type == "checkbox" || dyncField.Type == "radio")
                builder.InnerHtml = GetDropdownElement(dyncField, FormTemp.GetTempByKey(dyncField.Type, dyncField.Id.ToString(),
                        _isChecked, _checked, _value), defalutValue);

            var required = string.Empty;

            //if (dyncField.Reqd == "1")
            //{
            //	htmlAttributes.Add("required", "");
            //	htmlAttributes.Add("oninvalid", "setCustomValidity('请入输" + dyncField.Name + "！');");
            //	htmlAttributes.Add("oninput", "setCustomValidity('');");
            //}
            //builder.IdAttributeDotReplacement = "-";
            //builder.GenerateId(dyncField.Id.ToString());
            builder.MergeAttributes(htmlAttributes);

            return builder.TagName == "input"
                ? builder.ToString(TagRenderMode.SelfClosing)
                : builder.ToString(TagRenderMode.Normal);

        }
        private static void SetSourceValue(DyncField dyncField)
        {
            if (dyncField.Source == 0)
                return;

            if (dyncField.Source == 1 && dyncField.SourceValue.IsNullOrEmpty())
                throw new Exception("url Param is null error");

            var urlParam = dyncField.SourceValue.Split('=')[1];
            if (urlParam.IsNullOrEmpty())
                throw new Exception("url urlParam is length number error");

            var url = dyncField.SourceValue.Split('?')[0];
            if (url.IsNullOrEmpty())
                throw new Exception("api url error");

            var str = urlParam.Split('|');
            if (str.Length < 3)
                throw new Exception("url Param is length number error");

            var dyncExp = new DyncExp();
            if (str[0].IsNullOrEmpty())
                throw new Exception("url table Param is error");

            dyncExp.Table = str[0];
            if (str[1].IsNullOrEmpty())
                throw new Exception("url FiledName Param is error");

            dyncExp.FiledName = str[1];
            if (str[2].IsNotNullOrEmpty())
                dyncExp.Param = str[2].Split(',').ToDictionary(t => t.Split(':')[0], x => x.Split(':')[1]);

            var result = ApiProxy.Send(Common.Http.HttpMethod.POST, url, new StringContent(dyncExp.SerializeObject(), Encoding.UTF8));
            if (result.IsNullOrEmpty() || result == "null")
                return;

            dyncField.Itms = result.DeserializeObject<IEnumerable<SQLDynamicItem>>().Select(t =>
            {
                return new DropdownModel()
                {
                    value = t.Value,
                    Checked = t.Value
                };
            });
        }
        private static string GetDropdownElement(DyncField dyncField, string fromatHtml, string defaultValue)
        {
            var val = defaultValue.Split(',');
            var sourceValue = dyncField.Source == 1 ? (IEnumerable<DropdownModel>)dyncField.Itms : dyncField.SourceValue.DeserializeObject<IEnumerable<DropdownModel>>();

            if (sourceValue.IsNull())
                return string.Empty;

            return string.Join(string.Empty, sourceValue.Select(t =>
            {
                return fromatHtml.Replace(_value, t.value).Replace(_checked, t.Checked).Replace(_isChecked,
                    val.Contains(t.Checked) ? "checked='checked'" : string.Empty);
            }));
        }
        private static TagBuilder CreateTagBuilder(string type)
        {
            TagBuilder tag = null;
            if (type == "number" || type == "text" || type == "url" || type == "email"
                || type == "map" || type == "phone" || type == "name" || type == "address"
                || type == "date" || type == "time" || type == "datetime")
            {
                tag = new TagBuilder("input");
                if (type == "number" || type == "email"
                || type == "date" || type == "time" || type == "datetime")
                    tag.MergeAttribute("type", type);
                else
                    tag.MergeAttribute("type", "text");
                tag.MergeAttribute("data-type", type);
                tag.AddCssClass("tpl-form-input");
            }

            if (type == "dropdown2")
            {
                tag = new TagBuilder("input");
                tag.MergeAttribute("type", "text");
                tag.MergeAttribute("data-dropdown2", "text");
                tag.MergeAttribute("style", "display:none");
            }

            if (type == "checkbox" || type == "radio")
            {
                tag = new TagBuilder("div");
                tag.MergeAttribute("style", "margin-top:5px;");
                tag.MergeAttribute("data-type", type);
            }

            if (type == "file")
            {
                tag = new TagBuilder("input");
                tag.MergeAttribute("type", "text");
                tag.MergeAttribute("readonly", "readonly");
                tag.MergeAttribute("style", "float:left");
                tag.AddCssClass("tpl-form-input");
            }

            if (type == "textarea" || type == "html")
            {
                tag = new TagBuilder("textarea");
                tag.MergeAttribute("style", "border: 1px solid #cccccc; height:150px");
                tag.MergeAttribute("data-type", type);
            }

            if (type == "dropdown")
            {
                tag = new TagBuilder("select");
                tag.MergeAttribute("data-type", type);
            }

            if (tag.IsNull())
            {
                throw new Exception("不能识别的表单类型");
            }
            return tag;
        }
    }
}