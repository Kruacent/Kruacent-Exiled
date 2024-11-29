
namespace GEFExiled.Commands
{
    using Exiled.API.Features;
    using Exiled.API.Features.Pickups;
    using System;
    using CommandSystem;
    using System.Runtime.InteropServices.WindowsRuntime;
    using GEFExiled.GEFE.API.Features;
    using GEFExiled.GEFE.API.Interfaces;

    public class List : ICommand
    {
        public string Command { get; } = "list";
        public string[] Aliases { get; } = new string[] { "l","ls" };
        public string Description { get; } = "get the list of all Global Events";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            string result = "List of all global event ([o] if it's active in the round ; [ ] otherwise) : ";
            foreach (IGlobalEvent ge in GlobalEvent.GlobalEvents.Values)
            {
                
                if (GlobalEvent.ActiveGlobalEvents.Contains(ge))
                {
                    result += "[o]";
                }
                else
                {
                    result += "[ ]";
                }
                result += $" {ge.Id} : {ge.Name} : {ge.Description}";
            }
            response = result;
            return true;
        }
    }
}
