using CommandSystem;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KE.Utils.API.Models;
using UnityEngine;

namespace KE.Utils.API.Models.Commands
{
    public class ChangePrimType : ICommand
    {

        public string Command { get; } = "changeprimtype";

        public string[] Aliases { get; } = { "cpt" };

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
            string type = arguments.At(0);


            if(!Enum.TryParse(type, out PrimitiveType prim))
            {
                response = "wrong argument";
                return false;
            }

            
            Models.Instance.ModelCreator.ModelHandler.SelectedPrimitive.Type = prim;

            response = $"Mode set to " + prim ;
            return true;
        }

    }
}
