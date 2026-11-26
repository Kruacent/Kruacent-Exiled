using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using KruacentExiled.CustomRoles.API.Features;
using System.Collections.Generic;
namespace KruacentExiled.CustomRoles.CR.CustomSCPs.SCP049C.UnlockableAbilities.Tier2
{
    internal class DeflectDamageUnlockable : UnlockableAbility
    {
        public override byte Tier => 2;

        public override KEAbilities Ability => KEAbilities.Get("Parry");

        public override string GetName(ReferenceHub hub)
        {
            return "Parry";
        }
        public override string GetDescription(ReferenceHub hub)
        {
            return "You can now deflect damage for 0.5s (60s cooldown)";
        }
    }



    
}
