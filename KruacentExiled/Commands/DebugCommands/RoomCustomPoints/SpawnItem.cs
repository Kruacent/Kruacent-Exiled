using CommandSystem;
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
    internal class SpawnItem : KECommand
    {
        public override string Command => "SpawnItem";

        public override string[] Aliases => new string[0];

        public override string Description => "";

        public override string[] Usage => new string[] { "ItemType" };

        public override bool ExecuteCommand(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (AddRoomCustomPoint.Point == null)
            {
                response = "no point set";
                return false;
            }

            ItemType item = ItemType.Painkillers;

            if(arguments.Count > 0 && !Enum.TryParse(arguments.At(0),out item))
            {
                response = "couldn't parse";
                return false;
            }

            if(item == ItemType.None)
            {
                response = "can't spawn this item";
                return false;
            }

            AddRoomCustomPoint.Spawned.SpawnItem(item);


            response = "spawned " + item;
            return true;
        }
    }
}
