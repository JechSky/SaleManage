using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ProductInventoryService
    {
        ProductCategoryService proCategoryService = new ProductCategoryService();
        public ProductInventory GetEntityById(string id)
        {
            ProductInventory entity = new ProductInventory();
            using (SaleManagerDBEntities efdb = new SaleManagerDBEntities())
            {
                entity = efdb.ProductInventory.FirstOrDefault(r => r.ProductId.Equals(id));
            }
            return entity;
        }

        public ListDataView GetListBySearch(string productId = "", string productName = "", int? categoryId = null, int pageIndex = 1)
        {
            List<Products> list = new List<Products>();

            ListDataView view = new ListDataView();
            Expression<Func<Products, bool>> whereLambda = a => a != null;
            if(!string.IsNullOrEmpty(productId))
            {
                whereLambda = whereLambda.And(a => a.ProductId.Contains(productId));
            }
            if (!string.IsNullOrEmpty(productName))
            {
                whereLambda = whereLambda.And(a => a.ProductName.Contains(productName));
            }
            if (categoryId != null)
            {
                whereLambda = whereLambda.And(a => a.CategoryId == categoryId);
            }


            #region MyRegion
            //StringBuilder where = new StringBuilder();
            //where.Append(" where 1=1 ");
            //if (!string.IsNullOrEmpty(productId))
            //{
            //    where.Append(" and  ProductId like '%" + productId + "%'");
            //} 
            #endregion

            using (SaleManagerDBEntities efdb = new SaleManagerDBEntities())
            {
                #region MyRegion

                //list = efdb.Database.SqlQuery<ProductInventory>("select * from  ProductInventory " + where).ToList();

                //List<string> listProIds = list.Select(r => r.ProductId).ToList();
                //string ids = string.Empty;
                //if (listProIds.Count > 0)
                //{
                //    ids = "'" + string.Join("','", listProIds) + "'";
                //}
                //if (!string.IsNullOrEmpty(ids))
                //{
                //    listPros = efdb.Database.SqlQuery<Products>(string.Format(" select * from Products where ProductId in ({0}) ", ids)).ToList();
                //    Dictionary<string, string> dicProNM = new Dictionary<string, string>();
                //    Dictionary<string, string> dicProCategoryId = new Dictionary<string, string>();
                //    listPros.ForEach(r =>
                //    {
                //        dicProNM.Add(r.ProductId, r.ProductName);
                //        dicProCategoryId.Add(r.ProductId, r.CategoryId.ToString());
                //    });

                //    list.ForEach(r =>
                //    {
                //        foreach (KeyValuePair<string, string> item in dicProNM)
                //        {
                //            if (item.Key.Equals(r.ProductId))
                //            {
                //                r.ProductName = dicProNM[r.ProductId];
                //            }
                //        }
                //        foreach (KeyValuePair<string, string> item in dicProCategoryId)
                //        {
                //            if (item.Key.Equals(r.ProductId))
                //            {
                //                r.CategoryId = dicProCategoryId[r.ProductId];
                //            }
                //        }
                //        if (r.CategoryId != null)
                //        {
                //            r.CategoryName = proCategoryService.GetEntityById(r.CategoryId).CategoryName;
                //        }

                //    });

                //}

                //if (!string.IsNullOrEmpty(productName))
                //{
                //    list = list.Where(r => r.ProductName.Contains(productName)).ToList();
                //}
                //if (categoryId != null)
                //{
                //    list = list.Where(r => r.CategoryId.Equals(categoryId.ToString())).ToList();
                //} 

                #endregion

                int count = 0;
                list = efdb.Products
                    .LoadPageItems<Products, Int64>(efdb, Common.pageSize, pageIndex, out count, whereLambda, r => Convert.ToInt64(r.ProductId), false)
                    .ToList()
                    .Select(r => new Products() { ProductId = r.ProductId, ProductName = r.ProductName, CategoryId = r.ProductCategory.CategoryId, CategoryName = r.ProductCategory.CategoryName,
                        TotalCount = efdb.ProductInventory.Where(i => i.ProductId.Equals(r.ProductId)).Select(a => a.TotalCount).FirstOrDefault()
                        //r.ProductInventory.TotalCount
                    })
                    .ToList();


                if (list != null && list.Count > 0)
                {
                    view.PageIndex = pageIndex;
                    view.PageCount = Math.Max((count + Common.pageSize - 1) / Common.pageSize, 1);
                    view.ListData = list; //list.Skip((pageIndex - 1) * Common.pageSize).Take(Common.pageSize).ToList();
                }
                else
                {
                    view.PageIndex = 1;
                    view.PageCount = 1;
                    view.ListData = null;
                }
            }

            return view;
        }

        public int UpdateEntity(string proId, string value)
        {
            int result = 0;
            ProductInventory proInventory = new ProductInventory();
            using (SaleManagerDBEntities efdb = new SaleManagerDBEntities())
            {
                proInventory = efdb.ProductInventory.SingleOrDefault(r => r.ProductId.Equals(proId));
                if(!proInventory.TotalCount.ToString().Equals(value))
                {
                    proInventory.TotalCount = Convert.ToInt32(value);
                    efdb.Entry<ProductInventory>(proInventory).State = System.Data.Entity.EntityState.Modified;
                    result = efdb.SaveChanges();
                }
            }
            return result;
        }

        public int DelEntity(string proId)
        {
            int result = 0;
            ProductInventory proInventory = new ProductInventory();
            using (SaleManagerDBEntities efdb = new SaleManagerDBEntities())
            {
                proInventory = efdb.ProductInventory.SingleOrDefault(r => r.ProductId.Equals(proId));
                efdb.Entry<ProductInventory>(proInventory).State = System.Data.Entity.EntityState.Deleted;
                result = efdb.SaveChanges();
            }
            return result;
        }


    }
}
