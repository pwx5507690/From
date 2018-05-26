using GS.SQL.DataSource;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS.SQLModel.Cms
{
    public class SiteTemp : Base<SiteTemp>
    {
        public SiteTemp() : base("Cms") {
            
        }
        [JsonProperty(PropertyName = "siteId")]
        [Column("SiteId", DbType.Int32)]
        public int SiteId { get; set; }
        [JsonProperty(PropertyName = "tempName")]
        [Column("TempName", DbType.String)]
        public string TempName { get; set; }
        [JsonProperty(PropertyName = "tempType")]
        [Column("TempType", DbType.String)]
        public string TempType { get; set; }
    }
}
