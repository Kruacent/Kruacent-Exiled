using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using KE.Misc.Features.GamblingCoin.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

internal class RandomBadEffect : ICoinEffect
{
    public string Name { get; set; } = "RandomBadEffect";
    public string Message { get; set; } = "You got a random effect !";
    public int Weight { get; set; } = 20;
    public KE.Misc.Features.GamblingCoin.Types.EffectType Type { get; set; } = KE.Misc.Features.GamblingCoin.Types.EffectType.Positive;

    public void Execute(Player player)
    {
        var positiveEffects = Enum.GetValues(typeof(EffectType))
            .Cast<EffectType>()
            .Where(e => e.GetCategories().HasFlag(EffectCategory.Negative))
            .ToList();

        if (positiveEffects.Count == 0)
            return;

        var randomEffect = positiveEffects[UnityEngine.Random.Range(0, positiveEffects.Count)];

        player.EnableEffect(randomEffect, 45);
    }
}