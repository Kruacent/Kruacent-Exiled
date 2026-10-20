using CommandSystem;
using KE.Utils.API.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OtherMisc = Misc;

namespace KruacentExiled.Commands.DebugCommands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class ValidateCustomInfoTestCommand : KECommand
    {
        public override string Command => "ValidateCustomInfoTest";

        public override string[] Aliases => new string[0];

        public override string Description => "Check if a custom info is valid";

        public override string[] Usage => new string[] { "<CustomInfo>" };

        public override bool ExecuteCommand(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if(arguments.Count < 1)
            {
                response = "no args";
                return false;
            }
            string oldci = arguments.At(0);

            NicknameSync.ValidateCustomInfo(oldci, out response);
            response = $"test for {oldci} :  {response} ;";
            return true;

        }
    }
}
