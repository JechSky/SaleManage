using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public enum DisCountEnum
    {
        [Description("不打折")]
        CallNormal = 0,
        [Description("折扣率0.5")]
        CallRate = 1,
        [Description("满300送100")]
        CallMN = 2
    }
}
