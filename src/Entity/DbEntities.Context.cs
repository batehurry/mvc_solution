﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Dev_Admin : DbContext
    {
        public Dev_Admin()
            : base("name=Dev_Admin")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<sys_dict> sys_dict { get; set; }
        public DbSet<sys_dictclass> sys_dictclass { get; set; }
        public DbSet<sys_menu> sys_menu { get; set; }
        public DbSet<sys_menuinrole> sys_menuinrole { get; set; }
        public DbSet<sys_role> sys_role { get; set; }
        public DbSet<sys_user> sys_user { get; set; }
        public DbSet<sys_userinrole> sys_userinrole { get; set; }
    }
}