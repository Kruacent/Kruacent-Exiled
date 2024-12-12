
namespace GEFExiled.GEFE.Commands
{
    using CommandSystem;
    using System;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ParentCommandGEFE : ParentCommand
    {
        public ParentCommandGEFE()
        {
            LoadGeneratedCommands();
        }
        public override string Command { get; } = "globalevent";
        public override string Description { get; } = "the parent command to check the Global Events";
        public override string[] Aliases { get; } = { "ge" };

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new List());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {

            response = "";
            return true;
        }


    }

}