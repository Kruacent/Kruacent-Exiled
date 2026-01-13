using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using KE.Utils.API.Commands;
using MapGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Map.Others.BlackoutNDoor.Commands
{
    
    internal class BlackoutCommand : KECommand
    {
        public override string Command => "blackout";

        public override string[] Aliases => ["b"];

        public override string Description => "force a blackout";

        public override string[] Usage => ["FacilityZone"];

        public override bool ExecuteCommand(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = string.Empty;
            if (arguments.Count == 0)
            {
                return false;
            }
            string arg1 = arguments.At(0);
            if(!Enum.TryParse<FacilityZone>(arg1, true, out FacilityZone zone))
            {
                response = "Could not parse the zone";
                return false;
            }
            

            Blackout blackout = new();
            blackout.StartEvent(zone.GetZone(),-1);
            response = "blackout forced at " + zone.ToString();
            
            return true;
        }
    }
}
