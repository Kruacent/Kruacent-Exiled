using CommandSystem;
using Exiled.API.Features;
using KE.Misc.Features._914Upgrades;
using KE.Utils.API.Commands;
using KruacentExiled.CustomRoles.API.Features;
using Scp914;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Commands.DebugCommands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    internal class ResetAbilityCooldown : KECommand
    {
        public override string Command => "ResetAbilityCooldown";

        public override string[] Aliases => new string[0];

        public override string Description => "Reset all abilities of a player";

        public override string[] Usage => new string[0];

        public override bool ExecuteCommand(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if(!Player.TryGet(sender,out Player player))
            {
                response = "no player";
                return false;
            }
            

            if(!KEAbilities.PlayersAbility.TryGetValue(player,out var abilies))
            {

                response = "no abilities";
                return false;
            }

            foreach(KEAbilities abilities in abilies)
            {
                abilities.ResetCooldown(player);
            }


            response = "ok";
            return true;


        }
    }
}
