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

        public string[] Aliases => new string[] { "nbv" };

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
                response = "current min number : "+ MainPlugin.Configs.MinPlayerVote;
                return true;
            }

            if(!int.TryParse(arguments.At(0),out int result))
            {
                response = "couldn't parse the number";
                return false;
            }





            MainPlugin.Configs.MinPlayerVote = result;


            response = "vote set at " + MainPlugin.Configs.MinPlayerVote + " players";
            return true;
        }
    }
}
