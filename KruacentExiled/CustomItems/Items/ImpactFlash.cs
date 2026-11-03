using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using KruacentExiled.CustomItems.API.Features;
using KruacentExiled.CustomItems.API.Interface;

namespace KruacentExiled.CustomItems.Items
{
    public class ImpactFlash : KECustomGrenade, IViolentItem
    {
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Impact Flash",
                    [TranslationKeyDesc] = "The name is self-explanatory",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Impact Flash",
                    [TranslationKeyDesc] = "Une flashbang qui explose à l'impacte",
                },
            };
        }
        public override ItemType ItemType => ItemType.GrenadeFlash;
        public override string Name { get; set; } = "ImpactFlash";
        public override float Weight { get; set; } = 0.65f;
        public override float FuseTime => 3f;
        public override bool ExplodeOnCollision => true;
        public bool IsViolent => false;
        public override SpawnProperties SpawnProperties { get; set; } = new KESpawnProperties()
        {
            Limit = 5,
            RoomSpawnPoints = new List<RoomSpawnPoint>
            {
                new RoomSpawnPoint()
                {
                    Room = RoomType.HczArmory,
                    Offset = new UnityEngine.Vector3(1f, 0f, 1f),
                    Chance = 50
                },

                new RoomSpawnPoint()
                {
                    Room = RoomType.Lcz914,
                    Offset = new UnityEngine.Vector3(1f, 0f, 1f),
                    Chance = 2
                },

                new RoomSpawnPoint()
                {
                    Room = RoomType.HczArmory,
                    Offset = new UnityEngine.Vector3(1f, 0f, 1f),
                    Chance = 50
                },

                new RoomSpawnPoint()
                {
                    Room = RoomType.Hcz049,
                    Offset = new UnityEngine.Vector3(1f, 0f, 1f),
                    Chance = 50
                },

                new RoomSpawnPoint()
                {
                    Room = RoomType.LczArmory,
                    Offset = new UnityEngine.Vector3(1f, 0f, 1f),
                    Chance = 50
                },
            },
        };
    }
}
