using CommandSystem;
using KE.Utils.API.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Map.Surface.SupplyDrops
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]

    internal class ForceSupplyDrop : KECommand
    {

        public override string Command => "forcesupplydrop";

        public override string[] Aliases => new string[] { "fsd" };

        public override string Description => "force a supply to drop at a random position";

        public override string[] Usage => new string[0];



        public override bool ExecuteCommand(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!SupplyDrop.IsActivated)
            {
                response = "Supply drop not activated in config";
                return false;

            }


            SupplyDrop drop = SupplyDrop.SpawnRandom();
            response = " spawned at " + drop.Position;
            return true;

        }

    }
}
