using CustomPlayerEffects;
using Exiled.API.Features;
using InventorySystem.Items.Usables.Scp1344;
using System;
using LabPlayer = LabApi.Features.Wrappers.Player;
namespace KE.CustomRoles.CR.CustomSCPs.SCP049C.UnlockableAbilities
{
    internal class GogglesNoWarningUnlockable : UnlockableAbility
    {
        public override byte Tier => 99; //3

        public override void Grant(ReferenceHub hub)
        {
            LabPlayer lab = LabPlayer.Get(hub);

            lab.EnableEffect<Scp1344>();
            
        }

        public override void Remove(ReferenceHub hub)
        {
            LabPlayer lab = LabPlayer.Get(hub);

            lab.DisableEffect<Scp1344>();
        }
    }
}
