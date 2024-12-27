using Exiled.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using GEFExiled.GEFE.API.Features;
using GEFExiled.GEFE.API.Interfaces;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEFExiled.GEFE.Examples.GE
{
    public class BlackoutAgain : GlobalEvent
    {
        public override int Id { get; set; } = 36;
        public override string Name { get; set; } = "BlackoutGE";
        public override string Description { get; set; } = "Sortez vos grosse lampe là";
        public override int Weight { get; set; } = 0;
        public override IEnumerator<float> Start()
        {
            Room.List.ToList().ForEach(x => {
                x.AreLightsOff = true;
            });
            Timing.CallDelayed(1f, () =>
            {
                Player.List.ToList().ForEach(p =>
                {
                    p.AddItem(ItemType.Flashlight);
                });
            });

            yield return 0;
        }


        public override void SubscribeEvent()
        {
            Exiled.Events.Handlers.Map.GeneratorActivating += Gen;
        }

        public override void UnsubscribeEvent()
        {
            Exiled.Events.Handlers.Map.GeneratorActivating -= Gen;
        }

        public void Gen(GeneratorActivatingEventArgs ev)
        {
            if(Generator.List.Where(g => g.IsEngaged).ToList().Count() == 3)
            {
                Room.List.ToList().ForEach(x => {
                    x.AreLightsOff = false;
                });
            }
        }
    }
}
