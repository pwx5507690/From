using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using GS.Cache.Identity;
using GS.SQLModel;
using GS.Common.Util;
using GS.Api.Constants;

namespace GS.Api.Attributes
{
    public class AuthenticationAttribute : AuthorizationFilterAttribute
    {
        private void CreateAuthorizationResponse(HttpActionContext actionContext, string message)
        {
            var response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, message);
            response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue(Config._authorizationHeadKey));
            actionContext.Response = response;
        }
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (!Convert.ToBoolean(Config.GetValue("IsAuthentication")))
            {
                base.OnAuthorization(actionContext);
                return;
            }
            if (!actionContext.Request.Headers.Contains(Config._authorizationHeadKey))
            {
                CreateAuthorizationResponse(actionContext, "Unauthorized Access Attempt");
            }
            else
            {
                var token = actionContext.Request.Headers.Authorization.ToString();
                if (!token.StartsWith(Config._authorizationTarget))
                {
                    CreateAuthorizationResponse(actionContext, "Bearer token expected");
                }
                else
                {
                    token = token.Replace(Config._authorizationTarget + " ", "");
                    var user = BaseIdentity.GetIdentity<User>(token);
                    if (user.IsNotNull())
                        base.OnAuthorization(actionContext);
                    else
                        CreateAuthorizationResponse(actionContext, "User does not exist");
                }
            }
        }
    }
}