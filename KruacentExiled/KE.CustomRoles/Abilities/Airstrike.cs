using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups.Projectiles;
using Exiled.API.Features.Toys;
using Exiled.CustomRoles.API.Features;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.API.Interfaces;
using MapGeneration;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Light = Exiled.API.Features.Toys.Light;

namespace KE.CustomRoles.Abilities
{
    public class Airstrike : KEAbilities, ICustomIcon
    {
        public override string Name { get;  } = "AirStrike";

        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "Airstrike",
                    [TranslationKeyDesc] = "Don't overuse it or your co-op will not be happy",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "Bombardement",
                    [TranslationKeyDesc] = "Ne l'utilise pas trop sinon ton coop sera pas content",
                }
            };
        }
        public override float Cooldown { get; } = 60f;

        public float height = 1;
        public Utils.API.GifAnimator.TextImage IconName => MainPlugin.Instance.icons["Airstrike"];


        protected override bool AbilityUsed(Player player)
        {
            if (!SetPosition.TryGetTarget(player, out Vector3 target))
            {
                ShowEffectHint(player, "TeleportationNoTarget");
                return false;
            }

            Physics.Linecast(target, target + height * Vector3.up, out RaycastHit hit);
            if (hit.collider != null)
            {
                Log.Debug($"hit something [{hit.collider}]");
                return false;
            }
            
            var l = Light.Create(target,null,null,true,Color.red);

            ExplosiveGrenade grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE,player);
            grenade.ScpDamageMultiplier = 1;
            grenade.FuseTime = 10;
            Timing.CallDelayed(1.5f, () =>
            {
                Projectile gre = grenade.SpawnActive(target + (height - .5f) * Vector3.up);
                //explode on collision
                gre.GameObject.AddComponent<Exiled.API.Features.Components.CollisionHandler>().Init(player.GameObject, gre.Base);
                l.Destroy();
            });

            return base.AbilityUsed(player);
        }


    }
}
