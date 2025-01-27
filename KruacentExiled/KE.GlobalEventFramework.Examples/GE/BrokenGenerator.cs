using Player = Exiled.API.Features.Player;
using System.Collections.Generic;
using KE.GlobalEventFramework.GEFE.API.Features;
using KE.GlobalEventFramework.GEFE.API.Interfaces;
using MEC;
using Exiled.API.Features;
using Exiled.API.Enums;
using Exiled.API.Features.Doors;
using Exiled.Events.EventArgs.Map;
using System.Linq;
using System.Drawing.Imaging;

namespace KE.GlobalEventFramework.Examples.GE
{
    public class BrokenGenerator : GlobalEvent, IStart
    {
        public override uint Id { get; set; } = 1050;
        public override string Name { get; set; } = "Broken Generator";
        public override string Description { get; set; } = "Repair the generator to be able to see !";
        public override int Weight { get; set; } = 1;

        public List<ZoneType> zones = new List<ZoneType>
                {
                    ZoneType.LightContainment,
                    ZoneType.HeavyContainment,
                    ZoneType.Entrance,
                    ZoneType.Surface,
                    ZoneType.Pocket
                };

        public IEnumerator<float> Start()
        {
            zones.ForEach(zone => Map.TurnOffAllLights(99999999, zone));

            foreach (Player player in Player.List)
            {
                if (player.IsHuman)
                {
                    player.AddItem(ItemType.Flashlight);
                }
            }

            yield return 0;
        }

        public void SubscribeEvent()
        {
            Exiled.Events.Handlers.Map.GeneratorActivating += GenActivate;
        }

        public void UnsubscribeEvent()
        {
            Exiled.Events.Handlers.Map.GeneratorActivating -= GenActivate;
        }

        public void GenActivate(GeneratorActivatingEventArgs ev)
        {
            if (Generator.List.Where(g => g.IsEngaged).Count() == 3)
            {
                Map.TurnOnAllLights(zones);
            }
        }

    }
}