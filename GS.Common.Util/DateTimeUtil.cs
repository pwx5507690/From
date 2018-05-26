using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS.Common.Util
{
	public static class DateTimeUtil
	{
		public static int CalcTimeDifference(DateTime d1, DateTime d2)
		{
			var d = d2 - d1;
			return (int)d.TotalMinutes;
		}
		public static double GetCurrentTick()
		{
			var d1 = DateTime.UtcNow;
            var d2 = new DateTime(1970, 1, 1);
            var d = d1.Subtract(d2).TotalMilliseconds;
			return d;
		}
		
	}
}
