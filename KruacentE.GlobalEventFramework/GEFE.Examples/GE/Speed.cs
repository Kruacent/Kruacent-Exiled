using Exiled.API.Enums;
using Exiled.API.Features;
using GEFExiled.GEFE.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEFExiled.GEFE.Examples.GE
{
    public class Speed : GlobalEvent
    {
        public override int Id { get; set; } = 30;
        public override string Name { get; set; } = "Speed";
        public override string Description { get; set; } = "Gas! gas! gas!";
        public override double Weight { get; set; } = 1;

        public override IEnumerator<float> Start()
        {

            yield return 0;
        }


        public override void SubscribeEvent()
        {
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
        }

        public override void UnsubscribeEvent()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
        }

        private void OnRoundStarted()
        {
            foreach(Player p in Player.List)
            {
                p.EnableEffect(EffectType.MovementBoost,100,99999999f);
            }
        }

    }
}
