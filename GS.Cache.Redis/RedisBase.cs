using GS.Cache.Interface;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace GS.Cache.Redis
{
	public class RedisClient : ICache
	{
		public string Get(string key, string dataKey)
		{
			return RedisBase.HashGet(key, dataKey);
		}

		public T Get<T>(string key, string dataKey)
		{
			return RedisBase.HashGet<T>(key, dataKey);
		}

		public bool IsExisted(string key, string dataKey)
		{
			return RedisBase.HashExist(key, dataKey);
		}

		public void Remove(string key, string dataKey)
		{
			RedisBase.HashRemove(key, dataKey);
		}

		public void Set<T>(string key, string dataKey, T value)
		{
			RedisBase.HashSet(key, dataKey, value);
		}
	}
	public class RedisBase
	{
		private static string RedisRederPath = WebConfigurationManager.AppSettings["RedisReaderPath"];
		private static string RedisWriterPath = WebConfigurationManager.AppSettings["RedisWriterPath"];
		public static PooledRedisClientManager prcm = CreateManager(
			new string[] { RedisRederPath },
			new string[] { RedisWriterPath }
		);
		private static PooledRedisClientManager CreateManager(string[] readWriteHosts, string[] readOnlyHosts)
		{
			return new PooledRedisClientManager(readWriteHosts, readOnlyHosts, new RedisClientManagerConfig
			{
				MaxWritePoolSize = 5,
				MaxReadPoolSize = 5,
				AutoStart = true,
			});
		}
		public static bool ItemSet<T>(string key, T t)
		{
			try
			{
				using (IRedisClient redis = prcm.GetClient())
				{
					return redis.Set<T>(key, t, new TimeSpan(1, 0, 0));
				}
			}
			catch (Exception ex)
			{
				Common.Util.LogUtil.ErrorFormat(ex.Message);
			}
			return false;
		}
		public static T ItemGet<T>(string key) where T : class
		{
			using (IRedisClient redis = prcm.GetClient())
			{
				return redis.Get<T>(key);
			}
		}
		public static bool ItemRemove(string key)
		{
			using (IRedisClient redis = prcm.GetClient())
			{
				return redis.Remove(key);
			}
		}
		public static void ListAdd<T>(string key, T t)
		{
			using (IRedisClient redis = prcm.GetClient())
			{
				var redisTypedClient = redis.As<T>();
				redisTypedClient.AddItemToList(redisTypedClient.Lists[key], t);
			}
		}
		public static bool ListRemove<T>(string key, T t)
		{
			using (IRedisClient redis = prcm.GetClient())
			{
				var redisTypedClient = redis.As<T>();
				return redisTypedClient.RemoveItemFromList(redisTypedClient.Lists[key], t) > 0;
			}
		}
		public static void ListRemoveAll<T>(string key)
		{
			using (IRedisClient redis = prcm.GetClient())
			{
				var redisTypedClient = redis.As<T>();
				redisTypedClient.Lists[key].RemoveAll();
			}
		}
		public static long ListCount(string key)
		{
			using (IRedisClient redis = prcm.GetClient())
			{
				return redis.GetListCount(key);
			}
		}
		public static List<T> ListGetRange<T>(string key, int start, int count)
		{
			using (IRedisClient redis = prcm.GetClient())
			{
				var c = redis.As<T>();
				return c.Lists[key].GetRange(start, start + count - 1);
			}
		}
		public static List<T> ListGetList<T>(string key)
		{
			using (IRedisClient redis = prcm.GetClient())
			{
				var c = redis.As<T>();
				return c.Lists[key].GetRange(0, c.Lists[key].Count);
			}
		}
		public static List<T> ListGetList<T>(string key, int pageIndex, int pageSize)
		{
			int start = pageSize * (pageIndex - 1);
			return ListGetRange<T>(key, start, pageSize);
		}
		public static void ListSetExpire(string key, DateTime datetime)
		{
			using (IRedisClient redis = prcm.GetClient())
			{
				redis.ExpireEntryAt(key, datetime);
			}
		}
		public static void SetAdd<T>(string key, T t)
		{
			using (IRedisClient redis = prcm.GetClient())
			{
				var redisTypedClient = redis.As<T>();
				redisTypedClient.Sets[key].Add(t);
			}
		}
		public static bool SetContains<T>(string key, T t)
		{
			using (IRedisClient redis = prcm.GetClient())
			{
				var redisTypedClient = redis.As<T>();
				return redisTypedClient.Sets[key].Contains(t);
			}
		}
		public static bool SetRemove<T>(string key, T t)
		{
			using (IRedisClient redis = prcm.GetClient())
			{
				var redisTypedClient = redis.As<T>();
				return redisTypedClient.Sets[key].Remove(t);
			}
		}
		public static bool HashExist(string key, string dataKey)
		{
			using (IRedisClient redis = prcm.GetClient())
			{
				return redis.HashContainsEntry(key, dataKey);
			}
		}
		public static bool HashSet<T>(string key, string dataKey, T t)
		{
			using (IRedisClient redis = prcm.GetClient())
			{
				string value = ServiceStack.Text.JsonSerializer.SerializeToString<T>(t);
				return redis.SetEntryInHash(key, dataKey, value);
			}
		}
		public static bool HashRemove(string key, string dataKey)
		{
			using (IRedisClient redis = prcm.GetClient())
			{
				return redis.RemoveEntryFromHash(key, dataKey);
			}
		}
		public static bool HashRemove(string key)
		{
			using (IRedisClient redis = prcm.GetClient())
			{
				return redis.Remove(key);
			}
		}
		public static string HashGet(string key, string dataKey)
		{
			using (IRedisClient redis = prcm.GetClient())
			{
				return redis.GetValueFromHash(key, dataKey);
			}
		}
		public static T HashGet<T>(string key, string dataKey)
		{
			string value = HashGet(key, dataKey);
			if (string.IsNullOrEmpty(value))
			{
				return default(T);
			}
			return ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(value);
		}
		public static List<T> HashGetAll<T>(string key)
		{
			using (IRedisClient redis = prcm.GetClient())
			{
				var list = redis.GetHashValues(key);
				if (list != null && list.Count > 0)
				{
					List<T> result = new List<T>();
					foreach (var item in list)
					{
						var value = ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(item);
						result.Add(value);
					}
					return result;
				}
				return null;
			}
		}
		public static void HashSetExpire(string key, DateTime datetime)
		{
			using (IRedisClient redis = prcm.GetClient())
			{
				redis.ExpireEntryAt(key, datetime);
			}
		}
		public static bool SortedSetAdd<T>(string key, T t, double score)
		{
			using (IRedisClient redis = prcm.GetClient())
			{
				string value = ServiceStack.Text.JsonSerializer.SerializeToString<T>(t);
				return redis.AddItemToSortedSet(key, value, score);
			}
		}
		public static bool SortedSetRemove<T>(string key, T t)
		{
			using (IRedisClient redis = prcm.GetClient())
			{
				string value = ServiceStack.Text.JsonSerializer.SerializeToString<T>(t);
				return redis.RemoveItemFromSortedSet(key, value);
			}
		}
		public static long SortedSetTrim(string key, int size)
		{
			using (IRedisClient redis = prcm.GetClient())
			{
				return redis.RemoveRangeFromSortedSet(key, size, 9999999);
			}
		}
		public static long SortedSetCount(string key)
		{
			using (IRedisClient redis = prcm.GetClient())
			{
				return redis.GetSortedSetCount(key);
			}
		}
		public static List<T> SortedSetGetList<T>(string key, int pageIndex, int pageSize)
		{
			using (IRedisClient redis = prcm.GetClient())
			{
				var list = redis.GetRangeFromSortedSet(key, (pageIndex - 1) * pageSize, pageIndex * pageSize - 1);
				if (list != null && list.Count > 0)
				{
					List<T> result = new List<T>();
					foreach (var item in list)
					{
						var data = ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(item);
						result.Add(data);
					}
					return result;
				}
			}
			return null;
		}
		public static List<T> SortedSetGetListALL<T>(string key)
		{
			using (IRedisClient redis = prcm.GetClient())
			{
				var list = redis.GetRangeFromSortedSet(key, 0, 9999999);
				if (list != null && list.Count > 0)
				{
					List<T> result = new List<T>();
					foreach (var item in list)
					{
						var data = ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(item);
						result.Add(data);
					}
					return result;
				}
			}
			return null;
		}
		public static void SortedSetSetExpire(string key, DateTime datetime)
		{
			using (IRedisClient redis = prcm.GetClient())
			{
				redis.ExpireEntryAt(key, datetime);
			}
		}
	}
}
