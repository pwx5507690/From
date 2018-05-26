using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS.Common.Util
{
	public static class ObjectUtil
	{
		public static bool IsNotNull(this object obj)
		{
			return obj != null;
		}
		public static bool IsNull(this object obj)
		{
			return obj == null;
		}
		public static object CallMethod(this object obj, string method, params object[] param)
		{
			return obj.GetType().GetMethod(method).Invoke(obj, param);
		}
		public static T Clone<T>(this object obj)
		{
			return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj));
		}
	}
}
