using Infrastructure;
using MvcAdmin.Models;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcAdmin.Controllers
{
    public class OptDataController : Controller
    {
        //
        // GET: /OptData/

        public JsonResult Menu(string state)
        {
            var list = new List<TreeData>();
            var result = new TreeData
            {
                id = 0,
                text = "系统",
                state = "open",
                children = MenuChildren(0, !string.IsNullOrEmpty(state))
            };
            list.Add(result);
            return Json(list);
        }

        private List<TreeData> MenuChildren(int parentId, bool filter)
        {
            var service = Ioc.GetService<IMenuService>();
            var list = service.ChildMenu(parentId);
            if (filter)
            {
                list = list.Where(o => o.Status == 1).ToList();
            }
            var result = (from item in list
                          select new TreeData
                          {
                              id = item.ID,
                              text = item.MenuName,
                              state = "open",
                              children = MenuChildren(item.ID, filter)
                          }).ToList();
            return result;
        }

        public JsonResult DictClass()
        {
            var list = new List<TreeData>();
            var result = new TreeData
            {
                id = 0,
                text = "系统",
                state = "open",
                children = DictClassChildren(0)
            };
            list.Add(result);
            return Json(list);
        }

        private List<TreeData> DictClassChildren(int parentId)
        {
            var service = Ioc.GetService<IDictClassService>();
            var list = service.DictClasses(parentId);
            var result = (from item in list
                          select new TreeData
                          {
                              id = item.ID,
                              text = item.ClassName,
                              state = "open",
                              children = DictClassChildren(item.ID)
                          }).ToList();
            return result;
        }

        public JsonResult RoleTree()
        {
            var list = new List<TreeData>();
            var auths = new TreeData
            {
                id = -1,
                text = "菜单角色",
                state = "open",
                children = RoleChildren()
            };
            list.Add(auths);
            return Json(list);
        }

        public List<TreeData> RoleChildren()
        {
            var service = Ioc.GetService<IRoleService>();
            var list = service.Roles();
            var result = (from item in list
                          select new TreeData
                          {
                              id = item.ID,
                              text = item.RoleName,
                              type = 1,
                              state = "open"
                          }).ToList();
            return result;
        }
    }
}
