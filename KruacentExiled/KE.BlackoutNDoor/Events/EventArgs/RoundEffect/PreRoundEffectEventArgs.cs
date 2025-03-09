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
    public class PreRoundEffectEventArgs : IExiledEvent, IDeniableEvent, IZoneEvent
    {
        public bool IsAllowed { get; set; }
        public ZoneType Zone { get; }
        public RoundE RoundEffect { get; set; }


        public PreRoundEffectEventArgs(ZoneType zone, RoundE roundEffect, bool isAllowed = true)
        {
            Zone = zone;
            RoundEffect = roundEffect;
            IsAllowed = isAllowed;
        }
    }
}
