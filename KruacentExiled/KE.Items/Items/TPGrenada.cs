using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Map;
using KE.Items.API.Core.Models;
using KE.Items.API.Features;
using KE.Items.API.Interface;
using KE.Items.Items.ItemEffects;
using KE.Items.Items.PickupModels;

namespace KE.Items.Items
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
        public UnityEngine.Color Color { get; set; } = UnityEngine.Color.cyan;
        public CustomItemEffect Effect { get; set; }
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 3,
            DynamicSpawnPoints = new List<DynamicSpawnPoint>
            {
                new DynamicSpawnPoint()
                {
                    Chance = 50,
                    Location = SpawnLocationType.InsideHczArmory,
                },
                new DynamicSpawnPoint()
                {
                    Chance =2,
                    Location = SpawnLocationType.Inside914,
                },
                new DynamicSpawnPoint()
                {
                    Chance=50,
                    Location = SpawnLocationType.Inside049Armory,
                },
                new DynamicSpawnPoint()
                {
                    Chance=50,
                    Location = SpawnLocationType.InsideLczArmory,
                }
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
