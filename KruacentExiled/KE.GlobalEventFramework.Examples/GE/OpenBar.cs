using Exiled.API.Features.Doors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Map;
using KE.GlobalEventFramework.GEFE.API.Interfaces;
using KE.GlobalEventFramework.GEFE.API.Features;
using KE.GlobalEventFramework.GEFE.API.Features.Hints;
using KE.Map.Others.BlackoutNDoor.Events.EventArgs;
using Exiled.API.Interfaces;
namespace KE.GlobalEventFramework.Examples.GE
{
    public class OpenBar : GlobalEvent, IStart, IEvent
    {
        public override uint Id { get; set; } = 1048;
        public override string Name { get; set; } = "OpenBar";
        public override string Description { get; } = "j'espère que vous avez pas prévu de kampé";
        public override int WeightedChance { get; set; } = 3;
        public override uint[] IncompatibleEvents { get; set; } = { 1 };

        public int NbAdditionalDoor = 3;

        public static readonly HashSet<DoorType> DoorsToUnlock = new()
        {
            DoorType.GateA, DoorType.GateB,
        };

        public static readonly HashSet<DoorType> DoorsToMaybeUnlock = new()
        {
            DoorType.HczArmory, DoorType.HIDChamber,DoorType.Intercom,DoorType.Scp049Armory,DoorType.Scp096,DoorType.Scp106Primary,DoorType.Scp106Secondary,DoorType.Scp330,DoorType.Scp914Gate
        };

        private IEnumerable<Door> doorsLocked;

        public void Start()
        {
            List<DoorType> door = DoorsToMaybeUnlock.ToList();
            List<DoorType> result = new();

            for (int i = 0; i < NbAdditionalDoor; i++)
            {
                result.Add(door.PullRandomItem());
            }
            result.AddRange(DoorsToUnlock);

            doorsLocked = Door.List.Where(d => result.Contains(d.Type));

            UnlockAndOpen(doorsLocked);

        }

        private void UnlockAndOpen(IEnumerable<Door> doors)
        {

            foreach(Door door in doors)
            {
                if(door is CheckpointDoor && door is IDamageableDoor damage)
                {
                    damage.Break(Interactables.Interobjects.DoorUtils.DoorDamageType.ServerCommand);
                }
                else
                {
                    UnlockAndOpen(door);
                }
            }
        }


        private void UnlockAndOpen(Door door)
        {
            door.IsOpen = true;
            door.ChangeLock(DoorLockType.NoPower);
        }

        public void SubscribeEvent()
        {
            Map.Others.BlackoutNDoor.Events.Handlers.DoorStuckHandler.DoorStucking += OnDoorStucking;
        }

        public void UnsubscribeEvent()
        {
            Map.Others.BlackoutNDoor.Events.Handlers.DoorStuckHandler.DoorStucking -= OnDoorStucking;
        }


        private void OnDoorStucking(DoorStuckEventArgs ev)
        {

            int result = ev.Doors.RemoveWhere(door => doorsLocked.Contains(door));

            Log.Info(result);
        }
    }
}
