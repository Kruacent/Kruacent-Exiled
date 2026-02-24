using Exiled.API.Features;
using Exiled.API.Features.Roles;
using KE.Utils.API.Features;
using PlayerRoles.FirstPersonControl;
using System;
using LabPlayer = LabApi.Features.Wrappers.Player;
namespace KE.CustomRoles.CR.CustomSCPs.SCP049C.UnlockableAbilities.Tier2
{
    internal class WallAbilityUnlockable : Unlockable
    {
        public override byte Tier => 2;
        public override string GetName(ReferenceHub hub)
        {
            return "Deflect Damages";
        }
        public override string GetDescription(ReferenceHub hub)
        {
            return "1 chance sur 8 d'annuler un dégât (.5s par hp sauvé de cooldown)";
        }

        public override void Grant(ReferenceHub hub)
        {

        }

        public override void Remove(ReferenceHub hub)
        {
            
        }
    }
}
