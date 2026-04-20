using CommandSystem;
using Exiled.API.Features;
using HintServiceMeow.Core.Enum;
using KE.Utils.API.Commands;
using KE.Utils.API.Displays.DisplayMeow;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using KE.Utils.API.GifAnimator;
using LabApi.Features.Wrappers;
using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Player = Exiled.API.Features.Player;

namespace KruacentExiled.Map
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class SpawnImage : KECommand
    {
        public override string Command => "spawnimage";

        public override string[] Aliases => new string[0];

        public override string Description => "image";

        public override string[] Usage => new string[0];

        public override bool ExecuteCommand(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if(!Player.TryGet(sender, out Player player))
            {
                response = "player not foudn";
                return false;
            }



            Image img = Image.FromFile(Paths.Configs + "/ome.png");
            TextImage textimg = new TextImage(img,20);
            Log.Debug(textimg.RawString.Length);



            DisplayHandler.Instance.AddHint(position.HintPlacement, player, textimg.RawString, 30);


            response = "ok";
            return true;
        }

        private TestHintPosition position = new TestHintPosition();
    }


    public class TestHintPosition : HintPosition
    {
        public override float Xposition => 0;

        public override float Yposition => 500;

        public override HintAlignment HintAlignment => HintAlignment.Center;
    }
}
