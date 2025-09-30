using Exiled.Events.EventArgs.Interfaces;
using KE.Map.Others.BlackoutNDoor.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Map.Others.BlackoutNDoor.Events.EventArgs
{
    public class PostEventEventArgs : IExiledEvent
    {
        public MapEvent MapEvent { get; set; }

        public PostEventEventArgs(MapEvent mapEvent)
        {
            MapEvent = mapEvent;
        }


    }
}
