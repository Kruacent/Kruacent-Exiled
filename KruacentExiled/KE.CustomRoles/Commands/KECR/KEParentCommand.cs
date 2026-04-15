using CommandSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.CustomRoles.Commands.KECR
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class KEParentCommand : ParentCommand
    {

        public KEParentCommand()
        {
            LoadGeneratedCommands();
        }
        public override string Command => "kecustomrole";

        public override string[] Aliases => new string[1] { "kecr" };

        public override string Description => string.Empty;

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new Lists.List());
            RegisterCommand(new Give());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "Usage : list";
            return true;
        }
    }
}
