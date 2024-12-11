using Exiled.API.Features;
using Exiled.API.Features.Items;
using GEFExiled.GEFE.API.Features;
using InventorySystem.Items.ThrowableProjectiles;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.KruacentE.GlobalEventFramework.GEFE.Examples.GE
{
    public class R : GlobalEvent
    {
        public override int Id { get; set; } = 1;
        public override string Name { get; set; } = "Blitz";
        public override string Description { get; set; } = "éteignez les lumières la luftwaffe arrive";
        public override double Weight { get; set; } = 1;
        public int Cooldown { get; set; } = 120;
        public override IEnumerator<float> Start()
        {
            while (!Round.IsEnded)
            {
                yield return Timing.WaitForOneFrame;
                ((ExplosiveGrenade)Item.Create(ItemType.GrenadeHE)).SpawnActive(Room.Random().Position); 

            }
        }

    }
}
