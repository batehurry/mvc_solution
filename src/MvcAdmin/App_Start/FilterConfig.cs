using MvcAdmin.Filters;
using System.Web;
using System.Web.Mvc;

namespace MvcAdmin
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ExceptionFilterAttribute());
            filters.Add(new ModelValidateAttribute());//全局注册验证参数
            //filters.Add(new ActionLogAttribute());
        }
    }
}