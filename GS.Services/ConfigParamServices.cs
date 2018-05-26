using System;
using GS.Common.Util;
using System.Web.Configuration;
using GS.Api.Model.Config;

namespace GS.Services
{
    public interface IConfigParamServices
    {
        AppSettings Param();
        Tuple<int, AppSettings> Param(string uuid);
        int Param(string uuid, AppSettings appSettings);
        void Param(AppSettings appSettings);
    }
    public class ConfigParamServices : IConfigParamServices
    {
        public IAuthenticationServices _iAuthenticationServices { get; set; }

        private int Authentication(string uuid)
        {
            if (uuid.IsNullOrEmpty())
                return 401;

            var api = _iAuthenticationServices.GetAuthenticationSiteByUuid(uuid);
            if (api.IsNull())
                return 401;

            if (api.Uuid != uuid)
            {
                _iAuthenticationServices.Remove(uuid);
                return 401;
            }
            return 200;
        }
        public AppSettings Param()
        {
            var appSettings = new AppSettings();
            var isAuthentication = WebConfigurationManager.AppSettings[nameof(appSettings.IsAuthentication)];
            if (isAuthentication.IsNullOrEmpty())
                isAuthentication = "True";

            appSettings.CorsDomains = WebConfigurationManager.AppSettings[nameof(appSettings.CorsDomains)];
            appSettings.CacheStorage = WebConfigurationManager.AppSettings[nameof(appSettings.CacheStorage)];
            appSettings.RedisReaderPath = WebConfigurationManager.AppSettings[nameof(appSettings.RedisReaderPath)];
            appSettings.RedisWriterPath = WebConfigurationManager.AppSettings[nameof(appSettings.RedisWriterPath)];
            appSettings.IsAuthentication = Convert.ToBoolean(isAuthentication);
            return appSettings;
        }
        public Tuple<int, AppSettings> Param(string uuid)
        {
            var code = Authentication(uuid);
            if (code != 200)
                return new Tuple<int, AppSettings>(code, null);

            var appSettings = Param();

            return new Tuple<int, AppSettings>(code, appSettings);
        }

        public int Param(string uuid, AppSettings appSettings)
        {
            var code = Authentication(uuid);
            if (code == 200)
                Param(appSettings);
            return code;
        }

        public void Param(AppSettings appSettings)
        {
            AppSettingsUtil.Modified(nameof(appSettings.CacheStorage), appSettings.CacheStorage);
            AppSettingsUtil.Modified(nameof(appSettings.RedisReaderPath), appSettings.RedisReaderPath);
            AppSettingsUtil.Modified(nameof(appSettings.RedisWriterPath), appSettings.RedisWriterPath);
            AppSettingsUtil.Modified(nameof(appSettings.IsAuthentication), appSettings.IsAuthentication.ToString());
        }
    }
}
