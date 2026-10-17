using Exiled.API.Features;
using Exiled.API.Features.Items;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using KruacentExiled.Misc.Features.GamblingCoin.Types;

internal class GiveCom45 : ICoinEffect
{
    public string Name { get; set; } = "GiveCom45";
    public string Message { get; set; } = "Ohh a Com45!";
    public int Weight { get; set; } = 5;
    public EffectType Type { get; set; } = EffectType.Positive;

    public void Execute(Player player)
    {
        Item item = Item.Create(ItemType.GunCom45, player);
        item.CreatePickup(player.Position);
    }
}