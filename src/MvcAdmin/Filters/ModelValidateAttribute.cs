using System.Linq;
using System.Web.Mvc;

namespace MvcAdmin.Filters
{
    /// <summary>
    /// 接口参数验证
    /// </summary>
    public class ModelValidateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //跳过验证
            var passby = filterContext.ActionDescriptor.GetCustomAttributes(typeof(PassValidateAttribute), false).Any() || 
                filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(PassValidateAttribute), false).Any();
            
            if (passby)
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            var modelState = filterContext.Controller.ViewData.ModelState;
            if (!modelState.IsValid)
            {
                var errorMessage = modelState.Values
                    .SelectMany(m => m.Errors)
                    .Select(m => m.ErrorMessage)
                    .First();

                //直接响应验证结果
                filterContext.Result = new Models.ApiResult
                {
                    Status = Models.ApiResultType.FAIL,
                    Message = errorMessage
                };
            }
        }

    }
}