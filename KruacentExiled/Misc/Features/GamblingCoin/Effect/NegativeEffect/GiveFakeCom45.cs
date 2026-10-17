using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.CustomItems.API.Features;
using KE.Utils.API.Features;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using KruacentExiled.Misc.Features.GamblingCoin.Types;

internal class GiveFakeCom45 : ICoinEffect
{
    public string Name { get; set; } = "GiveFakeCom45";
    public string Message { get; set; } = "Ohh a Com45!";
    public int Weight { get; set; } = 5;
    public EffectType Type { get; set; } = EffectType.Negative;

    public void Execute(Player player)
    {

        bool success = CustomItem.TrySpawn("LifeEnder", player.Position, out Pickup gun);
        FirearmPickup firearm = (FirearmPickup)gun;
        firearm.Ammo = 20;

        KELog.Debug(success);
    }
}