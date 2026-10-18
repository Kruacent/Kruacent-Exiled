using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using KE.Utils.API.Commands;
using KruacentExiled.CustomSpawnPoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KruacentExiled.Commands.DebugCommands.RoomCustomPoints
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class GetAllRoomCustomPoint : KECommand
    {
        public override string Command => "GetAllRoomCustomPoint";

        public override string[] Aliases => new string[] { "garcp" };

        public override string Description => "";

        public override string[] Usage => new string[0];

        public override bool ExecuteCommand(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {

            StringBuilder sb = StringBuilderPool.Pool.Get();
            sb.AppendLine();

            foreach (RoomCustomPoint roomCustomPoint in RoomCustomPoint.All)
            {

                sb.Append(roomCustomPoint.roomType)
                    .Append(" - ")
                    .Append(roomCustomPoint.localPosition)
                    .AppendLine();
            }

            response = StringBuilderPool.Pool.ToStringReturn(sb);
            return true;

        }
    }
}
