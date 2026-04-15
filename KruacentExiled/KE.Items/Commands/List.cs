using CommandSystem;
using Exiled.API.Features.Pools;
using Exiled.CustomItems.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Items.Commands
{
    internal class List : ICommand
    {
        public string Command => "list";

        public string[] Aliases => new string[] { "l" };

        public string Description => "list";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {

            StringBuilder sb = StringBuilderPool.Pool.Get().AppendLine();



            sb.Append("[Registered custom items (").Append(CustomItem.Registered.Count).AppendLine(")]");

            foreach (CustomItem customItem in CustomItem.Registered.OrderBy(item => item.Name))
            {
                sb.Append('[');
                sb.Append(customItem.Name);
                sb.Append(" (");
                sb.Append(customItem.Type);
                sb.Append(')');
                sb.AppendLine("]");
            }
                

            response = StringBuilderPool.Pool.ToStringReturn(sb);
            return true;
        }
    }
}
