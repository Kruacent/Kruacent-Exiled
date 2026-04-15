using CommandSystem;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using KE.CustomRoles.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.CustomRoles.Commands.KECR.Lists
{
    public class Abilities : ICommand
    {
        public string Command => "Ability";

        public string[] Aliases => new string[1] { "a" };

        public string Description => "list abilities";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if(KEAbilities.Registered.Count == 0)
            {
                response = "no ability found";
                return false;
            }
            StringBuilder sb = StringBuilderPool.Pool.Get();
            foreach(KEAbilities ability in KEAbilities.Registered.OrderBy(a => a.Name))
            {
                sb.Append('[')
                    .Append(ability.Name)
                    .AppendLine("]");
            }

            response = StringBuilderPool.Pool.ToStringReturn(sb);
            return true;
        }
    }
}
