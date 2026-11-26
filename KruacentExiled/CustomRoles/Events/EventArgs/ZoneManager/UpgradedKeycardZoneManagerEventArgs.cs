using Exiled.API.Features;
using Exiled.Events.EventArgs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.CustomRoles.Events.EventArgs.ZoneManager
{
    public class UpgradedKeycardZoneManagerEventArgs : IExiledEvent, IPlayerEvent
    {
        public Player Player { get; }



        public UpgradedKeycardZoneManagerEventArgs(Player player)
        {
            Player = player;
        }
    }
}
