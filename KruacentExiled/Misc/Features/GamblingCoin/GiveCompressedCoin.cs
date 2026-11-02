using CommandSystem;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using KruacentExiled.CustomItems.API.Features;
using KruacentExiled.CustomItems.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace KruacentExiled.Misc.Features.GamblingCoin.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class GiveCompressedCoin : ICommand
    {
        public string Command => "givecompressedcoin";

        public string[] Aliases => new string[] { "gcc", "givecc" };

        public string Description => "Give une CompressedCoin à un joueur avec un nombre d'effets. Usage: gcc <nb effet> [joueur]";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count < 1)
            {
                response = "Usage : givecompressedcoin <nb_effets> [joueur]";
                return false;
            }

            if (!int.TryParse(arguments.At(0), out int flipCount) || flipCount <= 0)
            {
                response = $"Le nombre d'effets doit être un entier (reçu : '{arguments.At(0)}').";
                return false;
            }

            if (!KECustomItem.TryGet("CompressedCoin", out KECustomItem customItem))
            {
                response = "Erreur : Le CustomItem 'CompressedCoin' n'est pas enregistré !";
                return false;
            }

            List<ReferenceHub> targetsHub = RAUtils.ProcessPlayerIdOrNamesList(arguments, 1, out _);

            if (targetsHub.Count == 0)
            {
                Player senderPlayer = Player.Get(sender);
                if (senderPlayer == null)
                {
                    response = "Vous devez spécifier au moins un joueur";
                    return false;
                }

                targetsHub.Add(senderPlayer.ReferenceHub);
            }

            int successCount = 0;

            foreach (ReferenceHub hub in targetsHub)
            {
                Player target = Player.Get(hub);
                if (target == null || !target.IsConnected) continue;

                customItem.Give(target);

                Item givedItem = target.Items.LastOrDefault(i => i.Type == customItem.ItemType && KECustomItem.TryGet(i, out var found) && found == customItem);


                if (givedItem != null)
                {
                    CompressedCoin.TrySetCoinUses(givedItem.Serial, flipCount);
                    successCount++;
                }
            }

            response = $"CompressedCoin ({flipCount} effets) pour {successCount} joueur(s).";
            return true;
        }
    }
}