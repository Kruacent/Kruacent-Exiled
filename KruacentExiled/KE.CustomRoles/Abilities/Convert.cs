using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.CustomRoles.Abilities
{
    [CustomAbility]
    public class Convert : ActiveAbility
    {
        public override string Name { get; set; } = "Convert";

        public override string Description { get; set; } = "";

        public override float Duration { get; set; } = 0f;

        public override float Cooldown { get; set; } = 10*60f;

        public float MaxDistance { get; set; } = 5f;

        protected override void AbilityUsed(Player player)
        {
            Physics.Linecast(player.Position, player.Position + player.Rotation.eulerAngles * MaxDistance,out RaycastHit hit);

            Player playerHit = Player.Get(hit.collider);

            if (playerHit == null) return;

            if (playerHit.Role.Side == player.Role.Side) return;

            if (playerHit.IsScp && playerHit.Role != RoleTypeId.Scp0492) return;


            if (playerHit.IsScp)
            {
                playerHit.Role.Set(player.Role, RoleSpawnFlags.AssignInventory);
            }
            else
            {
                playerHit.Role.Set(player.Role, RoleSpawnFlags.None);
            }

        }

    }
}
