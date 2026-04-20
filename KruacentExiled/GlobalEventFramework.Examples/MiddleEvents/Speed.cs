using CustomPlayerEffects;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp173;
using InventorySystem.Items.Usables;
using KruacentExiled.GlobalEventFramework.GEFE.API.Features;
using KruacentExiled.GlobalEventFramework.GEFE.API.Interfaces;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.GlobalEventFramework.Examples.MiddleEvents
{
    public class SpeedM : MiddleEvent, IAsyncStart, IEvent
    {
        ///<inheritdoc/>
        public override uint Id { get; set; } = 10042;
        ///<inheritdoc/>
        public override string Name { get; set; } = "SpeedM";
        ///<inheritdoc/>
        public override string Description { get; set; } = "Gas! gas! gas!";
        ///<inheritdoc/>
        public override int WeightedChance { get; set; } = 1;
        public override uint[] IncompatibleEvents => new uint[] { 1042 };
        /// <summary>
        /// intensity of the movement boost effect
        /// </summary>
        public byte MovementBoost { get; set; } = 100;
        ///<inheritdoc/>
        public IEnumerator<float> Start()
        {
            yield return Timing.WaitForSeconds(1);
            Player.List.ToList().ForEach(p => p.EnableEffect<MovementBoost>(MovementBoost, 999999999, true));

        }
        ///<inheritdoc/>

        public void SubscribeEvent()
        {
            Exiled.Events.Handlers.Player.ChangingRole += ReactivateEffectSpawn;
            Exiled.Events.Handlers.Scp173.Blinking += SpeedyNut;
        }
        ///<inheritdoc/>
        public void UnsubscribeEvent()
        {
            Exiled.Events.Handlers.Player.ChangingRole -= ReactivateEffectSpawn;
            Exiled.Events.Handlers.Scp173.Blinking -= SpeedyNut;
        }

        protected override void Disable(KEEvents ev)
        {
            OnDisable();
            base.Disable(ev);
        }

        public void OnDisable()
        {
            Player.List.ToList().ForEach(p => p.DisableEffect<MovementBoost>());
        }

        /// <summary>
        /// Decrease the blink cooldown of SCP-173
        /// </summary>
        private void SpeedyNut(BlinkingEventArgs ev)
        {
            ev.BlinkCooldown = ev.BlinkCooldown / 4;
        }

        /// <summary>
        /// Reactivate the effect at each role changement
        /// </summary>
        private void ReactivateEffectSpawn(ChangingRoleEventArgs ev)
        {
            Timing.CallDelayed(.1f, () => ev.Player.EnableEffect<MovementBoost>(MovementBoost, 999999999, true));

        }
    }
}
