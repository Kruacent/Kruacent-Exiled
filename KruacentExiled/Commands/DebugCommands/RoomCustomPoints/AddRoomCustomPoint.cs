using CommandSystem;
using Exiled.API.Features;
using KE.Utils.API.Commands;
using KE.Utils.API.Features;
using KruacentExiled.CustomSpawnPoint;
using KruacentExiled.CustomSpawnPoint.Spawned;
using KruacentExiled.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KruacentExiled.Commands.DebugCommands.RoomCustomPoints
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class AddRoomCustomPoint : KECommand
    {
        public override string Command => "AddRoomCustomPoint";

        public override string[] Aliases => new string[0];

        public override string Description => "";

        public override string[] Usage => new string[] { "RoomCustomPointType", "<Round>" };



        public static RoomSpawnedCustomPoint Spawned { get; private set; } = null;
        public static RoomCustomPoint Point { get; private set; } = null;

        internal static void ResetTemporaryPoint()
        {
            Spawned = null;
            Point = null;
        }
        public override bool ExecuteCommand(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {

            Player player = Player.Get(sender);
            Room room = player.CurrentRoom;
            if (room == null)
            {
                response = "not in room";
                return false;
            }


            if(arguments.Count < 1)
            {
                response = "no args";
                return false;
            }


            if(!Enum.TryParse(arguments.At(0),out RoomCustomPointType type) || type == RoomCustomPointType.Unknown)
            {
                response = "RoomCustomPointType not found";
                return false;
            }
            Vector3 position = RoomCustomPoint.GetLocalPosition(room, player.Position);

            Quaternion rotation;
            //rotation = RoomCustomPoint.GetLocalRotation(room, player.Rotation);
            rotation = Quaternion.Euler(270,0,0);

            if (arguments.Count > 1)
            {
                string arg1 = arguments.At(1).ToUpper();
                if (arg1 == "R" || arg1 == "ROUND")
                {
                    position = position.Round();
                }
            }

            if(Spawned != null)
            {
                KELog.Debug("destroy old spawned");
                Spawned.Destroy();
            }

            


            Point = new RoomCustomPoint(room.Type, position , rotation, type);
            Spawned = RoomSpawnedCustomPoint.Create(Point, room, false);
            
            response = $"{Point.localPosition} {Point.localRotation} {Point.localRotation.eulerAngles} ({type})";
            return true;

        }
    }
}
