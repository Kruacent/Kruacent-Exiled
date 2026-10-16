using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Scp914;
using InventorySystem.Items.Usables.Scp330;
using KE.Utils.API.Features;
using KE.Utils.Extensions;
using KruacentExiled.CustomSpawnPoint;
using KruacentExiled.CustomSpawnPoint.Spawned;
using MEC;
using PlayerRoles.FirstPersonControl;
using Scp914;
using System;

namespace KE.Misc.Features._914Upgrades
{
    public class PlayerTeleport914 : Base914PlayerUpgrade
    {
        public const float ChanceTpEntrance = 1;
        protected override float Chance => 100;
        protected override bool OnUpgradingPlayer(UpgradingPlayerEventArgs ev)
        {
            KELog.Debug("Upgrade teleport");
            Player player = ev.Player;

            if(!(player.Role is FpcRole fpc))
            {
                return false;
            }
            Room room = GetRoom(ev.KnobSetting,player);

            if (room != null)
            {
                //idk why but need a delay
                Timing.CallDelayed(.1f, delegate
                {
                    KELog.Debug($"teleporting {player.Nickname} to {room.Name}");
                    RoomSpawnedCustomPoint point = RoomCustomPointHandler.GetRandomPoint(RoomCustomPointType.Teleport, room);
                    player.Teleport(point.Position);
                });
                
            }


            return true;
        }



        public Room GetRoom(Scp914KnobSetting setting,Player player)
        {
            Room room = null;

            KELog.Debug("tp entrance");
            if (setting == Scp914KnobSetting.Fine && LuckCheck(ChanceTpEntrance,player))
            {
                try
                {
                    room = ZoneType.Entrance.RandomSafeRoom();
                }
                catch (Exception e)
                {
                    Log.Error("error trying to get a room in entrance : " + e);
                    room = Room.Random(ZoneType.Entrance);
                }


            }
            if(setting == Scp914KnobSetting.Coarse && LuckCheck(25, player))
            {
                try
                {
                    room = ZoneType.LightContainment.RandomSafeRoom();
                }
                catch (Exception e)
                {
                    Log.Error("error trying to get a room in light : " + e );
                    room = Room.Random(ZoneType.LightContainment);
                }
            }

            return room;
        }


    }
}
