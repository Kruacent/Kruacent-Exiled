using CommandSystem;
using Exiled.API.Features;
using MEC;
using System;
using System.Collections.Generic;
using Utils;

namespace KruacentExiled.Misc.Features.GamblingCoin
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class GiveCompressedCoinEffect : ICommand
    {
        public string Command => "givecompressedcoineffect";

        public string[] Aliases => new string[] { "cfce" };

        public string Description => "give a compressed coin effect to a player";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count < 1)
            {
                response = "need effect count";
                return false;
            }

            if (!int.TryParse(arguments.At(0), out int flipCount) || flipCount <= 0)
            {
                response = $"invalid effect count {arguments.At(0)}";
                return false;
            }

            List<ReferenceHub> listhub = RAUtils.ProcessPlayerIdOrNamesList(arguments, 1, out string[] newargs);

            if (listhub.Count == 0)
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
                if (player is null) continue;

                try
                {
                    Timing.RunCoroutine(EventHandlers.CompressedFlips(player, flipCount));
                    
                }
                catch (Exception e)
                {
                    Log.Error(e);
                    response = "error with compressed coin effect";
                    return false;
                }
            }

            string text = string.Join(" ", newargs);

            response = $"given {flipCount} effects to {text}";
            return true;
        }
    }
}