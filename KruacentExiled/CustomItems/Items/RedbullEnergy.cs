using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using KruacentExiled.CustomItems.API.Features;
using KruacentExiled.CustomItems.API.Interface;
using KruacentExiled.CustomItems.Items.ItemEffects;

namespace KruacentExiled.CustomItems.Items
{
    public class RedbullEnergy : KECustomItem, ISwitchableEffect
    {
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "RedBull™ Energy",
                    [TranslationKeyDesc] = "RedBull™ gives you wings!",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "RedBull™ Energy",
                    [TranslationKeyDesc] = "RedBull™ donne des ailes ! Attention à la chute !!!",
                },
            };
        }
        public override ItemType ItemType => ItemType.SCP207;
        public override string Name { get; set; } = "RedBullEnergy";
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