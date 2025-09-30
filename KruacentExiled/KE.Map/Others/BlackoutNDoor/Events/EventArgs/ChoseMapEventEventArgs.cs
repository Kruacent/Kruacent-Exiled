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
    public class ChoseMapEventEventArgs : IExiledEvent, IMapEvent
    {
        public MapEvent MapEvent { get; set; }

        public ChoseMapEventEventArgs(MapEvent mapEvent)
        {
            MapEvent = mapEvent;
        }

    }
}
