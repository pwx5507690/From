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
    public class SiteResource : Base<SiteResource>
    {
        public SiteResource() : base("Cms") {

        }
        [JsonProperty(PropertyName = "siteId")]
        [Column("SiteId", DbType.Int32)]
        public int SiteId { get; set; }

        [JsonProperty(PropertyName = "resourceName")]
        [Column("ResourceName", DbType.String)]
        public string ResourceName { get; set; }

        [JsonProperty(PropertyName = "resourceType")]
        [Column("ResourceType", DbType.String)]
        public string ResourceType { get; set; }

        [JsonProperty(PropertyName = "path")]
        [Column("Path", DbType.String)]
        public string Path { get; set; }
    }
}
