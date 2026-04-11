using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups.Projectiles;
using System.Collections.Generic;

namespace KE.CustomRoles.Abilities.FireAbilities
{
    public class BlindingFlash : FireAbilityBase
    {
        public override string Name { get; } = "BlindingFlash";

        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "Blinding Flash",
                    [TranslationKeyDesc] = "Spawns an active flashbang at your feet",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "Le soleil",
                    [TranslationKeyDesc] = "IS THAT A MOTHERFUCKER RED FLOOD REFERENCE??????????????",
                }
            };
        }
        public override float Cooldown { get; } = 30f;

        public override int Cost => 50;



        protected override bool LaunchedAbility(Player player)
        {
            FlashGrenade flashbangProjectile = Item.Create<FlashGrenade>(ItemType.GrenadeFlash);

            flashbangProjectile.FuseTime = .1f;
            flashbangProjectile.SpawnActive(player.Position, player);
            return true;
        }



    }

}
