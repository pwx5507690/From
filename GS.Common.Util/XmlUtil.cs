using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.Web.Configuration;

namespace GS.Common.Util
{
    public static class XmlUtil
    {
        public class Attribute
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }
        private readonly static IDictionary<string, object> _cacheStorage;
        static XmlUtil()
        {
            _cacheStorage = new Dictionary<string, object>();
        }
        private static bool XmlCache
        {
            get
            {
                var xmlCache = WebConfigurationManager.AppSettings["XmlCache"];
                if (xmlCache.IsNullOrEmpty())
                    return false;
                return Convert.ToBoolean(xmlCache);
            }
        }
        public static IDictionary<string, IEnumerable<XElement>> Read(string path, params string[] name)
        {
            XDocument xd = null;

            if (_cacheStorage.ContainsKey(path) && XmlCache)
            {
                xd = (XDocument)_cacheStorage[path];
            }
            else
            {
                xd = XDocument.Load(path);
                if (XmlCache)
                    _cacheStorage[path] = xd;
            }
            return name.ToDictionary(t => t, t => xd.Root.Descendants(t));
        }
        public static IDictionary<string, string> GetValue(string path, string name, Attribute attribute)
        {
            var elm = Read(path, name)?[name].Where(t => t.Attributes().Any(v => v.Name == attribute.Name && v.Value == attribute.Value));
            if (elm.Any())
                return elm.ToDictionary(t => t.Attribute(attribute.Name).Value, t => t.Value);
            return null;
        }
        public static void serializeToXml<T>(T obj, string url)
        {
            using (var xtw = new XmlTextWriter(url, Encoding.Default))
            {
                new XmlSerializer(typeof(T)).Serialize(xtw, obj);
            }
        }
        public static T DeserializerXml<T>(string data)
        {
            using (var xtr = new XmlTextReader(data))
            {
                return (T)new XmlSerializer(typeof(T)).Deserialize(xtr);
            }
        }
    }
}
