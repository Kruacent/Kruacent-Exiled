using CommandSystem;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Utils.API.Models.Commands
{
    public class SelectModel : ICommand
    {

        public string Command { get; } = "select";

        public string[] Aliases { get; } = { "s" };

        public string Description { get; } = "select an existing model";

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

            if (string.IsNullOrEmpty(name))
            {
                response = "name null or empty";
                return false;
            }




            if (!Model.TryGet(name, out Model m))
            {
                response = "model not found";
                return false;
            }
            response = $"model ({m.Name}) selected {m.Center}";
            Models.Instance.ModelCreator.ModelHandler.SelectedModel = m;
            return true;
        }

    }
}
