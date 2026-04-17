using CustomPlayerEffects;
using Exiled.API.Features;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using EffectType = KruacentExiled.Misc.Features.GamblingCoin.Types.EffectType;

internal class Reversed : IDurationEffect
{
    public string Name { get; set; } = "Reversed";
    public string Message { get; set; } = "Oops ! I connected your keyboard in the wrong way, sorryyyy";
    public float Duration { get; set; } = 30;
    public int Weight { get; set; } = 5;
    public EffectType Type { get; set; } = EffectType.Negative;

    public void Execute(Player player)
    {
        StatusEffectBase effect = player.GetEffect(Exiled.API.Enums.EffectType.Slowness);
        if (effect.Intensity >= 200)
        {
            effect.Intensity = 0;
        } else
        {
            player.EnableEffect(Exiled.API.Enums.EffectType.Slowness, 200);
        }
    }

    public void ExecuteAfterDuration(Player player)
    {
        StatusEffectBase effect = player.GetEffect(Exiled.API.Enums.EffectType.Slowness);
        if (effect.Intensity >= 200)
        {
            effect.Intensity = 0;
        }
        else
        {
            player.EnableEffect(Exiled.API.Enums.EffectType.Slowness, 200);
        }
    }
}