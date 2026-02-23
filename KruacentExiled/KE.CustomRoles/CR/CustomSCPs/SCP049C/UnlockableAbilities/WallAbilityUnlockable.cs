using Exiled.API.Features;
using Exiled.API.Features.Roles;
using KE.Utils.API.Features;
using PlayerRoles.FirstPersonControl;
using System;
using LabPlayer = LabApi.Features.Wrappers.Player;
namespace KE.CustomRoles.CR.CustomSCPs.SCP049C.UnlockableAbilities
{
    internal class WallAbilityUnlockable : UnlockableAbility
    {
        public override byte Tier => 2;

        public override void Grant(ReferenceHub hub)
        {

        }

        public override void Remove(ReferenceHub hub)
        {
            KELog.Debug("remove");
        }
    }
}
