using Exiled.API.Features;
using KE.Utils.API.Features.SCPs;
using KE.Misc.Features.GamblingCoin.Interfaces;
using EffectType = KE.Misc.Features.GamblingCoin.Types.EffectType;

internal class NightVision : ICoinEffect
{
    public string Name { get; set; } = "NightVision";
    public string Message { get; set; } = "Yippee, you can see in the dark";
    public int Weight { get; set; } = 5;
    public EffectType Type { get; set; } = EffectType.Positive;
    public float Duration { get; set; } = 30;

    public void Execute(Player player)
    {
        if (SCPTeam.IsSCP(player.ReferenceHub)) return;
        player.EnableEffect<CustomPlayerEffects.NightVision>(100, Duration, true);
    }
}