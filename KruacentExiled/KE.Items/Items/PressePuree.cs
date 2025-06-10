
using System;
using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Scp914;
using KE.Items.Interface;
using KE.Items.Upgrade;
using Scp914;

namespace KE.Items.Items
{
    [CustomItem(ItemType.GrenadeHE)]
    public class PressePuree : KECustomGrenade, IUpgradableCustomItem
    {
        public override uint Id { get; set; } = 1046;
        public override string Name { get; set; } = "Presse Purée";
        public override string Description { get; set; } = "The grenade explode at impact but does less damage\n 5% to upgrade in 914 on very fine";
        public override float Weight { get; set; } = 0.65f;
        public override float FuseTime { get; set; } = 1.5f;
        public override bool ExplodeOnCollision { get; set; } = true;
        public float DamageModifier { get; set; } = 0.4f;
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
                    Chance = 5,
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

        public IReadOnlyDictionary<Scp914KnobSetting, UpgradeProperties> Upgrade { get; private set; } = new Dictionary<Scp914KnobSetting, UpgradeProperties>()
        {
            //very fine -> true divine pills 10%
            { Scp914KnobSetting.VeryFine,new UpgradeProperties(5, 1055)}
        };
    }
}
