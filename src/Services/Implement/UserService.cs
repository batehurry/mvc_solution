using CommonUtil;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Implement
{
    public class UserService : BaseService, IUserService
    {
        public bool Create(Entity.sys_user model)
        {
            using (var db = base.NewDB())
            {
                db.sys_user.Add(model);
                return db.SaveChanges() > 0;
            }
        }

        public bool Update(Entity.sys_user model)
        {
            using (var db = base.NewDB())
            {
                var user = db.sys_user.First(o => o.ID == model.ID);
                if (user == null)
                {
                    return false;
                }
                user.Status = model.Status;
                return db.SaveChanges() > 0;
            }
        }

        public bool Delete(int[] userid)
        {
            using (var db = base.NewDB())
            {
                var list = db.sys_user.Where(o => userid.Contains(o.ID)).ToList();
                foreach (var role in list)
                {
                    db.Entry(role).State = System.Data.EntityState.Deleted;
                }
                return db.SaveChanges() > 0;
            }
        }

        public Entity.sys_user Get(int userid)
        {
            using (var db = base.NewDB())
            {
                var user = db.sys_user.FirstOrDefault(o => o.ID == userid);
                return user;
            }
        }

        public Entity.sys_user Get(string username)
        {
            using (var db = base.NewDB())
            {
                var user = db.sys_user.FirstOrDefault(o => o.UserName == username);
                return user;
            }
        }

        public SignInStatus CanLogin(string username, string password)
        {
            using (var db = base.NewDB())
            {
                var loginUser = db.sys_user.FirstOrDefault(o => o.UserName == username && o.UserPwd == password);
                if (loginUser == null)
                {
                    return SignInStatus.Failure;
                }
                if (loginUser.Status == 0)
                {
                    return SignInStatus.LockedOut;
                }
                return SignInStatus.Success;
            }
        }


        public List<Entity.sys_user> Users(System.Linq.Expressions.Expression<Func<Entity.sys_user, bool>> predicate, int pageSize, int pageIndex, out int total)
        {
            using (var db = base.NewDB())
            {
                var source = db.sys_user.OrderBy(o => o.ID);
                if (predicate == null)
                {
                    return base.GetPage<Entity.sys_user>(source, pageSize, pageIndex, out total);
                }
                return base.GetPage<Entity.sys_user>(source, predicate, pageSize, pageIndex, out total);
            }
        }
    }
}
