using Exiled.API.Enums;
using Exiled.API.Features.Doors;
using Interactables.Interobjects.DoorUtils;
using KE.Map.Others.BlackoutNDoor.Handlers;
using KE.Utils.API.Map;
using System.Collections.Generic;
using System.Linq;

namespace KE.Map.Others.BlackoutNDoor
{
    
    public class DoorStuck : MapEvent
    {

        private static HashSet<Door> doors = new();

        public override string Cassie => MainPlugin.Translations.Doorstuck;

        public override string CassieTranslated => MainPlugin.Translations.DoorstuckTranslation;
        public override float Duration => 15;
        public override void Start(ZoneType zone)
        {
            bool open = UnityEngine.Random.value > .5f;
            foreach (Door door in Door.List.Where(d => d.Zone == zone && !d.IsElevator && d.Type != DoorType.Scp079First && d.Type != DoorType.Scp079Second))
            {

                if (door.DoorLockType == DoorLockType.None)
                {
                    doors.Add(door);
                    LockDoor(door, open);
                }
            }

            if(StructureSpawner.AdditionalDoors.TryGetValue(zone,out var addDoors))
            {
                foreach(DoorVariant doorVariant in addDoors)
                {
                    Door door2 = Door.Get(doorVariant);
                    if (door2.DoorLockType == DoorLockType.None)
                    {
                        doors.Add(door2);
                        LockDoor(door2, open);
                    }
                       
                }
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
        }
    }
}
