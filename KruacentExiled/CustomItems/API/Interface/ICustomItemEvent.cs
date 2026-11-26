using KruacentExiled.CustomItems.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.CustomItems.API.Interface
{
    public interface ICustomItemEvent<T> where T : KECustomItem
    {
        T Item { get; }
    }
}
