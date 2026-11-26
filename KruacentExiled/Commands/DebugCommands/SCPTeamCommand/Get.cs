using CommandSystem;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using KE.Utils.API.Commands;
using KE.Utils.API.Features;
using KruacentExiled.CustomRoles.CR.Human;
using KruacentExiled.CustomRoles.CustomSCPTeam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Commands.DebugCommands.SCPTeamCommand
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class Get : KECommand
    {
        public override string Command => "GetScpteam";

        public override string[] Aliases => new string[0];

        public override string Description => "show all player in the SCPTeam";

        public override string[] Usage => new string[0];

        public override bool ExecuteCommand(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            StringBuilder sb = StringBuilderPool.Pool.Get();

            sb.AppendLine()
                .Append("found ")
                .Append(SCPTeam.SCPs.Count)
                .Append(" scps in SCPTeam : ");

            foreach (ReferenceHub hub in SCPTeam.SCPs)
            {
                sb.AppendLine()
                    .Append(" - ");
                if (hub == null)
                {
                    sb.Append("found hub null");
                    continue;
                }

                sb.Append(hub.PlayerId);

                KELog.Debug("hub id = " + hub.PlayerId);

            }


            response = StringBuilderPool.Pool.ToStringReturn(sb);
            return true;

        }
    }
}
