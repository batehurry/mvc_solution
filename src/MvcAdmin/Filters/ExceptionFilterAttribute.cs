using System.Collections.Generic;
using System.Web.Mvc;

namespace MvcAdmin.Filters
{
    public class ExceptionFilterAttribute: HandleErrorAttribute
    {
        //public static IRedisClientsManager clientManager = new PooledRedisClientManager(new string[] { "127.0.0.1:6379" });
        //public static IRedisClient redisClent = clientManager.GetClient();
        public override void OnException(ExceptionContext filterContext)
        {
            CommonUtil.LogHelper.WriteException("系统异常",filterContext.Exception);
            //redisClent.EnqueueItemOnList("Error", filterContext.Exception.ToString());
            //filterContext.HttpContext.Response.Redirect("/Error.html");
            base.OnException(filterContext);//跳转到Error页面
        }
    }
}