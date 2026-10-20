using CommandSystem;
using Exiled.API.Features;
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
    internal class GetEmptyRoom : KECommand
    {
        public override string Command => "GetEmptyRoom";

        public override string[] Aliases => new string[0];

        public override string Description => "Gets all room with no Room Spawned Custom Point";

        public override string[] Usage => new string[] { "<RoomCustomPointType>" };

        public override bool ExecuteCommand(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            StringBuilder sb = StringBuilderPool.Pool.Get();

            RoomCustomPointType type = RoomCustomPointType.Unknown;


            if (arguments.Count >= 1)
            {
                if(!Enum.TryParse(arguments.At(0), out type))
                {
                    response = "couldn't parse args";
                    return false;
                }
            }

            int i = 0;
            foreach (Room room in Room.List)
            {
                bool result;
                if(type == RoomCustomPointType.Unknown)
                {
                    result = RoomCustomPointHandler.HasASpawnedPoint(room);
                }
                else
                {
                    result = RoomCustomPointHandler.HasASpawnedPoint(room,type);
                }

                if (!result)
                {
                    i++;
                    sb.Append(room.Type)
                        .AppendLine(" doesn't have a SpawnedPoint");
                }
            }



            response = StringBuilderPool.Pool.ToStringReturn(sb);
            return true;

        }
    }
}
