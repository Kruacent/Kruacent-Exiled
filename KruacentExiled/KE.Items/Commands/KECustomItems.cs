using CommandSystem;
using KE.CustomRoles.Commands.KECR;
using System;

namespace KE.Items.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class KECustomItems : KE.Utils.API.Commands.KEParentCommand
    {
        public override string Command => "keci";

        public override string[] Aliases => new string[0];

        public override string Description => "kecustom item parent command";

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new List());
            RegisterCommand(new Give());
        }

    }
}
