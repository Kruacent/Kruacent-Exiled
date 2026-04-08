using Exiled.API.Extensions;
using Exiled.API.Features;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.API.Interfaces;
using KE.CustomRoles.API.Interfaces.Ability;
using MapGeneration;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KE.CustomRoles.Abilities
{
    public class Teleportation : KEAbilities, ICustomIcon, IConditional
    {
        public override string Name { get; } = "Teleportation";

        public const string TranslationLift = "TeleportationLift";
        public const string TranslationLcz = "TeleportationLcz";
        public const string TranslationDifferentZone = "TeleportationDifferentZone";


        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "Teleportation",
                    [TranslationKeyDesc] = $"You lose {Damage} HP per teleportation, can't be used in lifts",
                    [TranslationLift] = "can't teleport in lifts",
                    [TranslationLcz] = "The target is inaccessible",
                    [TranslationDifferentZone] = "The target is inaccessible",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "Téléportation",
                    [TranslationKeyDesc] = $"Tu perds {Damage} HP/téléportation, ne peux pas être utilisé dans les ascenseurs",
                    [TranslationLift] = "Impossible de se téléporter dans un ascenseur",
                    [TranslationLcz] = "Position inaccessible",
                    [TranslationDifferentZone] = "Position inaccessible",
                }
            };
        }

        public Utils.API.GifAnimator.TextImage IconName => MainPlugin.Instance.icons[Name];
        public override float Cooldown { get; } = 130f;
        public static float Damage { get; set; } = 60;

        protected override bool AbilityUsed(Player player)
        {

            if (!CheckValid(player, true))
            {
                return false;
            }
            SetPosition.TryGetTarget(player, out Vector3 target);
            player.Hurt(Damage, Exiled.API.Enums.DamageType.Asphyxiation);

            if(UnityEngine.Random.Range(1f, 100f) < 5)
            {
                if (Player.Enumerable.Count() > 1)
                {
                    player.Teleport(Player.Enumerable.GetRandomValue(p => p != player));
                }
            }
            else
            {
                player.Position = target;
            }
            return base.AbilityUsed(player);

        }


        private bool CheckValid(Player player,bool showMessage)
        {
            if (!SetPosition.TryGetTarget(player, out Vector3 target))
            {
                if (showMessage)
                {
                    ShowEffectHint(player, SetPosition.TranslationNoTarget);
                }
                
                return false;
            }



            if (Lift.Get(target) is not null)
            {
                if (showMessage)
                {
                    ShowEffectHint(player, TranslationLift);
                }
                
                return false;
            }


            if (target.GetZone() == FacilityZone.LightContainment && Map.IsLczDecontaminated)
            {
                if (showMessage)
                {
                    ShowEffectHint(player, TranslationLcz);
                }

                
                return false;
            }

            if (target.GetZone() != player.Zone.GetZone())
            {
                if (showMessage)
                {
                    ShowEffectHint(player, TranslationDifferentZone);
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