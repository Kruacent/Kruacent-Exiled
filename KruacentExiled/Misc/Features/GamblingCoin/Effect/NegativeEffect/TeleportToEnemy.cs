using Exiled.API.Extensions;
using Exiled.API.Features;
using KE.Utils.API.Features.SCPs;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using PlayerRoles;
using System.Collections.Generic;
using System.Linq;
using EffectType = KruacentExiled.Misc.Features.GamblingCoin.Types.EffectType;

internal class TeleportToEnemy : ICoinEffect
{
    public string Name { get; set; } = "TeleportToEnemy";
    public string Message { get; set; } = "You were teleported to an enemy !!";
    public int Weight { get; set; } = 30;
    public EffectType Type { get; set; } = EffectType.Negative;

    public void Execute(Player player)
    {


        List<ReferenceHub> scp = SCPTeam.SCPs.ToList();


        Player target = null;

        if (scp.Count > 0)
        {
            target = Player.Get(scp.GetRandomValue());
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
            player.Teleport(target.Position);
        }
    }
}