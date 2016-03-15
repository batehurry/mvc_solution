using System.Collections.Generic;

namespace MvcAdmin.Models
{
    public class LeftMenu
    {
        public string Title_Name { get; set; }
        public List<SysMenu> children { get; set; }
    }

    public class SysMenu
    {
        public int Menu_Id { get; set; }
        public string Menu_Name { get; set; }
        public int Menu_ParentId { get; set; }
        public string Menu_Url { get; set; }
        public List<SysMenu> children { get; set; }
    }

    public class Menu : Entity.sys_menu
    {
        public List<Entity.sys_menu> children { get; set; }
    }
}