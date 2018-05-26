using GS.Cache.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GS.Common.Web;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using GS.Common.Util;
using GS.Cache.Interface;
using GS.Cache.Factory;
using System.Web.Configuration;

namespace GS.Cache.Identity
{
    public class BaseIdentity : IdentityConstant
    {
        private readonly static ICache _iCache = FactoryContainer.Cache;
        private static int Interval
        {
            get
            {
                var interval = _interval;
                return interval.IsNotNullOrEmpty() ? int.Parse(interval) : _defaultInterval;
            }
        }
        public static bool ValidateIdent<T>(string token)
        {
            if (!IsExist(token))
                return false;

            var model = _iCache.Get<IdentityModel<T>>(_hashKey, token);
            if (model.IsNull())
            {
                RemoveClientToken();
                return false;
            }
            if (DateTimeUtil.CalcTimeDifference(model.LoginTime, DateTime.UtcNow) > Interval)
            {
                Remove(token);
                return false;
            }
            return true;
        }
        public static IdentityModel<T> GetIdentityNoWebClient<T>(string token)
        {
            if (!IsExist(token))
                return null;

            var model = _iCache.Get<IdentityModel<T>>(_hashKey, token);
            if (model.IsNotNull())
                if (DateTimeUtil.CalcTimeDifference(model.LoginTime, DateTime.UtcNow) > Interval)
                    return null;

            return model;
        }
        public static IdentityModel<T> GetIdentity<T>(string token)
        {
            var model = GetIdentityNoWebClient<T>(token);
            if (model.IsNull())
            {
                RemoveClientToken();
                return null;
            }
            return model;
        }
        public static bool IsExist(string token)
        {
            return _iCache.IsExisted(_hashKey, token);
        }
        public static bool SetIdentity<T>(string token, IdentityModel<T> model)
        {
            if (IsExist(token))
                Remove(token);
            _iCache.Set(_hashKey, token, model);
            return true;
        }
        public static IdentityModel<T> GetUser<T>()
        {
            var token = GetClientToken();
            if (GetClientToken().IsNullOrEmpty())
                return default(IdentityModel<T>);

            return GetIdentity<T>(token);
        }
        public static void RemoveClientToken()
        {
            Client.ClearCookie(_storageName);
        }
        public static string GetClientToken()
        {
            return Client.GetCookieValue(_storageName);
        }
        public static void SetClientToken(string token)
        {
            Client.SetCookie(_storageName, token);
        }
        public static bool Remove()
        {
            _iCache.Remove(_hashKey, GetClientToken());
            RemoveClientToken();
            return true;
        }
        public static bool Remove(string token)
        {
            _iCache.Remove(_hashKey, token);
            RemoveClientToken();
            return true;
        }
    }
}
