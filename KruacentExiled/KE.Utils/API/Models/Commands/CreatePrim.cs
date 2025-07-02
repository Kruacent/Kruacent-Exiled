using CommandSystem;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Utils.API.Models.Commands
{
    public class CreatePrim : ICommand
    {

        public string Command { get; } = "createprimitive";

        public string[] Aliases { get; } = { "cp" }; //no not like that

        public string Description { get; } = "create a new primitive at your position";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {

            Player p = Player.Get(sender);
            if (p == null)
            {
                response = "This player can't do this command";
                return false;
            }


            Model m = Models.Instance.ModelCreator.SelectedModel;

            if (m == null)
            {
                response = "no model selected";
                return false;
            }
            m.Add(p.Position);


            response = "created!";
            return true;
        }

    }
}
