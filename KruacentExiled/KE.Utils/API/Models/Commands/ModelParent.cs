using CommandSystem;
using Exiled.Permissions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Utils.API.Models.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ModelParent : ParentCommand
    {
        public ModelParent()
        {
            LoadGeneratedCommands();

        }

        public override string Command { get; } = "model";

        /// <inheritdoc />
        public override string[] Aliases { get; } = { "m" };  

        /// <inheritdoc />
        public override string Description { get; } = "models";


        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new CreateModel());
            RegisterCommand(new ListModel());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = string.Empty;
            return true;
        }

    }
}
