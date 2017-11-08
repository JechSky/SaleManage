using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ProductCategoryService
    {
        public IEnumerable<ProductCategory> GetAll()
        {
            List<ProductCategory> listCategory = new List<ProductCategory>();
            using (SaleManagerDBEntities efdb = new SaleManagerDBEntities())
            {
                listCategory = efdb.ProductCategory.ToList();
            }
            return listCategory;
        }

        public ProductCategory GetEntityById(string id)
        {
            ProductCategory entity = new ProductCategory();
            using (SaleManagerDBEntities efdb = new SaleManagerDBEntities())
            {
                int cateid = Convert.ToInt32(id);
                entity = efdb.ProductCategory.FirstOrDefault(r => r.CategoryId.Equals(cateid));
            }
            return entity;
        }

    }
}
