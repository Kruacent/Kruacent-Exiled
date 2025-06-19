using CommandSystem;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Utils.API.Models.Commands
{
    internal class ListModel : ICommand
    {

        public string Command { get; } = "list";

        public string[] Aliases { get; } = { "l" };

        public string Description { get; } = "create a new model at your position";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            StringBuilder b = new();
            b.AppendLine($"Models ({Model.Models.Count}) :");
            foreach(Model m in Model.Models)
            {
                b.AppendLine($"({m.Id}) - {m.Name} pos: {m.Id}");
            }


            response = b.ToString();
            return true;
        }

    }
}
