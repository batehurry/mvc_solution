using Entity;
using System.Collections.Generic;

namespace Services.Interface
{
    public interface IMenuService
    {
        bool Create(sys_menu model);
        bool Update(sys_menu model);
        bool Delete(int menuid);
        /// <summary>
        /// 子菜单
        /// </summary>
        /// <param name="parentid"></param>
        /// <returns></returns>
        List<sys_menu> ChildMenu(int parentid);
        /// <summary>
        /// 获取所有菜单
        /// </summary>
        /// <returns></returns>
        List<sys_menu> Menus();
        /// <summary>
        /// 用户拥有的菜单权限
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        List<sys_menu> Menus(int userid);
    }
}
