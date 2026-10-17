namespace KruacentExiled.GlobalEventFramework.GEFE.Commands
{
    using CommandSystem;
    using Exiled.API.Features.Pools;
    using KE.Utils.API.Commands;
    using System;
    using System.Text;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ParentCommandGEFE : KEParentCommand
    {
        public override string Command { get; } = "globalevent";
        public override string Description { get; } = "the parent command to check the Global Events";
        public override string[] Aliases { get; } = { "ge" };

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new List());
            RegisterCommand(new ForceGE());
            RegisterCommand(new ForceNbGE());
            RegisterCommand(new ForceMiddleEvent());
        }

    }

}