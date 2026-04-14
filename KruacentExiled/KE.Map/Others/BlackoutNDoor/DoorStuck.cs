using Exiled.API.Enums;
using Exiled.API.Features.Doors;
using Interactables.Interobjects.DoorUtils;
using KE.Map.Others.BlackoutNDoor.Events.EventArgs;
using KE.Map.Others.BlackoutNDoor.Events.Handlers;
using KE.Map.Others.BlackoutNDoor.Handlers;
using KE.Utils.API.Map;
using NorthwoodLib.Pools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KE.Map.Others.BlackoutNDoor
{
    
    public class DoorStuck : MapEvent
    {

        private static HashSet<Door> doors;
        public override string Cassie => MainPlugin.Translations.Doorstuck;

        public override string CassieTranslated => MainPlugin.Translations.DoorstuckTranslation;
        public override float Duration => 15;

        public override void Start(ZoneType zone)
        {
            bool open = UnityEngine.Random.value > .5f;
            doors = HashSetPool<Door>.Shared.Rent();
            foreach (Door door in Door.List.Where(d => d != null&& d.Zone == zone && !d.IsElevator && d.Type != DoorType.Scp079First && d.Type != DoorType.Scp079Second))
            {
                if (door.DoorLockType == DoorLockType.None)
                {
                    doors.Add(door);
                    
                }
            }

            if(StructureSpawner.AdditionalDoors.TryGetValue(zone,out var addDoors))
            {
                foreach(DoorVariant doorVariant in addDoors)
                {
                    Door door2 = Door.Get(doorVariant);
                    if (door2 != null && door2.DoorLockType == DoorLockType.None)
                    {
                        doors.Add(door2);
                    }
                }
            }

            DoorStuckEventArgs ev = new(doors,zone,true);
            DoorStuckHandler.OnDoorStucking(ev);

            if (ev.IsAllowed && ev.Doors != null)
            {
                doors = ev.Doors;
                foreach (Door door in doors)
                {
                    LockDoor(door, open);
                }
            }
            else
            {
                doors.Clear();
            }


        }

        private void LockDoor(Door door, bool open)
        {
            door.ChangeLock(DoorLockType.NoPower);
            door.IsOpen = open;
        }

        public override void Stop(ZoneType zone)
        {
            bool open = UnityEngine.Random.value > .5f;
            foreach (Door door in doors)
            {
                door.IsOpen = open;
                door.ChangeLock(DoorLockType.None);
            }


            HashSetPool<Door>.Shared.Return(doors);

        }
    }
}
