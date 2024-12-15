using Exiled.API.Features.Doors;
using GEFExiled.GEFE.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Map;

namespace GEFExiled.GEFE.Examples.GE
{
    public class OpenBar : GlobalEvent
    {
        public override int Id { get; set; } = 38;
        public override string Name { get; set; } = "OpenBar";
        public override string Description { get; set; } = "j'espère que vous avez pas prévu de kampé";
        public override double Weight { get; set; } = 1;
        public override IEnumerator<float> Start()
        {
            var doors = Door.List.Where(d => new[] { DoorType.GateA, DoorType.GateB, DoorType.HczArmory, DoorType.HID,DoorType.Intercom,DoorType.Scp049Armory,DoorType.Scp096,DoorType.Scp106Primary,DoorType.Scp106Secondary,DoorType.Scp330,DoorType.Scp330Chamber,DoorType.Scp914Gate }.Contains(d.Type)).ToList();
            UnlockAndOpen(doors);

            yield return 0;
        }

        private void UnlockAndOpen(List<Door> doors)
        {
            doors.ForEach(d =>
            {
                d.IsOpen = true;
                d.ChangeLock(DoorLockType.Isolation);
            });
        }

        public override void SubscribeEvent()
        {
            Exiled.Events.Handlers.Map.GeneratorActivating += GenActivate;
        }

        public override void UnsubscribeEvent()
        {
            Exiled.Events.Handlers.Map.GeneratorActivating -= GenActivate;
        }

        public void GenActivate(GeneratorActivatingEventArgs ev)
        {
            if (Generator.List.Where(g => g.IsEngaged).Count() == 3)
            {
                UnlockAndOpen(Door.List.Where(d => new[] { DoorType.Scp079First, DoorType.Scp079Second }.Contains(d.Type)).ToList());
            }
        }

    }
}
