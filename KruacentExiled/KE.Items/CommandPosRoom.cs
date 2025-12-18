using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using KE.Items.API.Features.SpawnPoints;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Items.Patches
{

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class CommandPos : ICommand
    {
        public string Command => "posroom";

        public string[] Aliases => [];

        public string Description => "position";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var p = Player.Get(sender);
            response = string.Empty;

            if(p is not null)
            {
                Room room = p.CurrentRoom;

                if(room is null)
                {
                    response = "no room";
                    return false;
                }

                PoseRoomSpawnPointHandler.ShowPoses(room.Type);
                response= "ok";
                return true;
            }
            response = "no";
            return false;
        }
    }
}
