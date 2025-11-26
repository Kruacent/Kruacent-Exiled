using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Toys;
using Exiled.CustomRoles.API.Features;
using KE.CustomRoles.API.Features;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.CustomRoles.Abilities
{
    public class Convert : KEAbilities
    {
        public override string Name { get;  } = "Convert";
        public override string PublicName { get; } = "Convert";

        public override string Description { get; } = "Convert a zombie to your team";

        public override float Cooldown { get;  } = 10*60f;

        public float MaxDistance { get; set; } = 5f;


        protected override bool AbilityUsed(Player player)
        {

            if (!Physics.Linecast(player.Position, player.Position + player.Rotation.eulerAngles * MaxDistance, out RaycastHit hit)) return false;




            Player playerHit = Player.Get(hit.collider);

            if (playerHit == null) return false;

            if (playerHit.Role.Side == player.Role.Side) return false;

            if (playerHit.IsScp && playerHit.Role != RoleTypeId.Scp0492) return false;


            if (playerHit.IsScp)
            {
                playerHit.Role.Set(player.Role, RoleSpawnFlags.AssignInventory);
            }
            else
            {
                playerHit.Role.Set(player.Role, RoleSpawnFlags.None);
            }


            return base.AbilityUsed(player);
        }

    }
}
