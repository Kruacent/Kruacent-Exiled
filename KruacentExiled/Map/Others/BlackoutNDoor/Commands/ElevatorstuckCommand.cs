using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using KE.Utils.API.Commands;
using KruacentExiled.Map.Others.BlackoutNDoor;
using MapGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Map.Others.BlackoutNDoor.Commands
{
    
    internal class ElevatorstuckCommand : KECommand
    {
        public override string Command => "elevatorstuck";

        public override string[] Aliases => new string[] { "e" };

        public override string Description => "force a elevatorstuck";

        public override string[] Usage => new string[] { "FacilityZone" };

        public override bool ExecuteCommand(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = string.Empty;
            if (arguments.Count == 0)
            {
                return false;
            }
            string arg1 = arguments.At(0);
            if(!Enum.TryParse(arg1, true, out FacilityZone zone))
            {
                response = "Could not parse the zone";
                return false;
            }
            

            ElevatorStuck elevator = new ElevatorStuck();
            elevator.StartEvent(zone.GetZone(),-1);
            response = "elevator forced at " + zone.ToString();
            
            return true;
        }
    }
}
