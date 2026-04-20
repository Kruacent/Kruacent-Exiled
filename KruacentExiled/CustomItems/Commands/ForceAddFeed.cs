using CommandSystem;
using Exiled.API.Features;
using KE.Utils.API.Commands;
using KE.Utils.API.Displays.Feeds;
using MEC;
using System;

namespace KruacentExiled.CustomItems.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class ForceAddFeed : KECommand
    {
        public override string Command => "forceaddfeed";

        public override string[] Aliases => new string[0];

        public override string Description => "add a feed";

        public override string[] Usage => new string[0];

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