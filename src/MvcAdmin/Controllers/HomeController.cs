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
    public class HomeController : Controller
    {
        [Authorize]
        [OutputCache(Duration = 5)]
        public ActionResult Index()
        {
            ViewData["CurUser"] = User.Identity.Name;
            return View();
        }

        public JsonResult Menu()
        {
            var service = Ioc.GetService<IMenuService>();
            var menus = service.Menus(1);
            var left = (from m in menus
                        where m.ParentId == 0
                        select new LeftMenu
                        {
                            Title_Name = m.MenuName,
                            children = GetChildMenu(menus, m.ID)
                        }).ToList();
            return Json(left);
        }

        private List<SysMenu> GetChildMenu(List<Entity.sys_menu> menulist, int parentId)
        {
            var list = (from m in menulist
                        where m.ParentId == parentId
                        select new SysMenu
                        {
                            Menu_Id = m.ID,
                            Menu_Name = m.MenuName,
                            Menu_Url = m.MenuUrl,
                            Menu_ParentId = m.ParentId,
                            children = GetChildMenu(menulist, m.ID)
                        }).ToList();
            return list.Count > 0 ? list : null;
        }

        //[Filters.PassValidate]
        public ActionResult Test(RegisterModel model)
        {
            return Content(DateTime.Now.ToString());
        }
    }
}
