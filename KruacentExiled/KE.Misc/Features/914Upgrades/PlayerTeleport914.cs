using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Scp914;
using KE.Utils.Extensions;
using PlayerRoles;
using Scp914;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Misc.Features._914Upgrades
{
    public class PlayerTeleport914 : Base914PlayerUpgrade
    {
        public float ChanceTpEntrance = 1;
        protected override float Chance => 100;
        protected override void OnUpgradingPlayer(UpgradingPlayerEventArgs ev)
        {
            Log.Debug("Upgrade");
            Player player = ev.Player;
            Room room;
            if (ev.KnobSetting == Scp914KnobSetting.Fine && LuckCheck(1))
            {
                try
                {
                    room = ZoneType.Entrance.RandomSafeRoom();
                }
                catch (Exception)
                {
                    room = Room.Random(ZoneType.Entrance);
                }
                
                if(room != null)
                    player.Teleport(room);
            }
            if(ev.KnobSetting == Scp914KnobSetting.Coarse && LuckCheck(25))
            {
                try
                {
                    room = ZoneType.Entrance.RandomSafeRoom();
                }
                catch (Exception)
                {
                    room = Room.Random(ZoneType.LightContainment);
                }
                if (room != null)
                    player.Teleport(room);
            } 


        }




    }
}
