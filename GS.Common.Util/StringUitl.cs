using NPinyin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GS.Common.Util
{
    public static class StringUitl
    {
        public static bool IsNotNullOrEmpty(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
        public static string ByteToString(byte[] value)
        {
            return Encoding.GetEncoding("gb2312").GetString(value);
        }
        public static byte[] ToByte(this string value)
        {
            return Encoding.GetEncoding("gb2312").GetBytes(value);
        }
        public static string ConvertChinese(string text)
        {
            var gb2312 = Encoding.GetEncoding("GB2312");
            return Pinyin.GetPinyin(text, gb2312).Trim();
        }
        public static string GenerateOrderNumber()
        {
            string strDateTimeNumber = DateTime.Now.ToString("yyyyMMddHHmmssms");
            string strRandomResult = NextRandom(10000000, 1).ToString();
            return strDateTimeNumber + strRandomResult;
        }
        public static int NextRandom(int numSeeds, int length)
        {
            byte[] randomNumber = new byte[length];
            var rng = new RNGCryptoServiceProvider();

            rng.GetBytes(randomNumber);
            uint randomResult = 0x0;
            for (int i = 0; i < length; i++)
                randomResult |= ((uint)randomNumber[i] << ((length - 1 - i) * 8));

            return (int)(randomResult % numSeeds) + 1;
        }
        public static bool IsChinese(this string str)
        {
            return Regex.IsMatch(str, @"[\u4e00-\u9fa5]");
        }
        public static bool IsChartFirst(string str)
        {
            Regex regChar = new Regex("^[a-z]");
            Regex regDChar = new Regex("^[A-Z]");
            return regChar.IsMatch(str) || regDChar.IsMatch(str);
        }
        public static bool IsNumberic(string str)
        {
            var rex = new Regex(@"^\d+$");
            return rex.IsMatch(str);
        }
        public static string GetMd5Str32(this string convertString)
        {
            string cl = convertString;
            string pwd = string.Empty;
            MD5 md5 = MD5.Create();
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            for (int i = 0; i < s.Length; i++)
            {
                pwd = pwd + s[i].ToString("x");
            }
            return pwd;
        }
        public static string TrimSpace(this string convertString) {
            return convertString.Replace(" ","");
        }
        public static string UpperFirst(this string convertString)
        {
            return Regex.Replace(convertString, @"\b[a-z]\w+", delegate (Match match)
            {
                string v = match.ToString();
                return char.ToUpper(v[0]) + v.Substring(1);
            });
        }
        public static string GetMd5Str(this string convertString)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(convertString)), 4, 8);
            t2 = t2.Replace("-", "");
            t2 = t2.ToLower();
            return t2;
        }
    }
}
