using Exiled.API.Features;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using KruacentExiled.Misc.Features.GamblingCoin.Types;
using PlayerRoles;
using System.Collections.Generic;
using static UnityEngine.GraphicsBuffer;
internal class FlipPlayerRole : ICoinEffect
{
    public string Name { get; set; } = "FlipPlayerRole";
    public string Message { get; set; } = "That's what I call an UNO reverse card!";
    public int Weight { get; set; } = 10;
    public EffectType Type { get; set; } = EffectType.Negative;

    private static readonly Dictionary<RoleTypeId, RoleTypeId> RoleInversions = new Dictionary<RoleTypeId, RoleTypeId>
    {
        { RoleTypeId.Scientist, RoleTypeId.ClassD },
        { RoleTypeId.ClassD, RoleTypeId.Scientist },
        { RoleTypeId.ChaosConscript, RoleTypeId.NtfSergeant },
        { RoleTypeId.ChaosRifleman, RoleTypeId.NtfSergeant },
        { RoleTypeId.ChaosMarauder, RoleTypeId.NtfCaptain },
        { RoleTypeId.ChaosRepressor, RoleTypeId.NtfCaptain },
        { RoleTypeId.FacilityGuard, RoleTypeId.ChaosRifleman },
        { RoleTypeId.NtfPrivate, RoleTypeId.ChaosRifleman },
        { RoleTypeId.NtfSergeant, RoleTypeId.ChaosRifleman },
        { RoleTypeId.NtfSpecialist, RoleTypeId.ChaosRifleman },
        { RoleTypeId.NtfCaptain, RoleTypeId.ChaosRepressor },
    };

    public void Execute(Player player)
    {
        player.DropItems();
        player.Role.Set(RoleInversions[player.Role.Type], RoleSpawnFlags.None);
    }
}