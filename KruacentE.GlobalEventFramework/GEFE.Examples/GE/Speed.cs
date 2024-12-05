using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using GEFExiled.GEFE.API.Features;
using MEC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Utils.NonAllocLINQ;
using PlayerHandler = Exiled.Events.Handlers.Player;

namespace GEFExiled.GEFE.Examples.GE
{
    public class Speed : GlobalEvent
    {
        public override int Id { get; set; } = 30;
        public override string Name { get; set; } = "Speed";
        public override string Description { get; set; } = "Gas! gas! gas!";
        public override double Weight { get; set; } = 1;
        [Description("the movement speed that will be added to the player")]
        public byte MovementBoost { get; set; } = 100;

        public override IEnumerator<float> Start()
        {
            Player.List.ToList().ForEach(p => p.EnableEffect<MovementBoost>(MovementBoost,999999999, true));
            yield return 0;
        }
        public override void SubscribeEvent()
        {
            PlayerHandler.ChangingRole += ReactivateEffectSpawn;
        }

        public override void UnsubscribeEvent()
        {
            PlayerHandler.ChangingRole -= ReactivateEffectSpawn;
            Player.List.ToList().ForEach(p => p.DisableEffect<MovementBoost>());
        }


        private void ReactivateEffectSpawn(ChangingRoleEventArgs ev)
        {
            Timing.CallDelayed(1f, () => ev.Player.EnableEffect<MovementBoost>(MovementBoost, 999999999, true));
            
        }
    }
}
