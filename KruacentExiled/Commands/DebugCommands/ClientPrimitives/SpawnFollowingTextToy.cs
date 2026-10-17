using CommandSystem;
using Exiled.API.Features;
using KE.Utils.API.Commands;
using KruacentExiled.ClientPrimitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KruacentExiled.Commands.DebugCommands.ClientPrimitives
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class SpawnFollowingTextToy : KECommand
    {
        public override string Command => "SpawnFollowingTextToy";

        public override string[] Aliases => new string[] { "sftt" };

        public override string Description => "";

        public override string[] Usage => new string[0];


        public static ClientFollowingTextToy Text;

        public override bool ExecuteCommand(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            Text = new ClientFollowingTextToy(new List<Player> () { player } ,player.Position, Quaternion.identity, Vector3.one, "soucicse");

            Log.Info(player.Position);
            response = "ok";

            return true;
        }
    }
}
