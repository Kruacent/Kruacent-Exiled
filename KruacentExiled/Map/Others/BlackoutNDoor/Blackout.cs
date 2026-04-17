using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.Events.EventArgs.Player;
using KruacentExiled.Map;
using KruacentExiled.Map.Others.BlackoutNDoor.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KruacentExiled.Map.Others.BlackoutNDoor
{
    public class Blackout : MapEvent
    {
        public override string Cassie => MainPlugin.Translations.Blackout;

        public override string CassieTranslated => MainPlugin.Translations.BlackoutTranslation;
        public override float Duration => 20;

        private ZoneType currentZone = ZoneType.Unspecified;
        public override void Start(ZoneType zone)
        {
            currentZone = zone;
            foreach (Room room in Room.List.Where(r => r.Zone == zone))
            {
                room.TurnOffLights();
            }

            Exiled.Events.Handlers.Player.TriggeringTesla += OnTriggeringTesla;
        }

        private void OnTriggeringTesla(TriggeringTeslaEventArgs ev)
        {
            if (ev.Tesla.Room.Zone.HasFlagFast(currentZone))
            {
                ev.IsAllowed = false;
            }
            
        }

        public override void Stop(ZoneType zone)
        {

            Exiled.Events.Handlers.Player.TriggeringTesla -= OnTriggeringTesla;
            foreach (Room room in Room.List.Where(r => r.Zone == zone))
            {
                room.AreLightsOff = false;
            }
            currentZone = ZoneType.Unspecified;
        }


    }
}
