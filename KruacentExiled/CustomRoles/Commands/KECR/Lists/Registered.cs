using CommandSystem;
using Exiled.API.Features.Pools;
using KruacentExiled.CustomRoles.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.CustomRoles.Commands.KECR.Lists
{
    public class Registered : ICommand
    {
        public static Registered Instance = new Registered();
        public string Command => "registered";

        public string[] Aliases => new string[] { "r" };

        public string Description => "";

        private Registered() { }

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (KECustomRole.Registered.Count == 0)
            {
                response = "no role found";
                return false;
            }

            StringBuilder sb = StringBuilderPool.Pool.Get();
            sb.AppendLine();
            foreach (KECustomRole cr in KECustomRole.Registered.OrderBy(a => a.Name))
            {
                sb.AppendLine(cr.ShowConsole());
            }

            response = StringBuilderPool.Pool.ToStringReturn(sb);
            return true;
        }
    }
}
