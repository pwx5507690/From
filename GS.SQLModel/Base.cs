using System;
using GS.SQL.DataSource;
using System.Data;
using Newtonsoft.Json;

namespace GS.SQLModel
{
    public abstract class Base<T> : SQLDataEnity<T>
	{
		public Base() : base("Connection")
		{
			IsDelete = false;
		}
		public Base(string conStr) : base(conStr)
		{
			IsDelete = false;
		}
		[JsonProperty(PropertyName = "updatetime")]
		[Column("Updatetime", DbType.DateTime)]
		public DateTime? Updatetime { get; set; }

		[JsonProperty(PropertyName = "id")]
		[Column("Id", DbType.Int32, ident: true, key: true)]
		public int Id { get; set; }

		[JsonProperty(PropertyName = "isDelete")]
		[Column("IsDelete", DbType.Boolean)]
		public bool IsDelete { get; set; }
	}
}
