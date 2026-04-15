using CommandSystem;
using Exiled.API.Features.Pools;
using KE.Utils.API.Commands;
using RelativePositioning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Map.Others.BlackoutNDoor.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class MainCommand : KEParentCommand
    {
        public override string Command => "mapevent";

        public override string[] Aliases => new string[] { "me" };

        public override string Description => "mapevents";

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new BlackoutCommand());
            RegisterCommand(new DoorstuckCommand());
            RegisterCommand(new BothCommand());
        }

    }
}
