using CommandSystem;
using Exiled.API.Features;
using NorthwoodLib.Pools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Misc.Features.PatchNotes
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class PatchNoteChanger : ICommand
    {
        public string Command => "patchnotechanger";

        public string[] Aliases => ["pnc"];

        public string Description => "change patch note";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if(!Round.IsLobby)
            {
                response = "works only in lobby";
                return false;
            }

            if(arguments.Count < 1)
            {
                response = "current patchnote number : "+ MainPlugin.Instance.Config.PatchNote;
                return true;
            }




            StringBuilder sb = StringBuilderPool.Shared.Rent(arguments.Count);

            for(int i = 0; i < arguments.Count; i++)
            {
                sb.Append(arguments.At(i));
                sb.Append(" ");
            }


            string result = sb.ToString();


            StringBuilderPool.Shared.Return(sb);

            
            MainPlugin.Instance.Config.PatchNote = result;

            PatchNote.Reload();


            response = "changed";
            return true;
        }
    }
}
