using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GS.Cache.Memory
{
	public class RuntimeLibrary
	{
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, IntPtr lParam);

		[DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr CreateFileMapping(int hFile, IntPtr lpAttributes, uint flProtect, uint dwMaxSizeHi, uint dwMaxSizeLow, string lpName);

		[DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr OpenFileMapping(int dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, string lpName);

		[DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr MapViewOfFile(IntPtr hFileMapping, uint dwDesiredAccess, uint dwFileOffsetHigh, uint dwFileOffsetLow, uint dwNumberOfBytesToMap);

		[DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
		public static extern bool UnmapViewOfFile(IntPtr pvBaseAddress);

		[DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
		public static extern bool CloseHandle(IntPtr handle);

		[DllImport("kernel32", EntryPoint = "GetLastError")]
		public static extern int GetLastError();
	}
}
