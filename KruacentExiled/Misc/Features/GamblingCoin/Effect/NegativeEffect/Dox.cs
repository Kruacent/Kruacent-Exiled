using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using KruacentExiled.CustomRoles.CustomSCPTeam;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using KruacentExiled.Misc.Features.GamblingCoin.Types;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KruacentExiled.Misc.Features.GamblingCoin.Effect.NegativeEffect
{
    public class Dox : ICoinEffect
    {
        public string Name { get; set; } = "Dox";
        public string Message { get; set; } = "";
        public int Weight { get; set; } = 10;
        public EffectType Type { get; set; } = EffectType.Negative;

        public void Execute(Player player)
        {
            string first = player.IPAddress.Split('.')[0];

            byte second = (byte) UnityEngine.Random.Range(1, 255);
            byte third = (byte) UnityEngine.Random.Range(1, 255);

            int random = UnityEngine.Random.Range(0, 24) * 10;


            byte last = (byte)Mathf.Clamp(random + player.Id,0,byte.MaxValue);

            PlayerUtils.SendBroadcast(player, $"You've been doxxed : {first}.{second}.{third}.{last}");

            if (SCPTeam.SCPs.Count == 0)
            {
                return;
            }

            Npc npc = Npc.Spawn("dummy", PlayerRoles.RoleTypeId.Scp079, true);

            Timing.CallDelayed(Npc.SpawnSetRoleDelay, () =>
            {
                Scp079Role role = npc.Role as Scp079Role;


                role.Ping(player.Position, Exiled.API.Enums.PingType.Human, false);
                Timing.CallDelayed(.1f, () =>
                {
                    npc.Destroy();
                });
            });
        }
    }
}
