using CommandSystem;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using Exiled.CustomRoles.API.Features;
using Exiled.Permissions.Extensions;
using KE.CustomRoles.API.Features;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.CustomRoles.Commands
{
    internal class Give : ICommand
    {
        public string Command => "give";

        public string[] Aliases => ["g"];

        public string Description => "";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            try
            {
                if (!sender.CheckPermission("customroles.give"))
                {
                    response = "Permission Denied, required: customroles.give";
                    return false;
                }

                if (arguments.Count == 0)
                {
                    response = "give <Custom role name/Custom role ID> [Nickname/PlayerID/UserID/all/*]";
                    return false;
                }

                if (!KECustomRole.TryGet(arguments.At(0), out KECustomRole customRole) || customRole is null)
                {
                    response = "Custom role " + arguments.At(0) + " not found!";
                    return false;
                }
                KECustomRole kecr = customRole;

                if (arguments.Count == 1)
                {
                    if (sender is PlayerCommandSender sender2)
                    {
                        Player player = Player.Get(sender2);
                        kecr.AddRole(player);
                        response = kecr.Name + " given to " + player.Nickname + ".";
                        return true;
                    }

                    response = "Failed to provide a valid player.";
                    return false;
                }

                string text = string.Join(" ", arguments.Skip(1));
                if (text == "*" || text == "all")
                {
                    List<Player> list = ListPool<Player>.Pool.Get(Player.List);
                    foreach (Player item in list)
                    {
                        kecr.AddRole(item);
                    }

                    response = "Custom role " + kecr.Name + " given to all players.";
                    ListPool<Player>.Pool.Return(list);
                    return true;
                }

                IEnumerable<Player> processedData = Player.GetProcessedData(arguments, 1);
                if (processedData.IsEmpty())
                {
                    response = "Cannot find player! Try using the player ID!";
                    return false;
                }

                foreach (Player item2 in processedData)
                {
                    customRole.AddRole(item2);
                }

                response = $"Customrole {customRole.Name} given to {processedData.Count()} players!";
                return true;
            }
            catch (Exception message)
            {
                Log.Error(message);
                response = "Error";
                return false;
            }
        }
    }
}
