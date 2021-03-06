﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class SaleManagerDBEntities : DbContext
    {
        public SaleManagerDBEntities()
            : base("name=SaleManagerDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<InventoryStatus> InventoryStatus { get; set; }
        public virtual DbSet<LoginLogs> LoginLogs { get; set; }
        public virtual DbSet<ProductCategory> ProductCategory { get; set; }
        public virtual DbSet<ProductInventory> ProductInventory { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<SalesList> SalesList { get; set; }
        public virtual DbSet<SalesListDetail> SalesListDetail { get; set; }
        public virtual DbSet<SalesPerson> SalesPerson { get; set; }
        public virtual DbSet<SMMembers> SMMembers { get; set; }
        public virtual DbSet<SysAdmins> SysAdmins { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
    
        public virtual ObjectResult<usp_AdminLogin_Result> usp_AdminLogin(Nullable<int> loginId, string loginPwd)
        {
            var loginIdParameter = loginId.HasValue ?
                new ObjectParameter("LoginId", loginId) :
                new ObjectParameter("LoginId", typeof(int));
    
            var loginPwdParameter = loginPwd != null ?
                new ObjectParameter("LoginPwd", loginPwd) :
                new ObjectParameter("LoginPwd", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_AdminLogin_Result>("usp_AdminLogin", loginIdParameter, loginPwdParameter);
        }
    
        public virtual int usp_AddProduct(string supplierName, string productId, string productName, Nullable<decimal> inUnitPrice, Nullable<decimal> unitPrice, Nullable<int> categoryId, Nullable<int> count, Nullable<System.DateTime> addTime)
        {
            var supplierNameParameter = supplierName != null ?
                new ObjectParameter("SupplierName", supplierName) :
                new ObjectParameter("SupplierName", typeof(string));
    
            var productIdParameter = productId != null ?
                new ObjectParameter("ProductId", productId) :
                new ObjectParameter("ProductId", typeof(string));
    
            var productNameParameter = productName != null ?
                new ObjectParameter("ProductName", productName) :
                new ObjectParameter("ProductName", typeof(string));
    
            var inUnitPriceParameter = inUnitPrice.HasValue ?
                new ObjectParameter("InUnitPrice", inUnitPrice) :
                new ObjectParameter("InUnitPrice", typeof(decimal));
    
            var unitPriceParameter = unitPrice.HasValue ?
                new ObjectParameter("UnitPrice", unitPrice) :
                new ObjectParameter("UnitPrice", typeof(decimal));
    
            var categoryIdParameter = categoryId.HasValue ?
                new ObjectParameter("CategoryId", categoryId) :
                new ObjectParameter("CategoryId", typeof(int));
    
            var countParameter = count.HasValue ?
                new ObjectParameter("Count", count) :
                new ObjectParameter("Count", typeof(int));
    
            var addTimeParameter = addTime.HasValue ?
                new ObjectParameter("AddTime", addTime) :
                new ObjectParameter("AddTime", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_AddProduct", supplierNameParameter, productIdParameter, productNameParameter, inUnitPriceParameter, unitPriceParameter, categoryIdParameter, countParameter, addTimeParameter);
        }
    }
}
