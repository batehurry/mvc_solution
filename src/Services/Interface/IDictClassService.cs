using Entity;
using System;
using System.Collections.Generic;

namespace Services.Interface
{
    public interface IDictClassService
    {
        bool CreateClass(sys_dictclass model);
        bool UpdateClass(sys_dictclass model);
        bool DeleteClass(int[] classid);
        /// <summary>
        /// 获取所有字典类型
        /// </summary>
        /// <returns></returns>
        List<Entity.sys_dictclass> DictClasses();
        /// <summary>
        /// 字段类型
        /// </summary>
        /// <param name="parentId">父类型</param>
        /// <returns></returns>
        List<Entity.sys_dictclass> DictClasses(int parentId);
        /// <summary>
        /// 字典类型分页
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="total">总记录数</param>
        /// <returns></returns>
        List<sys_dictclass> DictClasses(System.Linq.Expressions.Expression<Func<sys_dictclass, bool>> predicate, int pageSize, int pageIndex, out int total);

    }
}
