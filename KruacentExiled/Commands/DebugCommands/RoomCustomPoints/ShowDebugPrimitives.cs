
using CommandSystem;
using DrawableLine;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using KE.Utils.API.Commands;
using KruacentExiled.CustomSpawnPoint.Spawned;
using ProjectMER.Commands.Modifying.Position;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Commands.DebugCommands.RoomCustomPoints
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class ShowDebugPrimitives : KECommand
    {
        public override string Command => "ShowDebugPrimitives";

        public override string[] Aliases => new string[] { "sdp" };

        public override string Description => "";

        public override string[] Usage => new string[0];


        public static bool AreCustomPointShowed { get; private set; } = false;

        public override bool ExecuteCommand(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!Player.TryGet(sender, out Player player))
            {
                response = "player null";
                return false;
            }



            if(RoomSpawnedCustomPoint.All.Count < 1)
            {
                response = "no custom point";
                return false;
            }



            int num = ShowPoses(player);


            response = "found " + num;
            return true;
        }



        private int ShowPoses(Player player)
        {

            int result = 0;
            AreCustomPointShowed = !AreCustomPointShowed;

            foreach (RoomSpawnedCustomPoint point in RoomSpawnedCustomPoint.All)
            {
                point.IsDebugPrimitiveVisible = AreCustomPointShowed;


                result++;
            }



            return result;
        }



    }
}
