using CommandSystem;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KE.Utils.API.Models;

namespace KE.Utils.API.Models.Commands
{
    public class ModeMovePrim : ICommand
    {

        public string Command { get; } = "mode";

        public string[] Aliases { get; } = { "m" };

        public string Description { get; } = "select the mode : scale,move,rotate";

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
            string mode = arguments.At(0);

            switch(mode)
            {
                case "m":
                case "move":
                    Models.Instance.ModelCreator.MovementMode = MovementMode.Move;
                    break;
                case "s":
                case "scale":
                    Models.Instance.ModelCreator.MovementMode = MovementMode.Scale;
                    break;
                case "r":
                case "rotate":
                    Models.Instance.ModelCreator.MovementMode = MovementMode.Rotate;
                    break;
                default:
                    response = "wrong argument";
                    return false;


            }


            response = $"Mode set to " + Models.Instance.ModelCreator.MovementMode.ToString();
            return true;
        }

    }
}
