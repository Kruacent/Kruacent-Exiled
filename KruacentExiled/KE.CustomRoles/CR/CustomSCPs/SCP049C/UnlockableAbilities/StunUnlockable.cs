using CustomPlayerEffects;
using Exiled.API.Features;
using InventorySystem.Items.Usables.Scp1344;
using KE.Utils.API.Features;
using System;
using LabPlayer = LabApi.Features.Wrappers.Player;
namespace KE.CustomRoles.CR.CustomSCPs.SCP049C.UnlockableAbilities
{
    internal class StunUnlockable : UnlockableAbility
    {
        public override byte Tier => 2;

        public override void Grant(ReferenceHub hub)
        {
            LabPlayer lab = LabPlayer.Get(hub);

            
            
        }

        public override void Remove(ReferenceHub hub)
        {
            LabPlayer lab = LabPlayer.Get(hub);

            KELog.Debug("remove");
        }
    }
}
