using Exiled.API.Features;
using KE.CustomRoles.Abilities.FireAbilities;
using KE.Utils.API.CustomStats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.CustomRoles.API.Features
{
    public abstract class KEAbilityCost : KEAbilities
    {

        public abstract int Cost { get; }
        protected sealed override bool AbilityUsed(Player player)
        {
            bool result = CanLaunchAbility(player);
            if (result)
            {
                result = LaunchedAbility(player);
            }
            return result;
        }

        protected virtual bool LaunchedAbility(Player player)
        {
            return true;
        }

        public abstract bool CanLaunchAbility(Player player);


        protected override void Gui(StringBuilder sb,Player player)
        {
            base.Gui(sb,player);
            sb.Append("(");
            sb.Append(Cost);
            sb.Append(")");
            sb.Append(" ");
        }
    }
}
