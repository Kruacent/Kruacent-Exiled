using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using KruacentExiled.Map.Others.BlackoutNDoor.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Map.Others.BlackoutNDoor
{
    internal class ElevatorStuck : MapEvent
    {

        private List<Lift> _lifts;
        public override string Cassie => MainPlugin.Translations.Elevatorstuck;

        public override string CassieTranslated => MainPlugin.Translations.ElevatorstuckTranslation;

        public override void Start(ZoneType zone)
        {
            _lifts = ListPool<Lift>.Pool.Get();

            foreach(Lift lift in Lift.List)
            {
                if(lift.Doors.Any(d => d.Zone == zone && !d.IsLocked))
                {
                    _lifts.Add(lift);
                    lift.ChangeLock(Interactables.Interobjects.DoorUtils.DoorLockReason.NoPower);
                }
            }


            


        }

        public override void Stop(ZoneType zone)
        {

            foreach (Lift lift in _lifts)
            {
                lift.ChangeLock(Interactables.Interobjects.DoorUtils.DoorLockReason.None);
            }


            ListPool<Lift>.Pool.Return(_lifts);
        }
    }
}
