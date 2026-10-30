using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Spawn;
using Exiled.Events.EventArgs.Player;
using KruacentExiled.CustomItems.API.Features;
using KruacentExiled.CustomItems.API.Interface;
using KruacentExiled.CustomRoles.API.Interfaces;
using MEC;
using System.Collections.Generic;
using UnityEngine;

namespace KruacentExiled.CustomItems.Items
{

    public class FaradayArmor : KECustomItem, IViolentItem, ILumosItem
    {

        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Faraday Armor",
                    [TranslationKeyDesc] = "Tesla doesn't affect you ! And Electrical Damage is reduced 2 times !",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Armure de Faraday",
                    [TranslationKeyDesc] = "T'es protégé des Teslas et tu prends 2 fois moins de dégats d'électricité !",
                },
            };
        }
        
        public override ItemType ItemType => ItemType.ArmorHeavy;
        public override string Name { get; set; } = "FaradayArmor";
        public override float Weight { get; set; } = 4f;
        public Color Color { get; set; } = UnityEngine.Color.green;
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 1,
            RoomSpawnPoints = new List<RoomSpawnPoint>
            {
                new RoomSpawnPoint()
                {
                    Room = Exiled.API.Enums.RoomType.Hcz079,
                    Offset = new UnityEngine.Vector3(1f, 0f, 1f),
                    Chance = 70
                }
            },

        };

        public bool IsViolent => true;

        // Electrical damage will be divided by this amount
        private int DamageReductionByElectricalDamage = 2;

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();
            Exiled.Events.Handlers.Player.TriggeringTesla += OnTriggering;
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            Exiled.Events.Handlers.Player.ItemRemoved += OnRemovedItem;
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();
            Exiled.Events.Handlers.Player.TriggeringTesla -= OnTriggering;
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            Exiled.Events.Handlers.Player.ItemRemoved -= OnRemovedItem;
        }

        protected void OnTriggering(TriggeringTeslaEventArgs ev)
        {
            if (!Check(ev.Player.CurrentArmor)) return;

            ev.IsAllowed = false;
        }

        protected void OnHurting(HurtingEventArgs ev)
        {
            if (!Check(ev.Player.CurrentArmor)) return;

            if(ev.DamageHandler.Type == DamageType.MicroHid)
            {
                ev.Amount /= DamageReductionByElectricalDamage;
            }

            if(ev.DamageHandler.Type == DamageType.ParticleDisruptor)
            {
                ev.Amount /= DamageReductionByElectricalDamage;
            }

            if(ev.DamageHandler.Type == DamageType.Tesla)
            {
                ev.Amount = 0;
            }
        }

        protected override void OnAcquired(Player player, Item item, bool displayMessage)
        {
            AddBuff(player);
        }

        protected override void OnDroppingItem(DroppingItemEventArgs ev)
        {
            if (!Check(ev.Item)) return;
            RemoveBuff(ev.Player);
        }

        protected void OnRemovedItem(ItemRemovedEventArgs ev)
        {
            if (!Check(ev.Item)) return;
            RemoveBuff(ev.Player);
        }

        private void AddBuff(Player player)
        {
            Timing.CallDelayed(.1f, () => player.EnableEffect<Slowness>(2, 999999999, true));
        }

        private void RemoveBuff(Player player)
        {
            Timing.CallDelayed(.1f, () => player.DisableEffect<Slowness>());
        }
    }
}
