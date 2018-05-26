using GS.Cache.Identity;
using GS.Common.Util;
using Microsoft.AspNet.SignalR.Client;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCilentProxy
{
    public static class IAppBuilderExtend
    {
        public static void RunConnectionMessageCilent(this IAppBuilder builder, string uuid, string user,Action<IHubProxy> initHubProxy = null)
        {
            MessageCilent.Init(new MessageToken()
            {
                User = user,
                Uuid = uuid,
            }, initHubProxy);
        }
    }
    public class MessageToken
    {
        public string Uuid { get; set; }
        public string User { get; set; }
    }
    public class MessageCilent
    {
        private static readonly string _url = System.Configuration.ConfigurationSettings.AppSettings["HubAddress"];
        private const string _hubName = "hub";
        private static HubConnection _connection;
        private static IHubProxy _hubProxy;
        public static void Init(MessageToken messageToken, Action<IHubProxy> initHubProxy = null)
        {
            if (string.IsNullOrEmpty(_url))
                return;
            _connection = new HubConnection(_url, GetHubToken(messageToken));
            _hubProxy = _connection.CreateHubProxy(_hubName);
            initHubProxy?.Invoke(_hubProxy);
            _connection.Error += Connection_Error;
            _connection.StateChanged += Connection_StateChanged;
            _connection.Start().Wait();
        }
        private static void Connection_StateChanged(StateChange obj)
        {
            LogUtil.InfoFormat($"State {obj.NewState.ToString()}");
        }
        private static void Connection_Error(Exception obj)
        {
            LogUtil.ErrorFormat(obj.StackTrace);
        }
        public static void Invoke(string method, params object[] args)
        {
            if (_hubProxy == null)
                return;
            _hubProxy.Invoke(method, args).Wait();
        }
        public static async Task<T> InvokeAsync<T>(string method, params object[] args)
        {
            if (_hubProxy == null)
                return default(T);
            return await _hubProxy.Invoke<T>(method, args);
        }
        private static IDictionary<string, string> GetHubToken(MessageToken messageToken)
        {
            var queryStrings = new Dictionary<string, string>();
            queryStrings.Add("token", IdentityConstant._messageToken);
            queryStrings.Add("uuid", messageToken.Uuid);
            queryStrings.Add("user", messageToken.User);
            return queryStrings;
        }
    }
}
