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
    public abstract class MessageBase<T> : Base<T>
    {
        [JsonProperty(PropertyName = "sendUserId")]
        [Column("SendUserId", DbType.Int32)]
        public int SendUserId { get; set; }

        [JsonProperty(PropertyName = "acceptUserId")]
        [Column("AcceptUserId", DbType.Int32)]
        public int AcceptUserId { get; set; }

        [JsonProperty(PropertyName = "messageCode")]
        [Column("MessageCode", DbType.String)]
        public string MessageCode { get; set; }

        [JsonProperty(PropertyName = "title")]
        [Column("Title", DbType.String)]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "content")]
        [Column("Content", DbType.String)]
        public string Content { get; set; }

        [JsonProperty(PropertyName = "sendUser")]
        [Column(false)]
        public User SendUser { get; set; }

        [JsonProperty(PropertyName = "acceptUser")]
        [Column(false)]
        public User AcceptUser { get; set; }
    }
    public class Message : MessageBase<Message>
    {

    }
}
