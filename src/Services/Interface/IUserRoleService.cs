using System.Collections.Generic;

namespace Services.Interface
{
    public interface IUserRoleService
    {
        /// <summary>
        /// 用户拥有的角色
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        List<Entity.sys_userinrole> UserRole(int userid);

        /// <summary>
        /// 设置用户角色
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="roleid"></param>
        /// <returns></returns>
        bool SetUserRole(int userid, int[] roleid);
    }
}
