using CommandSystem;
using KE.Utils.API.Commands;

namespace KruacentExiled.Map.Others.BlackoutNDoor.Commands
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
