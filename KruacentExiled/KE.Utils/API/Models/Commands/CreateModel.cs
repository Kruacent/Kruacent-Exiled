using CommandSystem;
using Exiled.API.Features;
using KE.Utils.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Utils.API.Models.Commands
{
    public class CreateModel : ICommand
    {

        public string Command { get; } = "create";

        public string[] Aliases { get; } = { "c" };

        public string Description { get; } = "create a new model at your position";

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
            string name = arguments.At(0);

            Model m = Model.Create(p.Position, name);
            response = $"Created model ({m.Name}) at {m.Center}";
            return true;
        }

    }
}
