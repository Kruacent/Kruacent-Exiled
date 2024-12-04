using CustomPlayerEffects;
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
            Player.List.ToList().ForEach(p => p.EnableEffect<MovementBoost>(100));
            yield return 0;
        }


        public override void SubscribeEvent()
        {
            
        }

        public override void UnsubscribeEvent()
        {
            
        }

    }
}
