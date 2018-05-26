using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS.Cache.Identity
{
	public class BaseIdentityModel
	{
		public DateTime LoginTime { get; set; }
	}

	public class IdentityModel<T> : BaseIdentityModel
	{
		public T Model { get; set; }
	}
}
