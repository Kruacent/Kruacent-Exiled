using Exiled.API.Features;
using KruacentExiled.CustomRoles.CR.CustomSCPs.SCP049C.UnlockableAbilities.Tier1;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using EffectType = KruacentExiled.Misc.Features.GamblingCoin.Types.EffectType;

public class BigJump : ICoinEffect, IDurationEffect
{
    public string Name { get; set; } = "BigJump";
    public string Message { get; set; } = "Congratulations, you can now jump higher";
    public int Weight { get; set; } = 5;
    public EffectType Type { get; set; } = EffectType.Positive;

    public float Duration { get; set; } = 90;

    public void Execute(Player player)
    {
        if (!player.GameObject.TryGetComponent<HigherJumpComp>(out var comp))
        {
            comp= player.GameObject.AddComponent<HigherJumpComp>();
        }
        comp.IsActive = !comp.IsActive;
    }

    public void ExecuteAfterDuration(Player player)
    {
        if (player.GameObject.TryGetComponent<HigherJumpComp>(out var comp))
        {
            comp.Destroy();
        }
    }
}