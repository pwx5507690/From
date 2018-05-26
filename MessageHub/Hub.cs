using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hubs = Microsoft.AspNet.SignalR.Hub;
namespace MessageHub
{
    [CustomAuthorize]
    [HubName("Hub")]
    public class Hub : Hubs
    {
        private CustomHubCallerContext CustomContext
        {
            get
            {
                return (CustomHubCallerContext)Context;
            }
        }
        public void Letter(int userId,int messageId) {
            Clients.All.Logout(userId);
        }
        public void Logout(string uuid) {
             Clients.All.Logout(uuid);
        }
        public void SendMessage(string message)
        {
            var id = Context;
            Clients.Caller.GetMessage(CustomContext.UserIdentity.Model.Name);
        }
        public void SendPrivateMessage(string message)
        {
            Clients.Others.Caller.absentSubscriber();
        }
    }
}

