
using System;
using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Scp914;

namespace KE.Items.Items
{
    [CustomItem(ItemType.GrenadeHE)]
    public class PressePuree : CustomGrenade
    {
        public override uint Id { get; set; } = 1406;
        public override string Name { get; set; } = "Presse Purée";
        public override string Description { get; set; } = "The grenade explode at impact but does less damage";
        public override float Weight { get; set; } = 0.65f;
        public override float FuseTime { get; set; } = 1.5f;
        public override bool ExplodeOnCollision { get; set; } = true;
        public float DamageModifier { get; set; } = 0.4f;
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 5,
            DynamicSpawnPoints = new List<DynamicSpawnPoint>
            {
                new DynamicSpawnPoint()
                {
                    Chance = 50,
                    Location = SpawnLocationType.InsideHczArmory,
                },
                new DynamicSpawnPoint()
                {
                    Chance = 5,
                    Location = SpawnLocationType.Inside914,
                },
                new DynamicSpawnPoint()
                {
                    Chance= 50,
                    Location = SpawnLocationType.Inside049Armory,
                },
                new DynamicSpawnPoint()
                {
                    Chance= 50,
                    Location = SpawnLocationType.InsideLczArmory,
                }
            },
        };


        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Scp914.UpgradingInventoryItem += OnUpgrading;
            base.SubscribeEvents();
        }

        /// <inheritdoc/>
        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Scp914.UpgradingInventoryItem -= OnUpgrading;
            base.UnsubscribeEvents();
        }

        private void OnUpgrading(UpgradingInventoryItemEventArgs ev)
        {
            if (!Check(ev.Item))
                return;
            if (ev.KnobSetting != Scp914.Scp914KnobSetting.VeryFine)
                return;

            var rng = UnityEngine.Random.Range(0, 101);
            Log.Debug($"inventory {Name} : {rng}");
            if (rng < 10)
            {
                //success
                ev.Player.RemoveItem(ev.Item);
                TryGive(ev.Player, "Sainte Grenada");
                ev.IsAllowed = true;
            }
            else
            {
                ev.Player.ShowHint("no luck");
                ev.IsAllowed = false;
            }

        }
    }
}
