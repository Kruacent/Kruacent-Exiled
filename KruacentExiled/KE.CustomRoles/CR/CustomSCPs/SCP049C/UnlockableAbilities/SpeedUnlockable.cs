using CustomPlayerEffects;
using Exiled.API.Features;
using InventorySystem.Items.Usables.Scp1344;
using KE.Utils.API.Features;
using System;
using LabPlayer = LabApi.Features.Wrappers.Player;
namespace KE.CustomRoles.CR.CustomSCPs.SCP049C.UnlockableAbilities
{
    internal class SpeedUnlockable : UnlockableAbility
    {
        public override byte Tier => 3;

        public override void Grant(ReferenceHub hub)
        {
            LabPlayer lab = LabPlayer.Get(hub);

            MovementBoost move = lab.GetEffect<MovementBoost>();

            lab.EnableEffect<MovementBoost>((byte)(move.Intensity + 50), 0, false);
            hub.GetComponent<SCP049CLevelSystem>().DisableAll();
            

        }

        public override void Remove(ReferenceHub hub)
        {
            LabPlayer lab = LabPlayer.Get(hub);
            MovementBoost move = lab.GetEffect<MovementBoost>();

            lab.EnableEffect<MovementBoost>((byte)(move.Intensity - 50), 0, false);


            
        }
    }
}
