using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.CustomRoles.API.Interfaces.Ability
{
    internal interface IDuration
    {
        float Duration { get; }
        void ActionAfterAbility(Player player);
    }
}
