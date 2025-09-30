using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using KE.Misc.Features.GamblingCoin.Interfaces;
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
        List<Exiled.API.Enums.EffectType> effect = new List<Exiled.API.Enums.EffectType>();
        effect.Where(e => e.GetCategories() == EffectCategory.Negative);

        player.EnableEffect(effect.GetRandomValue(), 9999999);
    }
}