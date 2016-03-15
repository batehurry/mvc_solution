using Entity;
using System;
using System.Collections.Generic;

namespace Services.Interface
{
    public interface IUserService
    {
        bool Create(sys_user model);
        bool Update(sys_user model);
        bool Delete(int[] userid);
        /// <summary>
        /// 用户分页
        /// </summary>
        /// <param name="predicate">搜索条件</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="total">总记录数</param>
        /// <returns></returns>
        List<sys_user> Users(System.Linq.Expressions.Expression<Func<sys_user, bool>> predicate, int pageSize, int pageIndex, out int total);
        /// <summary>
        /// 获取用户详细信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        Entity.sys_user Get(int userid);
        /// <summary>
        /// 获取用户详细信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        Entity.sys_user Get(string username);
        /// <summary>
        /// 判断用户是否可以登录
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        SignInStatus CanLogin(string username, string password);
    }
}
