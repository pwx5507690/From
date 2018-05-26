using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS.Cache.Interface
{
	public interface ICache
	{
		string Get(string key, string dataKey);

		T Get<T>(string key, string dataKey);

		void Set<T>(string key, string dataKey, T value);

		bool IsExisted(string key, string dataKey);

		void Remove(string key, string dataKey);
	}
}
