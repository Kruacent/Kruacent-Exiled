using Exiled.API.Enums;
using KruacentExiled.Map;
using KruacentExiled.Map.Others.BlackoutNDoor.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Map.Others.BlackoutNDoor
{
    public class Both : MapEvent
    {
        private DoorStuck doorstuck = new DoorStuck();
        private Blackout blackout = new Blackout();
        private ElevatorStuck elevator = new ElevatorStuck();
        public override string Cassie => MainPlugin.Translations.Both;
        public override float Duration => 20;

        public override string CassieTranslated => MainPlugin.Translations.BothTranslation;
        public override void Start(ZoneType zone)
        {
            doorstuck.Start(zone);
            blackout.Start(zone);
            elevator.Start(zone);
        }

        public override void Stop(ZoneType zone)
        {
            doorstuck.Stop(zone);
            blackout.Stop(zone);
            elevator.Stop(zone);
        }
    }
}
