using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using KE.Items.API.Features;

namespace KE.Items.Items
{
    public class ImpactFlash : KECustomGrenade
    {
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "Impact Flash",
                    [TranslationKeyDesc] = "The name is self-explanatory",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "Impact Flash",
                    [TranslationKeyDesc] = "Une flashbang qui explose à l'impacte",
                },
            };
        }
        public override ItemType ItemType => ItemType.GrenadeFlash;
        public override string Name { get; set; } = "ImpactFlash";
        public override string Description { get; set; } = "The grenade explode at impact";
        public override float Weight { get; set; } = 0.65f;
        public override float FuseTime => 3f;
        public override bool ExplodeOnCollision => true;
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
