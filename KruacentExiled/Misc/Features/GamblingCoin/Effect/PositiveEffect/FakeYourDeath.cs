using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using HarmonyLib;
using KruacentExiled.CustomRoles.CustomSCPTeam;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using KruacentExiled.Misc.Features.GamblingCoin.Types;
using KruacentExiled.Misc.Utils;
using PlayerStatsSystem;

internal class FakeYourDeath : ICoinEffect
{
    public string Name { get; set; } = "FakeYourDeath";
    public string Message { get; set; } = "Congrats ! You are now officially dead";
    public int Weight { get; set; } = 5;
    public EffectType Type { get; set; } = EffectType.Positive;

    public void Execute(Player player)
    {
        Ragdoll.CreateAndSpawn(player.Role, player.DisplayNickname, DeathTranslations.Unknown.DeathscreenTranslation, player.Position, player.ReferenceHub.PlayerCameraReference.rotation, player);
        
        if (SCPTeam.IsSCP(player.ReferenceHub)) return; // for an SCP a ragdoll will be created but the target counter don't be changed to not explode the target count
        TargetCountHider.Hide(player);
    }
}