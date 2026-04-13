
namespace KE.GlobalEventFramework.GEFE.Commands
{
    using CommandSystem;
    using Exiled.API.Features.Pools;
    using System;
    using System.Text;

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
            RegisterCommand(new ForceGE());
            RegisterCommand(new ForceNbGE());
            RegisterCommand(new ForceMiddleEvent());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {


            if (arguments.Count > 0)
            {
                response = "";
                return true;
            }

            StringBuilder builder = StringBuilderPool.Pool.Get();
            builder.AppendLine();
            foreach (ICommand command in AllCommands)
            {
                builder.Append(command.Command);

                builder.Append("(");
                string[] alias = command.Aliases;
                for (int i = 0; i < alias.Length; i++)
                {
                    builder.Append(alias[i]);
                    if (alias.Length > 1 && i < alias.Length - 1)
                    {
                        builder.Append(", ");
                    }
                }
                builder.Append(") - ");

                builder.Append(command.Description);
                builder.AppendLine();
            }


            response = builder.ToString();
            StringBuilderPool.Pool.Return(builder);
            return false;
        }


    }

}