using Exiled.API.Features;
using Exiled.API.Features.Items;
using KruacentE.GlobalEventFramework.GEFE.API.Features;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentE.GlobalEventFramework.Examples.GE
{
    /// <summary>
    /// Spawn fused grenades in random rooms in the map 
    /// </summary>
    public class Blitz : GlobalEvent
    {
        ///<inheritdoc/>
        public override int Id { get; set; } = 1;
        ///<inheritdoc/>
        public override string Name { get; set; } = "Blitz";
        ///<inheritdoc/>
        public override string Description { get; set; } = "éteignez les lumières la luftwaffe arrive";
        ///<inheritdoc/>
        public override int Weight { get; set; } = 1;
        /// <summary>
        /// The cooldown between 2 spawn of grenades
        /// </summary>
        public int Cooldown { get; set; } = 120;
        /// <summary>
        /// The number of grenades in each spawn
        /// </summary>
        public int NbGrenadeSpawned { get; set; } = 5;
        ///<inheritdoc/>
        public override IEnumerator<float> Start()
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
