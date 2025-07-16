using CommandSystem;
using KE.Utils.API.Models.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Map
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class ModelParent : ParentCommand
    {
        public ModelParent()
        {
            if (MainPlugin.Configs?.Debug ?? false)
            {
                LoadGeneratedCommands();
            }

        }

        public override string Command => "model";
        public override string Description => "";
        public override string[] Aliases { get; } = {"m"};

        public override void LoadGeneratedCommands()
        {
            foreach (ICommand command in KE.Utils.API.Models.Commands.AllCommands.Get())
            {
                RegisterCommand(command);
            }
        }

        

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "\n";
            foreach (ICommand command in Commands.Values)
            {
                response += command.Command + $" ({command.Aliases[0]}) -" + command.Description + "\n";
            }
            return true;
        }
    }
}
