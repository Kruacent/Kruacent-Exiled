using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.CustomRoles.API.Interfaces.Ability
{
    public interface IDynamicName
    {

        void GetName(StringBuilder sb, Player player);
    }
}
