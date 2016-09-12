using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using Entity;
using EntityFramework.Extensions;

namespace Services.Implement
{
    public class RoleService : BaseService, IRoleService, IUserRoleService, IMenuRoleService
    {
        public bool Create(sys_role model)
        {
            using (var db = base.NewDB())
            {
                db.sys_role.Add(model);
                return db.SaveChanges() > 0;
            }
        }

        public bool Delete(int[] roleid)
        {
            using (var db = base.NewDB())
            {
                //var list = db.sys_role.Where(o => roleid.Contains(o.ID)).ToList();
                //foreach (var role in list)
                //{
                //    db.Entry(role).State = System.Data.EntityState.Deleted;
                //}
                db.sys_role.Where(o => roleid.Contains(o.ID)).Delete();
                return db.SaveChanges() > 0;
            }
        }

        public bool Update(sys_role model)
        {
            using (var db = base.NewDB())
            {
                var role = db.sys_role.First(o => o.ID == model.ID);
                if (role == null)
                {
                    return false;
                }
                role.RoleName = model.RoleName;
                role.Remark = model.Remark;

                return db.SaveChanges() > 0;
            }
        }

        public List<Entity.sys_role> Roles()
        {
            using (var db = NewDB())
            {
                return db.sys_role.ToList();
            }
        }

        public List<sys_role> Roles(System.Linq.Expressions.Expression<Func<sys_role, bool>> predicate, int pageSize, int pageIndex, out int total)
        {
            using (var db = base.NewDB())
            {
                var source = db.sys_role.OrderBy(o => o.ID);
                if (predicate == null)
                {
                    return base.GetPage<Entity.sys_role>(source, pageSize, pageIndex, out total);
                }
                return base.GetPage<Entity.sys_role>(source, predicate, pageSize, pageIndex, out total);
            }
        }

        public List<sys_userinrole> UserRole(int userid)
        {
            using (var db = base.NewDB())
            {
                var list = db.sys_userinrole.Where(o => o.sys_user.ID == userid).ToList();
                return list;
            }
        }

        public bool SetUserRole(int userid, int[] roleid)
        {
            using (var db = base.NewDB())
            {
                db.sys_userinrole.Where(o => o.UserId == userid).Delete();
                foreach (var id in roleid)
                {
                    var record = new sys_userinrole();
                    record.UserId = userid;
                    record.RoleId = id;
                    db.sys_userinrole.Add(record);
                }
                return db.SaveChanges() > 0;
            }
        }

        public List<sys_menuinrole> MenuRoleByMenu(int menuid)
        {
            using (var db = base.NewDB())
            {
                var list = db.sys_menuinrole.Where(o => o.sys_menu.ID == menuid).ToList();
                return list;
            }
        }

        public List<sys_menuinrole> MenuRoleByRole(int roleid)
        {
            using (var db = base.NewDB())
            {
                var list = db.sys_menuinrole.Where(o => o.sys_role.ID == roleid).ToList();
                
                return list;
            }
        }

        public bool SetRoleMenu(int roleid, int[] menuid)
        {
            using (var db = base.NewDB())
            {
                db.sys_menuinrole.Where(o => o.RoleId == roleid).Delete();
                foreach (var id in menuid)
                {
                    var record = new sys_menuinrole();
                    record.RoleId = roleid;
                    record.MenuId = id;
                    db.sys_menuinrole.Add(record);
                }
                return db.SaveChanges() > 0;
            }
        }

        public List<int> RoleByUrl(string url)
        {
            using (var db = base.NewDB())
            {
                var list = db.sys_menuinrole.Where(o => o.sys_menu.MenuUrl == url).Select(o => o.RoleId).ToList();

                return list;
            }
        }
    }
}
