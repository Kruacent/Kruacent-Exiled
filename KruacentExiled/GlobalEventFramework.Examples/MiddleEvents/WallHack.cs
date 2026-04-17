using CustomPlayerEffects;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp173;
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
    public class WallHackM : MiddleEvent, IAsyncStart, IEvent
    {
        ///<inheritdoc/>
        public override uint Id { get; set; } = 10052;
        ///<inheritdoc/>
        public override string Name { get; set; } = "WallHack";
        ///<inheritdoc/>
        public override string Description { get; set; } = "Tout le monde wallhack yippee!!!!!";
        ///<inheritdoc/>
        public override int WeightedChance { get; set; } = 1;

        ///<inheritdoc/>
        public IEnumerator<float> Start()
        {
            yield return Timing.WaitForSeconds(.1f);
            Player.List.ToList().ForEach(p => p.EnableEffect<Scp1344>(999999999, true));

        }
        ///<inheritdoc/>
        public void SubscribeEvent()
        {
            Log.Debug("subscribe events wh");
            Exiled.Events.Handlers.Player.ChangingRole += ReactivateEffectSpawn;
        }
        ///<inheritdoc/>
        public void UnsubscribeEvent()
        {
            Exiled.Events.Handlers.Player.ChangingRole -= ReactivateEffectSpawn;

        }

        protected override void Disable(KEEvents ev)
        {
            OnDisable();
        }

        public void OnDisable()
        {
            Player.List.ToList().ForEach(p => p.DisableEffect<Scp1344>());
        }


        /// <summary>
        /// Reactivate the effect at each role changement
        /// </summary>
        private void ReactivateEffectSpawn(ChangingRoleEventArgs ev)
        {
            Timing.CallDelayed(.1f, () => ev.Player.EnableEffect<Scp1344>( 999999999, true));

        }
    }
}
