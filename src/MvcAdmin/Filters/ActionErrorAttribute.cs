using System.Collections.Generic;
using System.Web.Mvc;

namespace MvcAdmin.Filters
{
    public class ActionErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                //返回异常JSON
                filterContext.Result = new Models.ApiResult
                {
                    Status=Models.ApiResultType.FAIL,
                    Message=filterContext.Exception.Message
                };
                filterContext.ExceptionHandled = true;
            }
        }
    }
}