using Exiled.API.Features;
using System;
using CommandSystem;
using KruacentExiled.GlobalEventFramework.GEFE.API.Features;
using System.Linq;

namespace KruacentExiled.GlobalEventFramework.GEFE.Commands
{
    public class ForceNbGE : ICommand
    {
        public string Command { get; } = "forceNb";
        public string[] Aliases { get; } = new string[] { "nb","n" };
        public string Description { get; } = "force a number global event";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!Round.IsLobby)
            {
                response = "You can only force a global event in the lobby";
                return false;
            }
            


            if (arguments.Count == 1)
            {
                if (int.TryParse(arguments.At(0), out int nbge) && nbge > -1)
                {
                    if(nbge <= 0)
                    {
                        response = "You can't force 0 global event";
                        return false;
                    }
                    if(nbge > GlobalEvent.GlobalEventsList.Count())
                    {
                        response = $"You can't force {nbge} global event, there are only {GlobalEvent.GlobalEventsList.Count()} global events";
                        return false;
                    }


                    response = $"Forcing {nbge} global event";
                    GlobalEvent.NumberOfGE = nbge;
                    return true;
                }
            }

            response = "Too many arguments";
            return false;

        }
    }
}
