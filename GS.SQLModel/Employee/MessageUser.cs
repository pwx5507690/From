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
    public class MessageAcceptUser : MessageBase<MessageAcceptUser>
    {
        [JsonProperty(PropertyName = "isRead")]
        [Column("IsRead", DbType.Boolean)]
        public bool IsRead { get; set; }

        [JsonProperty(PropertyName = "Stats")]
        [Column("Stats", DbType.String)]
        public string Stats { get; set; }
    }

    public class MessageSendUser : MessageBase<MessageSendUser>
    {
        //[JsonProperty(PropertyName = "isReply")]
        //[Column("IsReply", DbType.Boolean)]
        //public bool IsReply { get; set; }
    }
}
