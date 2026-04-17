using Exiled.API.Features;
using Exiled.API.Features.Items;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using KruacentExiled.Misc.Features.GamblingCoin.Types;
using MEC;

internal class Vaporize : ICoinEffect
{
    public string Name { get; set; } = "Vaporize";
    public string Message { get; set; } = "I think eating mosses in your cells is not good for you";
    public int Weight { get; set; } = 1;
    public EffectType Type { get; set; } = EffectType.Negative;

    public static int MaxSeconds = 600;

    public void Execute(Player player)
    {
        Timing.CallDelayed(UnityEngine.Random.Range(30, MaxSeconds), () =>
        {
            player.Vaporize();
        });
    }
}