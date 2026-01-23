using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.CustomRoles.API.Features;
using InventorySystem.Items.ThrowableProjectiles;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.CustomRoles.Abilities
{
    public class Explode : KEAbilities, ICustomIcon
    {
        public override string Name { get; } = "Explode";
        public override string PublicName { get; } = "Explode";

        public override string Description { get; } = "Tu as une ceinture d'explosif autour de toi, fait attention n'appuie pas sur le bouton";

        public override float Cooldown { get; } = 4*60f;

        public Utils.API.GifAnimator.TextImage IconName => MainPlugin.Instance.icons["Explode"];

        private HashSet<ExplosionGrenade> Grenades = new();
        protected override void SubscribeEvents()
        {
            Grenades = new();
            Items.API.Events.ExplodeEvent.ExplodeDestructible += ExplodeEvent_ExplodeDestructible;

            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {

            Items.API.Events.ExplodeEvent.ExplodeDestructible -= ExplodeEvent_ExplodeDestructible;
            base.UnsubscribeEvents();
        }

        protected override bool AbilityUsed(Player player)
        {
            ExplosiveGrenade grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);

            
            grenade.FuseTime = 0.2f;
            grenade.SpawnActive(player.Position);
            Grenades.Add(grenade.Projectile.Base);

            Log.Debug("Grenade spawned");
            return base.AbilityUsed(player);
        }

        private void ExplodeEvent_ExplodeDestructible(Items.API.Events.OnExplodeDestructibleEventsArgs obj)
        {
            if (!Grenades.Contains(obj.ExplosionGrenade)) return;

            obj.Damage = 100;

        }
    }
}
