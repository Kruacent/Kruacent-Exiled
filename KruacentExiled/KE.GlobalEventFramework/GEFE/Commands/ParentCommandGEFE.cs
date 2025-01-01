
namespace KE.GlobalEventFramework.GEFE.Commands
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
            RegisterCommand(new ListGE());
            RegisterCommand(new ForceGE());
            RegisterCommand(new ForceNbGE());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if(arguments.Count == 0){
                response = "subcommand available : list, force, nb";
                return true;
            }
            response = "";
            return true;
        }


    }

}