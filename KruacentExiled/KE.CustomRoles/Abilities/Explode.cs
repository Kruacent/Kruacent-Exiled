using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pools;
using Exiled.CustomRoles.API.Features;
using InventorySystem.Items.ThrowableProjectiles;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.API.Interfaces;
using KE.Utils.API.Features;
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

        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "Explode",
                    [TranslationKeyDesc] = "You got an explosive belt",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "Explosion",
                    [TranslationKeyDesc] = "Tu as une ceinture d'explosif autour de toi",
                }
            };
        }


        public override float Cooldown { get; } = 4*60f;

        public Utils.API.GifAnimator.TextImage IconName => MainPlugin.Instance.icons["Explode"];

        private HashSet<ushort> GrenadesSerials = new();
        protected override void SubscribeEvents()
        {
            GrenadesSerials = HashSetPool<ushort>.Pool.Get();
            Items.API.Events.ExplodeEvent.ExplodeDestructible += ExplodeEvent_ExplodeDestructible;

            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {

            Items.API.Events.ExplodeEvent.ExplodeDestructible -= ExplodeEvent_ExplodeDestructible;
            HashSetPool<ushort>.Pool.Return(GrenadesSerials);
            base.UnsubscribeEvents();
        }

        protected override bool AbilityUsed(Player player)
        {
            ExplosiveGrenade grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);

            
            grenade.FuseTime = 0.2f;
            grenade.SpawnActive(player.Position);


            //idk why it's the next one but it is
            ushort serial = (ushort) (grenade.Serial + 1);

            GrenadesSerials.Add(serial);

            return base.AbilityUsed(player);
        }

        private void ExplodeEvent_ExplodeDestructible(Items.API.Events.OnExplodeDestructibleEventsArgs obj)
        {

            ushort serial = obj.ExplosionGrenade.Info.Serial;
            KELog.Debug("explode serial " + serial);



            if (!GrenadesSerials.Contains(serial)) return;

            obj.Damage /=3.1f;

            KELog.Debug("explode with "+ obj.Damage);

        }
    }
}
