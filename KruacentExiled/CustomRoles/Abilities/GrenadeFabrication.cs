using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using KruacentExiled.CustomRoles.API.Features;
using System.Collections.Generic;

namespace KruacentExiled.CustomRoles.Abilities
{
    public class GrenadeFabrication : KEAbilities
    {
        public override string Name { get; } = "GrenadeFabrication";

        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string> ()
                {
                    [TranslationKeyName] = "Grenade Fabrication",
                    [TranslationKeyDesc] = "You create a HE Grenade",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Fabrication de Grenade",
                    [TranslationKeyDesc] = "Tu fabriques une grenade",
                }
            };
        }

        public override float Cooldown { get; } = 100f;

        protected override bool AbilityUsed(Player player)
        {
            if(player.IsInventoryFull)
            {
                Pickup.CreateAndSpawn(ItemType.GrenadeHE, player.Position);
            } else
            {
                player.AddItem(ItemType.GrenadeHE);
            }

            return base.AbilityUsed(player);
        }
    }
}
