using CommandSystem;
using Exiled.API.Features;
using KE.Misc.Features._914Upgrades;
using KE.Utils.API.Commands;
using Scp914;
using System;

namespace KruacentExiled.Commands.DebugCommands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class GetRandomRoom914 : KECommand
    {
        public override string Command => "getrandomroom914";

        public override string[] Aliases => new string[0];

        public override string Description => "";

        public override string[] Usage => new string[] { "Scp914KnobSetting" };

        public override bool ExecuteCommand(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {

            if(arguments.Count == 0)
            {
                response = "no Scp914KnobSetting";
                return false;
            }

            if (!Enum.TryParse<Scp914KnobSetting>(arguments.At(0),true,out var knob))
            {
                response = "Scp914KnobSetting incorrect";
                return false;
            }



            Room room = PlayerTeleport914.GetRoom(knob);



            if(room == null)
            {
                response = "no room found";
            }
            else
            {
                response = "room : " + room.Name;
            }
            return true;


        }
    }
}
