using Exiled.API.Features;
using KE.Misc.Features.GamblingCoin.Interfaces;
using KE.Misc.Features.GamblingCoin.Types;
using PlayerRoles;
using System.Collections.Generic;

internal class FlipPlayerRole : ICoinEffect
{
    public string Name { get; set; } = "FlipPlayerRole";
    public string Message { get; set; } = "That's what I call an UNO reverse card !";
    public int Weight { get; set; } = 10;
    public EffectType Type { get; set; } = EffectType.Negative;

    public void Execute(Player player)
    {
        player.DropItems();
        switch (player.Role.Type)
        {
            case RoleTypeId.Scientist:
                player.Role.Set(RoleTypeId.ClassD, RoleSpawnFlags.AssignInventory);
                break;
            case RoleTypeId.ClassD:
                player.Role.Set(RoleTypeId.Scientist, RoleSpawnFlags.AssignInventory);
                break;
            case RoleTypeId.ChaosConscript:
            case RoleTypeId.ChaosRifleman:
                player.Role.Set(RoleTypeId.NtfSergeant, RoleSpawnFlags.AssignInventory);
                break;
            case RoleTypeId.ChaosMarauder:
            case RoleTypeId.ChaosRepressor:
                player.Role.Set(RoleTypeId.NtfCaptain, RoleSpawnFlags.AssignInventory);
                break;
            case RoleTypeId.FacilityGuard:
                player.Role.Set(RoleTypeId.ChaosRifleman, RoleSpawnFlags.AssignInventory);
                break;
            case RoleTypeId.NtfPrivate:
            case RoleTypeId.NtfSergeant:
            case RoleTypeId.NtfSpecialist:
                player.Role.Set(RoleTypeId.ChaosRifleman, RoleSpawnFlags.AssignInventory);
                break;
            case RoleTypeId.NtfCaptain:
                List<RoleTypeId> roles = new List<RoleTypeId>
                        {
                            RoleTypeId.ChaosMarauder,
                            RoleTypeId.ChaosRepressor
                        };
                player.Role.Set(roles.RandomItem(), RoleSpawnFlags.AssignInventory);
                break;
        }
    }
}