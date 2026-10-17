using CommandSystem;
using Exiled.API.Features.Pools;
using KE.Utils.API.Commands;
using KruacentExiled.CustomSpawnPoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Commands.DebugCommands.RoomCustomPoints
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class RecreateRoomFile : KECommand
    {
        public override string Command => "RecreateRoomFile";

        public override string[] Aliases => new string[0];

        public override string Description => "";

        public override string[] Usage => new string[0];

        public override bool ExecuteCommand(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            RoomCustomPoint.WriteToFile();
            response = "ok";
            return true;

        }
    }
}
