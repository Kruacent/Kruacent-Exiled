using Exiled.API.Features;
using KE.Misc.Features.GamblingCoin.Interfaces;
using EffectType = KE.Misc.Features.GamblingCoin.Types.EffectType;

internal class TurnOffLight : ICoinEffect
{
    public string Name { get; set; } = "TurnOffLight";
    public string Message { get; set; } = string.Empty;
    public int Weight { get; set; } = 10;
    public EffectType Type { get; set; } = EffectType.Negative;

    public static int BlackoutTime = 10;

    public void Execute(Player player)
    {
        Map.TurnOffAllLights(BlackoutTime);
    }
}