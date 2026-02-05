using CommandSystem;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Misc.Features.VoteStart
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ForceVoteNumber : ICommand
    {
        public string Command => "nbvote";

        public string[] Aliases => ["nbv"];

        public string Description => "force a number of people needed to vote for this round only";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if(!Round.IsLobby)
            {
                response = "works only in lobby";
                return false;
            }

            if(arguments.Count < 1)
            {
                response = "current min number : "+ MainPlugin.Instance.Config.MinPlayerVote;
                return true;
            }

            if(!int.TryParse(arguments.At(0),out int result))
            {
                response = "couldn't parse the number";
                return false;
            }





            MainPlugin.Instance.Config.MinPlayerVote = result;


            response = "vote set at " + MainPlugin.Instance.Config.MinPlayerVote + " players";
            return true;
        }
    }
}
