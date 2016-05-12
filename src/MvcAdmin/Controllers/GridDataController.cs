using Infrastructure;
using MvcAdmin.Filters;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcAdmin.Controllers
{
    public class GridDataController : Controller
    {
        [ActionError]
        public JsonResult Menu()
        {
            var service = Ioc.GetService<IMenuService>();
            var topmenu = service.Menus();
            var menus = (from m in topmenu
                         select new
                         {
                             ID = m.ID,
                             MenuName = m.MenuName,
                             OrderNo = m.OrderNo,
                             MenuUrl = m.MenuUrl,
                             Menu_ParentId = m.ParentId,
                             Status = m.Status,
                             _parentId = Get_parentId(m.ParentId)
                         }).ToList();
            var result = new
            {
                total = menus.Count,
                rows = menus
            };
            return Json(result);
        }
        [ActionError]
        public JsonResult Role(int page,int rows)
        {
            var service = Ioc.GetService<IRoleService>();
            int count =0;
            var roles = service.Roles(null, rows, page, out count);
            var data = (from r in roles
                        select new Entity.sys_role
                        {
                            ID = r.ID,
                            RoleName = r.RoleName,
                            Remark = r.Remark
                        }).ToList();
            var result = new
            {
                total = data.Count,
                rows = data
            };
            return Json(data);
        }
        [ActionError]
        public JsonResult Users(int page, int rows)
        {
            var service = Ioc.GetService<IUserService>();
            int count = 0;
            var list = service.Users(null, rows, page, out count);
            var data = (from o in list
                        select new Entity.sys_user
                        {
                            ID = o.ID,
                            UserName = o.UserName,
                            CreateTime = o.CreateTime,
                            Status = o.Status,
                            LastLogin = o.LastLogin
                        }).ToList();
            var result = new
            {
                total = count,
                rows = data
            };
            return Json(data);
        }
        [ActionError]
        public JsonResult Dict(int page, int rows)
        {
            var type = Convert.ToInt32(Request.Params["class"]);
            var service = Ioc.GetService<IDictService>();
            int count = 0;
            if (page <= 0)
            {
                page = 1;
            }
            var dictlist = new List<Entity.sys_dict>();
            if (type > 0)
            {
                dictlist = service.Dicts(o => o.ClassId == type, rows, page, out count);
            }
            else
            {
                dictlist = service.Dicts(null, rows, page, out count);
            }
            var data = (from o in dictlist
                        select new Entity.sys_dict
                        {
                            ID = o.ID,
                            ClassId = o.ClassId,
                            DictName = o.DictName,
                            DictNo = o.DictNo
                        }).ToList();
            var result = new
            {
                total = count,
                rows = data
            };
            return Json(data);
        }
        [ActionError]
        public JsonResult DictClass(int page, int rows)
        {
            var service = Ioc.GetService<IDictClassService>();
            int count = 0;
            if (page <= 0)
            {
                page = 1;
            }
            var dictlist = service.DictClasses(null, rows, page, out count);
            var data = (from o in dictlist
                        select new Entity.sys_dictclass
                        {
                            ID = o.ID,
                            ClassName = o.ClassName,
                            OrderNo = o.OrderNo,
                            ParentId = o.ParentId
                        }).ToList();
            var result = new
            {
                total = count,
                rows = data
            };
            return Json(data);
        }

        /// <summary>
        /// 获得TreeGrid的_parentId
        /// </summary>
        /// <param name="parentid"></param>
        /// <returns></returns>
        private int? Get_parentId(int parentid)
        {
            if (parentid > 0)
                return parentid;
            return null;
        }

    }
}
