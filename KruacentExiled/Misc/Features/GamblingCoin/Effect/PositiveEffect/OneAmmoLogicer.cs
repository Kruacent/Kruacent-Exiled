using Exiled.API.Features;
using Exiled.API.Features.Items;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using KruacentExiled.Misc.Features.GamblingCoin.Types;

internal class OneAmmoLogicer : ICoinEffect
{
    public string Name { get; set; } = "OneAmmoLogicer";
    public string Message { get; set; } = "Were you hiding that in your... ";
    public int Weight { get; set; } = 1;
    public EffectType Type { get; set; } = EffectType.Positive;

    public void Execute(Player player)
    {
        Firearm gun = (Firearm)Item.Create(ItemType.GunLogicer);
        gun.BarrelAmmo = 1;
        gun.CreatePickup(player.Position);
    }
}