using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS.Cache.Memory
{
	public class Constant
	{
		public const int ERROR_ALREADY_EXISTS = 183;

		public const int FILE_MAP_COPY = 0x0001;
		public const int FILE_MAP_WRITE = 0x0002;
		public const int FILE_MAP_READ = 0x0004;
		public const int FILE_MAP_ALL_ACCESS = 0x0002 | 0x0004;

		public const int PAGE_READONLY = 0x02;
		public const int PAGE_READWRITE = 0x04;
		public const int PAGE_WRITECOPY = 0x08;
		public const int PAGE_EXECUTE = 0x10;
		public const int PAGE_EXECUTE_READ = 0x20;
		public const int PAGE_EXECUTE_READWRITE = 0x40;

		public const int SEC_COMMIT = 0x8000000;
		public const int SEC_IMAGE = 0x1000000;
		public const int SEC_NOCACHE = 0x10000000;
		public const int SEC_RESERVE = 0x4000000;
		public const int INVALID_HANDLE_VALUE = -1;
	}
}
