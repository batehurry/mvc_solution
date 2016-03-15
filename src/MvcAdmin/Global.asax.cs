using CommonUtil;
using MvcAdmin.Filters;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MvcAdmin
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Infrastructure.Ioc.RegisterInheritedTypes(typeof(Services.BaseService).Assembly, typeof(Services.BaseService));
            //log4net.Config.XmlConfigurator.Configure();//获取Log4Net配置信息(配置信息定义在Web.config文件中)

            //通过线程池开启一个线程，从队列中获取数据
            /*ThreadPool.QueueUserWorkItem(o =>
            {
                while (true)
                {
                    try
                    {
                        if (ExceptionFilterAttribute.redisClent.GetListCount("Error") > 0)
                        {
                            //var name = MyExceptionAttribute.redisClent.Get<string>("name");
                            var errorMsg = ExceptionFilterAttribute.redisClent.DequeueItemFromList("Error");
                            //if (!string.IsNullOrEmpty(name)){
                            //    ILog logger = LogManager.GetLogger("RunInfo");
                            //    logger.Info(name);//将异常信息写到Log4Net中. 
                            //    MyExceptionAttribute.redisClent.Remove("name");
                            //}
                            if (!string.IsNullOrEmpty(errorMsg))
                            {
                                //ILog logger = LogManager.GetLogger("RunError");
                                //logger.Error(errorMsg);//将异常信息写到Log4Net中. 
                                LogHelper.WriteError(errorMsg);

                            }
                            else
                            {
                                Thread.Sleep(30);//避免CPU空转。
                            }
                        }
                        else
                        {
                            Thread.Sleep(30);
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionFilterAttribute.redisClent.EnqueueItemOnList("Error", ex.ToString());
                    }
                }
            });*/
        }
    }
}