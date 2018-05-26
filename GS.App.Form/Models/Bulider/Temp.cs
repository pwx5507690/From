using GS.App.Form.Temp;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using GS.Common.Util;
using System.Text;

namespace GS.App.Form.Models.Bulider
{
    public static class Temp
    {
        public static IHtmlString BuildByKey(this HtmlHelper htmlHelper, string key, params string[] param)
        {
            return htmlHelper.Raw(FormTemp.GetTempByKey(key, param));
        }

        public static IHtmlString BuildByTable(this HtmlHelper htmlHelper, Tuple<IList<string>, IList<Tuple<IList<string>, Func<string>>>, bool> param,string optionName= "编辑")
        {
            if (param.Item3)
                param.Item1.Add(optionName);

            var head = param.Item1.Select(t => $"<th>{t}</th>").JoinString();
            var content = new StringBuilder();

            for (int i = 0; i < param.Item2.Count; i++)
            {
                var clas = i % 2 == 0 ? "gradeX" : "even";
                var item = param.Item2[i];
                var td = item.Item1.Select(n => $"<td>{n}</td>").JoinString();
                if (param.Item3)
                    td = td + $"<td>{item.Item2()}</td>";
                content.Append($"<tr class='{clas}'>{td}</tr>");
            }
            return htmlHelper.BuildByKey("table", head, content.ToString());
        }
    }
}