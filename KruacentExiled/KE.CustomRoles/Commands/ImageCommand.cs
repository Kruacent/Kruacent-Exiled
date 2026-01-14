using CommandSystem;
using Exiled.API.Features;
using KE.CustomRoles.API.Features;
using KE.Utils.API.Commands;
using KE.Utils.API.Displays.DisplayMeow;
using KE.Utils.API.GifAnimator;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.CustomRoles.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ImageCommand : ICommand
    {
        public string Command => "img";

        public string[] Aliases => [];

        public string Description => "image";

        public string[] Usage => [];

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "ok";
            try
            {
                
                string arg1 = arguments.At(0);

                Log.Debug(arg1);
                if (string.IsNullOrEmpty(arg1))
                {
                    response = "arg null "+ arg1;
                    return false;
                }


                if (!float.TryParse(arg1, out float size))
                {
                    response = "float not found";
                    return false;
                }


                try
                {
                    TextImage imaeg = new TextImage(Image.FromFile(MainPlugin.ImageLocation + "/Explode.png"), size);

                    DisplayHandler.Instance.AddHint(KEAbilities.AbilityPosition.HintPlacement, Player.Get(sender), imaeg.RawString, 30);
                }
                catch (Exception e2)
                {
                    Log.Error(e2);
                }

            }
            catch (Exception e)
            {
                Log.Error(e);
            }

        
            
            return true;
        }
    }
}
