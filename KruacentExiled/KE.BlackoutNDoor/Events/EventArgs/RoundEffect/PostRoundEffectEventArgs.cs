using Exiled.API.Enums;
using Exiled.Events.EventArgs.Interfaces;
using KE.BlackoutNDoor.API.Features.RoundEffects;
using KE.BlackoutNDoor.Events.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoundE = KE.BlackoutNDoor.API.Features.RoundEffects.RoundEffect;

namespace KE.BlackoutNDoor.Events.EventArgs.RoundEffect
{
    public class PostRoundEffectEventArgs : IExiledEvent, IZoneEvent
    {
        public ZoneType Zone { get; }
        public RoundE RoundEffect { get; set; }


        public PostRoundEffectEventArgs(ZoneType zone, RoundE roundEffect)
        {
            Zone = zone;
            RoundEffect = roundEffect;
        }
    }
}
