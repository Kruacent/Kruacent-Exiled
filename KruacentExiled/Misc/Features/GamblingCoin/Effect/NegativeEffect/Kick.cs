using Exiled.API.Features;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using KruacentExiled.Misc.Features.GamblingCoin.Types;

internal class Kick : ICoinEffect
{
    public string Name { get; set; } = "Kick";
    public string Message { get; set; } = "Bye !";
    public int Weight { get; set; } = 5;
    public EffectType Type { get; set; } = EffectType.Negative;

    public void Execute(Player player)
    {
        player.Kick("You gamble too much !");
    }
}