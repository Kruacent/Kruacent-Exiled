using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
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
    public class Trade : KEAbilities, ICustomIcon
    {
        public override string Name { get; } = "Trade";
        public Utils.API.GifAnimator.TextImage IconName => MainPlugin.Instance.icons[Name];
        public override float Cooldown { get; } = 1f;

        public static float MaxHealthPercent = .1f;

        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "Trade",
                    [TranslationKeyDesc] = "Your link with casinos permit your to gain more coins in exchange of items",
                    ["TradeNoItem"] = "No item?",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "Trade",
                    [TranslationKeyDesc] = "T'es lien avec le casino t'as permis d'avoir un peu plus de pièce en échange d'un objet",
                    ["TradeNoItem"] = "Pas d'objet",
                }
            };
        }

        protected override bool AbilityUsed(Player player)
        {
            if (player.CurrentItem != null)
            {
                player.RemoveItem(player.CurrentItem);
            }
            else
            {


                ShowEffectHint(player, "no items?");
                return false;
                /*
                float newHealth = player.MaxHealth - player.MaxHealth * MaxHealthPercent;
                if (newHealth > 0)
                {
                    player.Health = Mathf.Min(player.Health, newHealth);
                }
                else
                {
                    player.Kill("The casino always win");
                    return base.AbilityUsed(player);
                }*/
            }

            player.AddItem(ItemType.Coin);
            return base.AbilityUsed(player);
        }

    }
}
