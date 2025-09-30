using Exiled.API.Enums;
using Exiled.Events.EventArgs.Interfaces;
using KE.Map.Others.BlackoutNDoor.Handlers;
using KE.Map.Others.BlackoutNDoor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Map.Others.BlackoutNDoor.Events.EventArgs
{
    public class ChoseZoneEventArgs : IExiledEvent
    {
        public ZoneType Zone { get; set; }

        public ChoseZoneEventArgs(ZoneType zone)
        {
            Zone = zone;
        }

    }
}
