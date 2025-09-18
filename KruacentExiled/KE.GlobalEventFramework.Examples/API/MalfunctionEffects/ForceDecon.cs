

using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using KE.GlobalEventFramework.Examples.API.Feature;
using MEC;
using System.Linq;

namespace KE.GlobalEventFramework.Examples.API.MalfunctionEffects
{
    internal class ForceDecon : MalfunctionEffect
    {
        public override string Name { get; } = "Forced Decontamination";
        public override string VoiceLine { get; } = "Malfunctions levels above . 25 percent . . decontamination of Light Containment Zone in . 30 seconds";
        public override string VoiceLineTranslated { get; } = "Malfunctions levels above 25%, decontamination of Light Containment Zone in 30 seconds";
        public override sbyte MalfunctionActivation { get; } = 25;

        public override void ActivateEffect()
        {
            if (Map.IsLczDecontaminated || !Map.IsLczDecontaminated) return;
            Door.List.ToList().ForEach(d =>
            {
                if (d.Zone == ZoneType.LightContainment)
                {
                    if (!d.IsElevator)
                    {
                        d.ChangeLock(DoorLockType.DecontEvacuate);
                        d.IsOpen = true;
                    }

                }
            });
            Timing.CallDelayed(30 + Cassie.CalculateDuration(VoiceLine), () =>
            {
                Map.StartDecontamination();

                foreach (Door d in Door.List)
                {
                    if (d.Type == DoorType.ElevatorLczA || d.Type == DoorType.ElevatorLczB)
                    {
                        d.Lock(DoorLockType.DecontEvacuate);
                        d.IsOpen = false;
                    }
                }
            });
        }

    }
}
