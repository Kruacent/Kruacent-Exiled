using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using KE.Misc.Features.GamblingCoin.Interfaces;
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
        List<Exiled.API.Enums.EffectType> effect = new List<Exiled.API.Enums.EffectType>();
        effect.Where(e => e.GetCategories() == EffectCategory.Positive);

        player.EnableEffect(effect.GetRandomValue(), 9999999);
    }
}