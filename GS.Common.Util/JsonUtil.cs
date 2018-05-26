using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS.Common.Util
{
	public static class JsonUtil
	{
		public static string SerializeObject(this object obj, JsonSerializerSettings setting, Formatting formatting)
		{
			return JsonConvert.SerializeObject(obj, formatting, setting);
		}
		public static string SerializeObject(this object obj)
		{
			return JsonConvert.SerializeObject(obj);
		}
		public static string SerializeObjectFilterNullValue(this object obj)
		{
			var setting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
			return SerializeObject(obj, setting, Formatting.Indented);
		}
		public static T DeserializeObject<T>(this string value)
		{
			return JsonConvert.DeserializeObject<T>(value);
		}
	}
}
