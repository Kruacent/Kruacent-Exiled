using CommandSystem;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using Exiled.CustomItems.API.Features;
using Exiled.Permissions.Extensions;
using KE.Items.API.Features;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Items.Commands
{
    internal class Give : ICommand
    {
        public string Command => "give";

        public string[] Aliases => new string[] { "g" };

        public string Description => "give";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("customitems.give"))
            {
                response = "Permission Denied, required: customitems.give";
                return false;
            }

            if (arguments.Count == 0)
            {
                response = "give <Custom item name> [Nickname/PlayerID/UserID/all/*]";
                return false;
            }

            if (!KECustomItem.TryGet(arguments.At(0), out CustomItem item))
            {
                response = $"Custom item {arguments.At(0)} not found!";
                return false;
            }

            if (arguments.Count == 1)
            {
                if (sender is PlayerCommandSender playerCommandSender)
                {
                    Player player = Player.Get(playerCommandSender.SenderId);

                    if (!CheckEligible(player))
                    {
                        response = "You cannot receive custom items!";
                        return false;
                    }

                    item.Give(player);
                    response = $"{item.Name} given to {player.Nickname} ({player.UserId})";
                    return true;
                }

                response = "Failed to provide a valid player, please follow the syntax.";
                return false;
            }

            string identifier = string.Join(" ", arguments.Skip(1));

            switch (identifier)
            {
                case "*":
                case "all":
                    List<Player> eligiblePlayers = Player.List.Where(CheckEligible).ToList();
                    foreach (Player ply in eligiblePlayers)
                        item.Give(ply);

                    response = $"Custom item {item.Name} given to all players who can receive them ({eligiblePlayers.Count} players)";
                    return true;
                default:
                    break;
            }

            IEnumerable<Player> list = Player.GetProcessedData(arguments, 1);
            int num = 0;

            if (list.IsEmpty())
            {
                response = "Cannot find player! Try using the player ID!";
                return false;
            }

            foreach (Player player in list)
            {
                if (CheckEligible(player))
                {
                    item.Give(player);
                    num++;
                }
            }

            response = $"{item.Name} given to {num} players!";
            return true;
        }
        private bool CheckEligible(Player player) => player.IsAlive && !player.IsCuffed && (player.Items.Count < 8);
    }
}
