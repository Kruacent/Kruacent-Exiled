using Exiled.API.Features;
using KE.Misc.Features.GamblingCoin.Interfaces;
using KE.Misc.Features.GamblingCoin.Types;
using UnityEngine;

internal class Erased : ICoinEffect
{
    public string Name { get; set; } = "Erased";
    public string Message { get; set; } = "Il y a un camion qui t'as roulé dessus.";
    public int Weight { get; set; } = 20;
    public EffectType Type { get; set; } = EffectType.Positive;

    public void Execute(Player player)
    {
        player.Scale = new Vector3(1.13f, 0.2f, 1.13f);
    }
}