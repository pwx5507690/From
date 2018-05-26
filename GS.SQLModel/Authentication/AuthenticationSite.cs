using GS.SQL.DataSource;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS.SQLModel
{
    public class AuthenticationSite : SQLDataEnity<AuthenticationSite>
    {
        public AuthenticationSite() : base("Authentication")
        {

        }
        [JsonProperty(PropertyName = "id")]
        [Column("Id", DbType.Int32, ident: true, key: true)]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "uuid")]
        [Column("Uuid", DbType.String)]
        public string Uuid { get; set; }
        [JsonProperty(PropertyName = "AuthenticationType")]
        [Column("authenticationType", DbType.String)]
        public string AuthenticationType { get; set; }
        
    }
}
