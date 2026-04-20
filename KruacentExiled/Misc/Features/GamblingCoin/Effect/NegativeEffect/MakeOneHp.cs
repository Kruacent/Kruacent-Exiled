using Exiled.API.Features;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using KruacentExiled.Misc.Features.GamblingCoin.Types;

internal class MakeOneHP : ICoinEffect
{
    public string Name { get; set; } = "MakeOneHP";
    public string Message { get; set; } = "";
    public int Weight { get; set; } = 15;
    public EffectType Type { get; set; } = EffectType.Negative;

    public void Execute(Player player)
    {
        if (player.Health > 1)
        {
            player.Health = 1;
        } else
        {
            player.Hurt(99999, "No luck");
        }
    }
}