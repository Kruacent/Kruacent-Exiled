using CommandSystem;
using Exiled.API.Features;
using KE.Utils.API.Commands;
using System;
using UnityEngine;

namespace KruacentExiled.Commands.DebugCommands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class TeleportTo : KECommand
    {
        public override string Command => "teleportto";

        public override string[] Aliases => new string[] { "tp" };

        public override string Description => "";

        public override string[] Usage => new string[] { "<x> <y> <z>" };

        public override bool ExecuteCommand(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {


            if(!Player.TryGet(sender,out Player player))
            {
                response = "wrong player";
                return false;
            }

            if(arguments.Count < 3)
            {
                response = "no coordinate";
                return false;
            }

            float x = float.Parse(arguments.At(0));
            float y = float.Parse(arguments.At(1));
            float z = float.Parse(arguments.At(2));

            Vector3 position = new Vector3(x, y, z);

            player.Teleport(position);
            response = "teleported to " + position;
            return true;

        }
    }
}
