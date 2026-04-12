using CommandSystem;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using KE.Utils.API.Commands;
using System;
using LabPlayer = LabApi.Features.Wrappers.Player;
namespace KE.Misc.Features._914Upgrades
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class GiveOmni : KECommand
    {
        public override string Command => "giveomni";

        public override string[] Aliases => [];

        public override string Description => "gives an omni card";

        public override string[] Usage => [];

        public override bool ExecuteCommand(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if(player is null)
            {
                response = "player null";
                return false;
            }

            if(player.Role is not FpcRole)
            {
                response = "wrong role";
                return false;
            }


            LabPlayer labPlayer = LabPlayer.Get(sender);
            labPlayer.CurrentItem = OmniCardUpgrade.CreateOmniCard(player);

            response = "done";
            return true;



        }
    }
}
