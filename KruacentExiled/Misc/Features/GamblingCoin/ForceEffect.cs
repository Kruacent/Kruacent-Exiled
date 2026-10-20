using CommandSystem;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;
using static HarmonyLib.Code;

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

            List<ReferenceHub> listhub = RAUtils.ProcessPlayerIdOrNamesList(arguments, 1, out string[] newargs);



            if(listhub.Count == 0)
            {
                
                Player player = Player.Get(sender);
                if (player is null)
                {
                    response = "player not found";
                    return false;
                }

                listhub.Add(player.ReferenceHub);
            }

            foreach (ReferenceHub hub in listhub)
            {
                Player player = Player.Get(hub);
                try
                {
                    chose.ExecuteEffect(player);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                    response = "error with coin effect";
                    return false;
                }
            }
            string text = string.Join(" ", newargs);
            
            response = $"forced effect {chose.Name} on {text}";
            return true;


        }


        
    }
}
