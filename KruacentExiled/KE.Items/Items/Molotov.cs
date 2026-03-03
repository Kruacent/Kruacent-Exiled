using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using KE.Items.API.Core.Models;
using KE.Items.API.Core.Upgrade;
using KE.Items.API.Features;
using KE.Items.API.Interface;
using KE.Items.Items.ItemEffects;
using KE.Items.Items.PickupModels;
using Scp914;
using System.Collections.Generic;
using UnityEngine;

namespace KE.Items.Items
{
    public class Molotov : KECustomGrenade, ISwichableEffect, ICustomPickupModel, IUpgradableCustomItem
    {
        public override ItemType ItemType => ItemType.GrenadeFlash;
        public override string Name { get; set; } = "Cocktail Molotov";
        public override string Description { get; set; } = "ARSON";
        public override float Weight { get; set; } = 0.65f;
        public override float FuseTime => 5f;
        public override bool ExplodeOnCollision => true;
        public Color Color { get; set; } = Color.yellow;
        public CustomItemEffect Effect { get; set; }
        public PickupModel PickupModel { get; }

        public IReadOnlyDictionary<Scp914KnobSetting, UpgradeProperties> Upgrade => new Dictionary<Scp914KnobSetting, UpgradeProperties>()
        {
            [Scp914KnobSetting.OneToOne] = new UpgradeProperties(100, 1051)
        };
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 2,
            LockerSpawnPoints = new List<LockerSpawnPoint>
            { 
                new LockerSpawnPoint() { Chance = 75, UseChamber = true, Type = LockerType.Misc, Zone = ZoneType.Entrance, },
                new LockerSpawnPoint() { Chance = 50, UseChamber = true, Type = LockerType.Misc, Zone = ZoneType.LightContainment, },
                new LockerSpawnPoint() { Chance = 50, UseChamber = true, Type = LockerType.Misc, Zone = ZoneType.HeavyContainment, },
            },

            RoomSpawnPoints = new List<RoomSpawnPoint>
            {
                new RoomSpawnPoint() { Chance = 75, Room = RoomType.LczGlassBox, },
                new RoomSpawnPoint() { Chance = 100, Room = RoomType.HczArmory, },
                new RoomSpawnPoint() { Chance = 100, Room = RoomType.Hcz049, },
            },
        };

        public Molotov()
        {
            Effect = new MolotovEffect();
            PickupModel = new MolotovPModel(this);
        }

        protected override void SubscribeEvents()
        {
            PickupModel.SubscribeEvents();
            Exiled.Events.Handlers.Player.ReceivingEffect += ReceivedEffect;
            Exiled.Events.Handlers.Player.PickingUpItem += PickingItem;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            PickupModel.UnsubscribeEvents();
            Exiled.Events.Handlers.Player.ReceivingEffect -= ReceivedEffect;
            Exiled.Events.Handlers.Player.PickingUpItem -= PickingItem;
            base.UnsubscribeEvents();
        }

        private void ReceivedEffect(ReceivingEffectEventArgs ev)
        {
            if (Effect is MolotovEffect molotovEffect)
            {
                molotovEffect.OnReceivingEffect(ev);
            }
        }

        private void PickingItem(PickingUpItemEventArgs ev)
        {
            if (Effect is MolotovEffect molotovEffect)
            {
                molotovEffect.OnPickingUp(ev);
            }
        }

        protected override void OnExplodingGrenade(ExplodingGrenadeEventArgs ev)
        {
            Effect.Effect(ev);
            ev.TargetsToAffect.Clear();
        }
    }
}