using Exiled.API.Features;
using KE.Misc.Features.GamblingCoin.Interfaces;
using PlayerRoles;
using System.Linq;
using EffectType = KE.Misc.Features.GamblingCoin.Types.EffectType;

internal class TeleportToEnemy : ICoinEffect
{
    public string Name { get; set; } = "TeleportToEnemy";
    public string Message { get; set; } = "You were teleported to an enemy !!";
    public int Weight { get; set; } = 30;
    public EffectType Type { get; set; } = EffectType.Negative;

    public void Execute(Player player)
    {
        var scps = Player.List.Where(p => p.IsScp && p.Role != RoleTypeId.Scp079 && p != player).ToList();

        Player target = null;

        if (scps.Count > 0)
        {
            target = scps[UnityEngine.Random.Range(0, scps.Count)];
        }
        else
        {
            var enemies = Player.List
                .Where(p => p != player && p.Role.Team != player.Role.Team)
                .ToList();

            if (enemies.Count > 0)
                target = enemies[UnityEngine.Random.Range(0, enemies.Count)];
        }

        if (target != null)
        {
            player.Position = target.Position;
        }
    }
}