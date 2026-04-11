using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.CustomRoles.API.Interfaces.Ability
{
    public interface IDynamicDescription
    {


        void GetDescription(StringBuilder sb, Player player);

    }
}
