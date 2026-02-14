using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Player;
using KE.GlobalEventFramework.Examples.API.Feature.CassieGoCrazy;
using KE.GlobalEventFramework.GEFE.API.Enums;
using KE.GlobalEventFramework.GEFE.API.Features;
using KE.GlobalEventFramework.GEFE.API.Features.Hints;
using KE.GlobalEventFramework.GEFE.API.Interfaces;
using KE.Utils.API.Displays.DisplayMeow;
using KE.Utils.API.Interfaces;
using MEC;
using System.Collections.Generic;
using System.Linq;

namespace KE.GlobalEventFramework.Examples.GE
{
    /// <summary>
    /// Spawn fused grenades in random rooms in the map 
    /// </summary>
    /*public class CassieGoCrazy : GlobalEvent, IStart
    {
        ///<inheritdoc/>
        public override uint Id { get; set; } = 1049;
        ///<inheritdoc/>
        public override string Name { get; set; } = "Cassie Go Crazy";
        ///<inheritdoc/>
        public override string Description => "Crazy Cassie !";
        ///<inheritdoc/>
        public override int WeightedChance { get; set; } = 1;

        /// <summary>
        /// The cooldown between 2 cassie event
        /// </summary>
        public int Cooldown { get; set; } = 380;



        public override ImpactLevel ImpactLevel => ImpactLevel.Low;

        private List<ICGCEffect> Effects;

        protected override void SubscribeEvents()
        {
            Effects = new()
            {
                new AntagonisticCassie(),
                new ChangeColor(),
                new NoLight(),
                new SadCassie(),
                new WarheadEffect()
            };

            foreach (ICGCEffect effect in Effects)
            {
                if (effect is IUsingEvents @event)
                {
                    @event.SubscribeEvents();
                }
            }

        }


        protected override void UnsubscribeEvents()
        {
            foreach (ICGCEffect effect in Effects)
            {
                if (effect is IUsingEvents @event)
                {
                    @event.UnsubscribeEvents();
                }
            }
        }



        /// <summary>
        /// Starts a coroutine to perform random actions during the game.
        /// </summary>
        /// <returns>A coroutine that runs until the game round ends.</returns>
        /// <inheritdoc />
        public IEnumerator<float> Start()
        {
            while (!Round.IsEnded)
            {
                Log.Debug("waiting");
                yield return Timing.WaitForSeconds(Cooldown);

                Effects.GetRandomValue().Effect();


            }
        }

    }*/
}
