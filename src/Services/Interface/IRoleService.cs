using Entity;
using System;
using System.Collections.Generic;

namespace Services.Interface
{
    public interface IRoleService
    {
        bool Create(sys_role model);
        bool Update(sys_role model);
        bool Delete(int[] roleid);
        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <returns></returns>
        List<sys_role> Roles();
        /// <summary>
        /// 角色分页
        /// </summary>
        /// <param name="predicate">搜索条件</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="total">总记录数</param>
        /// <returns></returns>
        List<sys_role> Roles(System.Linq.Expressions.Expression<Func<sys_role, bool>> predicate, int pageSize, int pageIndex, out int total);


    }
}
