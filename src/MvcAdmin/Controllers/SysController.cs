using Infrastructure;
using MvcAdmin.Filters;
using MvcAdmin.Models;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcAdmin.Controllers
{
    [Authorize]
    public class SysController : Controller
    {
        //
        // GET: /Sys/

        public ActionResult Users()
        {
            return View();
        }

        public ActionResult Menu()
        {
            return View();
        }

        public ActionResult Role()
        {
            return View();
        }

        public ActionResult Dict()
        {
            return View();
        }

        public ActionResult DictClass()
        {
            return View();
        }

        #region 查询数据

        //用户拥有的角色
        [ActionError]
        public JsonResult UserRole(int user)
        {
            var result = new OperateResult();
            var service = Ioc.GetService<IUserRoleService>();
            var data = service.UserRole(user);
            var list = (from o in data
                        select new Entity.sys_userinrole
                        {
                            ID = o.ID,
                            RoleId = o.RoleId,
                            UserId = o.UserId
                        }).ToList();

            result.result = true;
            result.data = list;
            return Json(result);
        }

        //角色拥有的菜单权限
        [ActionError]
        public JsonResult RolesMenu(int role)
        {
            var result = new OperateResult();
            var service = Ioc.GetService<IMenuRoleService>();
            var data = service.MenuRoleByRole(role);
            var list = (from o in data
                        select new Entity.sys_menuinrole
                        {
                            ID = o.ID,
                            MenuId = o.MenuId,
                            RoleId = o.RoleId
                        }).ToList();

            result.result = true;
            result.data = list;
            return Json(result);
        }

        #endregion

        #region 菜单操作
        [ActionError]
        public JsonResult AddMenu(Entity.sys_menu model)
        {
            var data = new OperateResult();
            var service = Ioc.GetService<IMenuService>();
            data.result = service.Create(model);
            return Json(data);
        }
        [ActionError]
        public JsonResult UpdateMenu(Entity.sys_menu model)
        {
            var data = new OperateResult();

            var service = Ioc.GetService<IMenuService>();
            data.result = service.Update(model);
            return Json(data);
        }
        [ActionError]
        public JsonResult DelMenu(int idList)
        {
            var data = new OperateResult();

            var service = Ioc.GetService<IMenuService>();
            data.result = service.Delete(idList);

            return Json(data);
        }

        #endregion

        #region 角色操作
        [ActionError]
        public JsonResult AddRole(Entity.sys_role model)
        {
            var data = new OperateResult();
            var service = Ioc.GetService<IRoleService>();
            data.result = service.Create(model);
            return Json(data);
        }
        [ActionError]
        public JsonResult UpdateRole(Entity.sys_role model)
        {
            var data = new OperateResult();
            var service = Ioc.GetService<IRoleService>();
            data.result = service.Update(model);
            return Json(data);
        }
        [ActionError]
        public JsonResult DelRole(List<int> idList)
        {
            var data = new OperateResult();
            if (idList.Count <= 0)
            {
                data.result = false;
                return Json(data);
            }
            
            var service = Ioc.GetService<IRoleService>();
            data.result = service.Delete(idList.ToArray());
            return Json(data);
        }

        public JsonResult SetUserRole(int user, List<int> role)
        {
            var data = new OperateResult();

            var service = Ioc.GetService<IUserRoleService>();
            data.result = service.SetUserRole(user, role.ToArray());
            return Json(data);
        }

        public JsonResult SetRoleMenu(int role, List<int> menu)
        {
            var data = new OperateResult();

            var service = Ioc.GetService<IMenuRoleService>();
            data.result = service.SetRoleMenu(role, menu.ToArray());
            return Json(data);
        }

        #endregion

        #region 用户操作

        [ActionError]
        public JsonResult AddUser(Entity.sys_user model)
        {
            var data = new OperateResult();
            var service = Ioc.GetService<IUserService>();
            model.CreateTime = DateTime.Now;
            data.result = service.Create(model);
            return Json(data);
        }
        [ActionError]
        public JsonResult UpdateUser(Entity.sys_user model)
        {
            var data = new OperateResult();
            var service = Ioc.GetService<IUserService>();
            data.result = service.Update(model);
            return Json(data);
        }
        [ActionError]
        public JsonResult DelUser(List<int> idList)
        {
            var data = new OperateResult();
            if (idList.Count <= 0)
            {
                data.result = false;
                return Json(data);
            }
            
            var service = Ioc.GetService<IUserService>();
            data.result = service.Delete(idList.ToArray());
            return Json(data);
        }

        #endregion

        #region 字典操作

        [ActionError]
        public JsonResult AddDict(Entity.sys_dict model)
        {
            var data = new OperateResult();
            var service = Ioc.GetService<IDictService>();
            data.result = service.Create(model);
            return Json(data);
        }
        [ActionError]
        public JsonResult UpdateDict(Entity.sys_dict model)
        {
            var data = new OperateResult();
            var service = Ioc.GetService<IDictService>();
            data.result = service.Update(model);
            return Json(data);
        }
        [ActionError]
        public JsonResult DelDict(List<int> idList)
        {
            var data = new OperateResult();
            if (idList.Count <= 0)
            {
                data.result = false;
                return Json(data);
            }

            var service = Ioc.GetService<IDictService>();
            data.result = service.Delete(idList.ToArray());
            return Json(data);
        }

        #endregion

        #region 字典类型操作

        [ActionError]
        public JsonResult AddDictClass(Entity.sys_dictclass model)
        {
            var data = new OperateResult();
            var service = Ioc.GetService<IDictClassService>();
            data.result = service.CreateClass(model);
            return Json(data);
        }
        [ActionError]
        public JsonResult UpdateDictClass(Entity.sys_dictclass model)
        {
            var data = new OperateResult();
            var service = Ioc.GetService<IDictClassService>();
            data.result = service.UpdateClass(model);
            return Json(data);
        }
        [ActionError]
        public JsonResult DelDictClass(List<int> idList)
        {
            var data = new OperateResult();
            if (idList.Count <= 0)
            {
                data.result = false;
                return Json(data);
            }

            var service = Ioc.GetService<IDictClassService>();
            data.result = service.DeleteClass(idList.ToArray());
            return Json(data);
        }

        #endregion
    }
}
