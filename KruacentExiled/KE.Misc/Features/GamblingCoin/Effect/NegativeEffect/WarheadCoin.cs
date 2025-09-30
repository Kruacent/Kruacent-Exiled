using Exiled.API.Features;
using KE.Misc.Features.GamblingCoin.Interfaces;
using EffectType = KE.Misc.Features.GamblingCoin.Types.EffectType;

internal class WarheadCoin : ICoinEffect
{
    public string Name { get; set; } = "Warhead";
    public string Message { get; set; } = "YOU TOUCHED THE RED BUTTON !?!";
    public int Weight { get; set; } = 10;
    public EffectType Type { get; set; } = EffectType.Negative;

    public void Execute(Player player)
    {
        if (!Warhead.IsDetonated || !Warhead.IsInProgress)
            Warhead.Start();
        else
            Warhead.Stop();
    }
}