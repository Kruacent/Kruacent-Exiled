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
    public class LoadModel : ICommand
    {

        public string Command { get; } = "load";

        public string[] Aliases { get; } = { "lo" };

        public string Description { get; } = "load a model";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {

            Player p = Player.Get(sender);
            if (p == null)
            {
                response = "This player can't do this command";
                return false;
            }

            if (arguments.Count < 1)
            {
                response = "not enough arguments";
                return false;
            }

            if (!int.TryParse(arguments.At(0),out int id))
            {
                response = "enter id";
                return false;
            }

            if(!ModelBlueprint.TryGet(id,out var mbp))
            {
                response = "blueprint not found";
                return false;
            }
            
            var m =Model.Create(mbp, p.Position);

            response = $"Created model ({m.Name}) at {m.Center}";
            return true;
        }

    }
}
