using CommandSystem;
using Exiled.API.Features.Pools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Map.Utils
{
    public abstract class KEParentCommand : ParentCommand
    {

        public KEParentCommand()
        {
            LoadGeneratedCommands();
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            StringBuilder builder = StringBuilderPool.Pool.Get();

            builder.Append("Usage : ");
            builder.AppendLine();
            foreach (ICommand command in Commands.Values)
            {
                builder.Append(command.Command);
                string[] aliases = command.Aliases;
                if(aliases.Length > 0)
                {
                    builder.Append(" (");
                    for (int i = 0; i < aliases.Length; i++)
                    {
                        builder.Append(aliases[i]);
                        if (i < aliases.Length - 1)
                        {
                            builder.Append(", ");
                        }
                    }
                    builder.Append(")");
                }
                builder.Append(" - ");
                

                builder.AppendLine(command.Description);
            }



            response = builder.ToString();
            StringBuilderPool.Pool.Return(builder);
            return false;
        }
    }
}
