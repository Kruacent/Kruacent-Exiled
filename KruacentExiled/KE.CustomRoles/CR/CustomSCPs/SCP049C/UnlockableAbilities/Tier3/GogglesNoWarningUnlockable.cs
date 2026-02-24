using CustomPlayerEffects;
using Exiled.API.Features;
using InventorySystem.Items.Usables.Scp1344;
using System;
using LabPlayer = LabApi.Features.Wrappers.Player;
namespace KE.CustomRoles.CR.CustomSCPs.SCP049C.UnlockableAbilities.Tier3
{
    internal class GogglesNoWarningUnlockable : Unlockable
    {
        public override byte Tier => 3; //3
        public override string GetName(ReferenceHub hub)
        {
            return "SCP-1344";
        }
        public override string GetDescription(ReferenceHub hub)
        {
            return "See through walls like when using SCP-1344";
        }
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
