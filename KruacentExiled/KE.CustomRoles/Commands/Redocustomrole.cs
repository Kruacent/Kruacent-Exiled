using CommandSystem;
using Exiled.API.Features;
using Exiled.CustomRoles;
using KE.CustomRoles.API.Features;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.CustomRoles.Commands
{

    [CommandHandler(typeof(ClientCommandHandler))]
    public class Redocustomrole : ICommand
    {
        public string Command => "rcr";

        public string[] Aliases => [];

        public string Description => "redo the custom role";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            response = "no";
            if (player is not null)
            {
                KECustomRole ke = null;
                RoleTypeId role = player.Role;
                List<KECustomRole> list = KECustomRole.Get(player).ToList();
                if (list is null)
                {
                    response = "no cr";
                    return false;
                }

                ke = list[0];


                if(role == RoleTypeId.ClassD)
                {
                    player.Role.Set(RoleTypeId.Scientist, RoleSpawnFlags.None);
                }
                else
                {
                    player.Role.Set(RoleTypeId.ClassD, RoleSpawnFlags.None);
                }
                ke.RemoveRole(player);
                Timing.CallDelayed(.1f, delegate
                {
                    ke.AddRole(player);
                });
                
            }


            response = "ok";
            return true;
        }
    }
}
