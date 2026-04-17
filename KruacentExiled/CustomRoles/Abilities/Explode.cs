using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pools;
using KE.Utils.API.Features;
using KE.Utils.API.GifAnimator;
using KruacentExiled.CustomItems.API.Events;
using KruacentExiled.CustomRoles.API.Features;
using KruacentExiled.CustomRoles.API.Interfaces;
using System.Collections.Generic;

namespace KruacentExiled.CustomRoles.Abilities
{
    public class Explode : KEAbilities, ICustomIcon
    {
        public override string Name { get; } = "Explode";

        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string> ()
                {
                    [TranslationKeyName] = "Explode",
                    [TranslationKeyDesc] = "You got an explosive belt",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Explosion",
                    [TranslationKeyDesc] = "Tu as une ceinture d'explosif autour de toi",
                }
            };
        }


        public override float Cooldown { get; } = 4*60f;

        public TextImage IconName => MainPlugin.Instance.icons["Explode"];

        private HashSet<ushort> GrenadesSerials = new HashSet<ushort>();
        protected override void SubscribeEvents()
        {
            GrenadesSerials = HashSetPool<ushort>.Pool.Get();
            ExplodeEvent.ExplodeDestructible += ExplodeEvent_ExplodeDestructible;

            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {

            ExplodeEvent.ExplodeDestructible -= ExplodeEvent_ExplodeDestructible;
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

        private void ExplodeEvent_ExplodeDestructible(OnExplodeDestructibleEventsArgs obj)
        {

            ushort serial = obj.ExplosionGrenade.Info.Serial;
            KELog.Debug("explode serial " + serial);



            if (!GrenadesSerials.Contains(serial)) return;

            obj.Damage /=3f;

            GrenadesSerials.Remove(serial);

            KELog.Debug("explode with "+ obj.Damage);

        }
    }
}
