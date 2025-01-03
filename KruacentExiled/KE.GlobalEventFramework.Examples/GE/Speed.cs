using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp173;
using KE.GlobalEventFramework.GEFE.API.Features;
using MEC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Utils.NonAllocLINQ;
using PlayerHandler = Exiled.Events.Handlers.Player;

namespace KE.GlobalEventFramework.Examples.GE
{
    /// <summary>
    /// Everyone has a movement boost effect (stackable)
    /// Maybe inspired by Dr Bright's Mayhem
    /// </summary>
    public class Speed : GlobalEvent
    {
        ///<inheritdoc/>
        public override uint Id { get; set; } = 1042;
        ///<inheritdoc/>
        public override string Name { get; set; } = "Speed";
        ///<inheritdoc/>
        public override string Description { get; set; } = "Gas! gas! gas!";
        ///<inheritdoc/>
        public override int Weight { get; set; } = 1;
        /// <summary>
        /// intensity of the movement boost effect
        /// </summary>
        public byte MovementBoost { get; set; } = 100;
        ///<inheritdoc/>
        public override IEnumerator<float> Start()
        {
            yield return Timing.WaitForSeconds(1);
            Player.List.ToList().ForEach(p => p.EnableEffect<MovementBoost>(MovementBoost,999999999, true));
            
        }
        ///<inheritdoc/>
        public override void SubscribeEvent()
        {
            PlayerHandler.ChangingRole += ReactivateEffectSpawn;
            Exiled.Events.Handlers.Scp173.Blinking += SpeedyNut;
        }
        ///<inheritdoc/>
        public override void UnsubscribeEvent()
        {
            PlayerHandler.ChangingRole -= ReactivateEffectSpawn;
            Exiled.Events.Handlers.Scp173.Blinking -= SpeedyNut;
            Player.List.ToList().ForEach(p => p.DisableEffect<MovementBoost>());
        }
        /// <summary>
        /// Decrease the blink cooldown of SCP-173
        /// </summary>
        private void SpeedyNut(BlinkingEventArgs ev)
        {
            ev.BlinkCooldown = ev.BlinkCooldown/2;
        }

        /// <summary>
        /// Reactivate the effect at each role changement
        /// </summary>
        private void ReactivateEffectSpawn(ChangingRoleEventArgs ev)
        {
            Timing.CallDelayed(1f, () => ev.Player.EnableEffect<MovementBoost>(MovementBoost, 999999999, true));
            
        }
    }
}
