using Exiled.API.Features;
using KE.GlobalEventFramework.GEFE.API.Features;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.GlobalEventFramework.Examples.MiddleEvents
{
    public class ShufflePosition : MiddleEvent
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

        public IEnumerator<float> Start()
        {

            List<Vector3> position = Player.Enumerable.Select(p => p.Position).ToList();

            Vector3 tmp = position[0];
            for(int i = 0; i < position.Count-1; i++)
            {
                position[i] = position[i + 1];
            }

            position[position.Count - 1] = tmp;


            yield return Timing.WaitForOneFrame;
        }


    }
}
