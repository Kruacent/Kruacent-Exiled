using CommandSystem;
using Exiled.API.Features;
using KE.Utils.API.Commands;
using KruacentExiled.CustomRoles.CR.CustomSCPs.SCP049C;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.CustomRoles.Commands
{

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class AddKill : KECommand
    {
        public override string Command => "addkill";

        public override string[] Aliases => new string[0];

        public override string Description => "add a new kill to a SCP-049-C";

        public override string[] Usage => new string[] { "number of kill to add" };

        public override bool ExecuteCommand(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!Player.TryGet(sender,out Player player))
            {
                response = "not a valid player";
                return false;
            }


            if(!player.GameObject.TryGetComponent<SCP049CLevelSystem>(out var comp))
            {
                response = "not a SCP-049-C";
                return false;
            }

            int numberadded = 1;

            if(arguments.Count < 1)
            {
                comp.AddKill();
            }
            else
            {
                if(!int.TryParse(arguments.At(0),out numberadded))
                {
                    response = "couldn't parse the number";
                    return false;
                }

                comp.AddKill(numberadded);
            }




            response = "added " + numberadded +" kills to "+ player.Nickname;
            return true;


        }
    }
}
