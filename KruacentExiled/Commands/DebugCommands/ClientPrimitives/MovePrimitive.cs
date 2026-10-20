using CommandSystem;
using Exiled.API.Features;
using KE.Utils.API.Commands;
using KruacentExiled.ClientPrimitives;
using System;
using UnityEngine;

namespace KruacentExiled.Commands.DebugCommands.ClientPrimitives
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class MovePrimitive : KECommand
    {
        public override string Command => "moveprimitive";

        public override string[] Aliases => new string[] { "mp" };

        public override string Description => "";

        public override string[] Usage => new string[0];



        public override bool ExecuteCommand(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            SpawnAtPlayer.Primitive.Position = player.Position;
           
            response = "ok";

            return true;
        }
    }
}
