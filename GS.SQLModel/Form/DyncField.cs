using GS.SQL.DataSource;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS.SQLModel.Form
{
    public class DyncField : DyncBaseForm<DyncField>
    {
        // 表code
        [ColumnAttribute("DyncForm", DbType.String)]
        [JsonProperty(PropertyName = "dyncForm")]
        public string DyncForm { get; set; }
        // 字段名称
        [JsonProperty(PropertyName = "name")]
        [ColumnAttribute("Name", DbType.String)]
        public string Name { get; set; }
        // 字段类型
        [JsonProperty(PropertyName = "TYP")]
        [ColumnAttribute("Type", DbType.String)]
        public string Type { get; set; }
        // 显示名称
        [JsonProperty(PropertyName = "LBL")]
        [ColumnAttribute("Lbl", DbType.String)]
        public string Lbl { get; set; }
        // 长度
        [JsonProperty(PropertyName = "FLDSZ")]
        [ColumnAttribute("Fldsz", DbType.String)]
        public string Fldsz { get; set; }
        // 备注
        [JsonProperty(PropertyName = "remark")]
        [ColumnAttribute("Remark", DbType.String)]
        public string Remark { get; set; }
        // 必须输入
        [JsonProperty(PropertyName = "REQD")]
        [ColumnAttribute("Reqd", DbType.String)]
        public string Reqd { get; set; }
        // 不许重复
        [JsonProperty(PropertyName = "UNIQ")]
        [ColumnAttribute("Uuiq", DbType.String)]
        public string Uuiq { get; set; }
        // 图片
        [JsonProperty(PropertyName = "IMG")]
        [ColumnAttribute("Img", DbType.String)]
        public string Img { get; set; }
        // 默认值
        [JsonProperty(PropertyName = "DEF")]
        [ColumnAttribute("DefaultValue", DbType.String)]
        public string DefaultValue { get; set; }
        // 字段说明
        [JsonProperty(PropertyName = "INSTR")]
        [ColumnAttribute("Instr", DbType.String)]
        public string Instr { get; set; }
        // 层级
        [JsonProperty(PropertyName = "pN")]
        [ColumnAttribute("PN", DbType.String)]
        public string PN { get; set; }
        // 姓名格式
        [JsonProperty(PropertyName = "FMT")]
        [ColumnAttribute("FMT", DbType.String)]
        public string Fmt { get; set; }

        [JsonProperty(PropertyName = "info")]
        [ColumnAttribute("Info", DbType.String)]
        public string Info { get; set; }

        [JsonProperty(PropertyName = "indication")]
        [ColumnAttribute("Indication", DbType.Int32)]
        public int Indication { get; set; }

        [JsonProperty(PropertyName = "Source")]
        [ColumnAttribute("Source", DbType.Int32)]
        public int Source { get; set; }

        [ColumnAttribute("SourceValue", DbType.String)]
        public string SourceValue { get; set; }

        [ColumnAttribute(false)]
        [JsonProperty(PropertyName = "ITMS")]
        public object Itms { get; set; }

        [ColumnAttribute(false)]
        [JsonProperty(PropertyName = "SECDESC")]
        public string Secdesc { get; set; }
    }
}
