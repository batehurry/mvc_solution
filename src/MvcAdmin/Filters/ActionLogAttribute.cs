using System.Collections.Generic;
using System.Web.Mvc;

namespace MvcAdmin.Filters
{
    public class ActionLogAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 运行时间
        /// </summary>
        //private Stopwatch timer;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var dict = new Dictionary<string, object>();
            var param = filterContext.ActionParameters;
            dict = new Dictionary<string, object>();
            dict.Add("UserHostAddress", filterContext.HttpContext.Request.UserHostAddress);
            dict.Add("Url", filterContext.HttpContext.Request.Url.ToString());
            if (param.Count > 0)
            {
                if (param.ContainsKey("user_identity"))
                {
                    dict.Add("user_identity", param["user_identity"]);
                }
                dict.Add("Params", param);
            }
            CommonUtil.LogUtil.WriteInfo(filterContext.ActionDescriptor.ActionName, dict);
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var dict = new Dictionary<string, object>();
            dict.Add("Url", filterContext.HttpContext.Request.Url.ToString());
            dict.Add("Result", filterContext.Result);
            CommonUtil.LogUtil.WriteInfo("接口请求日志", dict);
            base.OnActionExecuted(filterContext);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
        }
    }
}