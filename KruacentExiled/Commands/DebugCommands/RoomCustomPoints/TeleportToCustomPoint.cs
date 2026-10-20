using CommandSystem;
using Exiled.API.Features;
using KE.Utils.API.Commands;
using KruacentExiled.CustomSpawnPoint.Spawned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KruacentExiled.Commands.DebugCommands.RoomCustomPoints
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class TeleportToCustomPoint : KECommand
    {

        public override string Command => "TeleportToCustomPoint";

        public override string[] Aliases => new string[0];

        public override string Description => "";

        public override string[] Usage => new string[0];

        public override bool ExecuteCommand(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if(!Player.TryGet(sender,out Player player))
            {
                response = "no player";
                return false;
            }
            


            RoomSpawnedCustomPoint near = RoomSpawnedCustomPoint.All.OrderBy(s => Vector3.Distance(player.Position, s.Position)).FirstOrDefault();

            player.Teleport(near.Position);

            response = $"tped to at {near.Position} {near.Rotation} ({near.type})";
            return true;

        }
    }
}
