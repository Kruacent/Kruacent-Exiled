using Exiled.API.Features;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using KruacentExiled.Misc.Features.GamblingCoin.Types;
using UnityEngine;

internal class Gnomed : ICoinEffect
{
    public string Name { get; set; } = "Gnomed";
    public string Message { get; set; } = "You got gnomed.";
    public int Weight { get; set; } = 20;
    public EffectType Type { get; set; } = EffectType.Positive;

    public void Execute(Player player)
    {
        player.Scale = new Vector3(1.13f, 0.5f, 1.13f);
    }
}