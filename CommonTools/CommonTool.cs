using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommonTools
{
    public class CommonTool
    {
        public static CommonTool commonTool;
        public static void Info()
        {
            if (commonTool == null)
            {
                commonTool = new CommonTool();
            }
        }

        public V ChangeTypeTToV<T, V>(T entityFrom, V entityTo)
        {
            Type typeFrom = typeof(T);
            Type typeTo = typeof(V);
            PropertyInfo[] prosFrom = typeFrom.GetProperties();
            PropertyInfo[] prosTo = typeTo.GetProperties();
            foreach (var itemFrom in prosFrom)
            {
                foreach (var itemTo in prosTo)
                {
                    if (itemFrom.Name.Equals(itemTo.Name))
                    {
                        itemTo.SetValue(entityTo, itemFrom.GetValue(entityFrom));
                    }
                }
            }
            return entityTo;

        }

        public bool StartOnlyProcess()
        {
            Process[] processArray = Process.GetProcesses();
            int currentCount = 0;//当前进程的总数
            foreach (Process item in processArray)
            {
                if (item.ProcessName.Equals(Process.GetCurrentProcess().ProcessName))
                {
                    currentCount++;
                }
            }
            if (currentCount > 1)
            {
                return false;
            }
            return true;
        }


        public string GetEnumDiscription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute = (DescriptionAttribute)fi.GetCustomAttribute(typeof(DescriptionAttribute));
            return attribute.Description;
        }

    }


}
