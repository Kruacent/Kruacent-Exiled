using CommandSystem;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Items
{

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class CommandPos : ICommand
    {
        public string Command => "curpos";

        public string[] Aliases => [];

        public string Description => "position";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var p = Player.Get(sender);
            response = string.Empty;

            if(p is not null)
            {
                Primitive prim = Primitive.Create(p.Position, null, Vector3.one * .1f, true);

                Timing.CallDelayed(3f,prim.Destroy);

                if(p.CurrentRoom == null)
                {
                    response = p.Position.ToString();
                }
                else
                {
                    response = p.CurrentRoom.Type +
                    "\n" + p.CurrentRoom.LocalPosition(p.Position) +
                    "\n" + p.CurrentRoom.Rotation.eulerAngles +
                    "\n" + p.Position.ToString() +
                    "\n" + p.CurrentRoom.LocalPosition(p.Position);
                }



                    return true;
            }
            response = "no";
            return false;
        }
    }
}
