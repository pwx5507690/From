using GS.Common.Util;
using System.Web.Http.Filters;

namespace GS.Api.Attributes
{
    public class CustomErrorAttribute: ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            LogUtil.ErrorFormat(actionExecutedContext.Exception.StackTrace);
            base.OnException(actionExecutedContext);
        }
    }
}