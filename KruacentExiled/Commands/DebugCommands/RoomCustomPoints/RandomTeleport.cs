using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features;
using KE.Utils.API.Commands;
using KruacentExiled.CustomSpawnPoint;
using KruacentExiled.CustomSpawnPoint.Spawned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Commands.DebugCommands.RoomCustomPoints
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class RandomTeleport : KECommand
    {
        public override string Command => "RandomCustomPointTeleport";

        public override string[] Aliases => new string[0];

        public override string Description => "Randomly teleport a RoomCustomPoint Teleport";

        public override string[] Usage => new string[] { "FacilityZone" };

        public override bool ExecuteCommand(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if(!Player.TryGet(sender,out Player player))
            {
                response = "no player";
                return false;
            }

            RoomSpawnedCustomPoint point = RoomCustomPointHandler.GetRandomPoint(RoomCustomPointType.Teleport,ZoneType.LightContainment);

            player.Teleport(point.Position);

            response = "ok";
            return true;

        }
    }
}
