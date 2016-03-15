using System.Collections.Generic;

namespace Services.Interface
{
    public interface IMenuRoleService
    {
        /// <summary>
        /// 拥有菜单权限的角色
        /// </summary>
        /// <param name="menuid"></param>
        /// <returns></returns>
        List<Entity.sys_menuinrole> MenuRoleByMenu(int menuid);

        List<int> RoleByUrl(string url);

        /// <summary>
        /// 角色拥有的菜单权限
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        List<Entity.sys_menuinrole> MenuRoleByRole(int roleid);

        /// <summary>
        /// 设置角色拥有的菜单权限
        /// </summary>
        /// <param name="roleid"></param>
        /// <param name="menuid"></param>
        /// <returns></returns>
        bool SetRoleMenu(int roleid, int[] menuid);
    }
}
