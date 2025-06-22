using CommandSystem;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Utils.API.Models.Commands
{
    public class ShowCenter : ICommand
    {

        public string Command { get; } = "show";

        public string[] Aliases { get; } = { "sh" };

        public string Description { get; } = "toggle the center of the model";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {

            Player p = Player.Get(sender);
            if (p == null)
            {
                response = "This player can't do this command";
                return false;
            }

            Model m = Models.Instance.ModelCreator.ModelSelected;

            if(m == null)
            {
                response = "no model selected";
                return false;
            }
            
            if(!bool.TryParse(arguments.At(0),out bool result))
            {
                response = "write true or false";
                return false;
            }

            m.SetCenterPrimitive(result);

            response = "done";

            return true;
        }
    }
}
