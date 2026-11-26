using CommandSystem;
using Exiled.API.Features;
using KE.Utils.API.Commands;
using KruacentExiled.CustomRoles.CR.Human;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Commands.DebugCommands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class ForceShowProbableSCP : KECommand
    {
        public override string Command => "ForceShowProbableSCP";

        public override string[] Aliases => new string[0];

        public override string Description => "force the probable scp like Alzheimer";

        public override string[] Usage => new string[0];

        public override bool ExecuteCommand(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {


            if (!Player.TryGet(sender, out Player player))
            {
                response = "wrong player";
                return false;
            }

            Alzheimer.CreateSpawnHint(player);
            response = "ok";
            return true;

        }
    }
}
