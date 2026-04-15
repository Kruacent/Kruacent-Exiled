using CommandSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.CustomRoles.Commands.KECR.Lists
{
    public class List : ParentCommand
    {
        public List()
        {
            LoadGeneratedCommands();
        }
        public override string Command => "list";

        public override string[] Aliases => new string[] { "l" };

        public override string Description => "";

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(Registered.Instance);
            RegisterCommand(new Abilities());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.IsEmpty() && TryGetCommand(Registered.Instance.Command, out var command))
            {
                command.Execute(arguments, sender, out response);
                response = response + "\nTo view all abilities registered use command: " + string.Join(" ", arguments.Array) + " abilities";
                return true;
            }
            response = "Usage : registered, abilities";
            return false;
        }
    }
}
