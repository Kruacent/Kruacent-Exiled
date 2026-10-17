using CommandSystem;
using KE.Misc.Features._914Upgrades;
using KE.Utils.API.Commands;
using KruacentExiled.GlobalEventFramework.GEFE.API.Features;
using Scp914;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Commands.DebugCommands.GlobalEvents
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    internal class ChooseRandomGlobalEvent : KECommand
    {
        public override string Command => "ChooseRandomGlobalEvent";

        public override string[] Aliases => new string[0];

        public override string Description => "";

        public override string[] Usage => new string[0];

        public override bool ExecuteCommand(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            int nb = -1;
            if (arguments.Count > 0)
            {
                if (!int.TryParse(arguments.At(0),out nb))
                {

                    response = "can't parse";
                    return false;
                }
            }
            GlobalEvent.ChooseGlobalEvent(nb);

            response = "ok";
            return true;
        }
    }
}
