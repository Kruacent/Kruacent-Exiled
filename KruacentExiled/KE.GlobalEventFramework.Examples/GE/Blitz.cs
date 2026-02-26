using Exiled.API.Features;
using Exiled.API.Features.Items;
using KE.GlobalEventFramework.GEFE.API.Enums;
using KE.GlobalEventFramework.GEFE.API.Features;
using KE.GlobalEventFramework.GEFE.API.Interfaces;
using MEC;
using System.Collections.Generic;

namespace KE.GlobalEventFramework.Examples.GE
{
    /// <summary>
    /// Spawn fused grenades in random rooms in the map 
    /// </summary>
    public class Blitz : GlobalEvent, IStart
    {
        ///<inheritdoc/>
        public override uint Id { get; set; } = 1046;
        ///<inheritdoc/>
        public override string Name { get; set; } = "Blitz";
        ///<inheritdoc/>
        public override string Description { get; } = "La Luftwaffe arrive!";
        public override string[] AltDescription =>
        [
            "Attention aux bombardements!"
        ];
        ///<inheritdoc/>
        public override int WeightedChance => 4;
        /// <summary>
        /// The cooldown between 2 spawn of grenades
        /// </summary>
        public int Cooldown { get; set; } = 120;
        /// <summary>
        /// The number of grenades in each spawn
        /// </summary>
        public int NbGrenadeSpawned { get; set; } = 5;

        public override ImpactLevel ImpactLevel => ImpactLevel.Low;


        ///<inheritdoc/>
        public IEnumerator<float> Start()
        {
            while (!Round.IsEnded)
            {
                Log.Debug("waiting");
                yield return Timing.WaitForSeconds(Cooldown);
                for (int i = 0; i < NbGrenadeSpawned; i++)
                {
                    ((ExplosiveGrenade)Item.Create(ItemType.GrenadeHE)).SpawnActive(Room.Random().Position);
                }
                

            }
        }

    }
}
