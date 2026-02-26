using CustomPlayerEffects;
using Exiled.API.Features;
using InventorySystem.Items.Usables.Scp1344;
using KE.Utils.API.Features;
using System;
using LabPlayer = LabApi.Features.Wrappers.Player;
namespace KE.CustomRoles.CR.CustomSCPs.SCP049C.UnlockableAbilities.Tier3
{
    internal class SpeedUnlockable : Unlockable
    {
        public override byte Tier => 3;

        public override string GetName(ReferenceHub hub)
        {
            return "Speed";
        }
        public override string GetDescription(ReferenceHub hub)
        {
            return $"Become {Intensity}% faster but lose all of the other abilities";
        }


        public const byte Intensity = 50;

        public override void Grant(ReferenceHub hub)
        {
            LabPlayer lab = LabPlayer.Get(hub);

            MovementBoost move = lab.GetEffect<MovementBoost>();

            lab.EnableEffect<MovementBoost>((byte)(move.Intensity + Intensity), 0, false);
            hub.GetComponent<SCP049CLevelSystem>().DisableAll();
            

        }

        public override void Remove(ReferenceHub hub)
        {
            LabPlayer lab = LabPlayer.Get(hub);
            MovementBoost move = lab.GetEffect<MovementBoost>();

            lab.EnableEffect<MovementBoost>((byte)(move.Intensity - Intensity), 0, false);


            
        }
    }
}
