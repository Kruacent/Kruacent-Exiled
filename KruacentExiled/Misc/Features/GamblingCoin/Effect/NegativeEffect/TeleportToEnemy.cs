using Exiled.API.Extensions;
using Exiled.API.Features;
using KruacentExiled.CustomRoles.CustomSCPTeam;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using System.Linq;
using EffectType = KruacentExiled.Misc.Features.GamblingCoin.Types.EffectType;

namespace KruacentExiled.Misc.Features.GamblingCoin.Effect.NegativeEffect
{
    internal class TeleportToEnemy : ICoinEffect
    {
        public string Name { get; set; } = "TeleportToEnemy";
        public string Message { get; set; } = "You were teleported to an enemy !!";
        public int Weight { get; set; } = 30;
        public EffectType Type { get; set; } = EffectType.Negative;

        public void Execute(Player player)
        {
            Player enemy = Player.List.Where(p => p != player && p.Role.Team != player.Role.Team || SCPTeam.IsSCP(p.ReferenceHub)).GetRandomValue();

            player.Teleport(enemy);
        }
    }
}
