using Exiled.API.Features;
using KE.Misc.Features.GamblingCoin.Interfaces;
using KE.Misc.Features.GamblingCoin.Types;
using PlayerRoles;
using System.Collections.Generic;
using System.Linq;

internal class EveryoneChange : ICoinEffect
{
    public string Name { get; set; } = "EveryoneChange";
    public string Message { get; set; } = "You converted everyone in the opposite team !";
    public int Weight { get; set; } = 10;
    public EffectType Type { get; set; } = EffectType.Negative;

    private readonly Dictionary<RoleTypeId, RoleTypeId> RoleInversions = new Dictionary<RoleTypeId, RoleTypeId>
    {
        { RoleTypeId.ClassD, RoleTypeId.Scientist },
        { RoleTypeId.Scientist, RoleTypeId.ClassD },
        { RoleTypeId.ChaosConscript, RoleTypeId.NtfPrivate },
        { RoleTypeId.ChaosRifleman, RoleTypeId.NtfSergeant },
        { RoleTypeId.ChaosRepressor, RoleTypeId.NtfCaptain },
        { RoleTypeId.ChaosMarauder, RoleTypeId.NtfCaptain },
        { RoleTypeId.NtfPrivate, RoleTypeId.ChaosRifleman },
        { RoleTypeId.NtfSergeant, RoleTypeId.ChaosRifleman },
        { RoleTypeId.NtfCaptain, RoleTypeId.ChaosRepressor },
        { RoleTypeId.NtfSpecialist, RoleTypeId.ChaosMarauder }
    };

    public void Execute(Player caster)
    {
        foreach (Player target in Player.List.Where(p => p.IsAlive && RoleInversions.ContainsKey(p.Role.Type)))
        {
            target.Role.Set(RoleInversions[target.Role.Type], RoleSpawnFlags.None);
        }
    }
}