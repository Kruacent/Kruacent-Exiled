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

    public class ForceNbGE : ICommand
    {
        public string Command { get; } = "forceNb";
        public string[] Aliases { get; } = new string[] { "nb","n" };
        public string Description { get; } = "force a specified number global event";
        internal static int NbGE = -1;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!Round.IsLobby)
            {
                response = "You can only force a global event in the lobby";
                NbGE = -1;
                return false;
            }
            


            if (arguments.Count == 1)
            {
                if (int.TryParse(arguments.At(0), out int nbge) && nbge > -1)
                {
                    if(nbge <= 0)
                    {
                        response = "You can't force 0 global event";
                        NbGE = -1;
                        return false;
                    }
                    if(nbge > GlobalEvent.GlobalEventsList.Count)
                    {
                        response = $"You can't force {nbge} global event, there are only {GlobalEvent.GlobalEventsList.Count} global events";
                        NbGE = -1;
                        return false;
                    }


                    response = $"Forcing {nbge} global event";
                    NbGE = nbge;
                    return true;
                }
            }

            NbGE = -1;
            response = "Too much argument";
            return false;

        }
    }
}
