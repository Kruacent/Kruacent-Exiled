using Exiled.API.Enums;
using Exiled.API.Features.Doors;
using Exiled.Events.EventArgs.Interfaces;
using KruacentExiled.Map.Others.BlackoutNDoor.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Map.Others.BlackoutNDoor.Events.EventArgs
{
    public class DoorStuckEventArgs : IExiledEvent, IDeniableEvent, IZoneType
    {

        public HashSet<Door> Doors { get; set; }
        public bool IsAllowed { get; set; }
        public ZoneType Zone { get; set; }

        public DoorStuckEventArgs(HashSet<Door> doors,ZoneType zone, bool isAllowed = true)
        {
            Doors = doors;
            Zone = zone;
            IsAllowed = isAllowed;
        }
    }
}
