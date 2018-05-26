using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using GS.Common.Util;
using Autofac;
using GS.Common.Injection.Core;

namespace GS.Cache.Factory
{
	public enum CacheType
	{
		Redis,
		MemoryCache,
	}
	public class FactoryContainer
	{
		static FactoryContainer()
		{
			Init();
		}
		private static CacheType FactoryType
		{
			get
			{
				var cacheStorage = WebConfigurationManager.AppSettings["CacheStorage"];
				if (cacheStorage.IsNullOrEmpty())
				{
					return CacheType.MemoryCache;
				}
				return (CacheType)Enum.Parse(typeof(CacheType), cacheStorage);
			}
		}

		private static void Init()
		{
			var builder = new ContainerBuilder();
			switch (FactoryType)
			{
				case CacheType.Redis:
					builder.RegisterType<Redis.RedisClient>().AsImplementedInterfaces();
					break;
				case CacheType.MemoryCache:
					builder.RegisterType<Memory.MemoryCache>().AsImplementedInterfaces();
					break;
			}
			 builder.Update(IoC.Container);
		}

		public static Interface.ICache Cache
		{
			get
			{
				return IoC.Container.Resolve<Interface.ICache>();
			}
		}

		public static IContainer GetIContainer()
		{
			return IoC.Container;
		}
	}
}
