using Exiled.Events;
using Exiled.Events.Features;
using KruacentExiled.Map.Others.BlackoutNDoor.Events.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Map.Others.BlackoutNDoor.Events.Handlers
{
    public static class DoorStuckHandler
    {



        public static Event<DoorStuckEventArgs> DoorStucking { get; set; } = new Event<DoorStuckEventArgs>();



        public static void OnDoorStucking(DoorStuckEventArgs ev)
        {
            DoorStucking?.InvokeSafely(ev);
        }
    }
}
