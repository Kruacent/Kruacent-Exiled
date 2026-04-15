using CommandSystem;
using Exiled.API.Features;
using KE.Utils.API.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Misc.Features.VoteStart
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class RestractVote : KECommand
    {
        public override string Command => "removevote";

        public override string[] Aliases => new string[] { "rv", "retractv", "removev" };

        public override string Description => "remove the set vote";

        public override string[] Usage => new string[] { "" };
        public override bool ExecuteCommand(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if(!Round.IsLobby)
            {
                response = "works only in lobby";
                return false;
            }


            Player player = Player.Get(sender);

            VoteStart vote = MainPlugin.Instance.vote;

            if (!vote.DidVote(player))
            {
                response = "you didn't vote";
                return false;
            }


            vote.CancelVote(player);


            response = "vote set at " + MainPlugin.Configs.MinPlayerVote + " players";
            return true;
        }
    }
}
