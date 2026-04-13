using Exiled.API.Features;
using Exiled.API.Features.Items;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.GlobalEventFramework.Examples.API.Feature.CassieGoCrazy
{
    // Sad Cassie scenario.
    public class SadCassie : ICGCEffect
    {
        public void Effect()
        {

            //Cassie.Message("I wanna just be useful", true, true, true);
            Exiled.API.Features.Map.TurnOffAllLights(2, Exiled.API.Enums.ZoneType.HeavyContainment);
            Exiled.API.Features.Map.TurnOffAllLights(2, Exiled.API.Enums.ZoneType.Entrance);


            if (!Warhead.IsDetonated)
                Warhead.Start();

            for (int i = 0; i < 10; i++)
            {
                ((ExplosiveGrenade)Item.Create(ItemType.GrenadeHE)).SpawnActive(Room.Random().Position);
            }

            Timing.CallDelayed(10, delegate
            {
                Warhead.Stop();
            });

        }
    }
}
