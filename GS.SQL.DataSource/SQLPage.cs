using Newtonsoft.Json;
using System.Collections.Generic;

namespace GS.SQL.DataSource
{
    public class SQLPage<T> {
		[JsonProperty(PropertyName = "result")]
		public IEnumerable<T> Result { get; set; }
		[JsonProperty(PropertyName = "count")]
		public int Count { get; set; }
		[JsonProperty(PropertyName = "isNext")]
		public bool IsNext { get; set; }
		[JsonProperty(PropertyName = "pagination")]
		public int Pagination { get; set; }
	}	
}
