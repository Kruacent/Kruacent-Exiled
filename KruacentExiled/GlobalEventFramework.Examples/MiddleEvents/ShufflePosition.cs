using Exiled.API.Features;
using KruacentExiled.GlobalEventFramework.GEFE.API.Features;
using KruacentExiled.GlobalEventFramework.GEFE.API.Interfaces;
using MEC;
using NorthwoodLib.Pools;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KruacentExiled.GlobalEventFramework.Examples.MiddleEvents
{
    public class ShufflePosition : MiddleEvent, IStart
    {
        //shuffle de position une fois quand il est activé
        ///<inheritdoc/>
        public override uint Id { get; set; } = 10045;
        ///<inheritdoc/>
        public override string Name { get; set; } = "MShuffleP";
        ///<inheritdoc/>
        public override string Description { get; set; } = "Aller on change de place";
        ///<inheritdoc/>
        public override int WeightedChance { get; set; } = 1;

        public void Start()
        {




            List<Player> players = Player.Enumerable.ToList();
            List<Vector3> positions = ListPool<Vector3>.Shared.Rent(players.Count);
            if(players.Count > 1)
            {
                Vector3 tmp = positions[0];
                for (int i = 0; i < positions.Count - 1; i++)
                {
                    positions[i] = players[i + 1].Position;
                }

                positions[positions.Count - 1] = tmp;

                for (int i = 0; i < players.Count - 1; i++)
                {
                    players[i].Teleport(positions[i]);
                }
            }
        }


    }
}
