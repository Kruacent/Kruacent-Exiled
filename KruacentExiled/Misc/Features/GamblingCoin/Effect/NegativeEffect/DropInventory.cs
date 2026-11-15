using Exiled.API.Features;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using EffectType = KruacentExiled.Misc.Features.GamblingCoin.Types.EffectType;

public class DropInventory : ICoinEffect
{
    public string Name { get; set; } = "DropInventory";
    public string Message { get; set; } = "Empty your pocket... NOOOOWW !!";
    public int Weight { get; set; } = 8;
    public EffectType Type { get; set; } = EffectType.Negative;

    public void Execute(Player player)
    {
        player.DropItems();
    }
}