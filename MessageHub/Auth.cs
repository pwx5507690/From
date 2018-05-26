using GS.Cache.Identity;
using GS.Common.Util;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Owin;
using Microsoft.Owin;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace MessageHub
{
    public class User
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "uuid")]
        public string Uuid { get; set; }
    }
    public class UserIdProvider : IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
            if (request.Cookies.Keys.Contains(IdentityConstant._storageName))
                return request.Cookies[IdentityConstant._storageName].Value;
            else if (request.QueryString["token"] == IdentityConstant._messageToken && !string.IsNullOrEmpty(request.QueryString["uuid"]))
                return request.QueryString["uuid"];
            throw new Exception();
        }
    }
    public class CustomHubCallerContext : HubCallerContext
    {
        public CustomHubCallerContext(IRequest request, string connectionId) : base(request, connectionId)
        {
        }

        public IdentityModel<User> UserIdentity { get; set; }
    }
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public override bool AuthorizeHubConnection(HubDescriptor hubDescriptor, IRequest request)
        {
            if (request.Cookies.Keys.Contains(IdentityConstant._storageName))
                return true;
            else if (request.QueryString["token"] == IdentityConstant._messageToken && !string.IsNullOrEmpty(request.QueryString["uuid"]))
                return true;
            return false;
        }
        public override bool AuthorizeHubMethodInvocation(IHubIncomingInvokerContext hubIncomingInvokerContext, bool appliesToMethod)
        {
            var request = hubIncomingInvokerContext.Hub.Context.Request;
            IdentityModel<User> user = null;
            var uuid = string.Empty;

            if (request.Cookies.Keys.Contains(IdentityConstant._storageName)) {
               uuid = request.Cookies[IdentityConstant._storageName].Value;

                if (string.IsNullOrEmpty(uuid))
                    return false;

               user = BaseIdentity.GetIdentityNoWebClient<User>(uuid);
                if (user == null)
                    return false;
             
            }
            else if (request.QueryString["token"] == IdentityConstant._messageToken && !string.IsNullOrEmpty(request.QueryString["uuid"]))
            {
                user = new IdentityModel<User>();
                user.Model = request.QueryString["user"].DeserializeObject<User>();
                uuid = request.QueryString["uuid"];
            }

            user.Model.Uuid = uuid;

            var connectionId = hubIncomingInvokerContext.Hub.Context.ConnectionId;
            var customHubCallerContext = new CustomHubCallerContext(new ServerRequest(request.Environment),
                connectionId);

            customHubCallerContext.UserIdentity = user;
            hubIncomingInvokerContext.Hub.Context = customHubCallerContext;
            return true;
            
            
        }
    }
    class AuthMiddleware : OwinMiddleware
    {
        public AuthMiddleware(OwinMiddleware next) : base(next)
        {

        }

        public override Task Invoke(IOwinContext context)
        {
            var referer = context.Request.Headers["Referer"];
            if (!string.IsNullOrEmpty(referer))
            {
                var refererHost = referer.Replace("http://", "").Split('/')[0];
                if (refererHost != "localhost:58624")
                    context.Response.StatusCode = 401;
            }
            return Next.Invoke(context);
        }
    }
}
