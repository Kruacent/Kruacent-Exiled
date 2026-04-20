using Exiled.API.Enums;
using Exiled.Events.EventArgs.Interfaces;

namespace KruacentExiled.Map.Others.BlackoutNDoor.Events.EventArgs
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
