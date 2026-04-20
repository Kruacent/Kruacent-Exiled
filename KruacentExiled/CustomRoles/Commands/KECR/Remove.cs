using CommandSystem;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.CustomRoles.Commands.KECR
{
    public class Remove : ICommand
    {
        public string Command => "remove";

        public string[] Aliases => new string[] { "r" };

        public string Description => "remove a custom role";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {

            if (!Player.TryGet(sender, out Player player))
            {
                response = "not a valid player";
                return false;
            }



        }
    }
}
