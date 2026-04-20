using CommandSystem;
using Exiled.API.Features;
using System;

namespace KruacentExiled.Map
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ShowPos : ICommand
    {
        public string Command => "showposition";

        public string[] Aliases => new string[] { "showpos" };

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
