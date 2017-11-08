using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Common
    {
        public static Common common;
        public static readonly object obj = new object();
        public static SalesPerson objPerson = null;
        public static SysAdmins objSys = null;

        public static int pageSize = 10;

        public static List<ListItem> listCategoryItem = new List<ListItem>();
        static ProductCategoryService proCategoryService = new ProductCategoryService();
        private Common() { }
        public static void Info()
        {
            if (common == null)
            {
                lock (obj)
                {
                    if (common == null)
                    {
                        common = new Common();
                    }
                }
            }
            foreach (ProductCategory item in proCategoryService.GetAll())
            {
                listCategoryItem.Add(new ListItem(item.CategoryId.ToString(), item.CategoryName));
            }
        }

        public static DateTime GetServerTime()
        {
            DateTime? obj = null;
            using (SaleManagerDBEntities efdb = new SaleManagerDBEntities())
            {
                obj = efdb.Database.SqlQuery<DateTime>("select getdate()").FirstOrDefault();
            }
            return obj??DateTime.Now;
        }

        public int WriteLoginLog(LoginLogs log)
        {
            using (SaleManagerDBEntities efdb = new SaleManagerDBEntities())
            {
                efdb.Entry<LoginLogs>(log).State = System.Data.Entity.EntityState.Added;
                if (efdb.SaveChanges() == 1)
                    return log.LoginId;
            }
            return 0;
        }
        public int WriteExitLog(int logId)
        {
            LoginLogs loginLog = new LoginLogs();
            using (SaleManagerDBEntities efdb = new SaleManagerDBEntities())
            {
                loginLog = efdb.LoginLogs.FirstOrDefault(s => s.LoginId == logId);
                if (loginLog != null)
                {
                    loginLog.ExitTime = Common.GetServerTime();//DateTime.Now;
                    //获得给定实体的DbEntityEntry<TEntity>对象。以便提供对与该实体有关的信息的访问以及对实体执行操作的功能。
                    efdb.Entry<LoginLogs>(loginLog).State = System.Data.Entity.EntityState.Modified;
                    efdb.SaveChanges();
                }

                return 0;
            }
        }

    }

    public static class GetLists
    {
        public static IQueryable<T> LoadPageItems<T, Tkey>(this IQueryable<T> list, SaleManagerDBEntities efdb, int pageSize, int pageIndex, out int total, Expression<Func<T, bool>> whereLambda, Func<T, Tkey> orderbyLambda, bool isAsc) where T : class where Tkey : struct
        {
            total = efdb.Set<T>().Where(whereLambda).Count();
            if (isAsc)
            {
                //针对上下文和基础存储中给定类型的实体的访问返回一个DbSet<TEntity>实例
                var temp = efdb.Set<T>().Where(whereLambda)
                             .OrderBy<T, Tkey>(orderbyLambda)
                             .Skip(pageSize * (pageIndex - 1))
                             .Take(pageSize);
                return temp.AsQueryable();
            }
            else
            {
                var temp = efdb.Set<T>().Where(whereLambda)
                           .OrderByDescending<T, Tkey>(orderbyLambda)
                           .Skip(pageSize * (pageIndex - 1))
                           .Take(pageSize);
                return temp.AsQueryable();
            }
        }

    }
    
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> True<T>() { return f => true; }
        public static Expression<Func<T, bool>> False<T>() { return f => false; }
        public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.And);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.Or);
        }

    }

    public class ParameterRebinder : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> map;

        public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            ParameterExpression replacement;
            if (map.TryGetValue(p, out replacement))
            {
                p = replacement;
            }
            return base.VisitParameter(p);
        }
    }

}
