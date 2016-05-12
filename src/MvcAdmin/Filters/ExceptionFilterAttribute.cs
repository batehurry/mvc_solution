using System.Collections.Generic;
using System.Web.Mvc;

namespace MvcAdmin.Filters
{
    public class ExceptionFilterAttribute: HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            CommonUtil.LogUtil.WriteException("系统异常",filterContext.Exception);
            base.OnException(filterContext);//跳转到Error页面
        }
    }
}