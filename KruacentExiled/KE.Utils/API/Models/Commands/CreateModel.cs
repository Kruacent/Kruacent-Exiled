using CommandSystem;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Utils.API.Models.Commands
{
    internal class CreateModel : ICommand
    {

        public string Command { get; } = "create";

        public string[] Aliases { get; } = { "c" };

        public string Description { get; } = "create a new model at your position";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            
            Player p = Player.Get(sender);
            if(p == null)
            {
                response = "This player can't do this command";
                return false;
            }


            
            Model m = Model.Create(p.Position, arguments.At(1));
            response = $"Created model ({m.Name}) at {m.Center}";
            return true;
        }

    }
}
