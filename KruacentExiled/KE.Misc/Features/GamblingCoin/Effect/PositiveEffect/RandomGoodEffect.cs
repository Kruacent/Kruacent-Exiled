using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using KE.Misc.Features.GamblingCoin.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

internal class RandomGoodEffect : ICoinEffect
{
    public string Name { get; set; } = "RandomGoodEffect";
    public string Message { get; set; } = "You're wish is accepted !";
    public int Weight { get; set; } = 10;
    public KE.Misc.Features.GamblingCoin.Types.EffectType Type { get; set; } = KE.Misc.Features.GamblingCoin.Types.EffectType.Positive;

    public void Execute(Player player)
    {
        var positiveEffects = Enum.GetValues(typeof(EffectType))
            .Cast<EffectType>()
            .Where(e => e.GetCategories().HasFlag(EffectCategory.Positive))
            .ToList();

        if (positiveEffects.Count == 0)
            return;

        var randomEffect = positiveEffects[UnityEngine.Random.Range(0, positiveEffects.Count)];

        player.EnableEffect(randomEffect, 45);
    }
}