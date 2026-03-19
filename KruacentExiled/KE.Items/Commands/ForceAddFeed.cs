using CommandSystem;
using Exiled.API.Features;
using KE.Items.Utils;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Items.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class ForceAddFeed : KE.Utils.API.Commands.KECommand
    {
        public override string Command => "forceaddfeed";

        public override string[] Aliases => [];

        public override string Description => "add a feed";

        public override string[] Usage => [];

        public override bool ExecuteCommand(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            if(player == null)
            {

                response = "no";
                return false;
            }



            HintFeed hintfeed = HintFeed.GetOrCreate(player);


            HintFeed.AddFeed(player, "test1");

            Timing.CallDelayed(1, () => HintFeed.AddFeed(player, "test2"));
            Timing.CallDelayed(2, () => HintFeed.AddFeed(player, "test3"));
            response = "ok";
            return true;
        }


    }

}