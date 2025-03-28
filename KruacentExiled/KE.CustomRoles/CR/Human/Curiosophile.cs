/*
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using KE.CustomRoles.API;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils.NonAllocLINQ;

namespace KE.CustomRoles.CR.Human
{
    [CustomRole(RoleTypeId.None)]
    internal class Curiosophile : GlobalCustomRole
    {
        public override SideEnum Side { get; set; } = SideEnum.Human;
        public override string Name { get; set; } = "Curiosophile";
        public override string Description { get; set; } = "Tu veux récuperer tous ce que tu vois !";
        public override uint Id { get; set; } = 1059;
        public override string CustomInfo { get; set; } = "Curiosophile";
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = true;
        public override float SpawnChance { get; set; } = 100;

        private readonly List<ItemType> LowQualityItem =
        [
            ItemType.GunA7,
            ItemType.GunCOM15,
            ItemType.Flashlight,
            ItemType.Coin,
            ItemType.KeycardJanitor,
            ItemType.KeycardGuard,
        ];

        private readonly List<ItemType> HighQualityItem =
        [
            ItemType.MicroHID,
            ItemType.Jailbird,
            ItemType.GunFRMG0,
            ItemType.ArmorHeavy,
            ItemType.ParticleDisruptor,
            ItemType.Adrenaline,
            ItemType.SCP1344,
            ItemType.SCP500,
            ItemType.AntiSCP207,
            ItemType.KeycardMTFCaptain,
            ItemType.KeycardO5,
            ItemType.KeycardFacilityManager,
        ];

        private readonly List<EffectType> BuffEffect =
        [
            EffectType.Vitality,
            EffectType.RainbowTaste,
            EffectType.BodyshotReduction,
            EffectType.AntiScp207
        ];

        private readonly List<EffectType> NerfEffect =
        [
            EffectType.Deafened,
            EffectType.AmnesiaVision,
            EffectType.InsufficientLighting,
            EffectType.Stained,
            EffectType.Blurred
        ];

        private Dictionary<EffectType, int> activeEffect = new Dictionary<EffectType, int>();

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.ItemAdded += PickupItem;
            Exiled.Events.Handlers.Player.ItemRemoved += ThrowItem;
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.ItemAdded -= PickupItem;
            Exiled.Events.Handlers.Player.ItemRemoved -= ThrowItem;
        }

        public void PickupItem(ItemAddedEventArgs ev)
        {
            if (!Check(ev.Player)) return;

            CheckupInventory(ev.Player);            
        }

        public void ThrowItem(ItemRemovedEventArgs ev)
        {
            if (!Check(ev.Player)) return;

            CheckupInventory(ev.Player);
        }

        private void CheckupInventory(Player p)
        {
            IEnumerable<ItemType> playerItems = p.Items.Select(item => item.Type);

            int buffedItemCount = playerItems.Intersect(LowQualityItem).Count();
            int nerfedItemCount = playerItems.Intersect(HighQualityItem).Count();

            

            ApplyRandomBuffEffects(buffedItemCount, p);
            ApplyRandomNerfEffects(nerfedItemCount, p);
        }

        private void ApplyRandomBuffEffects(int buffedItemCount, Player p)
        {
            if (buffedItemCount > 0)
            {
                Log.Info("Buff Effect Applied");
                int effectsToApply = Mathf.Min(buffedItemCount, BuffEffect.Count);

                for (int i = 0; i < effectsToApply; i++)
                {
                    var randomBuff = BuffEffect[UnityEngine.Random.Range(0, BuffEffect.Count)];
                    p.EnableEffect(randomBuff, -1);
                }
            }
        }

        private void ApplyRandomNerfEffects(int nerfedItemCount, Player p)
        {
            if (nerfedItemCount > 0)
            {
                Log.Info("Nerf Effect Applied");

                int effectsToApply = Mathf.Min(nerfedItemCount, NerfEffect.Count);

                for (int i = 0; i < effectsToApply; i++)
                {
                    var randomNerf = NerfEffect[UnityEngine.Random.Range(0, NerfEffect.Count)];
                    p.EnableEffect(randomNerf, -1);
                }
            }
        }
    }
}
*/