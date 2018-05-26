using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GS.Common.Util;
using GS.Cache.Interface;

namespace GS.Cache.Memory
{
	public class MemoryCache : ICache
	{
		public string Get(string key, string dataKey)
		{
			return BaseMemoryCache.GetCache(dataKey);
		}

		public T Get<T>(string key, string dataKey)
		{
			return BaseMemoryCache.GetCache<T>(dataKey);
		}

		public bool IsExisted(string key, string dataKey)
		{
			return BaseMemoryCache.IsExisted(dataKey);
		}

		public void Remove(string key, string dataKey)
		{
			return;
		}

		public void Set<T>(string key, string dataKey, T value)
		{
			BaseMemoryCache.SetCache(dataKey, value);
		}
	}
	public class BaseMemoryCache
	{
		private static readonly long _size = 1024000;
		public static void SetCache<T>(string cacheKey, T objObject)
		{
			using (var memoryDb = new MemoryDb())
			{
				if (memoryDb.Init(cacheKey, _size) == MemoryDbResult.Success)
				{
					var data = objObject.SerializeObject().ToByte();
					memoryDb.Write(data, 0, data.Length);		
				}
			}			
		}
		public static string GetCache(string cacheKey)
		{
			using (var memoryDb = new MemoryDb())
			{
				if (memoryDb.Init(cacheKey, _size) == MemoryDbResult.Success)
				{
					var data = new byte[_size];
					memoryDb.Read(ref data, 0, data.Length);
					return StringUitl.ByteToString(data);
				}
				return null;
			}
		}
		public static T GetCache<T>(string cacheKey)
		{
			return GetCache(cacheKey).DeserializeObject<T>();
		}

		public static bool IsExisted(string cacheKey)
		{
			return GetCache(cacheKey).IsNotNullOrEmpty();
		}
	}
}
