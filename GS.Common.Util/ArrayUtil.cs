using System;
using System.Collections.Generic;
using System.Linq;

namespace GS.Common.Util
{
    public static class ArrayUtil
    {
        public static bool IsEmpty(this Array array)
        {
            return (array.IsNull() || array.Length == 0);
        }

        public static string JoinString(this IEnumerable<string> array, string separator = "")
        {
            if (!array.Any())
                return string.Empty;
            return string.Join(separator, array);
        }

        public static IEnumerable<T> Each<T>(this Array arrry, Func<object, T> callback)
        {
            var list = new List<T>();
            if (arrry.IsEmpty())
                return list;

            foreach (var item in arrry)
                list.Add(callback(item));
            return list;
        }

        public static void Each(this Array arrry, Action<object> callback)
        {
            if (arrry.IsEmpty())
                return;

            foreach (var item in arrry)
                callback(item);
        }
    }
}
