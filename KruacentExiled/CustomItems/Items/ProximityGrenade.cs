using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Spawn;
using Exiled.Events.EventArgs.Map;
using KruacentExiled.CustomItems.API.Features;
using KruacentExiled.CustomItems.API.Interface;
using KruacentExiled.CustomItems.Items.ItemEffects;

namespace KruacentExiled.CustomItems.Items
{
    public class ProximityGrenade : KECustomGrenade, ISwitchableEffect, IViolentItem
    {

        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Proximity Grenade",
                    [TranslationKeyDesc] = "Show lines to all players around 3 rooms",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Grenade de proximité",
                    [TranslationKeyDesc] = "Montre tous les joueurs dans un rayon de 3 salles",
                },
            };
        }

        public bool IsViolent => false;
        public override ItemType ItemType => ItemType.GrenadeFlash;
        public override string Name { get; set; } = "ProximityGrenade";
        public override float Weight { get; set; } = 0.65f;
        public override float FuseTime => 3f;
        public override bool ExplodeOnCollision => false;
        public UnityEngine.Color Color { get; set; } = UnityEngine.Color.red;
        public CustomItemEffect Effect { get; set; }
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 1,
            RoomSpawnPoints = new List<RoomSpawnPoint>
            {
                new RoomSpawnPoint()
                {
                    Room = RoomType.HczElevatorA,
                    Offset = new UnityEngine.Vector3(1f, 0f, 1f),
                    Chance = 50
                }
            },

        };

        public ProximityGrenade()
        {
            Effect = new ProximityGrenadeEffect();
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