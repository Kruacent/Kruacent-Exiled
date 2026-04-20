using Exiled.API.Features;
using Exiled.API.Features.Items;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using KruacentExiled.Misc.Features.GamblingCoin.Types;

internal class KaboomCandy : ICoinEffect
{
    public string Name { get; set; } = "KaboomCandy";
    public string Message { get; set; } = "Pink candy kaboooom !!";
    public int Weight { get; set; } = 10;
    public EffectType Type { get; set; } = EffectType.Positive;

    public void Execute(Player player)
    {
        Scp330 candy = (Scp330)Item.Create(ItemType.SCP330);
        candy.AddCandy(InventorySystem.Items.Usables.Scp330.CandyKindID.Pink);
        candy.CreatePickup(player.Position);
    }
}