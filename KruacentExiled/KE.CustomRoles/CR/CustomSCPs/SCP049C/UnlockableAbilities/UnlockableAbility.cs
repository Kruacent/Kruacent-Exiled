using Exiled.API.Features;
using KE.CustomRoles.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.CustomRoles.CR.CustomSCPs.SCP049C.UnlockableAbilities
{
    public abstract class UnlockableAbility : Unlockable
    {



        public abstract KEAbilities Ability { get; }

        public override void Grant(ReferenceHub hub)
        {
            Player player = Player.Get(hub);
            Ability.AddAbility(player);

            if(!KEAbilities.TryGetSelected(player, out _))
            {
                KEAbilities.SelectFirstAbility(player);
            }

        }

        public override void Remove(ReferenceHub hub)
        {
            Ability.RemoveAbility(Player.Get(hub));
        }

    }
}
