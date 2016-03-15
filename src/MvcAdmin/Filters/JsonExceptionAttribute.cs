using System.Collections.Generic;
using System.Web.Mvc;

namespace MvcAdmin.Filters
{
    public class JsonExceptionAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                //返回异常JSON
                filterContext.Result = new JsonResult
                {
                    Data = new MvcAdmin.Models.OperateResult
                    {
                        result = false,
                        info = filterContext.Exception.Message
                    }
                };
                filterContext.ExceptionHandled = true;
            }
        }
    }
}