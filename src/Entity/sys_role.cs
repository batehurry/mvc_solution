//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class sys_role
    {
        public sys_role()
        {
            this.sys_menuinrole = new HashSet<sys_menuinrole>();
            this.sys_userinrole = new HashSet<sys_userinrole>();
        }
    
        public int ID { get; set; }
        public string RoleName { get; set; }
        public string Remark { get; set; }
    
        public virtual ICollection<sys_menuinrole> sys_menuinrole { get; set; }
        public virtual ICollection<sys_userinrole> sys_userinrole { get; set; }
    }
}
