using Exiled.API.Features;
using GlobalEventFramework.API.Features;
using GlobalEventFramework.API.Interfaces;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KruacentExiled.GlobalEvents.MiddleEvents
{
    /// <summary>
    /// shuffle the position of all player when activated
    /// </summary>
    public class ShufflePosition : MiddleEvent, IStart
    {
        
        ///<inheritdoc/>
        public override string Name { get; set; } = "MShuffleP";
        ///<inheritdoc/>
        public override string Description { get; set; } = "Aller on change de place";
        ///<inheritdoc/>
        public override int WeightedChance { get; set; } = 1;

        public void Start()
        {
            List<Player> players = Player.Enumerable.Where(p => p.IsAlive).ToList();
            Vector3[] positions = new Vector3[players.Count];

            for(int i = 0; i < players.Count; i++)
            {
                positions[i] = players[i].Position;
            }

            if (players.Count > 1)
            {
                Vector3 tmp = positions[0];
                for (int i = 0; i < positions.Length - 1; i++)
                {
                    positions[i] = players[i + 1].Position;
                }

                positions[positions.Length - 1] = tmp;

                for (int i = 0; i < players.Count; i++)
                {
                    players[i].Teleport(positions[i]);
                }
            }
        }
    }
}
