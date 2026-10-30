using Exiled.API.Enums;
using Exiled.API.Features.Spawn;
using Exiled.Events.EventArgs.Player;
using KruacentExiled.CustomItems.API.Features;
using KruacentExiled.CustomItems.API.Interface;
using System.Collections.Generic;
using UnityEngine;

namespace KruacentExiled.CustomItems.Items
{

    public class ExplosiveProofArmor : KECustomItem, IViolentItem, ILumosItem
    {

        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Explosive Proof Armor",
                    [TranslationKeyDesc] = "Explosion damage is reduced by 4 times ! Yippee !",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Armure Anti Explosion",
                    [TranslationKeyDesc] = "Tu prends 4 fois moins de dégats lors d'explosion !",
                },
            };
        }
        
        public override ItemType ItemType => ItemType.ArmorLight;
        public Color Color { get; set; } = UnityEngine.Color.red;
        public override string Name { get; set; } = "ExplosiveProofArmor";
        public override float Weight { get; set; } = 2f;
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 1,
            RoomSpawnPoints = new List<RoomSpawnPoint>
            {
                new RoomSpawnPoint()
                {
                    Room = Exiled.API.Enums.RoomType.Hcz079,
                    Offset = new UnityEngine.Vector3(1f, 0f, 1f),
                    Chance = 100
                }
            },

        };

        public bool IsViolent => true;

        private int DamageReductionByExplosion = 3;

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
        }

        protected void OnHurting(HurtingEventArgs ev)
        {
            if (!Check(ev.Player.CurrentArmor)) return;

            if(ev.DamageHandler.Type == DamageType.Explosion)
            {
                ev.Amount /= DamageReductionByExplosion;
            }
        }
    }
}
