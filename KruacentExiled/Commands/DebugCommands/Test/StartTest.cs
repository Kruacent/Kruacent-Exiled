using CommandSystem;
using KE.Utils.API.Commands;
using KruacentExiled.CustomRoles.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Commands.DebugCommands.Test
{
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    internal class StartTest : KECommand
    {
        public override string Command => "StartTest";

        public override string[] Aliases => new string[] { "st" };

        public override string Description => "Start all tests";

        public override string[] Usage => new string[0];

        public override bool ExecuteCommand(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {

            MainPlugin.TestHandler.Start();
            response = "ok";
            return true;


        }
    }
}
