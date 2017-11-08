using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Models;
using System.Data.Entity;
using System.Reflection;
using System.Linq.Expressions;

namespace DAL
{
    public class SaleListService
    {
        ProductService proService = new ProductService();
        ProductCategoryService proCateService = new ProductCategoryService();
        ProductInventoryService proInventoryService = new ProductInventoryService();
        ProductCategoryService proCategoryService = new ProductCategoryService();

        ListDataView view = new ListDataView();

        public ListDataView GetListBySearch(DateTime? dts, DateTime? dte, int pageIndex = 1, string serialNum = "", string productId = "", string productName = "",int? categoryId=null)
        {
            List<SalesList> listSales = new List<SalesList>();
            List<SalesListDetail> list = new List<SalesListDetail>();

            using (SaleManagerDBEntities efdb = new SaleManagerDBEntities())
            {
                #region MyRegion

                //if (dts == null)
                //{
                //    dts = DateTime.Now.AddDays(-1);
                //}
                //if (dte == null)
                //{
                //    dts = DateTime.Now.AddDays(1);
                //}

                //StringBuilder whereStr = new StringBuilder();
                //whereStr.Append(" and 1= 1 ");
                //if (!string.IsNullOrEmpty(serialNum))
                //{
                //    whereStr.Append(" and SerialNum = '" + serialNum + "'");
                //}
                //if (!string.IsNullOrEmpty(productId))
                //{
                //    whereStr.Append(" and ProductId like '%" + productId + "%'");
                //}
                //if (!string.IsNullOrEmpty(productName))
                //{
                //    whereStr.Append(" and ProductName like '%" + productName + "%'");
                //}

                //string sql = string.Format("select * from SalesList where SaleDate between '{0}' and '{1}' ", dts, dte);
                //listSales = efdb.Database.SqlQuery<SalesList>(sql).ToList();

                //List<string> listSerial = listSales.Select(r => r.SerialNum).ToList();
                //string ids = string.Empty;
                //if (listSerial.Count > 0)
                //{
                //    ids = "'" + string.Join("','", listSerial) + "'";
                //}
                //if (!string.IsNullOrEmpty(ids))
                //{
                //    list = efdb.Database.SqlQuery<SalesListDetail>(
                //        string.Format(" select * from SalesListDetail where SerialNum in ({0}) " + whereStr.ToString(),
                //        ids)).ToList();
                //}


                //list.ForEach(r =>
                //{
                //    SalesList sale = listSales.SingleOrDefault(p => p.SerialNum.Equals(r.SerialNum));
                //    r.SaleDate = sale.SaleDate;
                //    r.TotalMoney = sale.TotalMoney;
                //    r.SaleDisCount = sale.SaleDisCount ?? 0;
                //});
                //list = list.OrderByDescending(r => r.SaleDate).ToList();



                //list.ForEach(r =>
                //{
                //    Products pro = proService.GetProductById(r.ProductId);
                //    r.InUnitPrice = pro.InUnitPrice;
                //    r.CategoryId = pro.CategoryId.ToString();
                //    r.CategoryName = proCategoryService.GetEntityById(pro.CategoryId.ToString()).CategoryName;
                //    // r.Profit = (r.UnitPrice - r.InUnitPrice) * r.Quantity;
                //    // r.CategoryName = proCateService.GetEntityById(r.CategoryId.ToString()).CategoryName;
                //});

                //for (int i = 0; i < list.Count; i++)
                //{
                //    decimal sumSubTotal = list[i].SubTotalMoney ?? 0.00m;
                //    decimal sumInPrice = list[i].InUnitPrice * list[i].Quantity;
                //    for (int j = i + 1; j < list.Count; j++)
                //    {
                //        if (list[i].SerialNum.Equals(list[j].SerialNum))
                //        {
                //            sumSubTotal += list[j].SubTotalMoney ?? 0.00m;
                //            sumInPrice += list[j].InUnitPrice * list[j].Quantity;
                //        }
                //    }
                //    list[i].Profit = sumSubTotal - sumInPrice;
                //}

                //if (categoryId != null)
                //{
                //    list = list.Where(r => r.CategoryId.Equals(categoryId.ToString())).ToList();
                //} 

                #endregion

                int count = 0;
                Expression<Func<SalesListDetail,bool>> whereLambda = a => a != null;
                if (!string.IsNullOrEmpty(serialNum))
                {
                    whereLambda = whereLambda.And(a => a.SerialNum.Equals(serialNum));
                }
                if (!string.IsNullOrEmpty(productId))
                {
                    whereLambda = whereLambda.And(a => a.ProductId.Contains(productId));
                }
                if (!string.IsNullOrEmpty(productName))
                {
                    whereLambda = whereLambda.And(a => a.Product.ProductName.Contains(productName));
                }
                if (categoryId != null)
                {
                    whereLambda = whereLambda.And(a => a.Product.ProductCategory.CategoryId==(int)(categoryId??0));
                }
                if (dts == null)
                {
                    dts = DateTime.Now.AddMonths(-3);
                }
                if (dte == null)
                {
                    dte = DateTime.Now.AddDays(1);
                }
                if (dts != null&&dte!=null)
                {
                    whereLambda = whereLambda.And(a => a.SalesList.SaleDate >= dts && a.SalesList.SaleDate <= dte);
                }

                list = efdb.SalesListDetail.LoadPageItems<SalesListDetail, DateTime>(efdb, Common.pageSize, pageIndex, out count, whereLambda, r => r.SalesList.SaleDate, false)
                    .ToList()
                    .Select(r => new SalesListDetail()
                    {
                        SerialNum = r.SerialNum,
                        ProductId = r.Product.ProductId,
                        ProductName = r.Product.ProductName,
                        CategoryId = r.Product.ProductCategory.CategoryId.ToString(),
                        CategoryName = r.Product.ProductCategory.CategoryName,
                        SaleDate = r.SalesList.SaleDate,
                        Quantity = r.Quantity,
                        InUnitPrice = r.Product.InUnitPrice,
                        UnitPrice = r.Product.UnitPrice,
                        TotalMoney = r.SalesList.TotalMoney,
                        SaleDisCount =r.SalesList.SaleDisCount ?? 0,
                        Profit = r.SalesList.TotalMoney
                        -
                        (from a in efdb.SalesListDetail where a.SerialNum.Equals(r.SerialNum) select a.Product.InUnitPrice * a.Quantity).Sum()
                    
                    })
                    .ToList();

                decimal prfit = 0.00M;
                List<SalesListDetail> listDis = list.Distinct(new FastPropertyComparer<SalesListDetail>("SerialNum")).ToList();
                prfit = listDis.Sum(r => r.Profit);
                view.Remark = prfit;

                if (list != null && list.Count > 0)
                {
                    view.PageIndex = pageIndex;
                    view.PageCount = Math.Max((list.Count() + Common.pageSize - 1) / Common.pageSize, 1);
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
 
        //结算时需要更新的数据 销售主表、销售明细表、更新商品库存、更新会员积分
        public bool SaveSaleInfo(SalesList objSalesList, List<SalesListDetail> detailList)
        {
            ProductInventory proInventory = new ProductInventory();
            using (SaleManagerDBEntities efdb = new SaleManagerDBEntities())
            {
                efdb.Entry<SalesList>(objSalesList).State = EntityState.Added;
                foreach (SalesListDetail item in detailList)
                {
                    efdb.Entry<SalesListDetail>(item).State = EntityState.Added;
                    proInventory = efdb.ProductInventory.SingleOrDefault(p => p.ProductId.Equals(item.ProductId));
                    proInventory.TotalCount -= item.Quantity;
                    efdb.Entry<ProductInventory>(proInventory).State = EntityState.Modified;
                }
                return efdb.SaveChanges() == 1 ? true : false;
            }
            return false;
        }

        /// <summary>
        /// 更新会员积分
        /// </summary>
        /// <param name="memeberId"></param>
        /// <param name="points"></param>
        /// <returns></returns>
        public int AddPoints(string memeberId,int points)
        {
            SMMembers member = new SMMembers();
            using (SaleManagerDBEntities efdb = new SaleManagerDBEntities())
            {
                member= efdb.SMMembers.SingleOrDefault(p => p.MemberId.ToString().Equals(memeberId));
                member.Points += points;
                efdb.Entry<SMMembers>(member).State = EntityState.Modified;
                return efdb.SaveChanges();
            }
            return 0;
        }

    }

    //class Compare:IEqualityComparer<SalesListDetail>
    //{
    //    public bool Equals(SalesListDetail t1, SalesListDetail t2)
    //    {
    //        if(t1==null)
    //        {
    //            return t2 == null;
    //        }
    //        return t1.SerialNum.Equals(t2.SerialNum);
    //    }

    //    public int GetHashCode(SalesListDetail obj)
    //    {
    //        if(obj==null)
    //        {
    //            return 0;
    //        }
    //        return obj.SerialNum.GetHashCode();
    //    }

    //}

    public class FastPropertyComparer<T> : IEqualityComparer<T>
    {
        Func<T, object> getPropertyValueFunc = null;

        public FastPropertyComparer(string propertyName)
        {
            PropertyInfo propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);
            if(propertyInfo==null)
            {
                throw new ArgumentException(string.Format("{0} is not a property of type {1}.", propertyName, typeof(T)));
            }
            //propertyInfo.GetGetMethod
            ParameterExpression expPara = Expression.Parameter(typeof(T), "obj");
            MemberExpression me = Expression.Property(expPara, propertyInfo);
            getPropertyValueFunc = Expression.Lambda<Func<T, object>>(me, expPara).Compile();

        }

        public bool Equals(T t1, T t2)
        {
            object t1Value = getPropertyValueFunc(t1);
            object t2Value = getPropertyValueFunc(t2);

            if (t1Value == null)
            {
                return t2Value == null;
            }
            return t1Value.Equals(t2Value);

        }
        public int GetHashCode(T obj)
        {
            object propertyValue = getPropertyValueFunc(obj);
            if (propertyValue == null)
            {
                return 0;
            }
            else
                return propertyValue.GetHashCode();

        }


    }



}
