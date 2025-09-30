using Exiled.API.Enums;
using KE.Map.Others.BlackoutNDoor.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Map.Others.BlackoutNDoor
{
    public class Both : MapEvent
    {
        private DoorStuck doorstuck = new();
        private Blackout blackout = new();
        public override string Cassie => MainPlugin.Translations.Both;

        public override string CassieTranslated => MainPlugin.Translations.BothTranslation;
        public override void Start(ZoneType zone)
        {
            doorstuck.Start(zone);
            blackout.Start(zone);
        }

        public override void Stop(ZoneType zone)
        {
            doorstuck.Stop(zone);
            blackout.Stop(zone);
        }
    }
}
