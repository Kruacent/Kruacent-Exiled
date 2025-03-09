using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using MEC;
using System.Collections.Generic;
using System.Linq;

namespace KE.BlackoutNDoor.API.Features.RoundEffects
{
    public class DoorStuck : RoundEffect
    {
        public DoorStuck()
        {
            base.EventTranslation = "Door system malfunction ";
            base.Chances = new Dictionary<ZoneType, int>()
            {
                { ZoneType.LightContainment, 20 },
                { ZoneType.HeavyContainment, 10 },
                { ZoneType.Entrance, 70 },
            };
        }


        public override void Effect(ZoneType zone)
        {
            bool openAfter = UnityEngine.Random.value < .5f;
            if (zone == ZoneType.LightContainment && Map.DecontaminationState >= DecontaminationState.Countdown) return;
            foreach(Door door in Door.List.Where(d => !d.IsElevator && d.Zone == zone))
            {
                if (!door.AllowsScp106 && Generator.List.Where(g => g.IsEngaged).ToList().Count != 3) continue;
                door.IsOpen = false;
                door.ChangeLock(DoorLockType.Lockdown2176);
                Timing.CallDelayed(MainPlugin.Instance.Config.DurationMalfunction / 2, () => door.IsOpen = true);
                Timing.CallDelayed(MainPlugin.Instance.Config.DurationMalfunction, () => door.Unlock());
                door.IsOpen = openAfter;
            }

        }

        public override void StopEffect(ZoneType zone)
        {
            
        }

    }
}
