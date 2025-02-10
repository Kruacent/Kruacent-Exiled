using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Items.Interface
{
    public interface ICustomItem
    {
        CustomItemEffect Effect { get; set; }
    }
}
