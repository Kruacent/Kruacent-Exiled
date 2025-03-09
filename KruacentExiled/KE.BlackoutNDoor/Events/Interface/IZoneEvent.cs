using Exiled.API.Enums;
using Exiled.Events.EventArgs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.BlackoutNDoor.Events.Interface
{
    internal interface IZoneEvent : IExiledEvent
    {

        public ZoneType Zone { get; }
    }
}
