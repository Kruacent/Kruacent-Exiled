using CommandSystem;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Utils.API.Models.Commands
{
    internal class SelectModel : ICommand
    {

        public string Command { get; } = "select";

        public string[] Aliases { get; } = { "s" };

        public string Description { get; } = "select an existing";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            
            Player p = Player.Get(sender);
            if(p == null)
            {
                response = "This player can't do this command";
                return false;
            }

            if(!Model.TryGet(int.Parse(arguments.At(1)),out Model m))
            {
                response = "model not found";
                return false;
            }
            response = $"model ({m.Name}) selected {m.Center}";
            return true;
        }

    }
}
