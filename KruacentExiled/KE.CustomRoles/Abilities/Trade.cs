using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using KE.CustomRoles.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.CustomRoles.Abilities
{
    public class Trade : KEAbilities
    {
        public override string Name { get; } = "Trade";

        public override string Description { get; } = "T'es lien avec le casino t'as permis d'avoir un peu plus de pièce en échange d'un item ou de pire";

        public override int Id => 2002;

        public override float Cooldown { get; } = 1f;

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
