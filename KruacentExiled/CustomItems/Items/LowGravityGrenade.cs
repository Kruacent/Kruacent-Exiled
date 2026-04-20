using System;
using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Spawn;
using Exiled.Events.EventArgs.Map;
using KruacentExiled.CustomItems.API.Features;
using KruacentExiled.CustomItems.API.Interface;
using KruacentExiled.CustomItems.Items.ItemEffects;
using LabApi.Events.Arguments.PlayerEvents;

namespace KruacentExiled.CustomItems.Items
{
    public class LowGravityGrenade : KECustomGrenade, ISwitchableEffect, IViolentItem
    {
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Low Gravity Grenade",
                    [TranslationKeyDesc] = "You always wanna be on the moon, if the answer is yes this grenade will grant your wishes!",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Grenade basse gravité",
                    [TranslationKeyDesc] = "Pour aller attraper les étoiles",
                },
            };
        }
        public override ItemType ItemType => ItemType.GrenadeHE;
        public override string Name { get; set; } = "Low Gravity Grenade";
        public override float Weight { get; set; } = 0.65f;
        public bool IsViolent => false;
        public override float FuseTime => 3f;
        public override bool ExplodeOnCollision => false;
        public UnityEngine.Color Color { get; set; } = UnityEngine.Color.gray;
        public CustomItemEffect Effect { get; set; }
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 2,
            RoomSpawnPoints = new List<RoomSpawnPoint>
            {
                new RoomSpawnPoint()
                {
                    Room = RoomType.Hcz127,
                    Chance = 25
                },

                new RoomSpawnPoint()
                {
                    Room = RoomType.Hcz939,
                    Chance = 25
                },
            },

            DynamicSpawnPoints = new List<DynamicSpawnPoint>
            {
                new DynamicSpawnPoint()
                {
                    Chance = 25,
                    Location = SpawnLocationType.InsideHidLab,
                },
            },

        };

        public LowGravityGrenade()
        {
            Effect = new LowGravityGrenadeEffect();
        }

        protected override void OnExplodingGrenade(ExplodingGrenadeEventArgs ev)
        {
            Effect.Effect(ev);
            ev.TargetsToAffect.Clear();
        }


        protected override void SubscribeEvents()
        {
            LabApi.Events.Handlers.PlayerEvents.ChangedRole += OnChangedRole;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            LabApi.Events.Handlers.PlayerEvents.ChangedRole -= OnChangedRole;
            base.UnsubscribeEvents();
        }

        private void OnChangedRole(PlayerChangedRoleEventArgs ev)
        {
            Player player = ev.Player;
            if (!LowGravityGrenadeEffect.affectedPlayers.TryGetValue(player,out var time)) return;

            if (time.Add(TimeSpan.FromSeconds(LowGravityGrenadeEffect.Duration)) > DateTime.UtcNow)
            {
                LowGravityGrenadeEffect.ResetGravity(player);
            }

            

        }
    }
}