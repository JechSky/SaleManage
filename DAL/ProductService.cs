using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Models;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using System.Data.Entity;

namespace DAL
{
    /// <summary>
    /// 商品操作数据访问类
    /// </summary>
    public class ProductService 
    {
        ProductCategoryService proCateService = new ProductCategoryService();
        ProductInventoryService proInventoryService = new ProductInventoryService();
        
        public ListDataView GetListBySearch(Dictionary<string,string> dics, int pageIndex = 1) //string productId="", string productName="", int? categoryId=null,int pageIndex=1)
        {
            Expression<Func<Products, bool>> whereLambda = a => a != null;
            if (!string.IsNullOrEmpty(dics["productId"]))
            {
                string proId = dics["productId"];
                whereLambda = whereLambda.And(r => r.ProductId.Contains(proId));
            }
            if (!string.IsNullOrEmpty(dics["productName"]))
            {
                string name = dics["productName"];
                whereLambda = whereLambda.And(a => a.ProductName.Contains(name));
            }
            if (!string.IsNullOrEmpty(dics["categoryId"]))
            {
                int obj= int.Parse(dics["categoryId"]);
                whereLambda = whereLambda.And(a => a.CategoryId.Equals(obj));
            }

            List<Products> list = new List<Products>();
            ListDataView view = new ListDataView();
            int count = 0;

            using (SaleManagerDBEntities efdb = new SaleManagerDBEntities())
            {
                //efdb.Configuration.AutoDetectChangesEnabled = false;//禁用自动跟踪变化
                //AsNoTracking() 对象不被状态管理，查询性能提高。适用场景纯粹查询。 
                list = efdb.Set<Products>().LoadPageItems<Products, DateTime>(efdb, Common.pageSize, pageIndex, out count, whereLambda, r => (DateTime)(r.AddTime??DateTime.Now.AddMonths(-3)), true).AsNoTracking().ToList()
                    .Select(r => new Products()
                    {
                        SupplierName = efdb.Suppliers.Where(a => a.SupplierId.Equals(r.SupplierId)).Select(a=>a.SupplierName).FirstOrDefault(),
                        ProductId = r.ProductId,
                        ProductName = r.ProductName,
                        CategoryName = r.ProductCategory.CategoryName,
                        InUnitPrice = r.InUnitPrice,
                        UnitPrice = r.UnitPrice,
                        Discount = r.Discount,
                        TotalCount = efdb.ProductInventory.Where(i => i.ProductId.Equals(r.ProductId)).Select(a => a.TotalCount).FirstOrDefault()
                        
                        // r.ProductInventory.TotalCount 
                    })

                    .ToList();

                #region MyRegion
                
                //list = (List<Products>)efdb.Set<Products>().Where<Products>(whereLambda)
                //    .OrderBy<Products, DateTime?>(r => r.AddTime)
                //    .Skip(Common.pageSize * (pageIndex - 1))
                //    .Take(Common.pageSize)
                //    .ToList()
                //.Select(r => new Products() { Supplier = r.Supplier, ProductId = r.ProductId, ProductName = r.ProductName, CategoryName = r.ProductCategory.CategoryName, InUnitPrice = r.InUnitPrice, UnitPrice = r.UnitPrice, Discount = r.Discount, TotalCount = r.ProductInventory.TotalCount })
                //.ToList();
                #region MyRegion
                //.GroupJoin(
                //efdb.ProductCategory, 
                //p => p.CategoryId, 
                //c => c.CategoryId, 
                //(p, c) => new Products(){ Supplier = p.Supplier, ProductId = p.ProductId, ProductName = p.ProductName, CategoryName = c.FirstOrDefault().CategoryName, InUnitPrice = p.InUnitPrice, UnitPrice = p.UnitPrice, Discount = p.Discount, TotalCount = p.TotalCount })
                //.ToList() 
                #endregion
                //.GroupJoin(
                //efdb.ProductInventory,
                //p => p.ProductId,
                //i => i.ProductId,
                //(p, i) => new Products() { Supplier = p.Supplier, ProductId = p.ProductId, ProductName = p.ProductName, CategoryName = p.CategoryName, InUnitPrice = p.InUnitPrice, UnitPrice = p.UnitPrice, Discount = p.Discount, TotalCount = i.FirstOrDefault().TotalCount }
                //).ToList();

                //list.ForEach(r =>
                //    {
                // r.CategoryName = proCateService.GetEntityById(r.CategoryId.ToString()).CategoryName;
                // r.TotalCount = proInventoryService.GetEntityById(r.ProductId).TotalCount;
                //   }); 

                #endregion

                if (list != null && list.Count > 0)
                {
                    view.PageIndex = pageIndex;
                    view.PageCount = Math.Max((count + Common.pageSize - 1) / Common.pageSize, 1);
                    view.ListData = list;//list.Skip((pageIndex - 1) * Common.pageSize).Take(Common.pageSize).ToList();
                }
                else
                {
                    view.PageIndex = 1;
                    view.PageCount = 1;
                    view.ListData = null;
                }
            }

            #region MyRegion

            //StringBuilder where = new StringBuilder();
            //where.Append("where 1=1 ");
            //if (!string.IsNullOrEmpty(productId))
            //{
            //    where.Append(" and  ProductId like '%" + productId + "%'");
            //}
            //if (!string.IsNullOrEmpty(productName))
            //{
            //    where.Append(" and  ProductName like '%" + productName + "%'");
            //}
            //if (categoryId != null)
            //{
            //    where.Append(" and  CategoryId = " + categoryId);
            //}

            //using (SaleManagerDBEntities efdb = new SaleManagerDBEntities())
            //{
            //    list = efdb.Database.SqlQuery<Products>("select * from Products " + where + " order by AddTime desc").ToList();

            //    #region MyRegion
            //    //if (!string.IsNullOrEmpty(productName))
            //    //{
            //    //    list = list.Where(p => p.ProductName.Contains(productName)).ToList();
            //    //}
            //    //if (!string.IsNullOrEmpty(productId))
            //    //{
            //    //    list = list.Where(p => p.ProductId.Equals(productId)).ToList();
            //    //}
            //    //if (categoryId != null)
            //    //{
            //    //    list = list.Where(p => p.CategoryId.Equals(categoryId)).ToList();
            //    //} 
            //    #endregion

            //    list.ForEach(r =>
            //    {
            //        r.CategoryName = proCateService.GetEntityById(r.CategoryId.ToString()).CategoryName;
            //        r.TotalCount = proInventoryService.GetEntityById(r.ProductId).TotalCount;
            //    });

            //    if (list != null && list.Count > 0)
            //    {
            //        view.PageIndex = pageIndex;
            //        view.PageCount = Math.Max((list.Count() + Common.pageSize - 1) / Common.pageSize, 1);
            //        view.ListData = list.Skip((pageIndex - 1) * Common.pageSize).Take(Common.pageSize).ToList();
            //    }
            //    else
            //    {
            //        view.PageIndex = 1;
            //        view.PageCount = 1;
            //        view.ListData = null;
            //    }

            //} 

            #endregion

            return view;
        }
        
        public Products GetProductById(string productId)
        {
            Products pro = new Products();
            List<SalesListDetail> sales = new List<SalesListDetail>();
            List<Products> list = new List<Products>();
            using (SaleManagerDBEntities efdb = new SaleManagerDBEntities())
            {
                pro = efdb.Products.Find(productId); //.SingleOrDefault(p => p.ProductId.Equals(productId));
                if(pro!=null)
                {
                    sales = efdb.SalesListDetail.Where(p => p.ProductId.Equals(productId)).ToList();
                    pro.SaleCount = sales.Sum(r => r.Quantity);
                }
            }
            return pro;
        }

        public List<Products> SelectByEntity(string productName)
        {
            List<Products> pro = new List<Products>();
            //List<SalesListDetail> sales = new List<SalesListDetail>();
            List<Products> list = new List<Products>();
            using (SaleManagerDBEntities efdb = new SaleManagerDBEntities())
            {
                pro = efdb.Products.Where(p => p.ProductName.Equals(productName)).ToList();
                List<string> listIds = pro.Select(p => p.ProductId).ToList();
                //sales = efdb.SalesListDetail.Where(p => listIds.Where(t => t == p.ProductId).Contains(p.ProductId)).ToList();

                foreach (string id in listIds)
                {
                    foreach (SalesListDetail item in efdb.SalesListDetail)
                    {
                        if (item.ProductId.Equals(id))
                        {
                            pro.Where(p => p.ProductId.Equals(id)).FirstOrDefault().SaleCount += item.Quantity;
                        }
                    }
                }

                //list = (from a in sales
                //            join b in pro on a.ProductId equals b.ProductId into temp
                //            from obj in temp.DefaultIfEmpty()
                //            select obj).ToList();

            }
            return pro;
        }

        //public List<Products> SelectByCategory(int categoryId)
        //{
        //    List<Products> pro = new List<Products>();
        //    List<SalesListDetail> sales = new List<SalesListDetail>();
        //    List<Products> list = new List<Products>();
        //    using (SaleManagerDBEntities efdb = new SaleManagerDBEntities())
        //    {
        //       // pro = efdb.Products.Find(categoryId);
        //        pro = efdb.Products.Where(p => p.CategoryId.Equals(categoryId)).ToList();
        //        List<string> listIds = pro.Select(p => p.ProductId).ToList();
        //        foreach (string id in listIds)
        //        {
        //            foreach (SalesListDetail item in efdb.SalesListDetail)
        //            {
        //                if (item.ProductId.Equals(id))
        //                {
        //                    pro.Where(p => p.ProductId.Equals(id)).FirstOrDefault().SaleCount += item.Quantity;
        //                }
        //            }
        //        }

        //        //string ids = "'" + string.Join("','", listIds) + "'";
        //        //sales = efdb.SalesListDetail.Where(p =>
        //        //listIds.Where(t => t == p.ProductId).Contains(p.ProductId)).ToList();
        //        //list = (from a in sales
        //        //        join b in pro on a.ProductId equals b.ProductId into temp
        //        //        from obj in temp.DefaultIfEmpty()
        //        //        select obj).ToList();

        //    }
        //    return list;
        //}

        public int AddProducts(Products product)
        {
            int result = 0;
            using (SaleManagerDBEntities efdb = new SaleManagerDBEntities())
            {
                //efdb.Database.ExecuteSqlCommand("EXEC [AddVote] @blockId,@titleId,@typeId,@num out", paramArray.ToArray());
                //efdb.Database.ExecuteSqlCommand("usp_AddProduct @ProductId @ProductName", new SqlParameter[] { });
                //var results = efdb.Database.SqlQuery<表名>("usp_AppendInfo @insertCount out,@updateCount out", insertParam, updateParam);
                result = efdb.usp_AddProduct(product.SupplierName, product.ProductId, product.ProductName, product.InUnitPrice, product.UnitPrice, product.CategoryId, product.TotalCount, product.AddTime);
            }
            return result;
        }

        public int UpdateEntity(string proId, string propertyName,object value)
        {
            int result = 0;
            string supplierId = string.Empty;

            using (SaleManagerDBEntities efdb = new SaleManagerDBEntities())
            {
                Products proUpdate = efdb.Products.SingleOrDefault(r => r.ProductId.Equals(proId));
                
                //supplierName =  proUpdate.SupplierId;
                bool bl = false;
                //更新供应商
                if (propertyName.Equals("Supplier"))
                {
                    Supplier supplier = efdb.Suppliers.SingleOrDefault(r => r.SupplierName.Equals(value));
                    //if (supplier != null)
                    //{
                    //    supplier.SupplierName = value.ToString();
                    //    efdb.Entry<Supplier>(supplier).State = EntityState.Modified;
                    //}
                    if (supplier == null)
                    {
                        bl = true;
                        Supplier sup = new Supplier()
                        {
                            SupplierId = proUpdate.SupplierId = Guid.NewGuid().ToString(),
                            SupplierName = value.ToString()
                        };
                        efdb.Entry<Supplier>(sup).State = EntityState.Added;
                    }
                }
                else
                {
                    Type type = typeof(Products);
                    PropertyInfo[] proInfos = type.GetProperties();

                    foreach (var item in proInfos)
                    {
                        if (item.Name.Equals(propertyName))
                        {
                            if (!item.GetValue(proUpdate).Equals(value))
                            {
                                item.SetValue(proUpdate, value);
                                bl = true;
                            }

                        }
                    }
                }

                if(bl)
                {
                    efdb.Entry<Products>(proUpdate).State = EntityState.Modified;
                }
                else
                {
                    return 0;
                }
                
                result = efdb.SaveChanges();
            }

            return result;
        }

        public int DelEntity(string proId)
        {
            int result = 0;
            using (SaleManagerDBEntities efdb = new SaleManagerDBEntities())
            {
                //删除产品
                Products pro = efdb.Products.SingleOrDefault(r => r.ProductId.Equals(proId));
                efdb.Entry<Products>(pro).State = EntityState.Deleted;

                //删除库存
                ProductInventory proInventory = efdb.ProductInventory.SingleOrDefault(r => r.ProductId.Equals(proId));
                efdb.Entry<ProductInventory>(proInventory).State = EntityState.Deleted;

                result = efdb.SaveChanges();
            }
            return result;
        }

    }
}
 
