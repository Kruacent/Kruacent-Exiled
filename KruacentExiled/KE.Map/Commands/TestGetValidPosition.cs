using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features;
using KE.Utils.API.Commands;
using KE.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Map.Commands
{

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    public class TestGetValidPosition : KECommand
    {
        public override string Command => "test";

        public override string[] Aliases => [];

        public override string Description => "";

        public override string[] Usage => [];

        public override bool ExecuteCommand(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {

            if (!MainPlugin.Instance.Config.Debug)
            {
                response = "you can't do that";
                return false;
            }

            Player player = Player.Get(sender);

            if(player == null)
            {
                response = "invalid player";
                return false;
            }

            if(arguments.Count == 0)
            {
                response = "give a roomtype";
                return false;
            }


            if(!Enum.TryParse(arguments.At(0),out RoomType roomtype))
            {
                response = "room type not found";
                return false;
            }
            Room room = Room.Get(roomtype);


            if(room == null)
            {
                response = "room not found";
                return false;
            }



            player.Teleport(room.GetValidPosition());
            response = "ok";

            return false;
        }
    }
}
