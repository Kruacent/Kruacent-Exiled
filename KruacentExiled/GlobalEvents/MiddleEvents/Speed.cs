using CustomPlayerEffects;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp173;
using GlobalEventFramework.API.Features;
using GlobalEventFramework.API.Interfaces;
using KruacentExiled.GlobalEvents.GE;
using MEC;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KruacentExiled.GlobalEvents.MiddleEvents
{
    public class SpeedM : MiddleEvent, IAsyncStart, IEvent
    {
        ///<inheritdoc/>
        public override string Name { get; set; } = "SpeedM";
        ///<inheritdoc/>
        public override string Description { get; set; } = "Gas! gas! gas!";
        ///<inheritdoc/>
        public override int DefaultWeightedChance { get; } = 1;
        public override string[] IncompatibleEvents => new string[] { "Speed" };
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
            Exiled.Events.Handlers.Scp173.Blinking += Speed.SpeedyNut;
        }
        ///<inheritdoc/>
        public void UnsubscribeEvent()
        {
            Exiled.Events.Handlers.Player.ChangingRole -= ReactivateEffectSpawn;
            Exiled.Events.Handlers.Scp173.Blinking -= Speed.SpeedyNut;
        }

        protected override void Disable()
        {
            OnDisable();
            base.Disable();
        }

        public void OnDisable()
        {
            Player.List.ToList().ForEach(p => p.DisableEffect<MovementBoost>());
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
