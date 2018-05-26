using GS.SQLModel.Form;
using GS.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;
using GS.SQL.DataSource;
using GS.Services;
using Autofac;
using GS.App.Form.Temp;

namespace GS.App.Form.Models.Bulider
{
    public static class FormView
    {
        private static string GetDyncFieldValueByRow(string name, SQLDynamicRow row)
        {
            if (row.IsNull())
                return null;

            var item = row.Row.Where(t => t.Name.Equals(name)).FirstOrDefault();
            return item?.Value;
        }
        private static string BuliderFormView(DyncField dyncField, string value)
        {
            var html = string.Empty;
            if (dyncField.Type == "name" && dyncField.Fmt == "extend")
            {
                var surname = string.Empty;
                var name = string.Empty;

                if (value.IsNotNullOrEmpty())
                {
                    var val = value.Split('.');
                    if (val.Length > 1)
                    {
                        surname = val[0];
                        name = val[1];
                    }
                }
                html = FormTemp.GetTempByKey("formItemName", dyncField.Lbl,
                       dyncField.Name, dyncField.Reqd, dyncField.Uuiq, surname, name
                       );
            }
            else if (dyncField.Type == "file")
            {
                html = FormTemp.GetTempByKey("formFileItem", dyncField.Lbl,
                        dyncField.Name,
                        FormItemView.BuliderFormItemView(dyncField, value));
            }
            else if (dyncField.Type == "section")
            {
                html = FormTemp.GetTempByKey("section", dyncField.Lbl, dyncField.Instr);
            }
            else
            {
                html = FormTemp.GetTempByKey("formItem", dyncField.Lbl, FormItemView.BuliderFormItemView(dyncField, value),
                                       (dyncField.Instr ?? string.Empty));
            }
            return html;
        }
        private static string GetLabel(DyncField dyncField, SQLDynamicItem dynamicItem)
        {
            var value = dynamicItem.Value.Split(',');
            return dyncField.SourceValue.DeserializeObject<IEnumerable<DropdownModel>>().Select(t =>
             {
                 var lab = string.Empty;
                 foreach (var item2 in value)
                     if (item2 == t.Checked) { lab = t.value; continue; }
                 return lab;
             }).Where(t => t.IsNotNullOrEmpty()).JoinString(",").TrimEnd(',');
        }
        public static IHtmlString BuliderDropdownView(this HtmlHelper htmlHelper, DropdownControlModel dropdownControlModel)
        {
            var builder = new TagBuilder("select");
            builder.InnerHtml = dropdownControlModel.Item.Select(item => $"<option {(item.Selected ? "selected" : string.Empty)} value='{item.Value}'>{item.Text}</option>").JoinString();
            builder.MergeAttributes(dropdownControlModel.HtmlAttributes);
            return htmlHelper.Raw(builder.ToString(TagRenderMode.Normal));
        }
        public static IHtmlString BuliderFormView(this HtmlHelper htmlHelper, IEnumerable<DyncField> dyncField, SQLDynamicRow row)
        {
            if (dyncField.IsNull() && !dyncField.Any())
                return htmlHelper.Raw(string.Empty);

            var content = string.Empty;

            foreach (var item in dyncField)
            {
                var vaule = GetDyncFieldValueByRow(item.Name, row);
                content += BuliderFormView(item, vaule);
            }
            var html = FormTemp.GetTempByKey("form", content);
            return htmlHelper.Raw(html);
        }
        public static IHtmlString BuildFormTable(this HtmlHelper htmlHelper, PagedList<SQLDynamicRow> sQLDynamicRow, int page, string code)
        {

            if (!sQLDynamicRow.Any())
                return htmlHelper.BuildByKey("formEmpty");

            var dynamicDataservices = Common.Injection.Core.IoC.Container.Resolve<IDynamicDataservices>();
            var field = dynamicDataservices.GetDyncFieldByFormCode(code);

            var row = sQLDynamicRow.FirstOrDefault().Row.ToList();
            var head = new List<string>();

            for (int i = 0; i < row.Count(); i++)
            {
                if (row[i].Name == "Id")
                    continue;

                var item = field.Where(t => t.Name == row[i].Name).FirstOrDefault();
                if (item.IsNull())
                    continue;

                head.Add(item.Lbl);
            }

            var tuple = new List<Tuple<IList<string>, Func<string>>>();
            for (int i = 0; i < sQLDynamicRow.Count; i++)
            {
                var content = new List<string>();
                var item = sQLDynamicRow[i].Row.ToList();
                var id = string.Empty;
                for (int j = 0; j < item.Count(); j++)
                {
                    if (item[j].Name == "Id")
                    {
                        id = item[j].Value;
                        continue;
                    }

                    var v = field.Where(t => t.Name == item[j].Name).FirstOrDefault();
                    if (v.IsNull())
                        continue;

                    if (v.SourceValue.IsNotNullOrEmpty() && (v.Type == "radio" || v.Type == "checkbox"))
                        content.Add(GetLabel(v, item[j]));
                    else
                        content.Add(item[j].Value);
                }
                tuple.Add(new Tuple<IList<string>, Func<string>>(content, () =>
                {
                    return FormTemp.GetTempByKey("formInfoLink", id, code, page.ToString());
                }));
            }

            return htmlHelper.BuildByTable(new Tuple<IList<string>, IList<Tuple<IList<string>, Func<string>>>, bool>(head,
                tuple, true));
        }
    }
}