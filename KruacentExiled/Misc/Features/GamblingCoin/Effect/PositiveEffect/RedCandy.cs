using Exiled.API.Features;
using Exiled.API.Features.Items;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using EffectType = KruacentExiled.Misc.Features.GamblingCoin.Types.EffectType;

public class RedCandy : ICoinEffect
{
    public string Name { get; set; } = "RedCandy";
    public string Message { get; set; } = "You got a pink candy ! Wait is that pink no ?";
    public int Weight { get; set; } = 20;
    public EffectType Type { get; set; } = EffectType.Positive;

    public void Execute(Player player)
    {
        Scp330 candy = (Scp330)Item.Create(ItemType.SCP330);
        candy.AddCandy(InventorySystem.Items.Usables.Scp330.CandyKindID.Red);
        candy.CreatePickup(player.Position);
    }
}