using CommandSystem;
using Exiled.API.Features;
using KE.Utils.API.Models.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace KE.Utils.API.Models.Commands
{
    public class SaveModel : ICommand
    {

        public string Command { get; } = "save";

        public string[] Aliases { get; } = { "sa" };

        public string Description { get; } = "save a model to file";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {

            Player p = Player.Get(sender);
            if (p == null)
            {
                response = "This player can't do this command";
                return false;
            }


            Model model = Models.Instance.ModelCreator.SelectedModel;
            if (model == null)
            {
                response = "no model selected";
                return false;
            }

            model.Save();

            response = $"Saved ({model.Name})";
            return true;
        }

    }
}
