﻿using CustomPlayerEffects;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using KE.Utils.API.Displays.DisplayMeow;
using KE.Utils.API.Features;
using KE.Utils.Extensions;
using KruacentExiled.CustomItems.API.Enum;
using KruacentExiled.CustomItems.API.Features;
using KruacentExiled.CustomRoles.API.Features;
using KruacentExiled.CustomRoles.API.Interfaces;
using MEC;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KruacentExiled.CustomRoles.CR.Human
{
    public class Curiosophile : GlobalCustomRole, IColor
    {
        public override SideEnum Side { get; set; } = SideEnum.Human;
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Curiosophile",
                    [TranslationKeyDesc] = "Junk items make you faster and tougher\nAn empty inventory slows you down\nPowerful items weigh you down",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Curiosophile",
                    [TranslationKeyDesc] = "Les objets de pacotille te rendent plus rapide et résistant\nSans rien sur toi, tu es ralenti\nLes objets puissants t'alourdissent",
                },
                ["legacy"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Curiosophile",
                    [TranslationKeyDesc] = "Les objets de pacotille te rendent plus rapide et résistant\nSans rien sur toi, tu es ralenti\nLes objets puissants t'alourdissent",
                }

            };
        }
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = true;
        public override float SpawnChance { get; set; } = 100;
        public Color32 Color => new Color32(9, 25, 67,0);
        private readonly Dictionary<Player, AppliedEffects> applied = new Dictionary<Player, AppliedEffects>();
        private const int ReversedSlowness = 200;
        private class AppliedEffects
        {
            public int MovementBoost;
            public int Slowness;
            public int DamageReduction;
            public int Exhausted;
        }

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.ItemAdded += OnItemAdded;
            Exiled.Events.Handlers.Player.ItemRemoved += OnItemRemoved;
            Exiled.Events.Handlers.Player.Spawned += OnSpawned;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();
            Exiled.Events.Handlers.Player.ItemAdded -= OnItemAdded;
            Exiled.Events.Handlers.Player.ItemRemoved -= OnItemRemoved;
            Exiled.Events.Handlers.Player.Spawned -= OnSpawned;
        }

        protected override void RoleAdded(Player player)
        {
            applied[player] = new AppliedEffects();
            Timing.CallDelayed(TimeAttributingInventory, () => UpdateEffects(player));
        }

        protected override void RoleRemoved(Player player)
        {
            if (applied.TryGetValue(player, out AppliedEffects effects))
            {
                SetEffects(player, effects, 0, 0, 0);
                applied.Remove(player);
            }
        }

        private void OnItemAdded(ItemAddedEventArgs ev)
        {
            Timing.CallDelayed(Timing.WaitForOneFrame, () =>
            {
                UpdateEffects(ev.Player);
                ShowItemScore(ev.Player, ev.Item);
            });
        }
        private void OnItemRemoved(ItemRemovedEventArgs ev)
        {
            Timing.CallDelayed(Timing.WaitForOneFrame, () =>
            {
                UpdateEffects(ev.Player);
                ShowItemScore(ev.Player, ev.Item);
            });
        }

        private void ShowItemScore(Player player, Item item)
        {
            if (!applied.ContainsKey(player)) return;

            int score = CheckInventoryScore(player.Items.ToList());
            string text = $"L'item vaut {CheckItemScore(item)} (score total : {score})";
            ShowEffectHint(player, text);
        }

        private void OnSpawned(SpawnedEventArgs ev)
        {
            if (!applied.ContainsKey(ev.Player)) return;

            applied[ev.Player] = new AppliedEffects();
            Timing.CallDelayed(TimeAttributingInventory, () => UpdateEffects(ev.Player));
        }

        private void UpdateEffects(Player player)
        {
            if (!applied.TryGetValue(player, out AppliedEffects effects)) return;

            int score = CheckInventoryScore(player.Items.ToList());
            int damageReduction = Mathf.Clamp(score / 2, 0, 20);

            int exhausted;
            if (score <= 20)
                exhausted = 1;
            else
                exhausted = 0;

            int speed;
            if (score > 0)
                speed = Mathf.Min(score / 4, 10);
            else
                speed = Mathf.Max(score / 4 - 8, -25);

            SetEffects(player, effects, speed, damageReduction, exhausted);
        }

        private void SetEffects(Player player, AppliedEffects effects, int speed, int damageReduction, int exhausted)
        {
            int movementBoost = 0;
            int slowness = 0;
            if (speed > 0)
                movementBoost = speed;
            else
                slowness = -speed;

            player.AddLevelEffect<MovementBoost>(movementBoost - effects.MovementBoost);

            // Reverse effect form Gambling coin still working with that
            if (player.GetEffect<Slowness>().Intensity < ReversedSlowness)
            {
                player.AddLevelEffect<Slowness>(slowness - effects.Slowness);
                effects.Slowness = slowness;
            }

            player.AddLevelEffect<DamageReduction>(damageReduction - effects.DamageReduction);
            player.AddLevelEffect<Exhausted>(exhausted - effects.Exhausted);

            effects.MovementBoost = movementBoost;
            effects.DamageReduction = damageReduction;
            effects.Exhausted = exhausted;
        }

        private int CheckInventoryScore(List<Item> inventory)
        {
            int score = 0;
            foreach (Item item in inventory)
            {
                score += CheckItemScore(item);
            }

            return score;
        }

        private int CheckItemScore(Item item)
        {
            ItemRank rank = GetRank(item);
            KELog.Debug($"item rank : {rank}");
            switch (GetRank(item))
            {
                case ItemRank.Basic:
                    return 15;
                case ItemRank.Good:
                    return 5;
                case ItemRank.Epic:
                    return -10;
                case ItemRank.Legendary:
                    return -20;
                case ItemRank.Exotic:
                    return -30;
                default:
                    return 0;
            }
        }

        private ItemRank GetRank(Item item)
        {
            if (CustomItem.TryGet(item, out CustomItem customItem) && customItem is KECustomItem keCustomItem)
            {
                return keCustomItem.Rank;
            }

            if (item.Type == ItemType.SCP244a || item.Type == ItemType.SCP244b) return ItemRank.Basic;

            if (item.Category == ItemCategory.SpecialWeapon) return ItemRank.Exotic;

            if ((item.TierFlags & ItemTierFlags.ExtraRare) != 0) return ItemRank.Legendary;
            if ((item.TierFlags & ItemTierFlags.MilitaryGrade) != 0) return ItemRank.Good;
            if ((item.TierFlags & ItemTierFlags.Rare) != 0) return ItemRank.Epic;
            return ItemRank.Basic;
        }
    }
}
