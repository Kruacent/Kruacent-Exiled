using CommandSystem;
using Exiled.API.Features;
using KE.Utils.API.Models.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Utils.API.Models.Commands
{
    public class ListModel : ICommand
    {

        public string Command { get; } = "list";

        public string[] Aliases { get; } = { "l" };

        public string Description { get; } = "create a new model at your position";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            StringBuilder b = new();
            b.AppendLine($"Models ({Model.Models.Count}) :");
            foreach (Model m in Model.Models)
            {
                b.AppendLine($"{m.Name} pos: {m.Center} spawned? :{m.Spawned}");
            }

            b.AppendLine($"Blueprints ({ModelBlueprint.Blueprints.Count}) :");
            foreach (ModelBlueprint m in ModelBlueprint.Blueprints)
            {
                b.AppendLine($"{m.Name}");
            }


            response = b.ToString();
            return true;
        }

    }
}
