using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.CustomRoles.Abilities
{
    [CustomAbility]
    public class Trade : ActiveAbility
    {
        public override string Name { get; set; } = "Trade";

        public override string Description { get; set; } = "T'es lien avec le casino t'as permis d'avoir un peu plus de pièce contre un item ou pire";

        public override float Duration { get; set; } = 0f;

        public override float Cooldown { get; set; } = 1f;

        public static float MaxHealthPercent = .1f;

        protected override void AbilityUsed(Player player)
        {
            if (player.CurrentItem != null)
            {
                player.RemoveItem(player.CurrentItem);
            }
            else
            {
                float newMaxHealth = player.MaxHealth - player.MaxHealth * MaxHealthPercent;
                if (newMaxHealth > 0)
                {
                    player.MaxHealth = newMaxHealth;
                    player.Health = Mathf.Min(player.Health, player.MaxHealth);
                }
                else
                {
                    player.Kill("The casino always win");
                    return;
                }
            }

            player.AddItem(ItemType.Coin);




        }

    }
}
