using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GS.Cache.Memory
{
    public enum MemoryDbResult
    {
        Success = 0,
        NotInit = 1,
        CreateError = 2,
        MapperError = 3,
        OpenError = 4,
        Runout = 5
    }
    public class MemoryDb : BaseDisposable
    {
        private IntPtr mhSharedMemoryFile = IntPtr.Zero;
        private IntPtr mpwData = IntPtr.Zero;
        private bool mbAlreadyExist = false;
        private bool mbInit = false;
        private long mMemSize = 0;
        private bool _disposed;
        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Close();
                }
                _disposed = true;

            }
            base.Dispose(disposing);
        }
        public void Close()
        {
            if (mbInit)
            {
                RuntimeLibrary.UnmapViewOfFile(mpwData);
                RuntimeLibrary.CloseHandle(mhSharedMemoryFile);
            }
        }
        public MemoryDbResult Init(string strName, long lngSize)
        {
            if (lngSize <= 0 || lngSize > 0x00800000) lngSize = 0x00800000;
            mMemSize = lngSize;
            if (strName.Length > 0)
            {
                mhSharedMemoryFile = RuntimeLibrary.CreateFileMapping(
                    Constant.INVALID_HANDLE_VALUE,
                    IntPtr.Zero,
                    (uint)Constant.PAGE_READWRITE,
                    0,
                    (uint)lngSize,
                    strName
                );
                if (mhSharedMemoryFile == IntPtr.Zero)
                {
                    mbAlreadyExist = false;
                    mbInit = false;
                    return MemoryDbResult.CreateError;
                }
                else
                {
                    mbAlreadyExist = RuntimeLibrary.GetLastError() == Constant.ERROR_ALREADY_EXISTS;
                }
                mpwData = RuntimeLibrary.MapViewOfFile(mhSharedMemoryFile, Constant.FILE_MAP_WRITE, 0, 0, (uint)lngSize);
                if (mpwData == IntPtr.Zero)
                {
                    mbInit = false;
                    RuntimeLibrary.CloseHandle(mhSharedMemoryFile);
                    return MemoryDbResult.MapperError;
                }
                else
                {
                    mbInit = true;
                    if (mbAlreadyExist == false)
                    {
                        var hander = RuntimeLibrary.OpenFileMapping(Constant.FILE_MAP_ALL_ACCESS, false,
                            strName);
                        mbAlreadyExist = true;
                        if (hander == IntPtr.Zero)
                            return MemoryDbResult.OpenError;
                    }
                }
            }
            else
            {
                return MemoryDbResult.NotInit;
            }

            return MemoryDbResult.Success;
        }
        public MemoryDbResult Read(ref byte[] bytData, int lngAddr, int lngSize)
        {
            if (lngAddr + lngSize > mMemSize) return MemoryDbResult.Runout;

            if (mbInit)
                Marshal.Copy(mpwData, bytData, lngAddr, lngSize);
            else
                return MemoryDbResult.NotInit;
            return MemoryDbResult.Success;
        }
        public MemoryDbResult Write(byte[] bytData, int lngAddr, int lngSize)
        {
            if (lngAddr + lngSize > mMemSize)
                return MemoryDbResult.Runout;

            if (mbInit)
                Marshal.Copy(bytData, lngAddr, mpwData, lngSize);
            else
                return MemoryDbResult.NotInit;
            return MemoryDbResult.Success;
        }
    }
}
