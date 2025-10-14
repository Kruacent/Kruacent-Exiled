using CommandSystem;
using Exiled.API.Features.Pools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Map.Utils
{
    public abstract class KECommand : ICommand, IUsageProvider
    {
        public abstract string Command { get; }

        public abstract string[] Aliases { get; }

        public abstract string Description { get; }
        public abstract string[] Usage { get; }

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            bool exe = ExecuteCommand(arguments, sender, out response);
            if (!exe)
            {
                response += $"\nUsage : {Command} " +this.DisplayCommandUsage();
            }
            return exe;
        }

        public abstract bool ExecuteCommand(ArraySegment<string> arguments, ICommandSender sender, out string response);
    }
}
