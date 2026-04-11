using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.CustomRoles.API.Interfaces
{
    public interface IHealable
    {

        public abstract HashSet<ItemType> HealItem { get; }
    }
}
