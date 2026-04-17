using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using KruacentExiled.Misc.Features.GamblingCoin.Types;
using System;
using System.Linq;

internal class RandomBadEffect : ICoinEffect
{
    public string Name { get; set; } = "RandomBadEffect";
    public string Message { get; set; } = "You got a random effect !";
    public int Weight { get; set; } = 20;
    public KruacentExiled.Misc.Features.GamblingCoin.Types.EffectType Type { get; set; } = KruacentExiled.Misc.Features.GamblingCoin.Types.EffectType.Positive;

    public void Execute(Player player)
    {
        var negativeEffects = Enum.GetValues(typeof(Exiled.API.Enums.EffectType))
            .Cast<Exiled.API.Enums.EffectType>()
            .Where(e => e.GetCategories().HasFlag(EffectCategory.Negative))
            .ToList();

        if (negativeEffects.Count == 0)
        {
            Log.Warn("no negative effect found");
            return;
        }
            

        var randomEffect = negativeEffects[UnityEngine.Random.Range(0, negativeEffects.Count)];

        player.EnableEffect(randomEffect, 5, true);
    }
}