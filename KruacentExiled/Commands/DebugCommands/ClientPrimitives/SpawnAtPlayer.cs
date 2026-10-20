using CommandSystem;
using Exiled.API.Features;
using KE.Utils.API.Commands;
using KruacentExiled.ClientPrimitives;
using System;
using UnityEngine;

namespace KruacentExiled.Commands.DebugCommands.ClientPrimitives
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class SpawnAtPlayer : KECommand
    {
        public override string Command => "spawnclientprimitive";

        public override string[] Aliases => new string[] { "scp" };

        public override string Description => "Spawn a client primitives at the player's position";

        public override string[] Usage => new string[0];


        public static ClientSidePrimitive Primitive;

        public override bool ExecuteCommand(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            Primitive = new ClientSidePrimitive(player.Position,Quaternion.identity,Vector3.one,PrimitiveType.Cube,Color.blue,AdminToys.PrimitiveFlags.Visible,player);

            Primitive.SpawnClientPrimitive();

            response = "ok";

            return true;
        }
    }
}
