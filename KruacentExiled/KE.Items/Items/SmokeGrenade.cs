using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Map;
using KE.Items.API.Features;
using KE.Items.API.Interface;
using KE.Items.ItemEffects;

namespace KE.Items.Items
{
    public class SmokeGrenade : KECustomGrenade, ISwichableEffect
    {
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "Smoke Grenade",
                    [TranslationKeyDesc] = "We finally put your grandma inside this thing ! Don't throw it or she will get out !",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "Fumigène",
                    [TranslationKeyDesc] = "Fait beaucoup de fumée",
                },
            };
        }
        public override ItemType ItemType => ItemType.GrenadeFlash;
        public override string Name { get; set; } = "Smoke Grenade";
        public override string Description { get; set; } = "";
        public override float Weight { get; set; } = 0.65f;
        public override float FuseTime => 3f;
        public override bool ExplodeOnCollision => false;
        public UnityEngine.Color Color { get; set; } = UnityEngine.Color.black;
        public CustomItemEffect Effect { get; set; }
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 2,
            RoomSpawnPoints = new List<RoomSpawnPoint>
            {
                new RoomSpawnPoint()
                {
                    Room = RoomType.HczStraightPipeRoom,
                    Chance = 35
                },

                new RoomSpawnPoint()
                {
                    Room = RoomType.Surface,
                    Chance = 35
                }
            },
        };

        public SmokeGrenade()
        {
            Effect = new SmokeGrenadeEffect();
        }

        protected override void OnExplodingGrenade(ExplodingGrenadeEventArgs ev)
        {
            Effect.Effect(ev);
            ev.TargetsToAffect.Clear();
        }

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();
        }
    }
}