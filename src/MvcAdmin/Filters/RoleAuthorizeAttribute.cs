using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcAdmin.Filters
{
    public class RoleAuthorizeAttribute : AuthorizeAttribute
    {
        public new string[] Roles { get; set; }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("HttpContext");
            }
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }
            //Action未设置权限时默认允许访问
            if (Roles == null)
            {
                return true;
            }
            if (Roles.Length == 0)
            {
                return true;
            }
            if (Roles.Any(httpContext.User.IsInRole))
            {
                return true;
            }
            var cookie = httpContext.Request.Cookies[System.Web.Security.FormsAuthentication.FormsCookieName];
            var ticket = System.Web.Security.FormsAuthentication.Decrypt(cookie.Value);
            string role = ticket.UserData;

            CommonUtil.LogHelper.WriteInfo(string.Format("当前操作需要角色:{0};当前用户（{1}）拥有角色:{2}",
                string.Join(",",Roles),
                httpContext.User.Identity.Name,
                role));
            return false;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = filterContext.ActionDescriptor.ActionName;
            var service = Infrastructure.Ioc.GetService<IMenuRoleService>();
            var roles = service.RoleByUrl(string.Format("/{0}/{1}", controllerName, actionName));
            if (roles.Count > 0)
            {
                this.Roles = roles.Select(o => o.ToString()).ToArray();
            }
            else
            {
                this.Roles = null;
            }
            this.Roles = null;
            base.OnAuthorization(filterContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.RequestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
            else
            {
                var result = new JsonResult();
                result.Data = "您没有操作权限！";
                //日志
                //CommonUtil.LogHelper.WriteInfo(string.Format("用户：{0}-请求：{1}-没有操作权限",
                //    filterContext.RequestContext.HttpContext.User.Identity.Name,
                //    filterContext.RequestContext.HttpContext.Request.Url.ToString()));
                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                filterContext.Result = result;
            }
        }

    }
}