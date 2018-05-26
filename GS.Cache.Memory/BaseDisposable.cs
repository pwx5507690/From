using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS.Cache.Memory
{
	public class BaseDisposable : IDisposable
	{
		bool _disposed;
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		~BaseDisposable()
		{
			Dispose(false);
		}
		protected virtual void Dispose(bool disposing)
		{
			if (_disposed) return; 
			if (disposing)
			{

			}
			_disposed = true;
		}
	}
}
