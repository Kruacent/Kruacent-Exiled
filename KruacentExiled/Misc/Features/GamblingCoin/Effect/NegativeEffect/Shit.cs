using Exiled.API.Features;
using Exiled.API.Features.Items;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using KruacentExiled.Misc.Features.GamblingCoin.Types;

internal class Shit : ICoinEffect
{
    public string Name { get; set; } = "Shit";
    public string Message { get; set; } = "The tacos of the otherday was not passed very good";
    public int Weight { get; set; } = 40;
    public EffectType Type { get; set; } = EffectType.Negative;

    public void Execute(Player player)
    {
        player.PlaceTantrum();
    }
}