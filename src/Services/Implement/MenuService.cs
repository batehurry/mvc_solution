using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Implement
{
    public class MenuService : BaseService, IMenuService
    {
        public bool Create(Entity.sys_menu model)
        {
            using (var db = base.NewDB())
            {
                db.sys_menu.Add(model);
                return db.SaveChanges() > 0;
            }
        }

        public bool Update(Entity.sys_menu model)
        {
            using (var db = base.NewDB())
            {
                var menu = db.sys_menu.FirstOrDefault(o => o.ID == model.ID);
                if (menu == null) {
                    return false;
                }
                menu.MenuName = model.MenuName;
                menu.MenuUrl = model.MenuUrl;
                menu.OrderNo = model.OrderNo;
                menu.ParentId = model.ParentId;
                menu.Status = model.Status;

                return db.SaveChanges() > 0;
            }
        }

        public bool Delete(int menuid)
        {
            using (var db = base.NewDB())
            {
                var menu = db.sys_menu.First(o => o.ID == menuid);
                db.Entry(menu).State = System.Data.EntityState.Deleted;
                return db.SaveChanges() > 0;
            }
        }

        public List<Entity.sys_menu> Menus(int userid)
        {
            using (var db = base.NewDB())
            {
                return db.sys_menu.Where(o => o.Status == 1).ToList();
            }
        }

        public List<Entity.sys_menu> ChildMenu(int parentid)
        {
            using (var db = base.NewDB())
            {
                return db.sys_menu.Where(o => o.ParentId == parentid).ToList();
            }
        }

        public List<Entity.sys_menu> Menus()
        {
            using (var db = base.NewDB())
            {
                return db.sys_menu.ToList();
            }
        }

        
    }
}
