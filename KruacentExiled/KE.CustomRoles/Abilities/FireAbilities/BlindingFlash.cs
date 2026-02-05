using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups.Projectiles;

namespace KE.CustomRoles.Abilities.FireAbilities
{
    public class BlindingFlash : FireAbilityBase
    {
        public override string Name { get; } = "BlindingFlash";
        public override string PublicName { get; } = "Blinding Flash";

        public override string Description { get; } = "Spawns an active flashbang at your feet";

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
