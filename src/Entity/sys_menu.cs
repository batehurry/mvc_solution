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
    
    public partial class sys_menu
    {
        public sys_menu()
        {
            this.sys_menuinrole = new HashSet<sys_menuinrole>();
        }
    
        public int ID { get; set; }
        public string MenuName { get; set; }
        public int ParentId { get; set; }
        public string MenuUrl { get; set; }
        public Nullable<int> OrderNo { get; set; }
        public Nullable<int> Status { get; set; }
    
        public virtual ICollection<sys_menuinrole> sys_menuinrole { get; set; }
    }
}
