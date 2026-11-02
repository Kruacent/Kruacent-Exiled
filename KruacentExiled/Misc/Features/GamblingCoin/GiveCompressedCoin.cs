using CommandSystem;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using KruacentExiled.CustomItems.API.Features;
using KruacentExiled.CustomItems.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace KruacentExiled.Misc.Features.GamblingCoin
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class GiveCompressedCoin : ICommand
    {
        public string Command => "givecompressedcoin";

        public string[] Aliases => new string[] { "gcc", "givecc" };

        public string Description => "give a compressed coin with custom effect count to a player";

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

            if (!KECustomItem.TryGet("CompressedCoin", out KECustomItem customItem))
            {
                response = "custom item CompressedCoin not found";
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
                    customItem.Give(player);

                    Item givedItem = player.Items.LastOrDefault(i => i.Type == customItem.ItemType && KECustomItem.TryGet(i, out var found) && found == customItem);

                    if (givedItem != null)
                    {
                        CompressedCoin.TrySetCoinUses(givedItem.Serial, flipCount);
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e);
                    response = "error with give compressed coin";
                    return false;
                }
            }

            string text = string.Join(" ", newargs);

            response = $"given CompressedCoin ({flipCount} effects) to {text}";
            return true;
        }
    }
}