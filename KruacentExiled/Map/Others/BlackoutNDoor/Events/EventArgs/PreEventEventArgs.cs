using Exiled.Events.EventArgs.Interfaces;
using KruacentExiled.Map.Others.BlackoutNDoor.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Map.Others.BlackoutNDoor.Events.EventArgs
{
    public class PreEventEventArgs: IExiledEvent,IDeniableEvent
    {
        public bool IsAllowed { get; set; }
        public MapEvent MapEvent { get; set; }

        public PreEventEventArgs(MapEvent mapEvent, bool isAllowed)
        {
            MapEvent = mapEvent;
            IsAllowed = isAllowed;
        }

    }
}
