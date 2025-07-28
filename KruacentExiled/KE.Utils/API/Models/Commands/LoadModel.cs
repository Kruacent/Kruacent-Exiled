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
    public class LoadModel : ICommand
    {

        public string Command { get; } = "load";

        public string[] Aliases { get; } = { "lo" };

        public string Description { get; } = "load a model from a blueprint";

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

            foreach(ModelBlueprint bp in ModelBlueprint.Blueprints)
            {

                Log.Info($"{bp.Name} == {name} : {bp.Name == name}");
            }


            if(!ModelBlueprint.TryGet(name,out var mbp))
            {
                response = "blueprint not found";
                return false;
            }

            mbp.Spawn(p.Position);
            response = $"Created model ({mbp.Name}) at {p.Position}";
            return true;
        }

    }
}
