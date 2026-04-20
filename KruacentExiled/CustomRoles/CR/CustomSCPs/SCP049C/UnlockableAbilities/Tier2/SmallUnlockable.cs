using CustomPlayerEffects;
using Exiled.API.Features;
using InventorySystem.Items.Usables.Scp1344;
using KE.Utils.API.Features;
using KruacentExiled.CustomRoles.API.Features;
using KruacentExiled.CustomRoles.CR.CustomSCPs.SCP049C.UnlockableAbilities;
using System;
using LabPlayer = LabApi.Features.Wrappers.Player;
namespace KruacentExiled.CustomRoles.CR.CustomSCPs.SCP049C.UnlockableAbilities.Tier2
{
    internal class SmallUnlockable : UnlockableAbility
    {
        public override byte Tier => 2;

        public override KEAbilities Ability => KEAbilities.Get("Small");

        public override string GetName(ReferenceHub hub)
        {
            return "Smol";
        }
        public override string GetDescription(ReferenceHub hub)
        {
            return "Gain a new ability to be small for 30 seconds (60s of cooldown)";
        }

    }
}
