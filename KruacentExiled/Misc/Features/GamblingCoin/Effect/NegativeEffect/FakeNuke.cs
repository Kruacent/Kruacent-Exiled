using Exiled.API.Features;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using KruacentExiled.Misc.Features.GamblingCoin.Types;

internal class FakeNuke : IDurationEffect
{
    public string Name { get; set; } = "FakeNuke";
    public string Message { get; set; } = string.Empty;
    public int Weight { get; set; } = 5;
    public EffectType Type { get; set; } = EffectType.Negative;
    public float Duration { get; set; } = 15;

    public void Execute(Player player)
    {
        if(!Warhead.IsDetonated) Warhead.Start();
    }

    public void ExecuteAfterDuration(Player player)
    {
        if (Warhead.IsInProgress && Warhead.Controller.Info.ScenarioType != WarheadScenarioType.DeadmanSwitch)
        {
            Warhead.Stop();
        }
    }
}