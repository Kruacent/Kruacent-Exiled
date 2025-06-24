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
            Player player = ev.Player;
            Room room;
            if (ev.KnobSetting == Scp914KnobSetting.Fine && LuckCheck(1))
            {
                room =ZoneType.Entrance.RandomSafeRoom();
                if(room != null)
                    player.Teleport(room);
            }
            if(ev.KnobSetting == Scp914KnobSetting.Coarse && LuckCheck(25))
            {
                room = ZoneType.LightContainment.RandomSafeRoom();
                if (room != null)
                    player.Teleport(room);
            } 


        }




    }
}
