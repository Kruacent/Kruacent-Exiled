using Exiled.API.Extensions;
using Exiled.API.Features;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.API.Interfaces;
using MapGeneration;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KE.CustomRoles.Abilities
{
    public class Teleportation : KEAbilities, ICustomIcon
    {
        public override string Name { get; } = "Teleportation";

        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "Teleportation",
                    [TranslationKeyDesc] = $"You lose {Damage} HP per teleportation, can't be used in lifts",
                    ["TeleportationNoTarget"] = "No target selected",
                    ["TeleportationLift"] = "can't teleport in lifts",
                    ["TeleportationLcz"] = "The target is inaccessible",
                    ["TeleportationDifferentZone"] = "The target is inaccessible",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "Téléportation",
                    [TranslationKeyDesc] = $"Tu perds {Damage} HP/téléportation, ne peux pas être utilisé dans les ascenseurs",
                    ["TeleportationNoTarget"] = "Pas de position mise",
                    ["TeleportationLift"] = "Impossible de se téléporter dans un ascenseur",
                    ["TeleportationLcz"] = "Position inaccessible",
                    ["TeleportationDifferentZone"] = "Position inaccessible",
                }
            };
        }

        public Utils.API.GifAnimator.TextImage IconName => MainPlugin.Instance.icons[Name];
        public override float Cooldown { get; } = 130f;
        public static float Damage { get; set; } = 60;

        protected override bool AbilityUsed(Player player)
        {
            if(!SetPosition.TryGetTarget(player, out Vector3 target))
            {
                ShowEffectHint(player, "TeleportationNoTarget");
                return false;
            }


            
            if(Lift.Get(target) is not null)
            {
                ShowEffectHint(player, "TeleportationLift");
                return false;
            }


            if (target.GetZone() == FacilityZone.LightContainment && Map.IsLczDecontaminated)
            {
                ShowEffectHint(player, "TeleportationLcz");
                return false;
            }

            if (target.GetZone() != player.Zone.GetZone())
            {
                ShowEffectHint(player, "TeleportationDifferentZone");
                return false;
            }

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
    }
}