using CommandSystem;
using Exiled.API.Features.Pools;
using KE.CustomRoles.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.CustomRoles.Commands.KECR.Lists
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
            string[] name;
            sb.AppendLine();
            foreach (KECustomRole cr in KECustomRole.Registered.OrderBy(a => a.Name))
            {
                name = cr.Name.Split('_');
                sb.Append('[')
                    .Append('(')
                    .Append(name[0])
                    .Append(") ")
                    .Append(name[1])
                    .Append("]")
                    .Append(" spawn chance:")
                    .Append(cr.SpawnChance)
                    .AppendLine();
            }

            response = StringBuilderPool.Pool.ToStringReturn(sb);
            return true;
        }
    }
}
