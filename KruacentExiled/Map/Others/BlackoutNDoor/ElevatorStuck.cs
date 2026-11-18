using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using KruacentExiled.Map.Others.BlackoutNDoor.Handlers;
using KruacentExiled.Map.Others.CustomElevators;
using KruacentExiled.Map.Others.CustomElevators.KECustomElevators;
using KruacentExiled.Map.Surface.ElevatorGateA;
using LabApi.Features.Wrappers;
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
        private List<CustomElevatorComp> _elevators;
        public override string Cassie => MainPlugin.Translations.Elevatorstuck;

        public override string CassieTranslated => MainPlugin.Translations.ElevatorstuckTranslation;

        public override void Start(ZoneType zone)
        {
            _lifts = ListPool<Lift>.Pool.Get();
            _elevators = ListPool<CustomElevatorComp>.Pool.Get();

            foreach(Lift lift in Lift.List)
            {
                if(lift.Doors.Any(d => d.Zone == zone && !d.IsLocked))
                {
                    _lifts.Add(lift);
                    lift.ChangeLock(Interactables.Interobjects.DoorUtils.DoorLockReason.NoPower);
                }
            }

            foreach(CustomElevatorComp elevator in CustomElevatorComp.List)
            {
                if (!elevator.Locked && elevator.Zone == zone)
                {
                    _elevators.Add(elevator);
                    elevator.Locked = true;
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
            foreach (CustomElevatorComp elevator in _elevators)
            {
                elevator.Locked = false;
            }

            ListPool<CustomElevatorComp>.Pool.Return(_elevators);

        }
    }
}
