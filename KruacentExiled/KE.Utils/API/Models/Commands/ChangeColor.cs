using CommandSystem;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KE.Utils.API.Models;
using UnityEngine;
using Exiled.API.Features.Toys;

namespace KE.Utils.API.Models.Commands
{
    public class ChangeColor : ICommand
    {

        public string Command { get; } = "changecolor";

        public string[] Aliases { get; } = { "cc" };

        public string Description { get; } = "change color rgba (0-255)";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player p = Player.Get(sender);
            if (p == null)
            {
                response = "This player can't do this command";
                return false;
            }

            if (arguments.Count < 3)
            {
                response = "not enough arguments";
                return false;
            }
            if(!byte.TryParse(arguments.At(0),out byte r) ||
               !byte.TryParse(arguments.At(1), out byte g) ||
               !byte.TryParse(arguments.At(2), out byte b))
            {
                response = "number between 0 & 255";
                return false;
            }

            Color32 c;

            if(arguments.Count > 3 && byte.TryParse(arguments.At(3),out byte a))
            {
                c = new Color32(r, g, b, a);
            }
            else
            {
                c = new Color32(r, g, b,255);
            }

            Primitive prim = Models.Instance.ModelCreator.ModelHandler.SelectedPrimitive;

            if(prim == null)
            {
                response = "no primitive selected";
                return false;
            }

            prim.Color = c;
            

            response = $"new color set to " + c.ToString();
            return true;
        }

    }
}
