using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using KE.Map.Others.BlackoutNDoor.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KE.Map.Others.BlackoutNDoor
{
    public class Blackout : MapEvent
    {
        public override string Cassie => MainPlugin.Translations.Blackout;

        public override string CassieTranslated => MainPlugin.Translations.BlackoutTranslation;

        public override void Start(ZoneType zone)
        {
            foreach(Room room in Room.List.Where(r => r.Zone == zone))
            {
                room.TurnOffLights();
            }
        }

        public override void Stop(ZoneType zone)
        {
            foreach (Room room in Room.List.Where(r => r.Zone == zone))
            {
                room.AreLightsOff = false;
            }
        }


    }
}
