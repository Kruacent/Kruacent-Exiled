using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using KE.Utils.API.Models.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Map.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ModelParent : ParentCommand
    {
        public ModelParent()
        {
            Log.Debug("loading");
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
            RegisterCommand(new SelectModel());
            RegisterCommand(new ShowCenter());
            RegisterCommand(new ModeMovePrim());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "hoi";
            return true;
        }

    }
}
