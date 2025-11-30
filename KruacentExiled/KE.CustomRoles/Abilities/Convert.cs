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

        public float MaxDistance { get; set; } = 15f;


        protected override bool AbilityUsed(Player player)
        {
            if (!Physics.Raycast(player.Position+ player.CameraTransform.rotation *Vector3.forward, player.Rotation.eulerAngles, out RaycastHit hit)) return false;


            Player playerHit = Player.Get(hit.collider);

            if (playerHit == null)
            {
                MainPlugin.ShowEffectHint(player, "But nobody's here");
                return false;
            }

            if (playerHit.Role.Side == player.Role.Side)
            {
                MainPlugin.ShowEffectHint(player, "I know you don't like them but they're in your team");
                return false;
            }

            if (playerHit.IsScp && playerHit.Role != RoleTypeId.Scp0492)
            {
                MainPlugin.ShowEffectHint(player, "That ain't a zombie");
                return false;
            }


            if (playerHit.IsScp)
            {
                playerHit.Role.Set(player.Role, RoleSpawnFlags.AssignInventory);
            }
            else
            {
                playerHit.Role.Set(player.Role, RoleSpawnFlags.None);
            }

            MainPlugin.ShowEffectHint(player, "New friend acquired!");
            return base.AbilityUsed(player);
        }

    }
}
