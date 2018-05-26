using System.Configuration;

namespace GS.Common.Util
{
    public class AppSettingsUtil
    {
        private static Configuration Configuration
        {
            get
            {
                return System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            }
        }

        private static void Save(Configuration Configuration)
        {
            Configuration.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        public static void add(string name, string value)
        {
            var config = Configuration;
            config.AppSettings.Settings.Add(name, value);
            Save(config);
        }

        public static void Remove(string name)
        {
            var config = Configuration;
            config.AppSettings.Settings.Remove("name");
            Save(config);
        }

        public static void Modified(string name, string value)
        {
            var config = Configuration;
            if (config.AppSettings.Settings[name] != null)
            {
                config.AppSettings.Settings[name].Value = value;
                Save(config);
            }            
        }
    }
}
