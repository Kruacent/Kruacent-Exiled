namespace KE.GlobalEventFramework.GEFE.Commands
{
    using Exiled.API.Features;
    using Exiled.API.Features.Pickups;
    using System;
    using CommandSystem;
    using System.Runtime.InteropServices.WindowsRuntime;
    using GEFE.API.Interfaces;
    using GEFE.API.Features;

    public class ForceMiddleEvent : ICommand
    {
        public string Command { get; } = "forcemiddle";
        public string[] Aliases { get; } = new string[] { "fm" };
        public string Description { get; } = "force a random middle event";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            bool res = MiddleEvent.Activate();

            response = res ? "activated" : "not activated";
            return res;
        }
    }
}
