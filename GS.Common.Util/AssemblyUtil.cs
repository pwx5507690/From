using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace GS.Common.Util
{
    public class AssemblyUtil
    {
        private static bool _assembliesLoaded;
        private static readonly object SyncRoot = new object();
        public static void ForceLoadAllReferencedAssemblies(string pattern = "GS")
        {
            lock (SyncRoot)
            {
                if (_assembliesLoaded)
                {
                    return;
                }

                LoadAllReferencedAssemblies(pattern);
                _assembliesLoaded = true;
            }
        }
        public static Assembly[] GetAllProjectAssemblies(string pattern = "GS")
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.FullName.StartsWith(pattern));
            return assembly?.ToArray();
        }
        private static void LoadAllReferencedAssemblies(string pattern = "GS")
        {
            FileInfo[] files = null;
            files = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).GetFiles("*.dll",
                SearchOption.AllDirectories).Where(f => Regex.IsMatch(f.Name, pattern)).ToArray();

            foreach (var fi in files)
            {
                var assemblyFullName = fi.FullName;
                var assemblyName = AssemblyName.GetAssemblyName(assemblyFullName);
                if (!AppDomain.CurrentDomain.GetAssemblies().Any(assembly =>
                  AssemblyName.ReferenceMatchesDefinition(assemblyName, assembly.GetName())))
                {
                    Assembly.Load(assemblyName);
                }
            }
        }
    }
}
