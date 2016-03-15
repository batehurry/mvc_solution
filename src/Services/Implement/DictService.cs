using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Services.Implement
{
    class DictService : BaseService, IDictService, IDictClassService
    {
        public bool Create(Entity.sys_dict model)
        {
            using (var db = base.NewDB())
            {
                db.sys_dict.Add(model);
                return db.SaveChanges() > 0;
            }
        }

        public bool Update(Entity.sys_dict model)
        {
            using (var db = base.NewDB())
            {
                var dict = db.sys_dict.First(o => o.ID == model.ID);
                if (dict == null)
                {
                    return false;
                }
                dict.DictName = model.DictName;
                dict.DictNo = model.DictNo;
                dict.ClassId = model.ClassId;
                return db.SaveChanges() > 0;
            }
        }

        public bool Delete(int[] dictid)
        {
            using (var db = base.NewDB())
            {
                var list = db.sys_dict.Where(o => dictid.Contains(o.ID)).ToList();
                foreach (var role in list)
                {
                    db.Entry(role).State = System.Data.EntityState.Deleted;
                }
                return db.SaveChanges() > 0;
            }
        }

        public List<Entity.sys_dict> Dicts()
        {
            using (var db = base.NewDB())
            {
                var list = db.sys_dict.ToList();

                return list;
            }
        }

        public List<Entity.sys_dict> DictByClass(int dictclass)
        {
            using (var db = base.NewDB())
            {
                var list = db.sys_dict.OrderBy(o => o.ID).Where(o => o.ClassId == dictclass).ToList();
                return list;
            }
        }


        public List<Entity.sys_dict> Dicts(Expression<Func<Entity.sys_dict, bool>> predicate, int pageSize, int pageIndex, out int total)
        {
            using (var db = base.NewDB())
            {
                var source = db.sys_dict.OrderBy(o => o.ID);
                if (predicate == null)
                {
                    return base.GetPage<Entity.sys_dict>(source, pageSize, pageIndex, out total);
                }
                return base.GetPage<Entity.sys_dict>(source, predicate, pageSize, pageIndex, out total);
            }
        }

        #region 字典类型操作

        public bool CreateClass(Entity.sys_dictclass model)
        {
            using (var db = base.NewDB())
            {
                db.sys_dictclass.Add(model);
                return db.SaveChanges() > 0;
            }
        }

        public bool UpdateClass(Entity.sys_dictclass model)
        {
            using (var db = base.NewDB())
            {
                var dictclass = db.sys_dictclass.First(o => o.ID == model.ID);
                if (dictclass == null)
                {
                    return false;
                }
                dictclass.ClassName = model.ClassName;
                dictclass.OrderNo = model.OrderNo;
                dictclass.ParentId = model.ParentId;
                return db.SaveChanges() > 0;
            }
        }

        public bool DeleteClass(int[] classid)
        {
            using (var db = base.NewDB())
            {
                var list = db.sys_dictclass.Where(o => classid.Contains(o.ID)).ToList();
                foreach (var role in list)
                {
                    db.Entry(role).State = System.Data.EntityState.Deleted;
                }
                return db.SaveChanges() > 0;
            }
        }

        public List<Entity.sys_dictclass> DictClasses()
        {
            using (var db = base.NewDB())
            {
                var list = db.sys_dictclass.ToList();

                return list;
            }
        }

        public List<Entity.sys_dictclass> DictClasses(int parentId)
        {
            using (var db = base.NewDB())
            {
                var list = db.sys_dictclass.Where(o => o.ParentId == parentId).ToList();

                return list;
            }
        }

        public List<Entity.sys_dictclass> DictClasses(Expression<Func<Entity.sys_dictclass, bool>> predicate, int pageSize, int pageIndex, out int total)
        {
            using (var db = base.NewDB())
            {
                var source = db.sys_dictclass.OrderBy(o => o.ID);
                if (predicate == null)
                {
                    return base.GetPage<Entity.sys_dictclass>(source, pageSize, pageIndex, out total);
                }
                return base.GetPage<Entity.sys_dictclass>(source, predicate, pageSize, pageIndex, out total);
            }
        }

        #endregion


        
    }
}
