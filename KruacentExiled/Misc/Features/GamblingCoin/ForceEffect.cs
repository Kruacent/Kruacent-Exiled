using CommandSystem;
using Exiled.API.Features;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Misc.Features.GamblingCoin
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ForceCoinEffect : ICommand
    {
        public string Command => "forcecoineffect";

        public string[] Aliases => new string[] { "fce" };

        public string Description => "force a effect for a player";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (player is null)
            {
                response = "player not found";
                return false;
            }

            if (arguments.Count < 1)
            {
                response = "need effect";
                return false;
            }

            ICoinEffect chose= null;
            foreach (ICoinEffect effect in GamblingCoinManager.EffectList)
            {
                if (effect.Name == arguments.At(0))
                {
                    chose = effect;
                    break;
                }
            }




            

            if (chose is null)
            {
                response = $"effect {arguments.At(0)} not found";
                return false;
            }


            try
            {
                chose.ExecuteEffect(player);
            }
            catch(Exception e)
            {
                Log.Error(e);
            }
            response = $"forced effect {chose.Name} on {player.Nickname}";
            return true;


        }
    }
}
