using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Services
{
    public abstract class BaseService
    {
        protected Entity.Dev_Admin NewDB()
        {
            return new Entity.Dev_Admin();
        }

        protected List<T> GetPage<T>(IQueryable<T> source, int pageSize, int pageIndex, out int total) where T : class
        {
            total = source.Count();
            //pageCount = total % pageSize == 0 ? (total / pageSize) : (total / pageSize + 1);
            return source.Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking().ToList();
        }

        protected List<T> GetPage<T>(IQueryable<T> source, Expression<Func<T, bool>> predicate, int pageSize, int pageIndex, out int total) where T : class
        {
            total = source.Count(predicate);
            return source.Where(predicate).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking().ToList();
        }
    }
}
