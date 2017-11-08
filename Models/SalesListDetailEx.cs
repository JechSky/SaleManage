using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks; 

namespace Models
{
    public partial class SalesListDetail
    {
         
        public string ProductName { get; set; }

        //进货价
        public decimal InUnitPrice { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalMoney { get; set; }

        public int SaleDisCount { get; set; }

        
        public string SaleDisCountName { get {
                if(SaleDisCount!=0)
                {
                    //DisCountEnum disEnum = (DisCountEnum)Enum.Parse(typeof(DisCountEnum), SaleDisCount.ToString());
                    //FieldInfo fi = disEnum.GetType().GetField(disEnum.ToString());
                    //DescriptionAttribute attr = (DescriptionAttribute)fi.GetCustomAttribute(typeof(DescriptionAttribute));
                    //return attr.Description;

                    return getDes((DisCountEnum)Enum.Parse(typeof(DisCountEnum), SaleDisCount.ToString()));
                }
                //DisCountEnum disenum = DisCountEnum.CallNormal;
                //FieldInfo field = disenum.GetType().GetField(disenum.ToString());
                //DescriptionAttribute attribute = (DescriptionAttribute)field.GetCustomAttribute(typeof(DescriptionAttribute));
                //return attribute.Description;
                return getDes((DisCountEnum)Enum.Parse(typeof(DisCountEnum), DisCountEnum.CallNormal.ToString()));
            }
        }

        //利润
        public decimal Profit { get; set; }

        public string CategoryId { get; set; }
        public string CategoryName { get; set; }

        public DateTime SaleDate { get; set; }

        string getDes(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            return ((DescriptionAttribute)field.GetCustomAttribute(typeof(DescriptionAttribute))).Description;
        }

    }
}
