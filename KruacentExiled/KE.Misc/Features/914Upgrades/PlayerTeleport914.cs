using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Scp914;
using InventorySystem.Items.Usables.Scp330;
using KE.Utils.API.Features;
using KE.Utils.Extensions;
using MEC;
using PlayerRoles.FirstPersonControl;
using Scp914;
using System;

namespace KE.Misc.Features._914Upgrades
{
    public class PlayerTeleport914 : Base914PlayerUpgrade
    {
        public float ChanceTpEntrance = 1;
        protected override float Chance => 100;
        protected override bool OnUpgradingPlayer(UpgradingPlayerEventArgs ev)
        {
            KELog.Debug("Upgrade teleport");
            Player player = ev.Player;
            Room room = null;

            if(player.Role is not PlayerRoles.FirstPersonControl.IFpcRole fpc)
            {
                return false;
            }

            //TeleportOutcome.GetBestExitPosition(fpc);
            if (ev.KnobSetting == Scp914KnobSetting.Fine && LuckCheck(ChanceTpEntrance))
            {
                try
                {
                    room = ZoneType.Entrance.RandomSafeRoom();
                }
                catch (Exception)
                {
                    room = Room.Random(ZoneType.Entrance);
                }
                
                
            }
            if(ev.KnobSetting == Scp914KnobSetting.Coarse && LuckCheck(25))
            {
                try
                {
                    room = ZoneType.LightContainment.RandomSafeRoom();
                }
                catch (Exception)
                {
                    room = Room.Random(ZoneType.LightContainment);
                }
            }

            if (room != null)
            {
                //idk why but need a delay
                Timing.CallDelayed(.1f, delegate
                {
                    KELog.Debug($"teleporting {player.Nickname} to {room.Name}");
                    player.Teleport(room);
                });
                
            }


            return true;
        }




    }
}
