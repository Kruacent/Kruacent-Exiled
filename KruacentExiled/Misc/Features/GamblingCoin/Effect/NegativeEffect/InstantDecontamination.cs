using Exiled.API.Extensions;
using Exiled.API.Features;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using LightContainmentZoneDecontamination;
using System.Linq;
using UnityEngine;
using EffectType = KruacentExiled.Misc.Features.GamblingCoin.Types.EffectType;

internal class InstantDecontamination : ICoinEffect
{
    public string Name { get; set; } = "InstantDecontamination";
    public string Message { get; set; } = "Ouuppss you have the covid";
    public int Weight { get; set; } = 1;
    public EffectType Type { get; set; } = EffectType.Negative;

    public void Execute(Player player)
    {
        MirrorExtensions.SpawnBlood(player, player.Position, Vector3.up);
        DecontaminationController.Singleton.DecontaminationOverride = DecontaminationController.DecontaminationStatus.Forced;
    }
}