using CommandSystem;
using Exiled.API.Features;
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
    internal class ConfirmAdd : KECommand
    {
        public override string Command => "ConfirmAdd";

        public override string[] Aliases => new string[0];

        public override string Description => "";

        public override string[] Usage => new string[0];

        public override bool ExecuteCommand(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {

            if(AddRoomCustomPoint.Point == null)
            {
                response = "no point set";
                return false;
            }

            RoomCustomPoint.Add(AddRoomCustomPoint.Point);
            AddRoomCustomPoint.Spawned.Destroy();
            AddRoomCustomPoint.Point.Spawn();
            


            response = $"confirmed the spawn of the CPT at {AddRoomCustomPoint.Point.roomType} {AddRoomCustomPoint.Point.localPosition} {AddRoomCustomPoint.Point.localRotation} ({AddRoomCustomPoint.Spawned.type})";
            AddRoomCustomPoint.ResetTemporaryPoint();
            return true;

        }
    }
}
