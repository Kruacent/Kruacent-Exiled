
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Pickups.Projectiles;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Scp914;
using KE.Items.API.Core.Models;
using KE.Items.API.Core.Upgrade;
using KE.Items.API.Events;
using KE.Items.API.Features;
using KE.Items.API.Interface;
using KE.Items.Items.PickupModels;
using KE.Utils.API.Features;
using Scp914;
using System;
using System.Collections.Generic;

namespace KE.Items.Items
{
    [CustomItem(ItemType.GrenadeHE)]
    public class PressePuree : KECustomGrenade, IUpgradableCustomItem, ICustomPickupModel
    {
        public override uint Id { get; set; } = 1046;
        public override string Name { get; set; } = "Presse Purée";
        public override string Description { get; set; } = "THIS ITEM DOESNT CURRENTLY DO DAMAGE !!!!!!\nThe grenade explode at impact but does less damage";
        public override float Weight { get; set; } = 0.65f;
        public override float FuseTime { get; set; } = 5f;
        public override bool ExplodeOnCollision { get; set; } = true;
        public PickupModel PickupModel { get; }
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

        public PressePuree()
        {
            PickupModel = new PressePureePModel(this);
        }


        protected override void SubscribeEvents()
        {
            PickupModel.SubscribeEvents();
            ExplodeEvent.ExplodeDestructible += OnExplodeDestructible;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            PickupModel.UnsubscribeEvents();
            ExplodeEvent.ExplodeDestructible -= OnExplodeDestructible;
            base.UnsubscribeEvents();
        }

        private void OnExplodeDestructible(OnExplodeDestructibleEventsArgs ev)
        {
            KELog.Debug("old dmagea="+ev.Damage);
            Player player = Player.Get(ev.Destructible.NetworkId);
            if (!Check(Projectile.Get(ev.ExplosionGrenade))) return;

            
            if (ev.Damage < 0f) return;
            ev.Damage /= 2f;

            if (player is not null && player.IsScp)
            {
                ev.Damage /= 3f;
            }

            KELog.Debug("new daamager="+ev.Damage);

        }

        public IReadOnlyDictionary<Scp914KnobSetting, UpgradeProperties> Upgrade { get; private set; } = new Dictionary<Scp914KnobSetting, UpgradeProperties>()
        {
            { Scp914KnobSetting.VeryFine,new UpgradeProperties(5, 1055)}
        };
    }
}
