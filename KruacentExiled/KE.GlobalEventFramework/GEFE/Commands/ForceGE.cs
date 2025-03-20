namespace KE.GlobalEventFramework.GEFE.Commands
{
    using Exiled.API.Features;
    using Exiled.API.Features.Pickups;
    using System;
    using CommandSystem;
    using System.Runtime.InteropServices.WindowsRuntime;
    using GEFE.API.Interfaces;
    using GEFE.API.Features;
    using System.Collections.Generic;
    using System.Linq;

    public class ForceGE : ICommand
    {
        public string Command { get; } = "force";
        public string[] Aliases { get; } = new string[] { "f" };
        public string Description { get; } = "force a or multiple global event";
        internal static List<IGlobalEvent> ForcedGE = new List<IGlobalEvent>();

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!Round.IsLobby)
            {
                response = "You can only force a global event in the lobby";
                ForcedGE = new List<IGlobalEvent>();
                return false;
            }

            if (!GlobalEvent.TryGet(arguments.At(0), out IGlobalEvent ge1) || ge1 == null)
            {
                response = $"Global event ({arguments.At(0)}) not found ";
                ForcedGE = new IGlobalEvent[] { ge1 }.ToList();
                return false;
            }
            


            if (arguments.Count == 1)
            {
                response = $"Forcing {ge1.Name}";
                ForcedGE = new IGlobalEvent[] { ge1 }.ToList();
                return true;
            }

            if (!GlobalEvent.TryGet(arguments.At(1), out IGlobalEvent ge2) || ge2 == null)
            {
                response = $"Global event ({arguments.At(1)}) not found ";
                ForcedGE = new List<IGlobalEvent>();
                return false;
            }

            if (arguments.Count == 2)
            {
                response = $"Forcing {ge1.Name} & {ge2.Name}";
                ForcedGE = new IGlobalEvent[] { ge1, ge2 }.ToList();
                return true;
            }

            ForcedGE = new List<IGlobalEvent>();
            response = "";
            return false;

        }
    }
}
