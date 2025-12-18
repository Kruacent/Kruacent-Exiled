using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using KE.Items.API.Features;
using KE.Items.API.Interface;
using KE.Items.Items.ItemEffects;

namespace KE.Items.Items
{
    [CustomItem(ItemType.SCP207)]
    public class RedbullEnergy : KECustomItem, ISwichableEffect
    {
        public override uint Id { get; set; } = 1042;
        public override string Name { get; set; } = "RedBull Energy";
        public override string Description { get; set; } = "RedBull donne des ailes ! Attention à la chute !!!";
        public override float Weight { get; set; } = 0.65f;
        public UnityEngine.Color Color { get; set; } = UnityEngine.Color.blue;
        public CustomItemEffect Effect { get; set; }

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 2,
            RoomSpawnPoints = new List<RoomSpawnPoint>
            {
                new RoomSpawnPoint() { Room = RoomType.EzStraight, Chance = 25 },
                new RoomSpawnPoint() { Room = RoomType.LczCrossing, Chance = 25 },
            },
            LockerSpawnPoints = new List<LockerSpawnPoint>
            {
                new LockerSpawnPoint { Chance = 20, UseChamber = true, Type = LockerType.Medkit, Zone = ZoneType.Entrance },
                new LockerSpawnPoint { Chance = 30, UseChamber = true, Type = LockerType.Misc, Zone = ZoneType.LightContainment}
            }
        };

        public RedbullEnergy()
        {
            Effect = new RedBullEnergyEffect();
        }

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.UsedItem += OnUsedItem;
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            Exiled.Events.Handlers.Player.Dying += OnDying;
            Exiled.Events.Handlers.Player.Left += OnLeft;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.UsedItem -= OnUsedItem;
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            Exiled.Events.Handlers.Player.Dying -= OnDying;
            Exiled.Events.Handlers.Player.Left -= OnLeft;
            base.UnsubscribeEvents();
        }

        private void OnUsedItem(UsedItemEventArgs ev)
        {
            if (!Check(ev.Item)) return;
            Effect.Effect(ev);
        }

        private void OnHurting(HurtingEventArgs ev) => (Effect as RedBullEnergyEffect)?.OnHurting(ev);
        private void OnDying(DyingEventArgs ev) => (Effect as RedBullEnergyEffect)?.Cleanup(ev.Player);
        private void OnLeft(LeftEventArgs ev) => (Effect as RedBullEnergyEffect)?.Cleanup(ev.Player);
    }
}