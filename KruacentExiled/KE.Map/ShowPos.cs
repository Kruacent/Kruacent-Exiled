using CommandSystem;
using Exiled.API.Features;
using KE.Map.Surface.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Map
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ShowPos : ICommand
    {
        public string Command => "showposition";

        public string[] Aliases => ["showpos"];

        public string Description => "show pos";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);


            response = player.Position.ToString() + "\n" 
                + player.CurrentRoom?.Position + "\n" 
                + (player.CurrentRoom?.Position - player.Position);

            return true;
        }
    }
}
