﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace bigeyedev
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class sugareyeEntities : DbContext
    {
        public sugareyeEntities()
            : base("name=sugareyeEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<bigeyedev_free> bigeyedev_free { get; set; }
        public virtual DbSet<bigeyedev_logic> bigeyedev_logic { get; set; }
        public virtual DbSet<bigeyedev_logic_address> bigeyedev_logic_address { get; set; }
        public virtual DbSet<bigeyedev_member> bigeyedev_member { get; set; }
        public virtual DbSet<bigeyedev_member_address> bigeyedev_member_address { get; set; }
        public virtual DbSet<bigeyedev_order_fashion_item> bigeyedev_order_fashion_item { get; set; }
        public virtual DbSet<bigeyedev_shop> bigeyedev_shop { get; set; }
        public virtual DbSet<product> product { get; set; }
        public virtual DbSet<bigeyedev_order> bigeyedev_order { get; set; }
        public virtual DbSet<bigeyedev_brand> bigeyedev_brand { get; set; }

        internal void Attach(object updatedCandidat)
        {
            throw new NotImplementedException();
        }
    }
}
