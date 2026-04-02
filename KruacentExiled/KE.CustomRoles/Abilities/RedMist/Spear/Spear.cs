using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.DamageHandlers;
using InventorySystem.Items.MicroHID.Modules;
using KE.CustomRoles.CR.MTF.RedMist;
using KE.Utils.API.Features;
using PlayerRoles;
using System.Collections.Generic;
using UnityEngine;
using static PlayerRoles.PlayableScps.VisionInformation;

namespace KE.CustomRoles.Abilities.RedMist.Spear
{
    public class Spear : EgoAbility
    {
        public const string FailWeapon = "SpearFailWeapon";
        public override string Name { get; } = "Spear";
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "Spear",
                    [TranslationKeyDesc] = "Manifest your E.G.O, gain powerful buff but rapid health drain.\nCan be deactivated anytime",
                    [FailWeapon] = "You need your weapon",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "todo",
                    [TranslationKeyDesc] = "todo",
                    [FailWeapon] = "todo",
                }
            };
        }
        public override float Cooldown { get; } = 0f;

        protected override NeedActive NeedEGOActive => NeedActive.Either;

        public const float Damage = 200;

        public const float MaxDistance = 5;




        protected override bool LaunchedAbility(Player player, EGO ego)
        {
            KELog.Debug("check weapopgn");
            if (player.CurrentItem is null || player.CurrentItem.Type != ItemType.SCP1509)
            {
                ShowEffectHint(player, FailWeapon);
                return false;
            }


            if (player.GameObject.TryGetComponent<AttackGreaterSplit>(out var comp))
            {
                comp.StartAttack();
            }


            return true;
        }




    }
}