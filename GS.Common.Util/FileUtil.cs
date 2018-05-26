using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace GS.Common.Util
{
    public static class FileUtil
    {
        private readonly static ReaderWriterLockSlim _logWriteLock = new ReaderWriterLockSlim();
        public static void ReNameFile(string oldName, string newName, bool isDelete = true)
        {
            var fi = new FileInfo(oldName);
            fi.MoveTo(newName);
            if (isDelete)
                DeleteFile(oldName);
        }
        public static void CreateFile(string path, byte[] content, bool isCover = true)
        {
            if (isCover)
                DeleteFile(path);
            try
            {
                _logWriteLock.EnterWriteLock();
                using (var fs = new FileStream(path, FileMode.Create))
                {
                    fs.Write(content, 0, content.Length);
                    fs.Flush();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                _logWriteLock.ExitWriteLock();
            }

        }
        public static void CreateFile(string path, string content, bool isCover = true)
        {
            try
            {
                if (isCover)
                    DeleteFile(path);
                _logWriteLock.EnterWriteLock();
                using (var fs = new FileStream(path, FileMode.OpenOrCreate))
                {
                    using (var sw = new StreamWriter(fs,Encoding.GetEncoding("GB2312")))
                    {
                        sw.Write(content);
                        sw.Flush();
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                _logWriteLock.ExitWriteLock();
            }
        }
        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }
        public static void DeleteFolder(string path) {
            if (Directory.Exists(path))
                Directory.Delete(path);
        }
        public static void CreateFolder(string path, bool isCovere = true)
        {
            if (Directory.Exists(path))
                if (isCovere)
                    Directory.Delete(path);
                else
                    return;
            Directory.CreateDirectory(path);
        }
        public static string ReadFile(string fileName)
        {
            if (!File.Exists(fileName))
                return null;

            using (var stream = new StreamReader(fileName, Encoding.GetEncoding("GB2312")))
                return stream.ReadToEnd();
        }
        public static string ValidateFileName(this string name)
        {
            var _asciicode = new [] { 47, 58, 92, 42, 63, 34, 37, 60, 124, 62 };
            var _defaultAsciicode = 46;
            return name.ToCharArray().Each(item =>
            {
                int asciicode = (int)(item);
                if (_asciicode.Contains(asciicode))
                    asciicode = _defaultAsciicode;
                return ((char)asciicode).ToString();
            }).JoinString(string.Empty);
        }
    }
}
