using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups.Projectiles;
using Exiled.API.Features.Toys;
using Exiled.CustomRoles.API.Features;
using KE.Utils.API.GifAnimator;
using KruacentExiled.CustomRoles;
using KruacentExiled.CustomRoles.API.Features;
using KruacentExiled.CustomRoles.API.Interfaces;
using KruacentExiled.CustomRoles.API.Interfaces.Ability;
using MapGeneration;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Light = Exiled.API.Features.Toys.Light;

namespace KruacentExiled.CustomRoles.Abilities
{
    public class Airstrike : KEAbilities, ICustomIcon, IConditional
    {
        public override string Name { get; } = "AirStrike";

        public const string TranslationSomethingHere = "AirStrikeSomethingHere";
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Airstrike",
                    [TranslationKeyDesc] = "Don't overuse it or your co-op will not be happy",
                    [TranslationSomethingHere] = "Something is in the way",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Bombardement",
                    [TranslationKeyDesc] = "Ne l'utilise pas trop sinon ton coop sera pas content",
                    [TranslationSomethingHere] = "Quelque chose gêne",
                }
            };
        }
        public override float Cooldown { get; } = 60f;

        public float height = 1;
        public TextImage IconName => MainPlugin.Instance.icons["Airstrike"];

        TextImage ICustomIcon.IconName => throw new NotImplementedException();

        protected override bool AbilityUsed(Player player)
        {
            if (CheckValid(player, true))
            {
                return false;
            }
            SetPosition.TryGetTarget(player, out Vector3 target);
            var l = Light.Create(target, null, null, true, Color.red);

            ExplosiveGrenade grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE, player);
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



        private bool CheckValid(Player player, bool showMessage)
        {
            if (!SetPosition.TryGetTarget(player, out Vector3 target))
            {
                if (showMessage)
                {
                    ShowEffectHint(player, SetPosition.TranslationNoTarget);
                }
                return false;
            }

            Physics.Linecast(target, target + height * Vector3.up, out RaycastHit hit);
            if (hit.collider != null)
            {
                if (showMessage)
                {
                    ShowEffectHint(player, TranslationSomethingHere);
                }
                return false;
            }

            return true;
        }

        public bool CheckCondition(Player player)
        {
            return CheckValid(player, false);
        }
    }
}
