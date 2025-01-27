using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;

namespace KE.Items.Items
{
    [CustomItem(ItemType.GrenadeFlash)]
    public class ImpactFlash : CustomGrenade
    {
        public override uint Id { get; set; } = 1412;
        public override string Name { get; set; } = "Impact Flash";
        public override string Description { get; set; } = "The grenade explode at impact";
        public override float Weight { get; set; } = 0.65f;
        public override float FuseTime { get; set; } = 3f;
        public override bool ExplodeOnCollision { get; set; } = true;
        public float DamageModifier { get; set; } = 1f;
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 5,
            DynamicSpawnPoints = new List<DynamicSpawnPoint>
        {
            new DynamicSpawnPoint()
            {
                Chance = 50,
                Location = SpawnLocationType.InsideHczArmory,
            },
            new DynamicSpawnPoint()
            {
                Chance = 2,
                Location = SpawnLocationType.Inside914,
            },
            new DynamicSpawnPoint()
            {
                Chance= 50,
                Location = SpawnLocationType.Inside049Armory,
            },
            new DynamicSpawnPoint()
            {
                Chance= 50,
                Location = SpawnLocationType.InsideLczArmory,
            }
        },

        };
    }
}
