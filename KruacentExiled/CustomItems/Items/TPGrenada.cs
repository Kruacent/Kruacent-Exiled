using Exiled.API.Enums;
using Exiled.API.Features.Spawn;
using Exiled.Events.EventArgs.Map;
using KruacentExiled.CustomItems.API.Core.Models;
using KruacentExiled.CustomItems.API.Enum;
using KruacentExiled.CustomItems.API.Features;
using KruacentExiled.CustomItems.API.Interface;
using KruacentExiled.CustomItems.Items.ItemEffects;
using KruacentExiled.CustomItems.Items.PickupModels;
using System.Collections.Generic;

namespace KruacentExiled.CustomItems.Items
{
    public class TPGrenada : KECustomGrenade, ISwitchableEffect, ICustomPickupModel,IViolentItem
    {

        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Teleportation Grenade",
                    [TranslationKeyDesc] = "This grenade does 0 damage but teleport nearby players in a random place (does work in other dimension ;3 )",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Grenade de Téléportation",
                    [TranslationKeyDesc] = "Ne fait pas de dégât mais téléporte dans une pièce aléatoire",
                },
            };
        }
        public bool IsViolent => false;
        public override ItemType ItemType => ItemType.GrenadeHE;
        public override string Name { get; set; } = "Teleportation Grenade";
        public override float Weight { get; set; } = 0.65f;
        public override float FuseTime => 3f;
        public override bool ExplodeOnCollision => false;
        public override ItemRank Rank { get; } = ItemRank.Epic;
        public UnityEngine.Color Color { get; set; } = UnityEngine.Color.cyan;
        public CustomItemEffect Effect { get; set; }
        public override SpawnProperties SpawnProperties { get; set; } = new KESpawnProperties()
        {
            Limit = 3,
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

        public PickupModel PickupModel { get; }

        public TPGrenada()
        {
            Effect = new TPGrenadaEffect();
            PickupModel = new TPGrenadaPModel(this);
        }

        protected override void OnExplodingGrenade(ExplodingGrenadeEventArgs ev)
        {
            Effect.Effect(ev);
            ev.TargetsToAffect.Clear();
        }

        protected override void SubscribeEvents()
        {
            PickupModel.SubscribeEvents();
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            PickupModel.UnsubscribeEvents();
            base.UnsubscribeEvents();
        }

    }

}
