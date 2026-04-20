using Exiled.API.Enums;
using Exiled.API.Features;
using KruacentExiled.CustomRoles.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Extensions
{
    public static class PlayerExtensions
    {
        public static void ChangeRole(this Player player, RoleTypeId newRole, SpawnReason spawnReason, RoleSpawnFlags spawnFlags)
        {
            KECustomRole oldcr = null;
            bool flag = false;
            if (KECustomRole.TryGet(player, out var roles))
            {
                oldcr = (KECustomRole)roles.FirstOrDefault();
                Log.Info("found " + oldcr.Name);
                flag = oldcr.RoleCheck(newRole);
                Log.Info("flag " + flag);

            }


            player.Role.Set(newRole, spawnReason, spawnFlags);


            if (flag)
            {
                oldcr.AddRole(player);
            }

        }
    }
}
